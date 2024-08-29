﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TestBackendSERU.Service;

namespace TestBackendSERU.Migrations
{
    [DbContext(typeof(VehicleDbContext))]
    [Migration("20240827041522_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TestBackendSERU.Entity.Pricelist", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Created_At")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Model_ID")
                        .HasColumnType("int");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<string>("Updated_At")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Year_ID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("Model_ID");

                    b.HasIndex("Year_ID");

                    b.ToTable("Pricelist");
                });

            modelBuilder.Entity("TestBackendSERU.Entity.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Created_At")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Is_Admin")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Updated_At")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("User");
                });

            modelBuilder.Entity("TestBackendSERU.Entity.Vehicle_Brand", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Created_At")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Updated_At")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Vehicle_Brand");
                });

            modelBuilder.Entity("TestBackendSERU.Entity.Vehicle_Model", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Created_At")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Type_ID")
                        .HasColumnType("int");

                    b.Property<string>("Updated_At")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("Type_ID");

                    b.ToTable("Vehicle_Model");
                });

            modelBuilder.Entity("TestBackendSERU.Entity.Vehicle_Type", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("Brand_ID")
                        .HasColumnType("int");

                    b.Property<string>("Created_At")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Updated_At")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("Brand_ID");

                    b.ToTable("Vehicle_Type");
                });

            modelBuilder.Entity("TestBackendSERU.Entity.Vehicle_Year", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Created_At")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Updated_At")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Year")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Vehicle_Year");
                });

            modelBuilder.Entity("TestBackendSERU.Entity.Pricelist", b =>
                {
                    b.HasOne("TestBackendSERU.Entity.Vehicle_Model", "Vehicle_Model")
                        .WithMany("Pricelist")
                        .HasForeignKey("Model_ID")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("TestBackendSERU.Entity.Vehicle_Year", "Vehicle_Year")
                        .WithMany("Pricelist")
                        .HasForeignKey("Year_ID")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("TestBackendSERU.Entity.Vehicle_Model", b =>
                {
                    b.HasOne("TestBackendSERU.Entity.Vehicle_Type", "Vehicle_Type")
                        .WithMany("Vehicle_Model")
                        .HasForeignKey("Type_ID")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("TestBackendSERU.Entity.Vehicle_Type", b =>
                {
                    b.HasOne("TestBackendSERU.Entity.Vehicle_Brand", "Vehicle_Brand")
                        .WithMany("Vehicle_Types")
                        .HasForeignKey("Brand_ID")
                        .OnDelete(DeleteBehavior.SetNull);
                });
#pragma warning restore 612, 618
        }
    }
}
