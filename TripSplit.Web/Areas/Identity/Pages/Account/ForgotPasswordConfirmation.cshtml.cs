using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TripSplit.Web.Areas.Identity.Pages.Account
{
    public class ForgotPasswordConfirmationModel : PageModel
    {
        public string? DevLink { get; private set; }
        public void OnGet() => DevLink = TempData["DevResetLink"] as string;
    }

}
