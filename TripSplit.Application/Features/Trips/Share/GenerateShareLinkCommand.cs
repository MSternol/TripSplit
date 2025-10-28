using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripSplit.Application.Features.Trips.Share
{
    public sealed record GenerateShareLinkCommand(
    Guid TripId,
    DateTime? ExpiresAtUtc
) : IRequest<string>;
}
