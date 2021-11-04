using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RepairMarketPlace;
using RepairMarketPlace.Infrastructure.Data;
using RepairMarketPlace.Infrastructure.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Extensions
{
    public static class DatabaseStartupExtensions
    {
        public static async Task<IHost> SetupDatabaseAsync(this IHost webHost)
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var env = services.GetRequiredService<IWebHostEnvironment>();
                var context = services.GetRequiredService<AppDbContext>();
                try
                {
                    if (!context.Components.Any())
                    {
                        await context.SeedDatabaseIfNoComponentsAsync(@"..\Infrastructure\Data\SeedData");
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
