using RepairMarketPlace.ApplicationCore.Entities;
using RepairMarketPlace.ApplicationCore.Interfaces;
using System.Threading.Tasks;
using Web.Interfaces;
using Web.ViewModels;

namespace Web.Services
{
    public class ShopProfileViewModelService : IShopProfileViewModelService
    {
        private readonly IShopService _shopService;

        public ShopProfileViewModelService(IShopService shopRepository)
        {
            _shopService = shopRepository;
        }

        public async Task UpdateShopProfileAsync(ShopProfileViewModel viewModel)
        {
            Shop shop = await _shopService.GetShopAsync(viewModel.UserId);
            shop.UpdateShopProfile(viewModel.Name, viewModel.Address, viewModel.WebSite, viewModel.IsOpen);
            await _shopService.UpdateShopAsync(shop);
        }
    }
}
