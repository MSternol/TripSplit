using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripSplit.Application.Features.Trips.CreateTrip
{
    public sealed record CreateTripCommand(
    string startName,
    string endName,
    double? startLat,
    double? startLon,
    double? endLat,
    double? endLon,
    double? distanceKm,
    decimal fuelPricePerL,
    double averageConsumptionLper100,
    double? litersUsed,
    int peopleCount,
    decimal parkingCost,
    decimal extraCosts,
    Guid? carId
) : IRequest<Guid>;
}
