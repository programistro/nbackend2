using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Db
{
    public class Task
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("icon")]
        public byte[] Icon { get; set; } = new byte[0];

        public Task(Guid id, string title, string description, decimal price, byte[] icon = null)
        {
            Id = id;
            Title = title;
            Description = description;
            Price = price;
            Icon = icon ?? new byte[0];
        }
    }

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Task> tasks { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<Miner> miners { get; set; }
    }

    public class TaskService
    {
        private readonly AppDbContext _dbContext;

        public TaskService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> AddTask(string title, string description, decimal price)
        {
            var newTask = new Task(Guid.NewGuid(), title, description, price);
            await _dbContext.tasks.AddAsync(newTask);
            await _dbContext.SaveChangesAsync();

            return newTask.Id;
        }

        public async Task<bool> UpdateTaskIcon(string id, byte[] iconBlob)
        {
            Guid taskId = Guid.Parse(id);
            var task = await _dbContext.tasks.FindAsync(taskId);

            if (task == null)
            {
                return false;
            }

            var entry = _dbContext.Entry(task);

            entry.CurrentValues.SetValues(task);
            entry.Property(t => t.Icon).CurrentValue = iconBlob;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditTask(Guid id, string title, string description, decimal price)
        {
            var task = await _dbContext.tasks.FindAsync(id);
            if (task == null)
            {
                throw new Exception("Task not found.");
            }

            task.Title = title;
            task.Description = description;
            task.Price = price;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveTask(Guid id)
        {
            var task = await _dbContext.tasks.FindAsync(id);
            if (task == null)
            {
                throw new Exception("Task not found.");
            }

            _dbContext.tasks.Remove(task);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<byte[]> GetTaskIcon(string id)
        {
            Guid taskId = Guid.Parse(id);
            var task = await _dbContext.tasks.FirstOrDefaultAsync(t => t.Id == taskId);

            if (task == null || task.Icon == null)
            {
                return null;
            }

            return task.Icon;
        }

        public IEnumerable<Task> GetTasks()
        {
            return _dbContext.tasks.ToList();
        }

        public async Task<Task> GetTaskById(Guid id)
        {
            return await _dbContext.tasks.FindAsync(id);
        }
    }

    public class User
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("balance")]
        public long Balance { get; set; }

        [Column("geo")]
        public string Geo { get; set; }

        [Column("invited_id")]
        public string? InvitedId { get; set; }

        [Column("premium")]
        public bool IsPremium { get; set; }

        [Column("telegram_id")]
        public int TelegramId { get; set; }

        [Column("telegram_name")]
        public string TelegramName { get; set; }

        [Column("referals")]
        public string Referals { get; set; } = "{}";

        [Column("tonuser_id")]
        public string TonUserId { get; set; }

        [Column("wallet_id")]
        public string WalletId { get; set; }

        public User(string geo, string invitedId = "", bool isPremium = false, int telegramId = 0, string telegramName = "", string tonUserId = "", string walletId = "")
        {
            Balance = 0;
            Geo = geo;
            InvitedId = invitedId;
            IsPremium = isPremium;
            TelegramId = telegramId;
            TelegramName = telegramName;
            TonUserId = tonUserId;
            WalletId = walletId;
        }
    }

    public class UserService
    {
        private readonly AppDbContext _dbContext;

        public UserService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> InsertUser(string telegram_id, string telegram_name, bool premium, string geo, string? invitedId, string walletId, string tonUserId)
        {
            if (!int.TryParse(telegram_id, out int parsedTelegramId))
            {
                throw new FormatException("Invalid telegram_id format");
            }

            var existingUser = await GetUserByTelegramId(telegram_id);
            if (existingUser != null)
            {
                throw new InvalidOperationException("This telegram_id already exists");
            }

            var newUser = new User(geo: geo, invitedId: invitedId, isPremium: premium, telegramId: parsedTelegramId, telegramName: telegram_name, walletId: walletId, tonUserId: tonUserId);
            
            _dbContext.users.Add(newUser);
            await _dbContext.SaveChangesAsync();

            if (!string.IsNullOrWhiteSpace(invitedId) && Guid.TryParse(invitedId, out Guid invitedGuid))
            {
                var inviter = await _dbContext.users.FindAsync(invitedGuid);
                if (inviter != null)
                {
                    Dictionary<string, int> currentReferalsDict;

                    try
                    {
                        currentReferalsDict = JsonSerializer.Deserialize<Dictionary<string, int>>(inviter.Referals);
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"Deserialization failed: {ex.Message}");
                        currentReferalsDict = new Dictionary<string, int>();
                    }

                    var newReferalKey = newUser.Id.ToString();
                    currentReferalsDict[newReferalKey] = 0;

                    var updatedReferalsJson = JsonSerializer.Serialize(currentReferalsDict);

                    inviter.Referals = updatedReferalsJson;
                    await _dbContext.SaveChangesAsync();
                }
            }

            return newUser.Id;
        }

        public async Task<User?> GetUser(string telegramId)
        {
            var user = await _dbContext.users.FirstOrDefaultAsync(u => u.TelegramId == int.Parse(telegramId));
            return user;
        }

        public async Task<User?> GetUserByTelegramId(string telegramId)
        {
            if (!int.TryParse(telegramId, out int parsedTelegramId))
            {
                return null;
            }

            return await _dbContext.users.FirstOrDefaultAsync(u => u.TelegramId == parsedTelegramId);
        }

        public async Task<User> GetUserById(Guid id)
        {
            return await _dbContext.users.FindAsync(id);
        }

        public async Task<Dictionary<string, dynamic>> GetReferalsByTelegramId(string telegramId)
        {
            var user = await GetUserByTelegramId(telegramId);
            if (user == null)
            {
                return new Dictionary<string, dynamic>();
            }

            var referalsJsonString = user.Referals;
            var referalsDict = JsonSerializer.Deserialize<Dictionary<string, int>>(referalsJsonString);

            var referalsWithDetails = new Dictionary<string, dynamic>();

            foreach (var kvp in referalsDict)
            {
                var referalId = kvp.Key;
                var profit = kvp.Value;

                var referalCount = await CountReferals(referalId);

                Guid referalUuid;
                if (Guid.TryParse(referalId, out referalUuid))
                {
                    var referalUser = await GetUserById(referalUuid);
                    if (referalUser != null)
                    {
                        var telegramName = referalUser.TelegramName;
                        referalsWithDetails[referalId] = new
                        {
                            profit = profit,
                            referals = referalCount,
                            telegramName = telegramName
                        };
                    }
                }
            }

            return referalsWithDetails;
        }

        private async Task<int> CountReferals(string referalId)
        {
            var referals = await _dbContext.users
                .Where(u => u.InvitedId == referalId)
                .Select(u => u.Id)
                .ToListAsync();

            return referals.Count;
        }
    }

    public class Miner
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("earn")]
        public string Earn { get; set; }

        [Column("timestamp")]
        public DateTimeOffset Timestamp { get; set; }

        public Miner(Guid id, string earn, DateTimeOffset timestamp)
        {
            Id = id;
            Earn = earn;
            Timestamp = timestamp;
        }
    }

    public class MinerService
    {
        private readonly AppDbContext _dbContext;
        private readonly UserService _userService;

        public MinerService(AppDbContext dbContext, UserService userService)
        {
            _dbContext = dbContext;
            _userService = userService;
        }

        public async Task<bool> UpdateBalance(Guid id, string earn)
        {
            var user = await _dbContext.users.FindAsync(id);

            if (user == null) return false;

            var earnedAmount = long.Parse(earn);

            user.Balance += earnedAmount;

            if (!string.IsNullOrEmpty(user.InvitedId) && Guid.TryParse(user.InvitedId, out Guid invitedId))
            {
                var invitedUser = await _dbContext.users.FindAsync(invitedId);
                if (invitedUser != null)
                {
                    double referralPercentage = invitedUser.IsPremium ? 0.2 : 0.1;

                    invitedUser.Balance += (long)(earnedAmount * referralPercentage);

                    var referalsJsonString = invitedUser.Referals;
                    var referalsDict = JsonSerializer.Deserialize<Dictionary<string, int>>(referalsJsonString);
                    var userIdToUpdate = id.ToString();
                    int referralAmount = referalsDict.ContainsKey(userIdToUpdate) ? referalsDict[userIdToUpdate] : 0;
                    long referralAmountToAdd = (long)(earnedAmount * referralPercentage);
                    referralAmount += (int)referralAmountToAdd;
                    referalsDict[userIdToUpdate] = referralAmount;
                    var updatedReferalsJson = JsonSerializer.Serialize(referalsDict);
                    invitedUser.Referals = updatedReferalsJson;

                    if (!string.IsNullOrEmpty(invitedUser.InvitedId) && Guid.TryParse(invitedUser.InvitedId, out Guid secondLevelReferralId))
                    {
                        var secondLevelReferral = await _dbContext.users.FindAsync(secondLevelReferralId);
                        if (secondLevelReferral != null)
                        {
                            double secondLevelReferralPercentage = secondLevelReferral.IsPremium ? 0.06 : 0.03;

                            var secondLevelReferralPercentageAmount = (long)(earnedAmount * secondLevelReferralPercentage);

                            secondLevelReferral.Balance += secondLevelReferralPercentageAmount;

                            var secondLevelReferalsJsonString = secondLevelReferral.Referals;
                            var secondLevelReferalsDict = JsonSerializer.Deserialize<Dictionary<string, int>>(secondLevelReferalsJsonString);
                            var secondLevelInvitedUserId = invitedUser.Id.ToString();
                            int secondLevelReferralAmount = secondLevelReferalsDict.ContainsKey(secondLevelInvitedUserId) ? secondLevelReferalsDict[secondLevelInvitedUserId] : 0;
                            secondLevelReferralAmount += (int)secondLevelReferralPercentageAmount;
                            secondLevelReferalsDict[secondLevelInvitedUserId] = secondLevelReferralAmount;
                            var updatedSecondLevelReferalsJson = JsonSerializer.Serialize(secondLevelReferalsDict);
                            secondLevelReferral.Referals = updatedSecondLevelReferalsJson;
                        }
                    }
                }
            }

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool?> Mining(string idStr, string? earn = null)
        {
            if (!Guid.TryParse(idStr, out Guid id))
            {
                throw new ArgumentException("Invalid UUID format");
            }

            var nowUtc = DateTimeOffset.UtcNow;

            var miner = await _dbContext.miners.FirstOrDefaultAsync(m => m.Id == id);
            if (miner != null)
            {
                if (!string.IsNullOrEmpty(earn))
                {
                    throw new InvalidOperationException("Miner with this id already exists");
                }
                else
                {
                    if (miner.Timestamp > nowUtc)
                    {
                        return false;
                    }
                    else
                    {
                        await UpdateBalance(id, miner.Earn);
                        _dbContext.miners.Remove(miner);
                        await _dbContext.SaveChangesAsync();
                        return true;
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(earn))
                {
                    var newMiner = new Miner(id, earn, DateTimeOffset.UtcNow.AddSeconds(40));
                    _dbContext.miners.Add(newMiner);
                    await _dbContext.SaveChangesAsync();
                    return null;
                }
                else
                {
                    throw new ArgumentException("User hasn't started mining yet");
                }
            }
        }

        public TimeSpan GetRemainingTime(Guid id)
        {
            var miner = _dbContext.miners.FirstOrDefault(m => m.Id == id);
            if (miner == null)
            {
                throw new InvalidOperationException("User not found");
            }

            var nowUtc = DateTimeOffset.UtcNow;
            var minerTimestamp = miner.Timestamp.ToUniversalTime();

            var timeDifference = nowUtc - minerTimestamp;

            if (timeDifference > TimeSpan.Zero)
            {
                return TimeSpan.Zero;
            }
            else
            {
                return TimeSpan.FromTicks(Math.Abs((long)(timeDifference.Ticks)));
            }
        }


    }
}