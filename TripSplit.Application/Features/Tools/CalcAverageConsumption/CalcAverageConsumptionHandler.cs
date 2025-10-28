using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Domain.Services;

namespace TripSplit.Application.Features.Tools.CalcAverageConsumption
{
    public sealed class CalcAverageConsumptionHandler : IRequestHandler<CalcAverageConsumptionQuery, double>
    {
        public Task<double> Handle(CalcAverageConsumptionQuery r, CancellationToken ct)
            => Task.FromResult(TripCostCalculator.AverageConsumption(r.DistanceKm, r.LitersUsed));
    }
}
