using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ThAmCo.Catering.Data
{

    public class CateringDbContext : DbContext
    {

        public DbSet<Food> Food { get; set; }

        public DbSet<Menu> Menus { get; set; }

        public DbSet<MenuFood> MenuFood { get; set; }

        private readonly IHostingEnvironment _hostEnv;

        public CateringDbContext(DbContextOptions<CateringDbContext> options,
                                 IHostingEnvironment env) : base(options)
        {
            _hostEnv = env;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema("thamco.catering");

            builder.Entity<MenuFood>()
                .HasKey(x => new { x.FoodId, x.MenuId });

            builder.Entity<Food>()
                .HasKey(s => s.Id);

            builder.Entity<Menu>()
                .HasKey(m => m.Id);

            builder.Entity<Menu>()
                .HasMany(m => m.Food);

            builder.Entity<MenuFood>()
                .HasOne(mf => mf.Menu)
                .WithMany(m => m.Food)
                .HasForeignKey(mf => mf.MenuId);

            builder.Entity<MenuFood>()
                .HasOne(mf => mf.Food)
                .WithMany(f => f.Menus)
                .HasForeignKey(mf => mf.FoodId);

            if (_hostEnv != null && _hostEnv.IsDevelopment())
            {
                builder.Entity<Food>()
                    .HasData(
                        new Food { Id = 1, Cost = 1.23f, Name = "Prawn Cocktail" },
                        new Food { Id = 2, Cost = 4.23f, Name = "Carbonara Basile" },
                        new Food { Id = 3, Cost = 5.32f, Name = "Rabbit Haunch" },
                        new Food { Id = 4, Cost = 6.22f, Name = "Steak & Kidney Pie" }
                    );

                builder.Entity<Menu>()
                    .HasData(
                        new Menu { Id = 1, Name = "Spring Formality" }
                    );

                builder.Entity<MenuFood>()
                    .HasData(
                        new MenuFood { FoodId = 1, MenuId = 1 },
                        new MenuFood { FoodId = 2, MenuId = 1 },
                        new MenuFood { FoodId = 3, MenuId = 1 },
                        new MenuFood { FoodId = 4, MenuId = 1 }
                    );
            }
        }
    }

}