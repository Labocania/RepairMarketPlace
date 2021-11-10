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
        public async Task SeedDatabaseIfNoComponentTypeHappyPath()
        {
            DbContextOptions<AppDbContext> options = DbContextExtensions.CreateUniqueClassOptions<AppDbContext>(this);
            using (AppDbContext context = new AppDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                string callingAssemblyPath = TestData.GetCallingAssemblyTopLevelDir();
                string dataDir = Path.GetFullPath(Path.Combine(callingAssemblyPath, @"..\Infrastructure\Data\SeedData"));
                int idealCount = 25;
                System.Random random = new();

                await context.SeedDatabaseIfNoComponentTypeAsync(dataDir);

                IQueryable<ComponentType> query = context.ComponentTypes.AsNoTracking();

                int queryCount = await query.CountAsync();
                Assert.Equal(idealCount, queryCount);

 
                int randomId = random.Next(1, queryCount);
                ComponentType componentType = await query.FirstAsync(x => x.Id == randomId);
                Assert.NotNull(componentType);
                Assert.NotNull(componentType.Name);
                Assert.False(componentType.Name == $"{componentType.Name}.json");
            }
        }

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

                await context.SeedDatabaseIfNoComponentTypeAsync(dataDir);
                await context.SeedDatabaseIfNoComponentsAsync(dataDir);

                IQueryable<Component> componentQuery = context.Components.AsNoTracking().Include(x => x.Type);
                bool nullNameQuery = await componentQuery.Where(x => x.Name == null).AnyAsync();
                bool nullRelationshipQuery = await componentQuery.Where(x => x.Type == null).AnyAsync();
                Component component = componentQuery.Where(x => x.Name == "Kingston HyperX Cloud Alpha S").FirstOrDefault();

                Assert.False(nullNameQuery);
                Assert.False(nullRelationshipQuery);
                Assert.NotNull(component);
                Assert.NotNull(component.Type);
                Assert.Equal("Headphones", component.Type.Name);

                IQueryable<ComponentType> componentTypeQuery = context.ComponentTypes.AsNoTracking().Include(x => x.Components);
                nullNameQuery = await componentTypeQuery.Where(x => x.Name == null).AnyAsync();
                nullRelationshipQuery = await componentTypeQuery.Where(x => x.Components == null).AnyAsync();
                ComponentType componentType = await componentTypeQuery.FirstOrDefaultAsync();

                Assert.False(nullNameQuery);
                Assert.False(nullRelationshipQuery);
                Assert.NotNull(componentType);
            }
        }
    }
}
