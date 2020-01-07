﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ThAmCo.Catering.Data;

namespace ThAmCo.Catering.Migrations
{
    [DbContext(typeof(CateringDbContext))]
    [Migration("20200106073029_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("thamco.catering")
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ThAmCo.Catering.Data.Food", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<float>("Cost");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Food");

                    b.HasData(
                        new { Id = 1, Cost = 1.23f, Name = "Prawn Cocktail" },
                        new { Id = 2, Cost = 4.23f, Name = "Carbonara Basile" },
                        new { Id = 3, Cost = 5.32f, Name = "Rabbit Haunch" },
                        new { Id = 4, Cost = 6.22f, Name = "Steak & Kidney Pie" }
                    );
                });

            modelBuilder.Entity("ThAmCo.Catering.Data.Menu", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Menus");

                    b.HasData(
                        new { Id = 1, Name = "Spring Formality" }
                    );
                });

            modelBuilder.Entity("ThAmCo.Catering.Data.MenuFood", b =>
                {
                    b.Property<int>("FoodId");

                    b.Property<int>("MenuId");

                    b.HasKey("FoodId", "MenuId");

                    b.HasIndex("MenuId");

                    b.ToTable("MenuFood");

                    b.HasData(
                        new { FoodId = 1, MenuId = 1 },
                        new { FoodId = 2, MenuId = 1 },
                        new { FoodId = 3, MenuId = 1 },
                        new { FoodId = 4, MenuId = 1 }
                    );
                });

            modelBuilder.Entity("ThAmCo.Catering.Data.MenuFood", b =>
                {
                    b.HasOne("ThAmCo.Catering.Data.Menu")
                        .WithMany("Food")
                        .HasForeignKey("MenuId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
