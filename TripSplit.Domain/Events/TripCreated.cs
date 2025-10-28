using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Domain.Common;

namespace TripSplit.Domain.Events
{
    public sealed class TripCreated(Guid tripId, Guid ownerUserId) : DomainEventBase
    {
        public Guid TripId { get; } = tripId;
        public Guid OwnerUserId { get; } = ownerUserId;
    }
}
