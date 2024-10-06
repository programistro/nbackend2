
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
    [Migration("20240809154810_init")]
    public class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(
#nullable disable
        MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable("Miners", table => new
            {
                Id = table.Column<Guid>("uuid"),
                UserId = table.Column<Guid>("uuid"),
                Reward = table.Column<int>("integer"),
                StartTime = table.Column<DateTime>("timestamp with time zone"),
                EndTime = table.Column<DateTime>("timestamp with time zone")
            }, constraints: (table => table.PrimaryKey("PK_Miners", x => x.Id)));
            migrationBuilder.CreateTable("Tasks", table => new
            {
                Id = table.Column<Guid>("uuid"),
                Title = table.Column<string>("text"),
                Description = table.Column<string>("text"),
                Icon = table.Column<string>("text"),
                Reward = table.Column<int>("integer")
            }, constraints: (table => table.PrimaryKey("PK_Tasks", x => x.Id)));
            migrationBuilder.CreateTable("Users", table => new
            {
                Id = table.Column<Guid>("uuid"),
                Balance = table.Column<int>("integer"),
                Location = table.Column<string>("text"),
                InvitedId = table.Column<Guid>("uuid"),
                IsPremium = table.Column<bool>("boolean"),
                TelegramId = table.Column<string>("text"),
                TelegramName = table.Column<string>("text"),
                Wallet = table.Column<string>("text")
            }, constraints: (table => table.PrimaryKey("PK_Users", x => x.Id)));
            migrationBuilder.CreateTable("UserTasks", table => new
            {
                Id = table.Column<Guid>("uuid"),
                UserId = table.Column<Guid>("uuid"),
                TaskId = table.Column<Guid>("uuid")
            }, constraints: (table =>
            {
                table.PrimaryKey("PK_UserTasks", x => x.Id);
                table.ForeignKey("FK_UserTasks_Tasks_TaskId", x => x.TaskId, "Tasks", "Id", (string)null, ReferentialAction.NoAction, ReferentialAction.Cascade);
                table.ForeignKey("FK_UserTasks_Users_UserId", x => x.UserId, "Users", "Id", (string)null, ReferentialAction.NoAction, ReferentialAction.Cascade);
            }));
            migrationBuilder.CreateIndex("IX_UserTasks_TaskId", "UserTasks", "TaskId");
            migrationBuilder.CreateIndex("IX_UserTasks_UserId", "UserTasks", "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("Miners");
            migrationBuilder.DropTable("UserTasks");
            migrationBuilder.DropTable("Tasks");
            migrationBuilder.DropTable("Users");
        }

        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", (object)"8.0.7").HasAnnotation("Relational:MaxIdentifierLength", (object)63);
            modelBuilder.UseIdentityByDefaultColumns();
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
                b.Property<int>("Balance").HasColumnType<int>("integer");
                b.Property<Guid>("InvitedId").HasColumnType<Guid>("uuid");
                b.Property<bool>("IsPremium").HasColumnType<bool>("boolean");
                b.Property<string>("Location").IsRequired(true).HasColumnType<string>("text");
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
            modelBuilder.Entity("Netherite.Infrastructure.Entities.UserTaskEntity", (Action<EntityTypeBuilder>)(b =>
            {
                b.HasOne("Netherite.Infrastructure.Entities.TaskEntity", "Task").WithMany("UserTasks").HasForeignKey("TaskId").OnDelete(DeleteBehavior.Cascade).IsRequired();
                b.HasOne("Netherite.Infrastructure.Entities.UserEntity", "User").WithMany("UserTasks").HasForeignKey("UserId").OnDelete(DeleteBehavior.Cascade).IsRequired();
                b.Navigation("Task");
                b.Navigation("User");
            }));
            modelBuilder.Entity("Netherite.Infrastructure.Entities.TaskEntity", (Action<EntityTypeBuilder>)(b => b.Navigation("UserTasks")));
            modelBuilder.Entity("Netherite.Infrastructure.Entities.UserEntity", (Action<EntityTypeBuilder>)(b => b.Navigation("UserTasks")));
        }
    }
}
