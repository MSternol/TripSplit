using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Application.Abstractions;
using TripSplit.Domain.Repositories;

namespace TripSplit.Application.Features.Trips.UpdateTripCosts
{
    public sealed class UpdateTripCostsHandler(
    ITripRepository trips,
    ICurrentUserService current,
    IUnitOfWork uow
) : IRequestHandler<UpdateTripCostsCommand, bool>
    {
        public async Task<bool> Handle(UpdateTripCostsCommand r, CancellationToken ct)
        {
            var entity = await trips.GetAsync(r.Id, current.GetUserId(), ct);
            if (entity is null) return false;

            entity.UpdateCosts(
                fuelPricePerL: r.FuelPricePerL,
                parking: r.ParkingCost,
                extras: r.ExtraCosts,
                avgLper100: r.AverageConsumptionLper100,
                litersUsed: r.LitersUsed,
                people: r.PeopleCount,
                distanceKm: r.DistanceKm
            );

            await uow.SaveChangesAsync(ct);
            return true;
        }
    }
}
