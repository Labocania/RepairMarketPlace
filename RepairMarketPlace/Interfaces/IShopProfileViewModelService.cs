using System.Threading.Tasks;
using Web.ViewModels;

namespace Web.Interfaces
{
    public interface IShopProfileViewModelService
    {
        public Task UpdateShopProfileAsync(ShopProfileViewModel shopProfileViewModel);
    }
}
