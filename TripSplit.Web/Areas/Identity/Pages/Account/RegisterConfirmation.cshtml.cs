using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using TripSplit.Infrastructure.Identity;

namespace TripSplit.Web.Areas.Identity.Pages.Account
{
    public class RegisterConfirmationModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSender? _sender;
        private readonly IHostEnvironment _env;

        public RegisterConfirmationModel(UserManager<AppUser> userManager, IEmailSender? sender, IHostEnvironment env)
        {
            _userManager = userManager;
            _sender = sender;
            _env = env;
        }

        public string Email { get; set; } = string.Empty;
        public bool DisplayConfirmAccountLink { get; set; }
        public string? EmailConfirmationUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(string email, string? returnUrl = null)
        {
            if (string.IsNullOrEmpty(email)) return RedirectToPage("/Index");
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) return NotFound($"Nie znaleziono użytkownika '{email}'.");

            Email = email;
            DisplayConfirmAccountLink = _sender is null || !_env.IsProduction();

            if (DisplayConfirmAccountLink)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var codeEnc = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                EmailConfirmationUrl = Url.Page(
                    "/Account/ConfirmEmail", null,
                    new { area = "Identity", userId = user.Id, code = codeEnc, returnUrl = returnUrl ?? Url.Content("~/") },
                    Request.Scheme);
            }

            return Page();
        }
    }

}
