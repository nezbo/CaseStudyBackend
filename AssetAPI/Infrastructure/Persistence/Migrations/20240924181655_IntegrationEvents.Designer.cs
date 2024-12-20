﻿// <auto-generated />
using System;
using AssetAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AssetAPI.Persistence.Migrations
{
    [DbContext(typeof(ApiDbContext))]
    [Migration("20240924181655_IntegrationEvents")]
    partial class IntegrationEvents
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("AssetAPI.Domain.Models.Asset", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.Property<DateOnly?>("ValidFrom")
                        .HasColumnType("TEXT");

                    b.Property<DateOnly?>("ValidTo")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Assets");
                });

            modelBuilder.Entity("Microservice.Common.Domain.Events.IntegrationEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("TraceId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Outbox");
                });
#pragma warning restore 612, 618
        }
    }
}
