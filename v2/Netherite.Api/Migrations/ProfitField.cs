
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
    [Migration("20240809205353_ProfitField")]
    public class ProfitField : Migration
    {
        /// <inheritdoc />
        protected override void Up(
#nullable disable
        MigrationBuilder migrationBuilder) => migrationBuilder.AddColumn<int>("Profit", "Users", "integer", defaultValue: ((object)0));

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropColumn("Profit", "Users");

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
