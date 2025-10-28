﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TripSplit.Infrastructure.Identity;

namespace TripSplit.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<AppUser> signInManager,
                          UserManager<AppUser> userManager,
                          ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        [BindProperty] public InputModel Input { get; set; } = new();

        public IList<AuthenticationScheme> ExternalLogins { get; private set; } = new List<AuthenticationScheme>();
        public string? ReturnUrl { get; set; }

        [TempData] public string? ErrorMessage { get; set; }

        public class InputModel
        {
            [Required, Display(Name = "E-mail lub login")]
            public string EmailOrUserName { get; set; } = string.Empty;

            [Required, DataType(DataType.Password), Display(Name = "Hasło")]
            public string Password { get; set; } = string.Empty;

            [Display(Name = "Zapamiętaj mnie")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string? returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
                ModelState.AddModelError(string.Empty, ErrorMessage);

            ReturnUrl = Sanitize(returnUrl);

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            ReturnUrl = Sanitize(returnUrl);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (!ModelState.IsValid)
                return Page();

            var login = (Input.EmailOrUserName ?? string.Empty).Trim();

            var user = await _userManager.FindByEmailAsync(login) ?? await _userManager.FindByNameAsync(login);
            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "Nieprawidłowy login lub hasło.");
                return Page();
            }

            if (_signInManager.Options.SignIn.RequireConfirmedAccount && !user.EmailConfirmed)
            {
                ModelState.AddModelError(string.Empty, "Potwierdź e-mail, aby się zalogować.");
                return Page();
            }

            var result = await _signInManager.PasswordSignInAsync(user, Input.Password, Input.RememberMe, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                _logger.LogInformation("Użytkownik {UserId} zalogował się.", user.Id);
                return LocalRedirect(ReturnUrl!);
            }

            if (result.RequiresTwoFactor)
                return RedirectToPage("./LoginWith2fa", new { ReturnUrl, RememberMe = Input.RememberMe });

            if (result.IsLockedOut)
            {
                _logger.LogWarning("Konto zablokowane (lockout).");
                ModelState.AddModelError(string.Empty, "Konto tymczasowo zablokowane.");
                return Page();
            }

            if (result.IsNotAllowed)
            {
                ModelState.AddModelError(string.Empty, "Logowanie niedozwolone dla tego konta.");
                return Page();
            }

            ModelState.AddModelError(string.Empty, "Nieprawidłowy login lub hasło.");
            return Page();
        }

        private string Sanitize(string? url)
            => (!string.IsNullOrEmpty(url) && Url.IsLocalUrl(url)) ? url : Url.Content("~/");
    }

}

