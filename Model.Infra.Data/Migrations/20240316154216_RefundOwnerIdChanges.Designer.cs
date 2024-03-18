﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Model.Infra.Data.Context;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Model.Infra.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240316154216_RefundOwnerIdChanges")]
    partial class RefundOwnerIdChanges
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Model.Domain.Entities.Category", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Model.Domain.Entities.Refund", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("CategoryId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("OwnerID")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<decimal>("Total")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Refunds");
                });

            modelBuilder.Entity("Model.Domain.Entities.RefundOperation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long?>("ApprovalRuleId")
                        .HasColumnType("bigint");

                    b.Property<string>("ApprovedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long?>("RefundId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ApprovalRuleId");

                    b.HasIndex("RefundId");

                    b.ToTable("RefundOperations");
                });

            modelBuilder.Entity("Model.Domain.Entities.Rule", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<bool>("Action")
                        .HasColumnType("boolean");

                    b.Property<long>("CategoryId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<decimal>("MaxValue")
                        .HasColumnType("numeric");

                    b.Property<decimal>("MinValue")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Rules");
                });

            modelBuilder.Entity("Model.Domain.Entities.Refund", b =>
                {
                    b.HasOne("Model.Domain.Entities.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Model.Domain.Entities.RefundOperation", b =>
                {
                    b.HasOne("Model.Domain.Entities.Rule", "ApprovalRule")
                        .WithMany()
                        .HasForeignKey("ApprovalRuleId");

                    b.HasOne("Model.Domain.Entities.Refund", null)
                        .WithMany("Operations")
                        .HasForeignKey("RefundId");

                    b.Navigation("ApprovalRule");
                });

            modelBuilder.Entity("Model.Domain.Entities.Rule", b =>
                {
                    b.HasOne("Model.Domain.Entities.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Model.Domain.Entities.Refund", b =>
                {
                    b.Navigation("Operations");
                });
#pragma warning restore 612, 618
        }
    }
}
