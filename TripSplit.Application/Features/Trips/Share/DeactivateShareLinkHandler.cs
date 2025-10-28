using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Domain.Repositories;

namespace TripSplit.Application.Features.Trips.Share
{
    public sealed class DeactivateShareLinkHandler(
    ITripShareLinkRepository links,
    IUnitOfWork uow
) : IRequestHandler<DeactivateShareLinkCommand, bool>
    {
        public async Task<bool> Handle(DeactivateShareLinkCommand r, CancellationToken ct)
        {
            var link = await links.GetByTokenAsync(r.Token, ct);
            if (link is null) return false;

            await links.DeactivateAsync(link, ct);
            await uow.SaveChangesAsync(ct);
            return true;
        }
    }
}
