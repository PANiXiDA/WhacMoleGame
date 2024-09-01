﻿// <auto-generated />
using System;
using Dal.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Dal.DbModels.Migrations
{
    [DbContext(typeof(DefaultDbContext))]
    [Migration("20240901152812_AddActiveColumnToGames")]
    partial class AddActiveColumnToGames
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Dal.DbModels.Models.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("GameEndTime")
                        .HasColumnType("timestamp");

                    b.Property<DateTime>("GameStartTime")
                        .HasColumnType("timestamp");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<int?>("MaxPointsCount")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("WinnerId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("WinnerId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("Dal.DbModels.Models.Session", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("GameId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<int>("PlayerId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("PlayerId");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("Dal.DbModels.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<bool>("IsBlocked")
                        .HasColumnType("boolean");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("timestamp");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Login")
                        .IsUnique()
                        .HasDatabaseName("Unique_Users_Login");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Dal.DbModels.Models.Game", b =>
                {
                    b.HasOne("Dal.DbModels.Models.User", "Winner")
                        .WithMany()
                        .HasForeignKey("WinnerId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("FK_Game_Winner");

                    b.Navigation("Winner");
                });

            modelBuilder.Entity("Dal.DbModels.Models.Session", b =>
                {
                    b.HasOne("Dal.DbModels.Models.Game", "Game")
                        .WithMany("Sessions")
                        .HasForeignKey("GameId")
                        .IsRequired()
                        .HasConstraintName("FK_Session_Game");

                    b.HasOne("Dal.DbModels.Models.User", "Player")
                        .WithMany("Sessions")
                        .HasForeignKey("PlayerId")
                        .IsRequired()
                        .HasConstraintName("FK_Session_User");

                    b.Navigation("Game");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("Dal.DbModels.Models.Game", b =>
                {
                    b.Navigation("Sessions");
                });

            modelBuilder.Entity("Dal.DbModels.Models.User", b =>
                {
                    b.Navigation("Sessions");
                });
#pragma warning restore 612, 618
        }
    }
}
