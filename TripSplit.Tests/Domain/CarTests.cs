using FluentAssertions;
using System;
using TripSplit.Domain.Entities;
using TripSplit.Domain.Enums;
using Xunit;

namespace TripSplit.Tests.Domain
{
    public class CarTests
    {
        [Fact]
        public void Ctor_Normalizes_And_Raises_CreatedEvent()
        {
            var car = new Car(Guid.NewGuid(), "  Civic  ", FuelType.Petrol, 7.123, 45.678);

            car.Name.Should().Be("Civic");
            car.AverageConsumptionLper100.Should().Be(7.12);
            car.TankCapacityL.Should().Be(45.68);

            car.DomainEvents.Should().ContainSingle()
                .Which.Should().BeOfType<TripSplit.Domain.Events.CarCreated>();
        }

        [Fact]
        public void UpdateBasics_Changes_And_Raises_UpdatedEvent()
        {
            var car = new Car(Guid.NewGuid(), "Civic", FuelType.Petrol, 7.1, 45.6);
            car.ClearDomainEvents();

            car.UpdateBasics(name: "Civic X", fuelType: FuelType.Diesel, avg: 5.555, tank: 50.444);

            car.Name.Should().Be("Civic X");
            car.FuelType.Should().Be(FuelType.Diesel);
            car.AverageConsumptionLper100.Should().Be(5.56);
            car.TankCapacityL.Should().Be(50.44);

            car.DomainEvents.Should().ContainSingle()
                .Which.Should().BeOfType<TripSplit.Domain.Events.CarUpdated>();
        }

        [Fact]
        public void FullTankCost_And_EstimateLiters_Work()
        {
            var car = new Car(Guid.NewGuid(), "Civic", FuelType.Petrol, 6.5, 50);
            car.EstimateLiters(200).Should().Be(13.000); // 200 * 6.5 / 100
            car.FullTankCost(6.99m).Should().Be(349.50m);
        }

        [Fact]
        public void SetReminders_Updates_And_Raises_UpdatedEvent()
        {
            var car = new Car(Guid.NewGuid(), "Fabia", FuelType.Petrol, 6.0, 45);
            car.ClearDomainEvents();

            car.SetReminders(enabled: false, leadTime: ReminderLeadTime.Month);

            car.RemindersEnabled.Should().BeFalse();
            car.ReminderLeadTime.Should().Be(ReminderLeadTime.Month);

            car.DomainEvents.Should().ContainSingle()
                .Which.Should().BeOfType<TripSplit.Domain.Events.CarUpdated>();
        }
    }
}
