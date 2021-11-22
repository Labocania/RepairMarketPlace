using RepairMarketPlace.ApplicationCore.Entities;
using RepairMarketPlace.ApplicationCore.Services;
using System.Threading.Tasks;
using Xunit;
using System;
using RepairMarketPlace.ApplicationCore.Exceptions;
using TestSupport.EfHelpers;
using RepairMarketPlace.Infrastructure.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace UnitTests.AppicationCore.Services
{
    public class ShopServiceTests
    {
        private EfRepository<Shop> _shopRepository;
        private ShopService _shopService;
        private AppDbContext _context;
        private const string userId = "382c74c3-721d-4f34-80e5-57657b6cbc27";
        private const string name = "Name Test";
        private const string address = "Test street 202";
        private const string email = "name@email.com";
        private const string phone = "+552133937895";
        private const string website = "https://www.namewebsite.com";

        public ShopServiceTests()
        {
            DbContextOptionsDisposable<AppDbContext> options = SqliteInMemory.CreateOptions<AppDbContext>();
            _context = new AppDbContext(options);
            _shopRepository = new EfRepository<Shop>(_context);
            _shopService = new ShopService(_shopRepository, _shopRepository);
        }

        [Fact]
        public async Task CreateShopAsyncGoodArguments()
        {            
            using (_context)
            {
                await _context.Database.EnsureDeletedAsync();
                await _context.Database.EnsureCreatedAsync();

                await _shopService.CreateShopAsync(Guid.Parse(userId), name, address, email, phone, website);

                int count = await _context.Shops.CountAsync();
                Assert.Equal(1, count);
                Shop testShop = _context.Shops.Where(shop => shop.UserId == Guid.Parse(userId)).FirstOrDefault();
                Assert.NotNull(testShop);

                await Assert.ThrowsAsync<SingleShopException>(async () => await _shopService.CreateShopAsync(Guid.Parse(userId), "Name Test2", "Test street 2023", "name@email.com", "+552133937895", "https://www.namewebsite.com"));
            }            
        }

        [Theory]
        [InlineData(null, name, address, email, phone, website)]
        [InlineData(userId, null, address, email, phone, website)]
        public async Task CreateShopAsyncNullRequiredArguments(string userId, string name, string address, string email, string phoneNumber, string webSite)
        {            
            using (_context)
            {
                await _context.Database.EnsureDeletedAsync();
                await _context.Database.EnsureCreatedAsync();

                await Assert.ThrowsAnyAsync<Exception>(async () => await _shopService.CreateShopAsync(Guid.Parse(userId), name, address, email, phoneNumber, webSite));               
            }
        }

        [Fact]
        public async Task AddServiceToShopGoodArguments()
        {
            using (_context)
            {
                await _context.Database.EnsureDeletedAsync();
                await _context.Database.EnsureCreatedAsync();

                await _shopService.CreateShopAsync(Guid.Parse(userId), name, address, email, phone, website);
                Assert.True(await _context.Shops.AnyAsync());
                Shop shop = await _shopService.GetShopAsync(Guid.Parse(userId));
                Assert.Empty(shop.ServiceTypes);
                Assert.NotNull(shop);
                await _shopService.AddServiceToShop(shop.Id, "Screen Repair", "Replacing cracked and scratched phone screens.");
                int count = _context.ServiceTypes.Count();
                Assert.Equal(1, count);
                ServiceType serviceType = _context.ServiceTypes.FirstOrDefault();
                Assert.NotNull(serviceType);
                Assert.Equal(shop.Id, serviceType.ShopId);
                shop = _context.Shops.Include(shop => shop.ServiceTypes).FirstOrDefault();
                Assert.NotNull(shop);
                Assert.NotEmpty(shop.ServiceTypes);
                Assert.Equal(1, shop.ServiceTypes.Count);
                Assert.Equal("Screen Repair", shop.ServiceTypes.FirstOrDefault().Name);
            }
        }

        [Theory]
        [InlineData(2, "Keyboard board repair")]
        [InlineData(1, null)]
        public async Task AddServiceToShopBadArguments(int shopId, string serviceName)
        {
            using (_context)
            {
                await _context.Database.EnsureDeletedAsync();
                await _context.Database.EnsureCreatedAsync();

                await _shopService.CreateShopAsync(Guid.Parse(userId), name, address, email, phone, website);
                Shop shop = await _shopService.GetShopAsync(Guid.Parse(userId));

                await Assert.ThrowsAsync<ArgumentNullException>(async () => await _shopService.AddServiceToShop(shopId, serviceName, "Repair keyboard main board and internal components")); ;
            }
        }
    }
}
