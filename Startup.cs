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
        public void ConfigureServices(IServiceCollection services)
        {
            var builder = new ConfigurationBuilder()
                    .AddEnvironmentVariables();
            var config = builder.Build();
            var tmDbBaseUrl = Environment.GetEnvironmentVariable("AppSettings:TmDbSettings:BaseUrl");
            var tmDbBaseImagePath = Environment.GetEnvironmentVariable("AppSettings:TmDbSettings:BaseImagePath");
            var tmDbBaseYouTubePath = Environment.GetEnvironmentVariable("AppSettings:TmDbSettings:BaseYouTubePath");
            var tmDbLanguage = Environment.GetEnvironmentVariable("AppSettings:TmDbSettings:QueryOptions:Language");
            var tmDbAppendToResponse = Environment.GetEnvironmentVariable("AppSettings:TmDbSettings:QueryOptions:AppendToResponse");
            var tmDbPage = Environment.GetEnvironmentVariable("AppSettings:TmDbSettings:QueryOptions:Page");
            var tmDbApiKey = Environment.GetEnvironmentVariable("AppSettings:MovieProSettings:TmDbApiKey");
            var defaultBackdropSize = Environment.GetEnvironmentVariable("AppSettings:MovieProSettings:DefaultBackdropSize");
            var defaultPosterSize = Environment.GetEnvironmentVariable("AppSettings:MovieProSettings:DefaultPosterSize");
            var defaultYouTubeKey = Environment.GetEnvironmentVariable("AppSettings:MovieProSettings:DefaultYouTubeKey");
            var defaultCastImage = Environment.GetEnvironmentVariable("AppSettings:MovieProSettings:DefaultCastImage");
            var defaultCollectionName = Environment.GetEnvironmentVariable("AppSettings:MovieProSettings:DefaultCollection:Name");
            var defaultCollectionDescription = Environment.GetEnvironmentVariable("AppSettings:MovieProSettings:DefaultCollection:Description");
            var defaultCredentialsRole = Environment.GetEnvironmentVariable("AppSettings:MovieProSettings:DefaultCredentials:Role");
            var defaultCredentialsEmail = Environment.GetEnvironmentVariable("AppSettings:MovieProSettings:DefaultCredentials:Email");
            var defaultCredentialsPassword = Environment.GetEnvironmentVariable("AppSettings:MovieProSettings:DefaultCredentials:Password");


            services.AddHttpClient();
            services.AddSingleton<IConfiguration>(
                x => new ConfigurationBuilder()
                    .AddUserSecrets<Startup>()
                    .Build());
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(
                    ConnectionService.GetConnectionString(Configuration)));
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
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
