
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;
using Netherite.Infrastructure;
using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


#nullable enable
namespace Netherite.API.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(NetheriteDbContext))]
    [Migration("20240822142355_orders")]
    public class orders : Migration
    {
        /// <inheritdoc />
        protected override void Up(
#nullable disable
        MigrationBuilder migrationBuilder)
        {
            MigrationBuilder migrationBuilder1 = migrationBuilder;
            Type type = typeof(int);
            bool? unicode = new bool?();
            int? maxLength = new int?();
            Type oldClrType = type;
            bool? oldUnicode = new bool?();
            int? oldMaxLength = new int?();
            bool? fixedLength = new bool?();
            bool? oldFixedLength = new bool?();
            int? precision = new int?();
            int? oldPrecision = new int?();
            int? scale = new int?();
            int? oldScale = new int?();
            bool? stored = new bool?();
            bool? oldStored = new bool?();
            migrationBuilder1.AlterColumn<Decimal>("Balance", "Users", "numeric", unicode, maxLength, oldClrType: oldClrType, oldType: "integer", oldUnicode: oldUnicode, oldMaxLength: oldMaxLength, fixedLength: fixedLength, oldFixedLength: oldFixedLength, precision: precision, oldPrecision: oldPrecision, scale: scale, oldScale: oldScale, stored: stored, oldStored: oldStored);
            migrationBuilder.CreateTable("Orders", table => new
            {
                Id = table.Column<Guid>("uuid"),
                UserId = table.Column<Guid>("uuid"),
                CurrencyPairsId = table.Column<Guid>("uuid"),
                Bet = table.Column<int>("integer"),
                StartPrice = table.Column<Decimal>("numeric"),
                StartTime = table.Column<DateTime>("timestamp with time zone"),
                EndTime = table.Column<DateTime>("timestamp with time zone"),
                PurchaseDirection = table.Column<bool>("boolean"),
                Ended = table.Column<bool>("boolean")
            }, constraints: (table => table.PrimaryKey("PK_Orders", x => x.Id)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("Orders");
            MigrationBuilder migrationBuilder1 = migrationBuilder;
            Type type = typeof(Decimal);
            bool? unicode = new bool?();
            int? maxLength = new int?();
            Type oldClrType = type;
            bool? oldUnicode = new bool?();
            int? oldMaxLength = new int?();
            bool? fixedLength = new bool?();
            bool? oldFixedLength = new bool?();
            int? precision = new int?();
            int? oldPrecision = new int?();
            int? scale = new int?();
            int? oldScale = new int?();
            bool? stored = new bool?();
            bool? oldStored = new bool?();
            migrationBuilder1.AlterColumn<int>("Balance", "Users", "integer", unicode, maxLength, oldClrType: oldClrType, oldType: "numeric", oldUnicode: oldUnicode, oldMaxLength: oldMaxLength, fixedLength: fixedLength, oldFixedLength: oldFixedLength, precision: precision, oldPrecision: oldPrecision, scale: scale, oldScale: oldScale, stored: stored, oldStored: oldStored);
        }

        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", (object)"8.0.7").HasAnnotation("Relational:MaxIdentifierLength", (object)63);
            modelBuilder.UseIdentityByDefaultColumns();
            modelBuilder.Entity("Netherite.Infrastructure.Entities.CurrencyPairsEntity", (Action<EntityTypeBuilder>)(b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType<Guid>("uuid");
                b.Property<string>("Icon").HasColumnType<string>("text");
                b.Property<Decimal>("InterestRate").HasColumnType<Decimal>("numeric");
                b.Property<string>("Name").HasColumnType<string>("text");
                b.Property<string>("NameTwo").HasColumnType<string>("text");
                b.HasKey("Id");
                b.ToTable("CurrencyPairs");
            }));
            modelBuilder.Entity("Netherite.Infrastructure.Entities.CurrencyPairsIntervalEntity", (Action<EntityTypeBuilder>)(b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType<Guid>("uuid");
                b.Property<Guid>("CurrencyPairsId").HasColumnType<Guid>("uuid");
                b.Property<Guid>("IntervalId").HasColumnType<Guid>("uuid");
                b.HasKey("Id");
                b.HasIndex("CurrencyPairsId");
                b.HasIndex("IntervalId");
                b.ToTable("CurrencyPairsIntervals");
            }));
            modelBuilder.Entity("Netherite.Infrastructure.Entities.FavoritesEntity", (Action<EntityTypeBuilder>)(b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType<Guid>("uuid");
                b.Property<Guid>("CurrencyPairsId").HasColumnType<Guid>("uuid");
                b.Property<Guid>("UserId").HasColumnType<Guid>("uuid");
                b.HasKey("Id");
                b.HasIndex("CurrencyPairsId");
                b.HasIndex("UserId");
                b.ToTable("Favorites");
            }));
            modelBuilder.Entity("Netherite.Infrastructure.Entities.IntervalEntity", (Action<EntityTypeBuilder>)(b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType<Guid>("uuid");
                b.Property<int>("Time").HasColumnType<int>("integer");
                b.HasKey("Id");
                b.ToTable("Intervals");
            }));
            modelBuilder.Entity("Netherite.Infrastructure.Entities.MinerEntity", (Action<EntityTypeBuilder>)(b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType<Guid>("uuid");
                b.Property<DateTime>("EndTime").HasColumnType<DateTime>("timestamp with time zone");
                b.Property<int>("Reward").HasColumnType<int>("integer");
                b.Property<DateTime>("StartTime").HasColumnType<DateTime>("timestamp with time zone");
                b.Property<Guid>("UserId").HasColumnType<Guid>("uuid");
                b.HasKey("Id");
                b.ToTable("Miners");
            }));
            modelBuilder.Entity("Netherite.Infrastructure.Entities.OrderEntity", (Action<EntityTypeBuilder>)(b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType<Guid>("uuid");
                b.Property<int>("Bet").HasColumnType<int>("integer");
                b.Property<Guid>("CurrencyPairsId").HasColumnType<Guid>("uuid");
                b.Property<DateTime>("EndTime").HasColumnType<DateTime>("timestamp with time zone");
                b.Property<bool>("Ended").HasColumnType<bool>("boolean");
                b.Property<bool>("PurchaseDirection").HasColumnType<bool>("boolean");
                b.Property<Decimal>("StartPrice").HasColumnType<Decimal>("numeric");
                b.Property<DateTime>("StartTime").HasColumnType<DateTime>("timestamp with time zone");
                b.Property<Guid>("UserId").HasColumnType<Guid>("uuid");
                b.HasKey("Id");
                b.ToTable("Orders");
            }));
            modelBuilder.Entity("Netherite.Infrastructure.Entities.TaskEntity", (Action<EntityTypeBuilder>)(b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType<Guid>("uuid");
                b.Property<string>("Description").IsRequired(true).HasColumnType<string>("text");
                b.Property<string>("Icon").IsRequired(true).HasColumnType<string>("text");
                b.Property<int>("Reward").HasColumnType<int>("integer");
                b.Property<string>("Title").IsRequired(true).HasColumnType<string>("text");
                b.HasKey("Id");
                b.ToTable("Tasks");
            }));
            modelBuilder.Entity("Netherite.Infrastructure.Entities.UserEntity", (Action<EntityTypeBuilder>)(b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType<Guid>("uuid");
                b.Property<Decimal>("Balance").HasColumnType<Decimal>("numeric");
                b.Property<Guid?>("InvitedId").HasColumnType<Guid?>("uuid");
                b.Property<bool>("IsPremium").HasColumnType<bool>("boolean");
                b.Property<string>("Location").IsRequired(true).HasColumnType<string>("text");
                b.Property<int>("Profit").HasColumnType<int>("integer");
                b.Property<string>("TelegramId").IsRequired(true).HasColumnType<string>("text");
                b.Property<string>("TelegramName").IsRequired(true).HasColumnType<string>("text");
                b.Property<string>("Wallet").IsRequired(true).HasColumnType<string>("text");
                b.HasKey("Id");
                b.ToTable("Users");
            }));
            modelBuilder.Entity("Netherite.Infrastructure.Entities.UserTaskEntity", (Action<EntityTypeBuilder>)(b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType<Guid>("uuid");
                b.Property<Guid>("TaskId").HasColumnType<Guid>("uuid");
                b.Property<Guid>("UserId").HasColumnType<Guid>("uuid");
                b.HasKey("Id");
                b.HasIndex("TaskId");
                b.HasIndex("UserId");
                b.ToTable("UserTasks");
            }));
            modelBuilder.Entity("Netherite.Infrastructure.Entities.CurrencyPairsIntervalEntity", (Action<EntityTypeBuilder>)(b =>
            {
                b.HasOne("Netherite.Infrastructure.Entities.CurrencyPairsEntity", "CurrencyPairs").WithMany("CurrencyPairsIntervals").HasForeignKey("CurrencyPairsId").OnDelete(DeleteBehavior.Cascade).IsRequired();
                b.HasOne("Netherite.Infrastructure.Entities.IntervalEntity", "Interval").WithMany("CurrencyPairsIntervals").HasForeignKey("IntervalId").OnDelete(DeleteBehavior.Cascade).IsRequired();
                b.Navigation("CurrencyPairs");
                b.Navigation("Interval");
            }));
            modelBuilder.Entity("Netherite.Infrastructure.Entities.FavoritesEntity", (Action<EntityTypeBuilder>)(b =>
            {
                b.HasOne("Netherite.Infrastructure.Entities.CurrencyPairsEntity", "CurrencyPairs").WithMany("Favorites").HasForeignKey("CurrencyPairsId").OnDelete(DeleteBehavior.Cascade).IsRequired();
                b.HasOne("Netherite.Infrastructure.Entities.UserEntity", "User").WithMany("Favorites").HasForeignKey("UserId").OnDelete(DeleteBehavior.Cascade).IsRequired();
                b.Navigation("CurrencyPairs");
                b.Navigation("User");
            }));
            modelBuilder.Entity("Netherite.Infrastructure.Entities.UserTaskEntity", (Action<EntityTypeBuilder>)(b =>
            {
                b.HasOne("Netherite.Infrastructure.Entities.TaskEntity", "Task").WithMany("UserTasks").HasForeignKey("TaskId").OnDelete(DeleteBehavior.Cascade).IsRequired();
                b.HasOne("Netherite.Infrastructure.Entities.UserEntity", "User").WithMany("UserTasks").HasForeignKey("UserId").OnDelete(DeleteBehavior.Cascade).IsRequired();
                b.Navigation("Task");
                b.Navigation("User");
            }));
            modelBuilder.Entity("Netherite.Infrastructure.Entities.CurrencyPairsEntity", (Action<EntityTypeBuilder>)(b =>
            {
                b.Navigation("CurrencyPairsIntervals");
                b.Navigation("Favorites");
            }));
            modelBuilder.Entity("Netherite.Infrastructure.Entities.IntervalEntity", (Action<EntityTypeBuilder>)(b => b.Navigation("CurrencyPairsIntervals")));
            modelBuilder.Entity("Netherite.Infrastructure.Entities.TaskEntity", (Action<EntityTypeBuilder>)(b => b.Navigation("UserTasks")));
            modelBuilder.Entity("Netherite.Infrastructure.Entities.UserEntity", (Action<EntityTypeBuilder>)(b =>
            {
                b.Navigation("Favorites");
                b.Navigation("UserTasks");
            }));
        }
    }
}
