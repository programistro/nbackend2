// Type: Program

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.StaticFiles.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Netherite.API;
using Netherite.Application.Services;
using Netherite.Domain.Repositories;
using Netherite.Domain.Services;
using Netherite.Infrastructure;
using Netherite.Infrastructure.Repositories;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

#nullable enable
[CompilerGenerated]
public class Program
{
	public static void Main(string[] args)
	{
		WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

		var port = ((IConfiguration)builder.Configuration).GetValue<int>("Port");
		builder.WebHost.UseUrls($"http://*:{port}");

		CorsServiceCollectionExtensions.AddCors(builder.Services, (Action<CorsOptions>)(options => options.AddDefaultPolicy((Action<CorsPolicyBuilder>)(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()))));
		MvcServiceCollectionExtensions.AddControllers(builder.Services);
		EndpointMetadataApiExplorerServiceCollectionExtensions.AddEndpointsApiExplorer(builder.Services);
		builder.Services.AddSwaggerGen((Action<SwaggerGenOptions>)(c =>
		{
			c.SwaggerDoc("v2", new OpenApiInfo()
			{
				Version = "v2",
				Title = "Netherite API",
				Description = "API for Netherite app"
			});
			SwaggerGenOptionsExtensions.TagActionsBy(c, (Func<ApiDescription, IList<string>>)(api => (IList<string>)new string[1]
			{
		((ControllerActionDescriptor) api.ActionDescriptor).ControllerName
			}));
			SwaggerGenOptionsExtensions.DocInclusionPredicate(c, (Func<string, ApiDescription, bool>)((name, api) => true));
			MethodInfo methodInfo = null;
			SwaggerGenOptionsExtensions.CustomOperationIds(c, (Func<ApiDescription, string>)(apiDesc => Swashbuckle.AspNetCore.SwaggerGen.ApiDescriptionExtensions.TryGetMethodInfo(apiDesc, out methodInfo) ? methodInfo.Name : (string)null));
			string filePath = Path.Combine(AppContext.BaseDirectory, Assembly.GetExecutingAssembly().GetName().Name + ".xml");
			c.IncludeXmlComments(filePath);
		}));
		builder.Services.AddDbContext<NetheriteDbContext>((Action<DbContextOptionsBuilder>)(options => options.UseNpgsql(((IConfiguration)builder.Configuration).GetConnectionString("NetheriteDbContext"), (Action<NpgsqlDbContextOptionsBuilder>)(b => b.MigrationsAssembly("Netherite.Api")))));

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(options =>
  {
      options.TokenValidationParameters = new TokenValidationParameters
      {
          ValidateIssuer = true,
          ValidIssuer = AuthOptions.ISSUER,
          ValidateAudience = true,
          ValidAudience = AuthOptions.AUDIENCE,
          ValidateLifetime = true,
          IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
          ValidateIssuerSigningKey = true
      };
  });

        SwaggerGenServiceCollectionExtensions.AddSwaggerGen(builder.Services, (Action<SwaggerGenOptions>)(c =>
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'maxSecurityPassword228'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                // Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
  {
    {
      new OpenApiSecurityScheme
      {
        Reference = new OpenApiReference
        {
          Type = ReferenceType.SecurityScheme,
          Id = "Bearer"
        },
        // Scheme = "oauth2",
        // Name = "Bearer",
        // In = ParameterLocation.Header,

      },
      new List<string>()
    }
  });

            SwaggerGenOptionsExtensions.SwaggerDoc(c, "v1", new OpenApiInfo()
            {
                Version = "v1",
                Title = "Netherite API",
                Description = "API for Netherite app"
            });
            SwaggerGenOptionsExtensions.TagActionsBy(c, (Func<ApiDescription, IList<string>>)(api => (IList<string>)new string[1]
            {
    ((ControllerActionDescriptor) api.ActionDescriptor).ControllerName
            }));
            // SwaggerGenOptionsExtensions.DocInclusionPredicate(c, (Func<string, ApiDescription, bool>) ((name, api) => true));
            // MethodInfo methodInfo;
            // SwaggerGenOptionsExtensions.CustomOperationIds(c, (Func<ApiDescription, string>) (apiDesc => Swashbuckle.AspNetCore.SwaggerGen.ApiDescriptionExtensions.TryGetMethodInfo(apiDesc, out methodInfo) ? methodInfo.Name : (string) null));
            // string filePath = Path.Combine(AppContext.BaseDirectory, Assembly.GetExecutingAssembly().GetName().Name + ".xml");
            // SwaggerGenOptionsExtensions.IncludeXmlComments(c, filePath);
        }));

        builder.Services.AddScoped<IReferalBonusesServices, ReferalBonusesServices>();
		builder.Services.AddScoped<ITasksServices, TasksServices>();
		builder.Services.AddScoped<ITasksRepository, TasksRepository>();
		builder.Services.AddScoped<IMinerServices, MinerServices>();
		builder.Services.AddScoped<IMinerRepository, MinerRepository>();
		builder.Services.AddScoped<IUserServices, UserServices>();
		builder.Services.AddScoped<IUserRepository, UserRepository>();
		builder.Services.AddScoped<IIntervalServices, IntervalServices>();
		builder.Services.AddScoped<IIntervalsRepository, IntervalsRepository>();
		builder.Services.AddScoped<ICurrencyPairsService, CurrencyPairsServices>();
		builder.Services.AddScoped<ICurrencyPairsRepository, CurrencyPairsRepository>();
		builder.Services.AddScoped<IFavoritesService, FavoritesService>();
		builder.Services.AddScoped<IFavoritesRepository, FavoritesRepository>();
		builder.Services.AddScoped<IOrderServices, OrderService>();
		builder.Services.AddHttpClient<IOrderRepository, OrderRepository>();
		builder.Services.AddHostedService<OrderBackgroundService>();
		builder.Services.AddSingleton<OrderBackgroundService>();

		WebApplication webApplication1 = builder.Build();
		if (!Directory.Exists("storage"))
			Directory.CreateDirectory("storage");
		WebApplication webApplication2 = webApplication1;
		StaticFileOptions staticFileOptions = new StaticFileOptions();

		((SharedOptionsBase)staticFileOptions).FileProvider = new PhysicalFileProvider(Path.Combine(((IHostEnvironment)builder.Environment).ContentRootPath, "storage"));
		((SharedOptionsBase)staticFileOptions).RequestPath = "/storage";

		StaticFileExtensions.UseStaticFiles((IApplicationBuilder)webApplication2, staticFileOptions);
		SwaggerBuilderExtensions.UseSwagger((IApplicationBuilder)webApplication1, (Action<SwaggerOptions>)null);
		SwaggerUIBuilderExtensions.UseSwaggerUI((IApplicationBuilder)webApplication1, (Action<SwaggerUIOptions>)(c => c.SwaggerEndpoint("/swagger/v2/swagger.json", "Netherite API v2")));
		HttpsPolicyBuilderExtensions.UseHttpsRedirection((IApplicationBuilder)webApplication1);
		CorsMiddlewareExtensions.UseCors((IApplicationBuilder)webApplication1);
		EndpointRoutingApplicationBuilderExtensions.UseRouting((IApplicationBuilder)webApplication1);
		AuthAppBuilderExtensions.UseAuthentication((IApplicationBuilder)webApplication1);
		AuthorizationAppBuilderExtensions.UseAuthorization((IApplicationBuilder)webApplication1);
		ControllerEndpointRouteBuilderExtensions.MapControllers((IEndpointRouteBuilder)webApplication1);

		try
		{
			var serviceProvider = builder.Services.BuildServiceProvider();

			var dbContext = serviceProvider.GetRequiredService<NetheriteDbContext>();
			dbContext.Database.Migrate();
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.ToString());
		}

		webApplication1.Run((string)null);
	}
}

public class AuthOptions
{
    public const string ISSUER = "MyAuthServer"; // издатель токена
    public const string AUDIENCE = "MyAuthClient"; // потребитель токена
    const string KEY = "mysupersecret_secretsecretsecretkey!123";   // ключ для шифрации
    public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
      new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
}