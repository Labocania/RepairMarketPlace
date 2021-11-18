using Ardalis.GuardClauses;
using RepairMarketPlace.ApplicationCore.Entities;
using RepairMarketPlace.ApplicationCore.Exceptions;
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
            if (await GetShopAsync(userId) != null) throw new SingleShopException(email);
            await _shopRepository.AddAsync(new Shop(userId, name, address, email, phoneNumber, webSite));
            await _shopRepository.SaveChangesAsync();
        }

        public async Task<Shop> GetShopAsync(Guid userId)
        {
           return await _shopReadRepository.GetBySpecAsync(new ShopByIdSpec(userId));
        }

        public async Task<Shop> GetShopAsync(int shopId)
        {
            return await _shopReadRepository.GetByIdAsync(shopId);
        }

        public async Task UpdateShopAsync(Shop shop)
        {
            await _shopRepository.UpdateAsync(shop);
        }

        public async Task AddServiceToShop(int shopId, string name, string description)
        {
            Shop shop = await GetShopAsync(shopId);
            Guard.Against.Null<Shop>(shop, nameof(shop));
            shop.AddServiceType(name, description);
            await UpdateShopAsync(shop);
        }
    }
}
