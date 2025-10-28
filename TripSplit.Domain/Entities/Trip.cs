using System;
using TripSplit.Domain.Common;
using TripSplit.Domain.Services;
using TripSplit.Domain.ValueObjects;

namespace TripSplit.Domain.Entities
{
    public sealed class Trip : BaseEntity
    {
        public Guid OwnerUserId { get; private set; }

        public DateTime StartedAt { get; private set; }
        public DateTime? EndedAt { get; private set; }

        public Location Start { get; private set; }
        public Location End { get; private set; }

        public double DistanceKm { get; private set; }
        public decimal FuelPricePerL { get; private set; }
        public double AverageConsumptionLper100 { get; private set; }
        public double LitersUsed { get; private set; }
        public int PeopleCount { get; private set; }

        public decimal ParkingCost { get; private set; }
        public decimal ExtraCosts { get; private set; }

        public decimal FuelCostTotal { get; private set; }
        public decimal TripCostTotal { get; private set; }
        public decimal CostPerPerson { get; private set; }

        public Guid? CarId { get; private set; }
        public string? CarName { get; private set; }
        public double? CarAvgConsumptionSnapshot { get; private set; }

        private Trip() { }

        public Trip(
            Guid ownerUserId,
            DateTime startedAt,
            Location start,
            Location end,
            double distanceKm,
            decimal fuelPricePerL,
            double averageConsumptionLper100,
            double? litersUsed,
            int peopleCount,
            decimal parkingCost,
            decimal extraCosts,
            Guid? carId = null,
            string? carName = null)
        {
            if (peopleCount <= 0) throw new ArgumentException("PeopleCount must be >= 1", nameof(peopleCount));

            OwnerUserId = ownerUserId;
            StartedAt = startedAt;

            Start = start ?? throw new ArgumentNullException(nameof(start));
            End = end ?? throw new ArgumentNullException(nameof(end));

            DistanceKm = distanceKm < 0 ? 0 : Math.Round(distanceKm, 3);
            FuelPricePerL = fuelPricePerL < 0 ? 0 : fuelPricePerL;
            AverageConsumptionLper100 = averageConsumptionLper100 < 0 ? 0 : Math.Round(averageConsumptionLper100, 2);
            LitersUsed = litersUsed.HasValue && litersUsed.Value >= 0
                                            ? Math.Round(litersUsed.Value, 3)
                                            : TripCostCalculator.LitersUsed(DistanceKm, AverageConsumptionLper100);
            PeopleCount = peopleCount;
            ParkingCost = parkingCost < 0 ? 0 : parkingCost;
            ExtraCosts = extraCosts < 0 ? 0 : extraCosts;

            CarId = carId;
            CarName = carName;
            CarAvgConsumptionSnapshot = AverageConsumptionLper100;

            RecalculateTotals();
            AddDomainEvent(new TripSplit.Domain.Events.TripCreated(Id, OwnerUserId));
        }

        public void AttachCarSnapshot(Guid carId, string name, double carAvg)
        {
            CarId = carId;
            CarName = name;
            CarAvgConsumptionSnapshot = carAvg;
        }
        public void UpdateCosts(
            decimal? fuelPricePerL = null,
            decimal? parking = null,
            decimal? extras = null,
            double? avgLper100 = null,
            double? litersUsed = null,
            int? people = null,
            double? distanceKm = null)
        {
            if (fuelPricePerL.HasValue) FuelPricePerL = Math.Max(0, fuelPricePerL.Value);
            if (parking.HasValue) ParkingCost = Math.Max(0, parking.Value);
            if (extras.HasValue) ExtraCosts = Math.Max(0, extras.Value);

            if (distanceKm.HasValue) DistanceKm = distanceKm.Value < 0 ? 0 : Math.Round(distanceKm.Value, 3);
            if (avgLper100.HasValue) AverageConsumptionLper100 = Math.Max(0, Math.Round(avgLper100.Value, 2));
            if (people.HasValue) PeopleCount = Math.Max(1, people.Value);

            if (litersUsed.HasValue)
            {
                LitersUsed = Math.Max(0, Math.Round(litersUsed.Value, 3));
            }
            else
            {
                if (avgLper100.HasValue || distanceKm.HasValue)
                {
                    LitersUsed = TripCostCalculator.LitersUsed(DistanceKm, AverageConsumptionLper100);
                }
            }

            RecalculateTotals();
            AddDomainEvent(new TripSplit.Domain.Events.TripRecalculated(Id));
        }

        public void Close(DateTime endedAt) => EndedAt = endedAt;

        public void RecalculateTotals()
        {
            FuelCostTotal = TripCostCalculator.FuelCost(FuelPricePerL, LitersUsed);
            TripCostTotal = TripCostCalculator.TotalCost(FuelCostTotal, ParkingCost, ExtraCosts);
            CostPerPerson = TripCostCalculator.PerPerson(TripCostTotal, PeopleCount);
        }
    }
}
