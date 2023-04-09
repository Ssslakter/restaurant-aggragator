﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using RestaurantAggregator.DAL;

#nullable disable

namespace RestaurantAggregator.DAL.Migrations
{
    [DbContext(typeof(RestaurantDbContext))]
    partial class RestaurantDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("RestaurantAggregator.Core.Data.DTO.DishDTO", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Category")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("IsVegeterian")
                        .HasColumnType("boolean");

                    b.Property<Guid>("MenuId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<Guid?>("OrderId")
                        .HasColumnType("uuid");

                    b.Property<string>("Photo")
                        .HasColumnType("text");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.ToTable("DishDTO");
                });

            modelBuilder.Entity("RestaurantAggregator.DAL.Data.Dish", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Category")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("IsVegeterian")
                        .HasColumnType("boolean");

                    b.Property<Guid>("MenuId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Photo")
                        .HasColumnType("text");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("MenuId");

                    b.ToTable("Dishes");
                });

            modelBuilder.Entity("RestaurantAggregator.DAL.Data.Menu", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<Guid>("RestaurantId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("RestaurantId");

                    b.ToTable("Menus");
                });

            modelBuilder.Entity("RestaurantAggregator.DAL.Data.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<Guid>("ClientId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CookId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CourierId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("DeliveryTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("OrderNumber")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("OrderNumber"));

                    b.Property<DateTime>("OrderTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasAlternateKey("ClientId", "OrderTime");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("RestaurantAggregator.DAL.Data.Restaurant", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Restaurants");
                });

            modelBuilder.Entity("RestaurantAggregator.DAL.Data.Review", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ClientId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("DishId")
                        .HasColumnType("uuid");

                    b.Property<long>("Value")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasAlternateKey("DishId", "ClientId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("RestaurantAggregator.Core.Data.DTO.DishDTO", b =>
                {
                    b.HasOne("RestaurantAggregator.DAL.Data.Order", null)
                        .WithMany("Dishes")
                        .HasForeignKey("OrderId");
                });

            modelBuilder.Entity("RestaurantAggregator.DAL.Data.Dish", b =>
                {
                    b.HasOne("RestaurantAggregator.DAL.Data.Menu", "Menu")
                        .WithMany("Dishes")
                        .HasForeignKey("MenuId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Menu");
                });

            modelBuilder.Entity("RestaurantAggregator.DAL.Data.Menu", b =>
                {
                    b.HasOne("RestaurantAggregator.DAL.Data.Restaurant", "Restaurant")
                        .WithMany("Menus")
                        .HasForeignKey("RestaurantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Restaurant");
                });

            modelBuilder.Entity("RestaurantAggregator.DAL.Data.Review", b =>
                {
                    b.HasOne("RestaurantAggregator.DAL.Data.Dish", null)
                        .WithMany("Reviews")
                        .HasForeignKey("DishId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RestaurantAggregator.DAL.Data.Dish", b =>
                {
                    b.Navigation("Reviews");
                });

            modelBuilder.Entity("RestaurantAggregator.DAL.Data.Menu", b =>
                {
                    b.Navigation("Dishes");
                });

            modelBuilder.Entity("RestaurantAggregator.DAL.Data.Order", b =>
                {
                    b.Navigation("Dishes");
                });

            modelBuilder.Entity("RestaurantAggregator.DAL.Data.Restaurant", b =>
                {
                    b.Navigation("Menus");
                });
#pragma warning restore 612, 618
        }
    }
}
