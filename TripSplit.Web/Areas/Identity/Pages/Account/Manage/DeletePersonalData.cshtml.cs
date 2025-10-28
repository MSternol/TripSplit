using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using TripSplit.Infrastructure.Identity;

namespace TripSplit.Web.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public class DeletePersonalDataModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<DeletePersonalDataModel> _logger;

        public DeletePersonalDataModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ILogger<DeletePersonalDataModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty] public InputModel Input { get; set; } = new();

        public class InputModel
        {
            [DataType(DataType.Password), Display(Name = "Hasło")]
            public string? Password { get; set; }
        }

        public bool RequirePassword { get; private set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
                return NotFound($"Nie można załadować użytkownika o ID '{_userManager.GetUserId(User)}'.");

            RequirePassword = await _userManager.HasPasswordAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
                return NotFound($"Nie można załadować użytkownika o ID '{_userManager.GetUserId(User)}'.");

            RequirePassword = await _userManager.HasPasswordAsync(user);
            if (RequirePassword)
            {
                if (string.IsNullOrWhiteSpace(Input.Password) || !await _userManager.CheckPasswordAsync(user, Input.Password))
                {
                    ModelState.AddModelError(string.Empty, "Nieprawidłowe hasło.");
                    return Page();
                }
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                _logger.LogError("Błąd podczas usuwania konta {UserId}: {Errors}",
                    userId, string.Join(", ", result.Errors.Select(e => e.Description)));
                ModelState.AddModelError(string.Empty, "Wystąpił błąd podczas usuwania konta.");
                return Page();
            }

            await _signInManager.SignOutAsync();
            _logger.LogInformation("Użytkownik {UserId} usunął swoje konto.", userId);

            return Redirect("~/");
        }
    }

}
