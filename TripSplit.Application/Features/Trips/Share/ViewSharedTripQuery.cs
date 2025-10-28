using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Application.DTOs;

namespace TripSplit.Application.Features.Trips.Share
{
    public sealed record ViewSharedTripQuery(
    string Token,
    string FirstName,
    string LastName
) : IRequest<TripDto?>;
}
