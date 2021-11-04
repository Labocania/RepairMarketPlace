using Xunit;
using System.Collections.Generic;
using TestSupport.Helpers;
using System.IO;
using TestSupport.EfHelpers;
using RepairMarketPlace.Infrastructure.Data;
using RepairMarketPlace.Infrastructure.Extensions;
using System.Threading.Tasks;
using System.Linq;

namespace UnitTests
{
    public class SeedDatabaseExtensionsTest
    {
        [Fact]
        public async Task SeedDatabaseIfNoComponentsHappyPath()
        {
            Microsoft.EntityFrameworkCore.DbContextOptions<AppDbContext> options = this.CreateUniqueClassOptions<AppDbContext>();
            using (AppDbContext context = new AppDbContext(options))
            {
                context.Database.EnsureClean();

                string callingAssemblyPath = TestData.GetCallingAssemblyTopLevelDir();
                var dataDir = Path.GetFullPath(Path.Combine(callingAssemblyPath, "..\\Infrastructure\\SeedData"));

                await context.SeedDatabaseIfNoComponentsAsync(dataDir);

                Assert.True(context.Components.Any());
            }

        }
    }
}
