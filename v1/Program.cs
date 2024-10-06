using Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<TaskService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<MinerService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Policy",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("Policy");

app.UseStaticFiles();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/docs/openapi.yaml", "API Documentation");
    c.RoutePrefix = "swagger";
});
app.UseHttpsRedirection();

app.MapGet("/docs/openapi.yaml", async context =>
{
    context.Response.ContentType = "application/x-yaml";
    await context.Response.SendFileAsync("wwwroot/openapi.yaml");
});

app.MapPost("/tasks", async (TaskService taskService, [FromQuery] string title, [FromQuery] string description, [FromQuery] string price, HttpRequest request) =>
{
    try
    {
        if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(description))
        {
            return Results.BadRequest(new { status = 400, callback = "Missing required parameters: title, description" });
        }

        if (!decimal.TryParse(price, out decimal priceDecimal) || priceDecimal <= 0)
        {
            return Results.BadRequest(new { status = 400, callback = "Price must be a positive number" });
        }

        var taskId = await taskService.AddTask(title, description, priceDecimal);

        return Results.Ok(new { status = 200, callback = new { id = taskId } });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { status = 500, callback = $"An unexpected error occurred: {ex.Message}" });
    }
})
.WithName("CreateTask")
.WithOpenApi();

app.MapPost("/tasks/img", async (TaskService taskService, HttpRequest request) =>
{
    if (!request.HasFormContentType || !request.Form.Any())
    {
        return Results.BadRequest("No file uploaded.");
    }

    var form = await request.ReadFormAsync();

    string id = form["id"].ToString();
    var img = form.Files["img"];

    if (img == null || img.Length == 0)
    {
        return Results.NotFound(new { status = 404, callback = "Image file not found." });
    }

    using var memoryStream = new MemoryStream();
    await img.CopyToAsync(memoryStream);
    byte[] imageBytes = memoryStream.ToArray();

    bool success = await taskService.UpdateTaskIcon(id, imageBytes);

    if (!success)
    {
        return Results.NotFound(new { status = 404, callback = $"Task with ID {id} not found." });
    }

    return Results.Ok(new { status = 200, callback = $"Icon updated successfully for task with ID {id}"});
})
.WithName("InsertImg")
.WithOpenApi()
.DisableAntiforgery();

app.MapPut("/tasks/edit", async (TaskService taskService, [FromQuery] string id, [FromQuery] string title, [FromQuery] string description, [FromQuery] decimal price, HttpRequest request) =>
{
    try
    {
        Guid idGuid;
        if (!Guid.TryParse(id, out idGuid))
        {
            return Results.BadRequest(new { status = 400, callback = "Invalid task ID format" });
        }

        await taskService.EditTask(idGuid, title, description, price);
        return Results.Ok(new { status = 200, message = "Task edited successfully" });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
})
.WithName("editTask")
.WithOpenApi();

app.MapDelete("/tasks/remove", async (TaskService taskService, [FromQuery] string id, HttpRequest request) =>
{
    try
    {
        Guid idGuid;
        if (!Guid.TryParse(id, out idGuid))
        {
            return Results.BadRequest(new { status = 400, callback = "Invalid task ID format." });
        }
        await taskService.RemoveTask(idGuid);
        return Results.Ok(new { status = 200, message = "Task removed successfully" });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
})
.WithName("removeTask")
.WithOpenApi();

app.MapGet("/tasks", (TaskService taskService) =>
{
    var tasks = taskService.GetTasks();
    return tasks;
})
.WithName("GetTasks")
.WithOpenApi();

app.MapGet("/img/{id}", async (TaskService taskService, string id) =>
{
    var iconBlob = await taskService.GetTaskIcon(id);
    if (iconBlob == null)
    {
        return Results.NotFound(new { status = 404, callback = "Image not found" });
    }

    return Results.File(iconBlob, "image/jpeg", $"icon_{id}.jpg");
})
.WithName("GetImage")
.WithOpenApi();

app.MapPost("/register", async (UserService userService, string telegram_id, string telegram_name, bool premium, string geo, string? invitedId, string walletId, string tonUserId) =>
{
    try
    {
        Guid registrationSuccess = await userService.InsertUser(telegram_id, telegram_name, premium, geo, invitedId, walletId, tonUserId);

        return Results.Ok(new { status = 200, callback = new { msg = "Registration successful", id = registrationSuccess } });
    }
    catch (FormatException ex)
    {
        return Results.BadRequest(new { status = 400, callback = ex.Message });
    }
    catch (InvalidOperationException ex)
    {
        return Results.Conflict(new { status = 409, callback = ex.Message });
    }
})
.WithName("RegisterUser")
.WithOpenApi();

app.MapGet("/user/{telegram_id}", async (string telegram_id, UserService userService) =>
{
    var user = await userService.GetUserByTelegramId(telegram_id);

    if (user == null)
    {
        return Results.NotFound(new { code = 404, callback = "User not found" });
    }

    return Results.Ok(new
    {
        status = 200,
        callback = new
        {
            id = user.Id,
            balance = user.Balance,
            tonUserId = user.TonUserId,
            walletId = user.WalletId
        }
    });
});

app.MapGet("/user/{telegram_id}/referals", async (string telegram_id, UserService userService) =>
{
    var referals = await userService.GetReferalsByTelegramId(telegram_id);

    if (referals.Count == 0)
    {
        return Results.NotFound(new { code = 404, message = "No referrals found" });
    }

    return Results.Ok(new
    {
        status = 200,
        callback = new
        {
            referals
        }
    });
});

app.MapPost("/user/{telegram_id}/complete/{task_id}", async (TaskService taskService, MinerService minerService, UserService userService, [FromRoute] string telegram_id, [FromRoute] string task_id, HttpRequest request) =>
{
    try
    {
        Guid taskIdGuid;
        if (!Guid.TryParse(task_id, out taskIdGuid))
        {
            return Results.BadRequest(new { status = 400, message = "Invalid task ID format" });
        }

        var task = await taskService.GetTaskById(taskIdGuid);
        if (task == null)
        {
            return Results.NotFound(new { status = 404, message = "Task not found" });
        }

        string priceStr = task.Price.ToString();

        var user = await userService.GetUserByTelegramId(telegram_id);
        if (user == null)
        {
            return Results.NotFound(new { status = 404, message = "User not found" });
        }

        bool balanceUpdated = await minerService.UpdateBalance(user.Id, priceStr);

        return Results.Ok(new { status = 200, message = "Task completed successfully" });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { status = 500, message = $"An unexpected error occurred: {ex.Message}" });
    }
})
.WithName("CompleteTask")
.WithOpenApi();

app.MapPost("/mining/start", async (MinerService minerService, [FromQuery] string id, HttpRequest request) =>
{
    if (string.IsNullOrEmpty(id))
    {
        return Results.BadRequest("Missing required parameter: id or earn");
    }

    try
    {
        bool? result = await minerService.Mining(id, "100");
        return Results.Ok(new { status = 200, callback = result });
    }
    catch (InvalidOperationException ex)
    {
        return Results.Conflict(new { status = 409, callback = ex.Message });
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { status = 400, callback = ex.Message });
    }
})
.WithName("StartMining")
.WithOpenApi();

app.MapPost("/mining/complete", async (MinerService minerService, [FromQuery] string id, HttpRequest request) =>
{
    if (string.IsNullOrEmpty(id))
    {
        return Results.BadRequest(new { status = 400, callback = "Missing required parameter: id" });
    }

    try
    {
        bool? result = await minerService.Mining(id);
        return Results.Ok(new { status = 200, callback = result });
    }
    catch (ArgumentException ex)
    {
        if (ex.Message == "Invalid UUID format")
        {
            return Results.BadRequest(new { status = 400, callback = ex.Message });
        }
        else
        {
            return Results.Conflict(new { status = 409, callback = ex.Message });
        }
    }
})
.WithName("CompleteMiningPOST")
.WithOpenApi();

app.MapGet("/mining/complete", async (MinerService minerService, [FromQuery] string id, HttpRequest request) =>
{
    try
    {
        Guid idGuid;
        if (!Guid.TryParse(id, out idGuid))
        {
            return Results.BadRequest(new { status = 400, callback = "Неверный формат ID минера." });
        }

        var remainingTime = minerService.GetRemainingTime(idGuid);

        return Results.Ok(new { status = 200, callback = remainingTime.TotalSeconds });
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(new { status = 400, callback = ex.Message });
    }
})
.WithName("CompleteMiningGET")
.WithOpenApi();

app.Run();