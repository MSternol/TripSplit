using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Domain.Services;

namespace TripSplit.Tests.Domain
{
    public class ServicesTests
    {
        [Fact]
        public void TripCostCalculator_BasicFlow()
        {
            var liters = TripCostCalculator.LitersUsed(500, 6.8);
            liters.Should().Be(34.000);

            var fuelCost = TripCostCalculator.FuelCost(6.49m, liters);
            fuelCost.Should().Be(220.66m);

            var total = TripCostCalculator.TotalCost(fuelCost, 20m, 10m);
            total.Should().Be(250.66m);

            var perPerson = TripCostCalculator.PerPerson(total, 4);
            perPerson.Should().Be(62.66m);
        }
    }
}
