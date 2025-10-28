using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using TripSplit.Infrastructure.Identity;

namespace TripSplit.Web.Areas.Identity.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;

        public ConfirmEmailModel(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public string StatusMessage { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync(Guid userId, string code, string? returnUrl = null)
        {
            if (userId == Guid.Empty || string.IsNullOrWhiteSpace(code))
                return RedirectToPage("/Index");

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null) return NotFound("Nie znaleziono użytkownika.");

            var decoded = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, decoded);

            StatusMessage = result.Succeeded ? "Adres e-mail został potwierdzony." : "Nie udało się potwierdzić e-maila.";
            return Page();
        }
    }

}
