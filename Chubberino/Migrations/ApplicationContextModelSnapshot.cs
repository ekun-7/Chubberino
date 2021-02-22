﻿// <auto-generated />
using System;
using Chubberino.Database.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Chubberino.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Chubberino.Database.Models.ApplicationCredentials", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("InitialTwitchPrimaryChannelName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TwitchAPIClientID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WolframAlphaAppID")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("ApplicationCredentials");
                });

            modelBuilder.Entity("Chubberino.Database.Models.StartupChannel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserID")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("StartupChannels");
                });

            modelBuilder.Entity("Chubberino.Database.Models.UserCredentials", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccessToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsBot")
                        .HasColumnType("bit");

                    b.Property<string>("TwitchUsername")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("UserCredentials");
                });

            modelBuilder.Entity("Chubberino.Modules.CheeseGame.Models.Player", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CheeseUnlocked")
                        .HasColumnType("int");

                    b.Property<bool>("IsMouseInfested")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastPointsGained")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastQuestVentured")
                        .HasColumnType("datetime2");

                    b.Property<int>("MaximumPointStorage")
                        .HasColumnType("int");

                    b.Property<int>("MouseTrapCount")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NextQuestSuccessUpgradeUnlock")
                        .HasColumnType("int");

                    b.Property<int>("NextStorageUpgradeUnlock")
                        .HasColumnType("int");

                    b.Property<int>("NextWorkerProductionUpgradeUnlock")
                        .HasColumnType("int");

                    b.Property<int>("NextWorkerQuestSuccessUpgradeUnlock")
                        .HasColumnType("int");

                    b.Property<int>("Points")
                        .HasColumnType("int");

                    b.Property<int>("PopulationCount")
                        .HasColumnType("int");

                    b.Property<int>("Prestige")
                        .HasColumnType("int");

                    b.Property<int>("Rank")
                        .HasColumnType("int");

                    b.Property<string>("TwitchUserID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WorkerCount")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("Players");
                });
#pragma warning restore 612, 618
        }
    }
}
