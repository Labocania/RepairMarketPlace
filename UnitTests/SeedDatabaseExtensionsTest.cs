using Xunit;
using TestSupport.Helpers;
using System.IO;
using RepairMarketPlace.Infrastructure.Data;
using RepairMarketPlace.Infrastructure.Extensions;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UnitTests.Extensions;
using RepairMarketPlace.ApplicationCore.Entities;

namespace UnitTests
{
    public class SeedDatabaseExtensionsTest
    {
        [Fact]
        public async Task SeedDatabaseIfNoComponentsHappyPath()
        {
            DbContextOptions<AppDbContext> options = DbContextExtensions.CreateUniqueClassOptions<AppDbContext>(this);

            using (AppDbContext context = new AppDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                string callingAssemblyPath = TestData.GetCallingAssemblyTopLevelDir();
                var dataDir = Path.GetFullPath(Path.Combine(callingAssemblyPath, @"..\Infrastructure\Data\SeedData"));

                await context.SeedDatabaseIfNoComponentsAsync(dataDir);
                int count = await context.Components.CountAsync();
                IQueryable<Component> query = context.Components.AsNoTracking();
                bool nameQuery = await query.Where(x => x.Name == null).AnyAsync();
                bool typeQuery = await query.Where(x => x.Type == ComponentType.CPU).AnyAsync();
                Component component = query.Where(x => x.Name == "Kingston HyperX Cloud Alpha S" && x.Type == ComponentType.Headphones).FirstOrDefault();

                Assert.False(nameQuery);
                Assert.True(typeQuery);
                Assert.NotNull(component);
            }
        }

        [Fact]
        public async Task SeedDatabaseIfNoComponentsEdgePath()
        {
            DbContextOptions<AppDbContext> options = DbContextExtensions.CreateUniqueClassOptions<AppDbContext>(this);

            using (AppDbContext context = new AppDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                string callingAssemblyPath = TestData.GetCallingAssemblyTopLevelDir();
                var dataDir = Path.GetFullPath(Path.Combine(callingAssemblyPath, @"..\Infrastructure\Data\SeedData"));

                await context.SeedDatabaseIfNoComponentsAsync(dataDir);
                int count = await context.Components.CountAsync();
                Assert.True(count > 0);

                await context.SeedDatabaseIfNoComponentsAsync(dataDir);
                int secondCount = await context.Components.CountAsync();
                Assert.Equal(count, secondCount);
            }
        }
    }
}
