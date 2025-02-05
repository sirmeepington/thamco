﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ThAmCo.CateringFacade.Food;
using ThAmCo.CateringFacade.Menu;
using ThAmCo.CateringFacade.MenuFood;
using ThAmCo.Events.Data;
using ThAmCo.VenuesFacade;
using ThAmCo.VenuesFacade.EventTypes;

namespace ThAmCo.Events
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbContext<EventsDbContext>(options =>
            {
                var cs = Configuration.GetConnectionString("EventsSqlConnection");
                options.UseSqlServer(cs);
            });
            services.AddScoped<IVenueAvailabilities, VenueAvailabilities>();
            services.AddScoped<IVenueReservation, VenueReservation>();
            services.AddScoped<IMenuManagement, MenuManagement>();
            services.AddScoped<IMenuFoodManagement, MenuFoodManagement>();
            services.AddScoped<IFoodManagement, FoodManagement>();
            services.AddScoped<IEventTypes, EventTypes>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
