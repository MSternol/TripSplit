using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TripSplit.Application.Features.Trips.Share;

namespace TripSplit.Web.Controllers
{
    [AllowAnonymous]
    [Route("share")]
    public sealed class ShareController(IMediator mediator) : Controller
    {
        [HttpGet("{token}")]
        public IActionResult EnterName(string token)
            => View("EnterName", model: token);

        [HttpPost("{token}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ViewTrip(string token, string firstName, string lastName)
        {
            var dto = await mediator.Send(new ViewSharedTripQuery(token, firstName, lastName));
            if (dto is null)
            {
                ViewBag.Error = "Brak dostępu. Sprawdź token lub imię i nazwisko.";
                return View("EnterName", model: token);
            }
            return View("SharedTripDetails", dto);
        }
    }
}
