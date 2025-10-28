using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Domain.Services;

namespace TripSplit.Application.Features.Tools.CalcFullTankCost
{
    public sealed class CalcFullTankCostHandler : IRequestHandler<CalcFullTankCostQuery, decimal>
    {
        public Task<decimal> Handle(CalcFullTankCostQuery r, CancellationToken ct)
            => Task.FromResult(TripCostCalculator.FullTankCost(r.TankCapacityL, r.FuelPricePerL));
    }
}
