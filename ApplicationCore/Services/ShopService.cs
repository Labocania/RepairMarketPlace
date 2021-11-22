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
        }

        public async Task<Shop> GetShopAsync(Guid userId)
        {
           return await _shopReadRepository.GetBySpecAsync(new ShopByIdSpec(userId), default);
        }

        public async Task<Shop> GetShopAsync(int shopId)
        {
            return await _shopReadRepository.GetByIdAsync(shopId, default);
        }

        public async Task AddServiceToShop(int shopId, string name, string description)
        {           
            Shop shop = await GetShopAsync(shopId);

            Guard.Against.Null<Shop>(shop, nameof(shop), "Must be an existing shop.");
            Guard.Against.NullOrEmpty(name, nameof(name), "Name of the service must be provided");

            shop.AddServiceType(name, description);
            await _shopRepository.UpdateAsync(shop);
        }
    }
}
