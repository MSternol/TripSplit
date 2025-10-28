using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripSplit.Application.Features.Trips.UpdateTripCosts
{
    public sealed record UpdateTripCostsCommand(
     Guid Id,
     decimal? FuelPricePerL,
     decimal? ParkingCost,
     decimal? ExtraCosts,
     double? AverageConsumptionLper100,
     double? LitersUsed,
     int? PeopleCount,
     double? DistanceKm
 ) : IRequest<bool>;
}
