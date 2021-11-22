using Ardalis.GuardClauses;
using RepairMarketPlace.ApplicationCore.Entities;
using RepairMarketPlace.ApplicationCore.Interfaces.Repository;
using RepairMarketPlace.ApplicationCore.Specifications;
using System;
using System.Threading.Tasks;
using Web.Interfaces;
using Web.ViewModels;

namespace Web.Services
{
    public class ShopProfileViewModelService : IShopProfileViewModelService
    {
        private readonly IRepository<Shop> _shopRepository;
        private readonly IReadRepository<Shop> _shopReadRepository;

        public ShopProfileViewModelService(IRepository<Shop> shopRepository, IReadRepository<Shop> shopReadRepository)
        {
            _shopRepository = shopRepository;
            _shopReadRepository = shopReadRepository;
        }

        public async Task<ShopProfileViewModel> GetShop(Guid userId)
        {
            return CreateShopProfileViewModel(await _shopReadRepository.GetBySpecAsync(new ShopByIdSpec(userId)));
        }

        public async Task<ShopProfileViewModel> GetShop(int shopId)
        {
            return CreateShopProfileViewModel(await _shopReadRepository.GetByIdAsync(shopId));
        }

        private ShopProfileViewModel CreateShopProfileViewModel(Shop shop)
        {
            Guard.Against.Null(shop, "Requested Shop", "Requested Shop doesn't exists.");

            return new ShopProfileViewModel()
            {
                UserId = shop.UserId,
                Name = shop.Name,
                Address = shop.Address,
                WebSite = shop.WebSite,
                IsOpen = shop.IsOpen
            };
        }

        public async Task UpdateShopProfileAsync(ShopProfileViewModel viewModel)
        {
            Shop shop = await _shopReadRepository.GetBySpecAsync(new ShopByIdSpec(viewModel.UserId));
            Guard.Against.Null(shop, "Requested Shop", "Requested Shop doesn't exists.");

            shop.UpdateShopProfile(viewModel.Name, viewModel.Address, viewModel.WebSite, viewModel.IsOpen);
            await _shopRepository.UpdateAsync(shop);
        }
    }
}
