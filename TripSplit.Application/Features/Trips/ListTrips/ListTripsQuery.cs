using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Application.DTOs;

namespace TripSplit.Application.Features.Trips.ListTrips
{
    public sealed record ListTripsQuery() : IRequest<IReadOnlyList<TripDto>>;
}
