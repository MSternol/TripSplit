using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using TripSplit.Infrastructure.Identity;

namespace TripSplit.Web.Areas.Identity.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSender? _emailSender;
        private readonly IHostEnvironment _env;
        private readonly ILogger<ForgotPasswordModel> _logger;

        public ForgotPasswordModel(
            UserManager<AppUser> userManager,
            IEmailSender? emailSender,
            IHostEnvironment env,
            ILogger<ForgotPasswordModel> logger)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _env = env;
            _logger = logger;
        }

        [BindProperty] public InputModel Input { get; set; } = new();

        public class InputModel
        {
            [Required, EmailAddress, Display(Name = "E-mail")]
            public string Email { get; set; } = string.Empty;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var user = await _userManager.FindByEmailAsync(Input.Email);

            if (user is not null)
            {
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var codeEnc = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                var callback = Url.Page(
                    pageName: "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code = codeEnc, email = Input.Email },
                    protocol: Request.Scheme);

                if (_emailSender is not null)
                {
                    await _emailSender.SendEmailAsync(
                        Input.Email,
                        "Reset hasła",
                        $"Zresetuj hasło klikając <a href='{callback}'>ten link</a>.");
                }

                if (_env.IsDevelopment())
                {
                    TempData["DevResetLink"] = callback;
                }
            }

            _logger.LogInformation("Jeśli konto istnieje, wygenerowano link resetu dla {Email}", Input.Email);
            return RedirectToPage("./ForgotPasswordConfirmation");
        }
    }

}

