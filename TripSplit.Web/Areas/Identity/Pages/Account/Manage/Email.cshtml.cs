using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using TripSplit.Infrastructure.Identity;

namespace TripSplit.Web.Areas.Identity.Pages.Account.Manage
{
    public class EmailModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailSender? _emailSender;

        public EmailModel(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailSender? emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        public string Email { get; set; } = string.Empty;
        public bool IsEmailConfirmed { get; set; }
        [TempData] public string? StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            [Required, EmailAddress, Display(Name = "Nowy e-mail")]
            public string NewEmail { get; set; } = string.Empty;
        }

        private async Task LoadAsync(AppUser user)
        {
            Email = await _userManager.GetEmailAsync(user) ?? string.Empty;
            Input = new InputModel { NewEmail = Email };
            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();
            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostChangeEmailAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var current = await _userManager.GetEmailAsync(user);
            if (string.Equals(Input.NewEmail, current, StringComparison.OrdinalIgnoreCase))
            {
                StatusMessage = "E-mail bez zmian.";
                return RedirectToPage();
            }

            var code = await _userManager.GenerateChangeEmailTokenAsync(user, Input.NewEmail);
            var codeEnc = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmailChange", null,
                new { area = "Identity", userId = user.Id, email = Input.NewEmail, code = codeEnc },
                Request.Scheme);

            if (_emailSender is null)
            {
                StatusMessage = "Brak nadawcy e-mail – dodaj IEmailSender, aby wysyłać linki.";
                return RedirectToPage();
            }

            await _emailSender.SendEmailAsync(
                Input.NewEmail,
                "Potwierdź zmianę e-mail",
                $"Potwierdź zmianę klikając <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>ten link</a>.");

            StatusMessage = "Wysłano link potwierdzający. Sprawdź skrzynkę.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var codeEnc = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail", null,
                new { area = "Identity", userId = user.Id, code = codeEnc }, Request.Scheme);

            if (_emailSender is null)
            {
                StatusMessage = "Brak nadawcy e-mail – dodaj IEmailSender, aby wysyłać linki.";
                return RedirectToPage();
            }

            await _emailSender.SendEmailAsync(
                user.Email!, "Potwierdź adres e-mail",
                $"Potwierdź adres klikając <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>ten link</a>.");
            StatusMessage = "Wysłano e-mail weryfikacyjny.";
            return RedirectToPage();
        }
    }
}

