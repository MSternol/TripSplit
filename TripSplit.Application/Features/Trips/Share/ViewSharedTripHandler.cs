using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Application.Common.Text;
using TripSplit.Application.DTOs;
using TripSplit.Domain.Repositories;

namespace TripSplit.Application.Features.Trips.Share
{
    public sealed class ViewSharedTripHandler(
        ITripShareLinkRepository links,
        ITripRepository trips,
        ITripParticipantRepository participants
    ) : IRequestHandler<ViewSharedTripQuery, TripDto?>
    {
        public async Task<TripDto?> Handle(ViewSharedTripQuery r, CancellationToken ct)
        {
            var link = await links.GetByTokenAsync(r.Token, ct);
            if (link is null || !link.IsValidNowUtc(DateTime.UtcNow))
                return null;

            var trip = await trips.GetByIdAsync(link.TripId, ct);
            if (trip is null)
                return null;

            var list = await participants.ListAsync(trip.Id, ct);

            if (list.Count == 0 || list.Any(p =>
                TextNormalizer.EqualsLoose(p.Name?.FirstName, r.FirstName) &&
                TextNormalizer.EqualsLoose(p.Name?.LastName, r.LastName)))
            {
                return new TripDto
                {
                    Id = trip.Id,
                    StartedAt = trip.StartedAt,
                    EndedAt = trip.EndedAt,

                    StartName = trip.Start?.Name ?? string.Empty,
                    EndName = trip.End?.Name ?? string.Empty,

                    DistanceKm = trip.DistanceKm,
                    FuelPricePerL = trip.FuelPricePerL,
                    AverageConsumptionLper100 = trip.AverageConsumptionLper100,
                    LitersUsed = trip.LitersUsed,
                    PeopleCount = trip.PeopleCount,

                    ParkingCost = trip.ParkingCost,
                    ExtraCosts = trip.ExtraCosts,

                    FuelCostTotal = trip.FuelCostTotal,
                    TripCostTotal = trip.TripCostTotal,
                    CostPerPerson = trip.CostPerPerson,

                    CarId = trip.CarId,
                    CarName = trip.CarName,
                    CarAvgConsumptionSnapshot = trip.CarAvgConsumptionSnapshot
                };

            }

            return null;
        }
    }
}
