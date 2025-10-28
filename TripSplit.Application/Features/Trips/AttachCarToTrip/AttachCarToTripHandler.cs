using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Application.Abstractions;
using TripSplit.Domain.Repositories;

namespace TripSplit.Application.Features.Trips.AttachCarToTrip
{
    public sealed class AttachCarToTripHandler(
    ITripRepository trips,
    ICurrentUserService current,
    IUnitOfWork uow
) : IRequestHandler<AttachCarToTripCommand, bool>
    {
        public async Task<bool> Handle(AttachCarToTripCommand r, CancellationToken ct)
        {
            var trip = await trips.GetAsync(r.TripId, current.GetUserId(), ct);
            if (trip is null) return false;

            trip.AttachCarSnapshot(r.CarId, r.CarName, r.CarAvg);
            trip.RecalculateTotals();

            await uow.SaveChangesAsync(ct);
            return true;
        }
    }
}
