﻿// <auto-generated />
using System;
using FastWiki.Service.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FastWiki.Service.Migrations.Sqlite
{
    [DbContext(typeof(SqliteContext))]
    partial class SqliteContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.7");

            modelBuilder.Entity("FastWiki.Service.Domain.ChatApplications.Aggregates.ChatApplication", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ChatModel")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("Creator")
                        .HasColumnType("TEXT");

                    b.Property<string>("Extend")
                        .HasColumnType("TEXT");

                    b.Property<string>("FunctionIds")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MaxResponseToken")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ModificationTime")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("Modifier")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("NoReplyFoundTemplate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Opener")
                        .HasColumnType("TEXT");

                    b.Property<string>("Parameter")
                        .HasColumnType("TEXT");

                    b.Property<string>("Prompt")
                        .HasColumnType("TEXT");

                    b.Property<int>("ReferenceUpperLimit")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Relevancy")
                        .HasColumnType("REAL");

                    b.Property<bool>("ShowSourceFile")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Temperature")
                        .HasColumnType("REAL");

                    b.Property<string>("Template")
                        .HasColumnType("TEXT");

                    b.Property<string>("WikiIds")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Name");

                    b.ToTable("wiki-chat-application", (string)null);
                });

            modelBuilder.Entity("FastWiki.Service.Domain.ChatApplications.Aggregates.ChatRecord", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ApplicationId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("Creator")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ModificationTime")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("Modifier")
                        .HasColumnType("TEXT");

                    b.Property<string>("Question")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CreationTime");

                    b.ToTable("wiki-chat-record", (string)null);
                });

            modelBuilder.Entity("FastWiki.Service.Domain.ChatApplications.Aggregates.ChatShare", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("APIKey")
                        .HasColumnType("TEXT");

                    b.Property<int>("AvailableQuantity")
                        .HasColumnType("INTEGER");

                    b.Property<long>("AvailableToken")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ChatApplicationId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("Creator")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("Expires")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ModificationTime")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("Modifier")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<long>("UsedToken")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ChatApplicationId");

                    b.ToTable("wiki-chat-share", (string)null);
                });

            modelBuilder.Entity("FastWiki.Service.Domain.ChatApplications.Aggregates.Questions", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ApplicationId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("Order")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Question")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("wiki-questions", (string)null);
                });

            modelBuilder.Entity("FastWiki.Service.Domain.Function.Aggregates.FastWikiFunctionCall", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Content")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("Creator")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Enable")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Imports")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Items")
                        .HasColumnType("TEXT");

                    b.Property<string>("Main")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ModificationTime")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("Modifier")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Parameters")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CreationTime");

                    b.ToTable("wiki-function-calls", (string)null);
                });

            modelBuilder.Entity("FastWiki.Service.Domain.Storage.Aggregates.FileStorage", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("Creator")
                        .HasColumnType("TEXT");

                    b.Property<string>("FullName")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsCompression")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ModificationTime")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("Modifier")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("Path")
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<long>("Size")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("wiki-file-storages", (string)null);
                });

            modelBuilder.Entity("FastWiki.Service.Domain.Users.Aggregates.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Account")
                        .HasColumnType("TEXT");

                    b.Property<string>("Avatar")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("Creator")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDisable")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ModificationTime")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("Modifier")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("Phone")
                        .HasColumnType("TEXT");

                    b.Property<int>("Role")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Salt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("wiki-users", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("6206211d-6e07-42ea-8750-7d415a2ffaa8"),
                            Account = "admin",
                            Avatar = "https://blog-simple.oss-cn-shenzhen.aliyuncs.com/Avatar.jpg",
                            CreationTime = new DateTime(2024, 8, 18, 14, 8, 56, 546, DateTimeKind.Utc).AddTicks(8987),
                            Email = "239573049@qq.com",
                            IsDeleted = false,
                            IsDisable = false,
                            ModificationTime = new DateTime(2024, 8, 18, 14, 8, 56, 546, DateTimeKind.Utc).AddTicks(8990),
                            Name = "admin",
                            Password = "f0f6d913839798fff09760727a015264",
                            Phone = "13049809673",
                            Role = 2,
                            Salt = "ff4cdcea6d814b80a4777acf20cdc52f"
                        });
                });

            modelBuilder.Entity("FastWiki.Service.Domain.Wikis.Aggregates.QuantizedList", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ProcessTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Remark")
                        .HasColumnType("TEXT");

                    b.Property<int>("State")
                        .HasColumnType("INTEGER");

                    b.Property<long>("WikiDetailId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("WikiId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("WikiDetailId");

                    b.HasIndex("WikiId");

                    b.ToTable("wiki-quantized-lists", (string)null);
                });

            modelBuilder.Entity("FastWiki.Service.Domain.Wikis.Aggregates.Wiki", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("Creator")
                        .HasColumnType("TEXT");

                    b.Property<string>("EmbeddingModel")
                        .HasColumnType("TEXT");

                    b.Property<string>("Icon")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Model")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ModificationTime")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("Modifier")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<int>("VectorType")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("Name");

                    b.ToTable("wiki-wikis", (string)null);
                });

            modelBuilder.Entity("FastWiki.Service.Domain.Wikis.Aggregates.WikiDetail", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("TEXT");

                    b.Property<long>("Creator")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DataCount")
                        .HasColumnType("INTEGER");

                    b.Property<long>("FileId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FileName")
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<int>("MaxTokensPerLine")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MaxTokensPerParagraph")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Mode")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ModificationTime")
                        .HasColumnType("TEXT");

                    b.Property<long>("Modifier")
                        .HasColumnType("INTEGER");

                    b.Property<int>("OverlappingTokens")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Path")
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<string>("QAPromptTemplate")
                        .HasColumnType("TEXT");

                    b.Property<int>("State")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TrainingPattern")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Type")
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<long>("WikiId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("wiki-wiki-details", (string)null);
                });

            modelBuilder.Entity("mem0.Core.Model.History", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Event")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("MemoryId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("NewValue")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PrevValue")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("TrackId")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("MemoryId");

                    b.HasIndex("TrackId");

                    b.HasIndex("UserId");

                    b.ToTable("wiki-histories", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
