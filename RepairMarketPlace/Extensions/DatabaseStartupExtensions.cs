using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RepairMarketPlace;
using RepairMarketPlace.Infrastructure.Data;
using RepairMarketPlace.Infrastructure.Extensions;
using RepairMarketPlace.Infrastructure.Identity;
using System;
using System.Threading.Tasks;

namespace Web.Extensions
{
    public static class DatabaseStartupExtensions
    {
        public static async Task<IHost> SetupDatabaseAsync(this IHost webHost)
        {
            using (var scope = webHost.Services.CreateScope())
            {
                IServiceProvider services = scope.ServiceProvider;
                IWebHostEnvironment env = services.GetRequiredService<IWebHostEnvironment>();
                IConfiguration config = services.GetRequiredService<IConfiguration>();
                AppDbContext context = services.GetRequiredService<AppDbContext>();
                UserManager<User> userManager = services.GetRequiredService<UserManager<User>>();
                try
                {
                    if (!await context.ComponentTypes.AnyAsync())
                    {
                        await context.SeedDatabaseIfNoComponentTypeAsync(@"..\Infrastructure\Data\SeedData");
                        await context.SeedDatabaseIfNoComponentsAsync(@"..\Infrastructure\Data\SeedData");
                    }

                    if (!await context.AppUsers.AnyAsync(user => user.Email == config["AdminCredentials:Login"]))
                    {
                        await userManager.SeedAdminUser(config);
                    }
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while creating/migrating or seeding the database.");

                    throw;
                }
            }

            return webHost;
        }
    }
}
