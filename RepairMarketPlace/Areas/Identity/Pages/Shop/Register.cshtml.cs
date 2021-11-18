using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using RepairMarketPlace.ApplicationCore.Exceptions;
using RepairMarketPlace.ApplicationCore.Interfaces;
using RepairMarketPlace.Infrastructure.Identity;
using Web.Extensions;

namespace Web.Areas.Identity.Pages.Shop
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IShopService _shopService;

        public RegisterModel(UserManager<User> userManager, SignInManager<User> signInManager,
            ILogger<RegisterModel> logger, IEmailSender emailSender, IShopService shopService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _shopService = shopService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Full name*")]
            public string Name { get; set; }

            [Required]
            [MaxLength(200)]
            [DataType(DataType.Text)]
            [Display(Name = "Shop's name*")]
            public string ShopName { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Shop's full address*")]
            public string ShopAddress { get; set; }
            [Required]
            [Phone]
            [DataType(DataType.PhoneNumber)]
            [Display(Name = "Shop's Phone Number*")]
            public string PhoneNumber { get;  set; }

            [Required]
            [Display(Name = "Birth Date*")]
            [DataType(DataType.Date)]
            public DateTime Birthday { get; set; }
            [Required]
            [EmailAddress]
            [DataType(DataType.EmailAddress)]
            [Display(Name = "Email*")]
            public string Email { get; set; }
            [Url]
            [DataType(DataType.Url)]
            public string WebSite { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password*")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password*")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;          
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new User { UserName = Input.Email, Email = Input.Email, Name = Input.Name, Birthday = Input.Birthday, PhoneNumber = Input.PhoneNumber };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    try
                    {
                        await _shopService.CreateShopAsync(Guid.Parse(await _userManager.GetUserIdAsync(user)),
                            Input.ShopName, Input.ShopAddress, Input.Email, Input.PhoneNumber, Input.WebSite);
                    }
                    catch (SingleShopException ex)
                    {
                        _logger.LogError(ex.Message);
                    }
                    _logger.LogInformation("Shop created.");

                    await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("Role", "ShopOwner"));
                    _logger.LogInformation("Shop Owner claim added.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailConfirmationAsync(Input.Email, callbackUrl);

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("/Account/RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
