using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RepairMarketPlace.Infrastructure.Identity;
using Web.Interfaces;
using Web.ViewModels;

namespace Web.Areas.Identity.Pages.Account.Manage
{
    [Authorize("IsShopOwner")]
    public class ShopModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IShopProfileViewModelService _viewModelService;

        public ShopModel(UserManager<User> userManager, IShopProfileViewModelService viewModelService)
        {
            _userManager = userManager;
            _viewModelService = viewModelService;
        }

        [BindProperty]
        public ShopProfileViewModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        private async Task LoadAsync(User user)
        {
            Guid userId = Guid.Parse(await _userManager.GetUserIdAsync(user));
            Input = await _viewModelService.GetShop(userId);
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

            await _viewModelService.UpdateShopProfileAsync(Input);
            StatusMessage = "Your shop has been updated";
            return RedirectToPage();
        }
    }
}
