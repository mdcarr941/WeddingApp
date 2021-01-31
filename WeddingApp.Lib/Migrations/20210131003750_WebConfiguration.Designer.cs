﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WeddingApp.Lib.Data;

namespace WeddingApp.Lib.Migrations
{
    [DbContext(typeof(WeddingDbContext))]
    [Migration("20210131003750_WebConfiguration")]
    partial class WebConfiguration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("WeddingApp.Lib.Data.Rsvp", b =>
                {
                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Email");

                    b.ToTable("Rsvps");
                });

            modelBuilder.Entity("WeddingApp.Lib.Data.WebConfiguration", b =>
                {
                    b.Property<short>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("RsvpPassword")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("WebConfiguration");
                });
#pragma warning restore 612, 618
        }
    }
}
