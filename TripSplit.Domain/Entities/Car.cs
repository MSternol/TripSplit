using System;
using TripSplit.Domain.Common;
using TripSplit.Domain.Enums;
using TripSplit.Domain.Services;

namespace TripSplit.Domain.Entities
{
    public sealed class Car : BaseEntity
    {
        public Guid OwnerUserId { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public FuelType FuelType { get; private set; }
        public double AverageConsumptionLper100 { get; private set; }
        public double TankCapacityL { get; private set; }

        public CarInsurance? Insurance { get; private set; }
        public CarInspection? Inspection { get; private set; }
        public bool RemindersEnabled { get; private set; } = true;
        public ReminderLeadTime ReminderLeadTime { get; private set; } = ReminderLeadTime.Week;

        private Car() { }

        public Car(Guid ownerUserId, string name, FuelType fuelType, double averageConsumptionLper100, double tankCapacityL)
        {
            OwnerUserId = ownerUserId;
            Name = (name ?? string.Empty).Trim();
            FuelType = fuelType;
            AverageConsumptionLper100 = averageConsumptionLper100 < 0 ? 0 : Math.Round(averageConsumptionLper100, 2);
            TankCapacityL = tankCapacityL < 0 ? 0 : Math.Round(tankCapacityL, 2);
            AddDomainEvent(new Events.CarCreated(Id, OwnerUserId));
        }

        public void UpdateBasics(string? name = null, FuelType? fuelType = null, double? avg = null, double? tank = null)
        {
            if (!string.IsNullOrWhiteSpace(name)) Name = name.Trim();
            if (fuelType.HasValue) FuelType = fuelType.Value;
            if (avg.HasValue) AverageConsumptionLper100 = Math.Max(0, Math.Round(avg.Value, 2));
            if (tank.HasValue) TankCapacityL = Math.Max(0, Math.Round(tank.Value, 2));
            AddDomainEvent(new Events.CarUpdated(Id));
        }

        public void SetReminders(bool? enabled, ReminderLeadTime? leadTime)
        {
            if (enabled.HasValue) RemindersEnabled = enabled.Value;
            if (leadTime.HasValue) ReminderLeadTime = leadTime.Value;
            AddDomainEvent(new Events.CarUpdated(Id));
        }

        public double EstimateLiters(double distanceKm)
            => TripCostCalculator.LitersUsed(distanceKm, AverageConsumptionLper100);

        public decimal FullTankCost(decimal fuelPricePerL)
            => TripCostCalculator.FullTankCost(TankCapacityL, fuelPricePerL);
    }
}
