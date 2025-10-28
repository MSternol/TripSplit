using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using TripSplit.Infrastructure.Identity;

namespace TripSplit.Web.Areas.Identity.Pages.Account
{
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;

        public ResetPasswordModel(UserManager<AppUser> userManager) => _userManager = userManager;

        [BindProperty] public InputModel Input { get; set; } = new();

        public class InputModel
        {
            [Required, EmailAddress, Display(Name = "E-mail")]
            public string Email { get; set; } = string.Empty;

            [Required, StringLength(100, MinimumLength = 8)]
            [DataType(DataType.Password), Display(Name = "Nowe hasło")]
            public string Password { get; set; } = string.Empty;

            [DataType(DataType.Password), Display(Name = "Powtórz hasło")]
            [Compare(nameof(Password), ErrorMessage = "Hasła nie są takie same.")]
            public string ConfirmPassword { get; set; } = string.Empty;

            [Required] public string Code { get; set; } = string.Empty;
        }

        public IActionResult OnGet(string? code = null, string? email = null)
        {
            if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(email))
                return RedirectToPage("./ForgotPassword");

            Input = new InputModel
            {
                Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code)),
                Email = email
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user is null) return RedirectToPage("./Login");

            var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
            if (result.Succeeded) return RedirectToPage("./Login");

            foreach (var e in result.Errors)
                ModelState.AddModelError(string.Empty, e.Description);

            return Page();
        }
    }

}
