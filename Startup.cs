using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MoviePro.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration.UserSecrets;
using MoviePro.Models.Settings;
using MoviePro.Services;
using MoviePro.Services.Interfaces;


namespace MoviePro
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        //    public void ConfigureServices(IServiceCollection services)
        //    {
        //        services.AddHttpClient();
        //        services.AddDbContext<ApplicationDbContext>(options =>
        //            options.UseNpgsql(
        //                ConnectionService.GetConnectionString(Configuration)));
        //        services.AddSingleton<IConfiguration>(
        //x => new ConfigurationBuilder()
        //    .AddUserSecrets<Startup>()
        //    .Build());
        ////        services.AddDbContext<ApplicationDbContext>(options =>
        ////options.UseNpgsql(
        ////    Environment.GetEnvironmentVariable("CONNECTION_STRING")));
        //        services.AddOptions<AppSettings>()
        //                .BindConfiguration("AppSettings");
        //        services.Configure<MovieProSettings>(
        //            Configuration.GetSection("AppSettings:MovieProSettings"));
        //        services.Configure<TMDBSettings>(
        //            Configuration.GetSection("AppSettings:TMDBSettings"));
        //        services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
        //        services.AddDatabaseDeveloperPageExceptionFilter();
        //        services.AddIdentity<IdentityUser, IdentityRole>()
        //            .AddEntityFrameworkStores<ApplicationDbContext>();
        //        services.AddTransient<SeedService>();
        //        services.AddTransient<IDataMappingService, TMDBMappingService>();
        //        services.AddTransient<IRemoteMovieService, TMDBMovieService>();
        //        services.AddScoped<IImageService, BasicImageService>();
        //        services.AddRazorPages();
        //        services.AddControllersWithViews();
        //    }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(
                    ConnectionService.GetConnectionString(Configuration)));
            services.AddSingleton<IConfiguration>(
                x => new ConfigurationBuilder()
                    .AddEnvironmentVariables()
                    .Build());
            services.AddOptions<AppSettings>()
                    .BindConfiguration("AppSettings");
            services.Configure<MovieProSettings>(
                Configuration.GetSection("AppSettings:MovieProSettings"));
            services.Configure<TMDBSettings>(
                Configuration.GetSection("AppSettings:TMDBSettings"));
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddTransient<SeedService>();
            services.AddTransient<IDataMappingService, TMDBMappingService>();
            services.AddTransient<IRemoteMovieService, TMDBMovieService>();
            services.AddScoped<IImageService, BasicImageService>();
            services.AddRazorPages();
            services.AddControllersWithViews();
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //var configuration = BuildServiceProvider().GetService<IConfiguration>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
