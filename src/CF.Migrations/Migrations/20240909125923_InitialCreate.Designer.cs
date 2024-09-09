﻿// <auto-generated />
using System;
using CF.VirtualCard.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CF.Migrations.Migrations
{
    [DbContext(typeof(VirtualCardContext))]
    [Migration("20240909125923_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CF.VirtualCard.Domain.Entities.BillingCycle", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<decimal>("CurrentBalance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("From")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("To")
                        .HasColumnType("datetime2");

                    b.Property<long>("VirtualCardId")
                        .HasColumnType("bigint");

                    b.Property<int>("WithdrawalsCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("VirtualCardId")
                        .IsUnique();

                    b.ToTable("BillingCycle", (string)null);
                });

            modelBuilder.Entity("CF.VirtualCard.Domain.Entities.VirtualCard", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("VirtualCard", (string)null);
                });

            modelBuilder.Entity("CF.VirtualCard.Domain.Entities.BillingCycle", b =>
                {
                    b.HasOne("CF.VirtualCard.Domain.Entities.VirtualCard", null)
                        .WithOne("CurrentBillingCycle")
                        .HasForeignKey("CF.VirtualCard.Domain.Entities.BillingCycle", "VirtualCardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CF.VirtualCard.Domain.Entities.VirtualCard", b =>
                {
                    b.OwnsOne("CF.VirtualCard.Domain.Entities.CardNumber", "CardNumber", b1 =>
                        {
                            b1.Property<long>("VirtualCardId")
                                .HasColumnType("bigint");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(19)
                                .HasColumnType("nvarchar(19)")
                                .HasColumnName("CardNumber");

                            b1.HasKey("VirtualCardId");

                            b1.HasIndex("Value");

                            b1.ToTable("VirtualCard");

                            b1.WithOwner()
                                .HasForeignKey("VirtualCardId");
                        });

                    b.Navigation("CardNumber");
                });

            modelBuilder.Entity("CF.VirtualCard.Domain.Entities.VirtualCard", b =>
                {
                    b.Navigation("CurrentBillingCycle");
                });
#pragma warning restore 612, 618
        }
    }
}
