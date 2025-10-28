using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripSplit.Application.Features.Tools.CalcFullTankCost
{
    public sealed record CalcFullTankCostQuery(double TankCapacityL, decimal FuelPricePerL) : IRequest<decimal>;
}
