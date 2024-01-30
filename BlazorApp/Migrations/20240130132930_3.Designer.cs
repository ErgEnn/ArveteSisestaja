﻿// <auto-generated />
using System;
using BlazorApp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BlazorApp.Migrations
{
    [DbContext(typeof(DbContext))]
    [Migration("20240130132930_3")]
    partial class _3
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.1");

            modelBuilder.Entity("BlazorApp.AncClassifier", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("UnitCoefficient")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("AncClassifiers");
                });

            modelBuilder.Entity("BlazorApp.AncClassifierMapping", b =>
                {
                    b.Property<int?>("AncClassifierId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ProductName")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("Multiplier")
                        .HasColumnType("TEXT");

                    b.HasKey("AncClassifierId", "ProductName");

                    b.ToTable("AncClassifierMappings");
                });

            modelBuilder.Entity("InvoiceDownloader.Invoice", b =>
                {
                    b.Property<string>("InvoiceNo")
                        .HasColumnType("TEXT");

                    b.Property<string>("InvoiceSender")
                        .HasColumnType("TEXT");

                    b.Property<DateOnly>("InvoiceDateTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("PdfSrc")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("XML")
                        .HasColumnType("TEXT");

                    b.HasKey("InvoiceNo", "InvoiceSender", "InvoiceDateTime");

                    b.ToTable("Invoices");
                });
#pragma warning restore 612, 618
        }
    }
}
