using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RepairMarketPlace.ApplicationCore.Interfaces;
using RepairMarketPlace.Infrastructure.Identity;

namespace Web.Areas.Identity.Pages.Account.Manage
{
    public class ShopModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IShopService _shopService;

        public ShopModel(UserManager<User> userManager, SignInManager<User> signInManager, IShopService shopService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _shopService = shopService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Shop's Name")]
            [MaxLength(200)]
            public string Name { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Shop's Address")]
            public string Address { get; set; }

            [Url]
            [DataType(DataType.Url)]
            [Display(Name = "Shop's Website")]
            public string WebSite { get; set; }

            [Display(Name = "Shop's Status")]
            public bool IsOpen { get; set; }
        }

        private async Task LoadAsync(User user)
        {
            Guid userId = Guid.Parse(await _userManager.GetUserIdAsync(user));
            RepairMarketPlace.ApplicationCore.Entities.Shop shop = await _shopService.GetShopAsync(userId);

            Input = new InputModel
            {
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
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            Guid userId = Guid.Parse(await _userManager.GetUserIdAsync(user));
            RepairMarketPlace.ApplicationCore.Entities.Shop shop = await _shopService.GetShopAsync(userId);

            if (Input.Name != shop.Name)
            {
                shop.Name = Input.Name;
            }

            if (Input.Address != shop.Address)
            {
                shop.Address = Input.Address;
            }

            if (Input.WebSite != shop.WebSite)
            {
                shop.WebSite = Input.WebSite;
            }

            if (Input.IsOpen != shop.IsOpen)
            {
                shop.IsOpen = Input.IsOpen;
            }
            
        }
    }
}
