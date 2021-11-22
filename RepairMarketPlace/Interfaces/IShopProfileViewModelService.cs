using System;
using System.Threading.Tasks;
using Web.ViewModels;

namespace Web.Interfaces
{
    public interface IShopProfileViewModelService
    {
        public Task<ShopProfileViewModel> GetShop(Guid userId);
        public Task<ShopProfileViewModel> GetShop(int shopId);
        public Task UpdateShopProfileAsync(ShopProfileViewModel shopProfileViewModel);
    }
}
