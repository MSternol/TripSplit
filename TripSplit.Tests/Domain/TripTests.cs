using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Domain.Entities;
using TripSplit.Domain.ValueObjects;

namespace TripSplit.Tests.Domain
{
    public class TripTests
    {
        private static Location L(string n) => new(n, 0, 0);

        [Fact]
        public void Ctor_Computes_And_Raises_Event()
        {
            var trip = new Trip(Guid.NewGuid(), DateTime.UtcNow, L("Kraków"), L("Gdańsk"),
                500, 6.49m, 6.8, null, 4, 20, 10);

            trip.LitersUsed.Should().Be(34.000);
            trip.FuelCostTotal.Should().Be(220.66m);
            trip.TripCostTotal.Should().Be(250.66m);
            trip.CostPerPerson.Should().Be(62.66m);

            trip.DomainEvents.Should().ContainSingle()
                .Which.Should().BeOfType<TripSplit.Domain.Events.TripCreated>();
        }

        [Fact]
        public void UpdateCosts_Recalculates_And_Raises()
        {
            var trip = new Trip(Guid.NewGuid(), DateTime.UtcNow, L("A"), L("B"),
                100, 6m, 6.5, null, 2, 5m, 5m);
            trip.ClearDomainEvents();

            trip.UpdateCosts(6.99m, 10m, 0m, 7.2, null, 3, 150);

            trip.LitersUsed.Should().Be(10.800);
            trip.FuelCostTotal.Should().Be(75.49m);
            trip.TripCostTotal.Should().Be(85.49m);
            trip.CostPerPerson.Should().Be(28.50m);

            trip.DomainEvents.Should().ContainSingle()
                .Which.Should().BeOfType<TripSplit.Domain.Events.TripRecalculated>();
        }
    }
}
