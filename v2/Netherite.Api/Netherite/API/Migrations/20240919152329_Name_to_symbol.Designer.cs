﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Netherite.Infrastructure;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Netherite.API.Migrations
{
    [DbContext(typeof(NetheriteDbContext))]
    [Migration("20240919152329_Name_to_symbol")]
    partial class Name_to_symbol
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Netherite.Infrastructure.Entities.CurrencyPairsEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Icon")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("InterestRate")
                        .HasColumnType("numeric");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SymbolTwo")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("CurrencyPairs");
                });

            modelBuilder.Entity("Netherite.Infrastructure.Entities.CurrencyPairsIntervalEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CurrencyPairsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("IntervalId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CurrencyPairsId");

                    b.HasIndex("IntervalId");

                    b.ToTable("CurrencyPairsIntervals");
                });

            modelBuilder.Entity("Netherite.Infrastructure.Entities.FavoritesEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CurrencyPairsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CurrencyPairsId");

                    b.HasIndex("UserId");

                    b.ToTable("Favorites");
                });

            modelBuilder.Entity("Netherite.Infrastructure.Entities.IntervalEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Time")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Intervals");
                });

            modelBuilder.Entity("Netherite.Infrastructure.Entities.MinerEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Reward")
                        .HasColumnType("integer");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Miners");
                });

            modelBuilder.Entity("Netherite.Infrastructure.Entities.OrderEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Bet")
                        .HasColumnType("integer");

                    b.Property<Guid>("CurrencyPairsId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("Ended")
                        .HasColumnType("boolean");

                    b.Property<bool>("PurchaseDirection")
                        .HasColumnType("boolean");

                    b.Property<decimal>("StartPrice")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Netherite.Infrastructure.Entities.TaskEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Icon")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Reward")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("Netherite.Infrastructure.Entities.UserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("Balance")
                        .HasColumnType("numeric");

                    b.Property<Guid?>("InvitedId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsPremium")
                        .HasColumnType("boolean");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Profit")
                        .HasColumnType("integer");

                    b.Property<string>("TelegramId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TelegramName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Wallet")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Netherite.Infrastructure.Entities.UserTaskEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("TaskId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("TaskId");

                    b.HasIndex("UserId");

                    b.ToTable("UserTasks");
                });

            modelBuilder.Entity("Netherite.Infrastructure.Entities.CurrencyPairsIntervalEntity", b =>
                {
                    b.HasOne("Netherite.Infrastructure.Entities.CurrencyPairsEntity", "CurrencyPairs")
                        .WithMany("CurrencyPairsIntervals")
                        .HasForeignKey("CurrencyPairsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Netherite.Infrastructure.Entities.IntervalEntity", "Interval")
                        .WithMany("CurrencyPairsIntervals")
                        .HasForeignKey("IntervalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CurrencyPairs");

                    b.Navigation("Interval");
                });

            modelBuilder.Entity("Netherite.Infrastructure.Entities.FavoritesEntity", b =>
                {
                    b.HasOne("Netherite.Infrastructure.Entities.CurrencyPairsEntity", "CurrencyPairs")
                        .WithMany("Favorites")
                        .HasForeignKey("CurrencyPairsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Netherite.Infrastructure.Entities.UserEntity", "User")
                        .WithMany("Favorites")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CurrencyPairs");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Netherite.Infrastructure.Entities.UserTaskEntity", b =>
                {
                    b.HasOne("Netherite.Infrastructure.Entities.TaskEntity", "Task")
                        .WithMany("UserTasks")
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Netherite.Infrastructure.Entities.UserEntity", "User")
                        .WithMany("UserTasks")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Task");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Netherite.Infrastructure.Entities.CurrencyPairsEntity", b =>
                {
                    b.Navigation("CurrencyPairsIntervals");

                    b.Navigation("Favorites");
                });

            modelBuilder.Entity("Netherite.Infrastructure.Entities.IntervalEntity", b =>
                {
                    b.Navigation("CurrencyPairsIntervals");
                });

            modelBuilder.Entity("Netherite.Infrastructure.Entities.TaskEntity", b =>
                {
                    b.Navigation("UserTasks");
                });

            modelBuilder.Entity("Netherite.Infrastructure.Entities.UserEntity", b =>
                {
                    b.Navigation("Favorites");

                    b.Navigation("UserTasks");
                });
#pragma warning restore 612, 618
        }
    }
}
