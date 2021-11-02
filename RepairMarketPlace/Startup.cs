using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RepairMarketPlace.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepairMarketPlace
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbProvider(Environment, Configuration);

            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AppDbContext>();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
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
                endpoints.MapRazorPages();
            });
        }
    }

    public static class AddDbProviderExtensions
    {
        // Reference: https://github.com/jincod/dotnetcore-buildpack/issues/33#issuecomment-409935057
        public static IServiceCollection AddDbProvider(this IServiceCollection services, IWebHostEnvironment env, IConfiguration config)
        {
            string connStr = "";
            if (env.EnvironmentName == "Development")
            {
                connStr = config.GetConnectionString("DefaultConnection");
            }

            if (env.EnvironmentName == "Production")
            {
                string connUrl = config.GetSection("DATABASE_URL").Value;

                // Parse connection URL to connection string for Npgsql
                connUrl = connUrl.Replace("postgres://", string.Empty);

                string pgUserPass = connUrl.Split("@")[0];
                string pgHostPortDb = connUrl.Split("@")[1];
                string pgHostPort = pgHostPortDb.Split("/")[0];

                string pgDb = pgHostPortDb.Split("/")[1];
                string pgUser = pgUserPass.Split(":")[0];
                string pgPass = pgUserPass.Split(":")[1];
                string pgHost = pgHostPort.Split(":")[0];
                string pgPort = pgHostPort.Split(":")[1];

                connStr = $"Server={pgHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb};sslmode=Prefer;Trust Server Certificate=true";
            }

            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connStr));

            //services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connStr,
                //o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));
            return services;
        }
    }
}
