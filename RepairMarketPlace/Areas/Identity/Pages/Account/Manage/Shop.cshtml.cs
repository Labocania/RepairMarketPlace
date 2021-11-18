using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RepairMarketPlace.ApplicationCore.Interfaces;
using RepairMarketPlace.Infrastructure.Identity;
using Web.Interfaces;
using Web.ViewModels;

namespace Web.Areas.Identity.Pages.Account.Manage
{
    [Authorize("IsShopOwner")]
    public class ShopModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IShopProfileViewModelService _shopProfileViewModelService;
        private readonly IShopService _shopService;

        public ShopModel(UserManager<User> userManager, IShopService shopService, 
                        IShopProfileViewModelService shopProfileViewModelService)
        {
            _userManager = userManager;
            _shopService = shopService;
            _shopProfileViewModelService = shopProfileViewModelService;
        }

        [BindProperty]
        public ShopProfileViewModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        private async Task LoadAsync(User user)
        {
            Guid userId = Guid.Parse(await _userManager.GetUserIdAsync(user));
            RepairMarketPlace.ApplicationCore.Entities.Shop shop = await _shopService.GetShopAsync(userId);

            Input = new ShopProfileViewModel
            {
                UserId = shop.UserId,
                Name = shop.Name,
                Address = shop.Address,
                WebSite = shop.WebSite,
                IsOpen = shop.IsOpen
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            Input.UserId = Guid.Parse(await _userManager.GetUserIdAsync(user));

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            await _shopProfileViewModelService.UpdateShopProfileAsync(Input);
            StatusMessage = "Your shop has been updated";
            return RedirectToPage();
        }
    }
}
