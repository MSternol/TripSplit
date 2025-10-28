using System.Security.Claims;
using TripSplit.Application.Abstractions;

namespace TripSplit.Web.Services
{
    public sealed class CurrentUserService(IHttpContextAccessor accessor) : ICurrentUserService
    {
        public Guid GetUserId()
        {
            var id = accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(id, out var g) ? g : Guid.Empty;
        }
    }
}
