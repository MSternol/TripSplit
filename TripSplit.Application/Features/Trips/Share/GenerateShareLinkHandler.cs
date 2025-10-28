using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Application.Abstractions;
using TripSplit.Domain.Entities;
using TripSplit.Domain.Repositories;

namespace TripSplit.Application.Features.Trips.Share
{
    public sealed class GenerateShareLinkHandler(
    ICurrentUserService current,
    ITripRepository trips,
    ITripShareLinkRepository links,
    IUnitOfWork uow
) : IRequestHandler<GenerateShareLinkCommand, string>
    {
        public async Task<string> Handle(GenerateShareLinkCommand r, CancellationToken ct)
        {
            var trip = await trips.GetAsync(r.TripId, current.GetUserId(), ct);
            if (trip is null) throw new InvalidOperationException("Trip not found or not owned.");

            var token = $"{Guid.NewGuid():N}{Random.Shared.Next(1000, 9999)}";
            var link = new TripShareLink(trip.Id, token, r.ExpiresAtUtc);

            await links.AddAsync(link, ct);
            await uow.SaveChangesAsync(ct);
            return token;
        }
    }
}
