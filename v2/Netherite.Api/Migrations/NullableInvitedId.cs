
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
    [Migration("20240809185940_NullableInvitedId")]
    public class NullableInvitedId : Migration
    {
        /// <inheritdoc />
        protected override void Up(
#nullable disable
        MigrationBuilder migrationBuilder)
        {
            MigrationBuilder migrationBuilder1 = migrationBuilder;
            Type type = typeof(Guid);
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
            migrationBuilder1.AlterColumn<Guid>("InvitedId", "Users", "uuid", unicode, maxLength, nullable: true, oldClrType: oldClrType, oldType: "uuid", oldUnicode: oldUnicode, oldMaxLength: oldMaxLength, fixedLength: fixedLength, oldFixedLength: oldFixedLength, precision: precision, oldPrecision: oldPrecision, scale: scale, oldScale: oldScale, stored: stored, oldStored: oldStored);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            MigrationBuilder migrationBuilder1 = migrationBuilder;
            object obj = (object)new Guid("00000000-0000-0000-0000-000000000000");
            Type type = typeof(Guid);
            bool? unicode = new bool?();
            int? maxLength = new int?();
            object defaultValue = obj;
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
            migrationBuilder1.AlterColumn<Guid>("InvitedId", "Users", "uuid", unicode, maxLength, defaultValue: defaultValue, oldClrType: oldClrType, oldType: "uuid", oldUnicode: oldUnicode, oldMaxLength: oldMaxLength, oldNullable: true, fixedLength: fixedLength, oldFixedLength: oldFixedLength, precision: precision, oldPrecision: oldPrecision, scale: scale, oldScale: oldScale, stored: stored, oldStored: oldStored);
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
                b.Property<Guid?>("InvitedId").HasColumnType<Guid?>("uuid");
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
