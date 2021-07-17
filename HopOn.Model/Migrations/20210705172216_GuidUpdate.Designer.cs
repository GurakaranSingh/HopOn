﻿// <auto-generated />
using System;
using HopOn.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HopOn.Model.Migrations
{
    [DbContext(typeof(AppDBContext))]
    [Migration("20210705172216_GuidUpdate")]
    partial class GuidUpdate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.7");

            modelBuilder.Entity("HopOn.Model.EtagModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("AmazonID")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ETag")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("PartNumber")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ETags");
                });

            modelBuilder.Entity("HopOn.Model.Model.ProgressBarList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("AwsId")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("FileName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Guid")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("LastUpdateDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ProgressBarLists");
                });

            modelBuilder.Entity("HopOn.Model.UploadedFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("AwsId")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("FileName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("FilePath")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("FileSize")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Guid")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("UploadedFiles");
                });
#pragma warning restore 612, 618
        }
    }
}
