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
    [Migration("20240128220137_1")]
    partial class _1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.1");

            modelBuilder.Entity("InvoiceDownloader.Invoice", b =>
                {
                    b.Property<string>("InvoiceNo")
                        .HasColumnType("TEXT");

                    b.Property<string>("InvoiceSender")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("InvoiceDateTime")
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
