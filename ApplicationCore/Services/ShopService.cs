using Ardalis.GuardClauses;
using RepairMarketPlace.ApplicationCore.Entities;
using RepairMarketPlace.ApplicationCore.Interfaces;
using RepairMarketPlace.ApplicationCore.Interfaces.Repository;
using RepairMarketPlace.ApplicationCore.Specifications;
using System;
using System.Threading.Tasks;

namespace RepairMarketPlace.ApplicationCore.Services
{
    public class ShopService : IShopService
    {
        private readonly IRepository<Shop> _shopRepository;
        private readonly IReadRepository<Shop> _shopReadRepository;

        public ShopService(IRepository<Shop> shopRepository, IReadRepository<Shop> shopReadRepository)
        {
            _shopRepository = shopRepository;
            _shopReadRepository = shopReadRepository;
        }

        public async Task CreateShopAsync(Guid userId, string name, string address, string email, string phoneNumber, string webSite)
        {
            await _shopRepository.AddAsync(new Shop(userId, name, address, email, phoneNumber, webSite));
            await _shopRepository.SaveChangesAsync();
        }

        public async Task<Shop> GetShopAsync(Guid userId)
        {
           return await _shopReadRepository.GetBySpecAsync(new ShopByIdSpec(userId));
        }

        public async Task UpdateShopAsync(Shop shop)
        {
            Shop shopUpdate = await GetShopAsync(shop.UserId);
            shopUpdate.Name = shop.Name;
            shopUpdate.Address = shop.Address;
            shopUpdate.Email = shop.Email;
            shopUpdate.PhoneNumber = shop.PhoneNumber;
            shopUpdate.WebSite = shop.WebSite;
            shop.IsOpen = shop.IsOpen;
            await _shopRepository.AddAsync(shopUpdate);
        }
    }
}
