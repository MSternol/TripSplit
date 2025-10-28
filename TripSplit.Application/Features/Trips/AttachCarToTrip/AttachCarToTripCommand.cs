using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripSplit.Application.Features.Trips.AttachCarToTrip
{
    public sealed record AttachCarToTripCommand(Guid TripId, Guid CarId, string CarName, double CarAvg) : IRequest<bool>;
}
