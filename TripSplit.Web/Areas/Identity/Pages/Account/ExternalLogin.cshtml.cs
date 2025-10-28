using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TripSplit.Infrastructure.Identity;

namespace TripSplit.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<ExternalLoginModel> _logger;

        public ExternalLoginModel(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, ILogger<ExternalLoginModel> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        public string? ReturnUrl { get; set; }

        [ValidateAntiForgeryToken]
        public IActionResult OnPost(string provider, string? returnUrl = null)
        {
            var safeReturn = Sanitize(returnUrl);
            if (string.IsNullOrWhiteSpace(provider))
            {
                TempData["ErrorMessage"] = "Brak dostawcy logowania.";
                return RedirectToPage("./Login", new { ReturnUrl = safeReturn });
            }

            var redirectUrl = Url.Page("/Account/ExternalLogin", pageHandler: "Callback", values: new { area = "Identity", returnUrl = safeReturn });
            var props = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl!);
            return new ChallengeResult(provider, props);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string? returnUrl = null, string? remoteError = null)
        {
            var safeReturn = Sanitize(returnUrl);

            if (!string.IsNullOrEmpty(remoteError))
            {
                TempData["ErrorMessage"] = $"Błąd dostawcy: {remoteError}";
                return RedirectToPage("./Login", new { ReturnUrl = safeReturn });
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info is null)
            {
                TempData["ErrorMessage"] = "Nie udało się pobrać informacji logowania zewnętrznego.";
                return RedirectToPage("./Login", new { ReturnUrl = safeReturn });
            }

            var ext = await _signInManager.ExternalLoginSignInAsync(
                info.LoginProvider, info.ProviderKey, isPersistent: true, bypassTwoFactor: true);

            if (ext.Succeeded)
            {
                _logger.LogInformation("{Name} zalogował(a) się przez {Provider}.", info.Principal.Identity?.Name, info.LoginProvider);
                return LocalRedirect(safeReturn);
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var given = info.Principal.FindFirstValue(ClaimTypes.GivenName);
            var family = info.Principal.FindFirstValue(ClaimTypes.Surname);
            var name = info.Principal.FindFirstValue(ClaimTypes.Name);

            if (string.IsNullOrWhiteSpace(given) && !string.IsNullOrWhiteSpace(name))
            {
                var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 1) given = parts[0];
                if (parts.Length >= 2) family = parts[^1];
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                email = $"{info.ProviderKey}@{info.LoginProvider}.local";
                TempData["InfoMessage"] = "Dostawca nie udostępnił adresu e-mail. Użyto adresu technicznego.";
            }

            var user = new AppUser
            {
                UserName = email,
                Email = email,
                FirstName = given,
                LastName = family
            };

            var create = await _userManager.CreateAsync(user);
            if (!create.Succeeded)
            {
                foreach (var e in create.Errors)
                    ModelState.AddModelError(string.Empty, e.Description);

                TempData["ErrorMessage"] = "Nie udało się utworzyć konta użytkownika.";
                return RedirectToPage("./Login", new { ReturnUrl = safeReturn });
            }

            var link = await _userManager.AddLoginAsync(user, info);
            if (!link.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                foreach (var e in link.Errors)
                    ModelState.AddModelError(string.Empty, e.Description);

                TempData["ErrorMessage"] = "Nie udało się powiązać konta z dostawcą.";
                return RedirectToPage("./Login", new { ReturnUrl = safeReturn });
            }

            if (!user.Email!.EndsWith($".{info.LoginProvider}.local", StringComparison.OrdinalIgnoreCase))
            {
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);
            }

            await _signInManager.SignInAsync(user, isPersistent: true);
            _logger.LogInformation("Utworzono i zalogowano użytkownika z {Provider}.", info.LoginProvider);
            return LocalRedirect(safeReturn);
        }

        private string Sanitize(string? returnUrl)
            => (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)) ? returnUrl : Url.Content("~/");
    }

}

