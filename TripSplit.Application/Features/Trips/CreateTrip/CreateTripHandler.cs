using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Application.Abstractions;
using TripSplit.Domain.Entities;
using TripSplit.Domain.Repositories;
using TripSplit.Domain.ValueObjects;

namespace TripSplit.Application.Features.Trips.CreateTrip
{
    public sealed class CreateTripHandler(
     ICurrentUserService current,
     ITripRepository trips,
     IUnitOfWork uow
 ) : IRequestHandler<CreateTripCommand, Guid>
    {
        public async Task<Guid> Handle(CreateTripCommand r, CancellationToken ct)
        {
            var start = new Location(r.startName, r.startLat ?? 0, r.startLon ?? 0);
            var end = new Location(r.endName, r.endLat ?? 0, r.endLon ?? 0);

            var trip = new Trip(
                ownerUserId: current.GetUserId(),
                startedAt: DateTime.UtcNow,
                start: start,
                end: end,
                distanceKm: r.distanceKm ?? 0,
                fuelPricePerL: r.fuelPricePerL,
                averageConsumptionLper100: r.averageConsumptionLper100,
                litersUsed: r.litersUsed,
                peopleCount: r.peopleCount,
                parkingCost: r.parkingCost,
                extraCosts: r.extraCosts,
                carId: r.carId,
                carName: null
            );

            await trips.AddAsync(trip, ct);
            await uow.SaveChangesAsync(ct);
            return trip.Id;
        }
    }
}
