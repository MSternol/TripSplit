using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Application.Features.Tools.CalcAverageConsumption;
using TripSplit.Application.Features.Tools.CalcFullTankCost;

namespace TripSplit.Tests.Application.Tools
{
    public class CalcToolsTests
    {
        [Fact]
        public async Task CalcFullTankCost_Works()
        {
            var h = new CalcFullTankCostHandler();
            var res = await h.Handle(new CalcFullTankCostQuery(50, 6.99m), CancellationToken.None);
            res.Should().Be(349.50m);
        }

        [Fact]
        public async Task CalcAverageConsumption_Works()
        {
            var h = new CalcAverageConsumptionHandler();
            var res = await h.Handle(new CalcAverageConsumptionQuery(500, 34), CancellationToken.None);
            res.Should().Be(6.8);
        }
    }
}
