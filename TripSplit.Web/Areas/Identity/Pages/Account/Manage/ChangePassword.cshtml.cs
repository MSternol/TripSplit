using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TripSplit.Infrastructure.Identity;

namespace TripSplit.Web.Areas.Identity.Pages.Account.Manage
{
    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public ChangePasswordModel(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty] public InputModel Input { get; set; } = new();
        [TempData] public string? StatusMessage { get; set; }

        public class InputModel
        {
            [Required, DataType(DataType.Password), Display(Name = "Aktualne hasło")]
            public string OldPassword { get; set; } = string.Empty;

            [Required, StringLength(100, MinimumLength = 8),
             DataType(DataType.Password), Display(Name = "Nowe hasło")]
            public string NewPassword { get; set; } = string.Empty;

            [DataType(DataType.Password), Display(Name = "Powtórz nowe hasło"),
             Compare(nameof(NewPassword), ErrorMessage = "Hasła nie są takie same.")]
            public string ConfirmPassword { get; set; } = string.Empty;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();
            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (!hasPassword) return RedirectToPage("./SetPassword");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var res = await _userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);
            if (!res.Succeeded)
            {
                foreach (var e in res.Errors) ModelState.AddModelError(string.Empty, e.Description);
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Hasło zostało zmienione.";
            return RedirectToPage();
        }
    }
}

