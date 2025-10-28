using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripSplit.Application.Features.Tools.CalcAverageConsumption
{
    public sealed record CalcAverageConsumptionQuery(double DistanceKm, double LitersUsed) : IRequest<double>;
}
