﻿// <auto-generated />
using System;
using InvoiceAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace InvoiceAPI.Persistence.Migrations
{
    [DbContext(typeof(InvoiceDbContext))]
    [Migration("20240923152239_ServiceAssetId")]
    partial class ServiceAssetId
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("InvoiceAPI.Domain.Models.Invoice", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateOnly>("IssuingDate")
                        .HasColumnType("TEXT");

                    b.Property<ushort>("Month")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Total")
                        .HasColumnType("TEXT");

                    b.Property<ushort>("Year")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Invoices");
                });

            modelBuilder.Entity("InvoiceAPI.Domain.Models.Invoice", b =>
                {
                    b.OwnsMany("InvoiceAPI.Domain.Models.Service", "_services", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("TEXT");

                            b1.Property<Guid>("AssetId")
                                .HasColumnType("TEXT");

                            b1.Property<Guid>("InvoiceId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasColumnType("TEXT");

                            b1.Property<decimal>("Price")
                                .HasColumnType("TEXT");

                            b1.Property<DateOnly?>("ValidFrom")
                                .HasColumnType("TEXT");

                            b1.Property<DateOnly?>("ValidTo")
                                .HasColumnType("TEXT");

                            b1.HasKey("Id");

                            b1.HasIndex("InvoiceId");

                            b1.ToTable("Services");

                            b1.WithOwner()
                                .HasForeignKey("InvoiceId");
                        });

                    b.Navigation("_services");
                });
#pragma warning restore 612, 618
        }
    }
}