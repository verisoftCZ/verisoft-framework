﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Verisoft.DemoApi.Data.EF.Context;


#nullable disable

namespace Verisoft.DemoApi.Database.Migrations
{
    [DbContext(typeof(DemoApiDbContext))]
    [Migration("20240412145601_AddedContractTable")]
    partial class AddedContractTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Verisoft.DemoApi.Core.Entities.ClientEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AccountingSystemClientId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyActivity")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactNote")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("NumberOfEmployees")
                        .HasColumnType("int");

                    b.Property<int?>("ParentClientId")
                        .HasColumnType("int");

                    b.Property<string>("RecruitmentUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Representative")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TaxDomicile")
                        .HasColumnType("int");

                    b.Property<string>("TradeId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TradeRegisterEntry")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("VatId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WebsiteUrl")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ParentClientId");

                    b.ToTable("Client");
                });

            modelBuilder.Entity("Verisoft.DemoApi.Core.Entities.ContractEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("ClientEntityId")
                        .HasColumnType("int");

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime?>("DateSigned")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasMaxLength(4000)
                        .HasColumnType("nvarchar(4000)");

                    b.Property<double?>("FeePercentage")
                        .HasColumnType("float");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int?>("SignedById")
                        .HasColumnType("int");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime?>("ValidFrom")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ValidTo")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("WarrantyPeriod")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ClientEntityId");

                    b.HasIndex("SignedById");

                    b.ToTable("Contract");
                });

            modelBuilder.Entity("Verisoft.DemoApi.Core.Entities.DocumentEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("ClientId")
                        .HasColumnType("int");

                    b.Property<byte[]>("Content")
                        .HasColumnType("varbinary(max)");

                    b.Property<int?>("ContractId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Description")
                        .HasMaxLength(4000)
                        .HasColumnType("nvarchar(4000)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ValidFrom")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ValidTo")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("ContractId");

                    b.HasIndex("UserId");

                    b.ToTable("Document");
                });

            modelBuilder.Entity("Verisoft.DemoApi.Core.Entities.UserEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("nvarchar(1024)");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Verisoft.DemoApi.Core.Entities.ClientEntity", b =>
                {
                    b.HasOne("Verisoft.DemoApi.Core.Entities.ClientEntity", "ParentClient")
                        .WithMany("Subsidiaries")
                        .HasForeignKey("ParentClientId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("ParentClient");
                });

            modelBuilder.Entity("Verisoft.DemoApi.Core.Entities.ContractEntity", b =>
                {
                    b.HasOne("Verisoft.DemoApi.Core.Entities.ClientEntity", "ClientEntity")
                        .WithMany("Contracts")
                        .HasForeignKey("ClientEntityId");

                    b.HasOne("Verisoft.DemoApi.Core.Entities.UserEntity", "SignedBy")
                        .WithMany()
                        .HasForeignKey("SignedById");

                    b.Navigation("ClientEntity");

                    b.Navigation("SignedBy");
                });

            modelBuilder.Entity("Verisoft.DemoApi.Core.Entities.DocumentEntity", b =>
                {
                    b.HasOne("Verisoft.DemoApi.Core.Entities.ClientEntity", "Client")
                        .WithMany("Documents")
                        .HasForeignKey("ClientId");

                    b.HasOne("Verisoft.DemoApi.Core.Entities.ContractEntity", "Contract")
                        .WithMany("Documents")
                        .HasForeignKey("ContractId");

                    b.HasOne("Verisoft.DemoApi.Core.Entities.UserEntity", "User")
                        .WithMany("Documents")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("Contract");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Verisoft.DemoApi.Core.Entities.ClientEntity", b =>
                {
                    b.Navigation("Contracts");

                    b.Navigation("Documents");

                    b.Navigation("Subsidiaries");
                });

            modelBuilder.Entity("Verisoft.DemoApi.Core.Entities.ContractEntity", b =>
                {
                    b.Navigation("Documents");
                });

            modelBuilder.Entity("Verisoft.DemoApi.Core.Entities.UserEntity", b =>
                {
                    b.Navigation("Documents");
                });
#pragma warning restore 612, 618
        }
    }
}