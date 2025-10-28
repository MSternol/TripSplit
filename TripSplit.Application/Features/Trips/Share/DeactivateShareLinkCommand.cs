using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripSplit.Application.Features.Trips.Share
{
    public sealed record DeactivateShareLinkCommand(string Token) : IRequest<bool>;
}
