using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TripSplit.Infrastructure.Identity;

namespace TripSplit.Web.Areas.Identity.Pages.Account
{

    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(UserManager<AppUser> userManager,
                             SignInManager<AppUser> signInManager,
                             ILogger<RegisterModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty] public InputModel Input { get; set; } = new();

        public string? ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; private set; } = new List<AuthenticationScheme>();

        public class InputModel
        {
            [MaxLength(100), Display(Name = "Imię")]
            public string? FirstName { get; set; }

            [MaxLength(100), Display(Name = "Nazwisko")]
            public string? LastName { get; set; }

            [Required, EmailAddress, Display(Name = "E-mail")]
            public string Email { get; set; } = string.Empty;

            [Required, StringLength(100, MinimumLength = 8,
                ErrorMessage = "{0} musi mieć od {2} do {1} znaków."),
             DataType(DataType.Password), Display(Name = "Hasło")]
            public string Password { get; set; } = string.Empty;

            [DataType(DataType.Password), Display(Name = "Powtórz hasło"),
             Compare(nameof(Password), ErrorMessage = "Hasła nie są takie same.")]
            public string ConfirmPassword { get; set; } = string.Empty;
        }

        public async Task OnGetAsync(string? returnUrl = null)
        {
            ReturnUrl = Sanitize(returnUrl);
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            ReturnUrl = Sanitize(returnUrl);
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (!ModelState.IsValid) return Page();

            var user = new AppUser
            {
                UserName = Input.Email,
                Email = Input.Email,
                FirstName = Input.FirstName,
                LastName = Input.LastName
            };

            var result = await _userManager.CreateAsync(user, Input.Password);
            if (!result.Succeeded)
            {
                foreach (var e in result.Errors)
                    ModelState.AddModelError(string.Empty, e.Description);
                return Page();
            }

            _logger.LogInformation("Utworzono konto użytkownika {UserId}.", user.Id);

            if (_signInManager.Options.SignIn.RequireConfirmedAccount)
            {
                TempData["InfoMessage"] = "Konto utworzone. Potwierdź adres e-mail, aby się zalogować.";
                return RedirectToPage("./Login", new { ReturnUrl });
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            return LocalRedirect(ReturnUrl!);
        }

        private string Sanitize(string? url)
            => (!string.IsNullOrEmpty(url) && Url.IsLocalUrl(url)) ? url : Url.Content("~/");
    }

}
