using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Domain.Common;

namespace TripSplit.Domain.Events
{
    public sealed class CarCreated(Guid carId, Guid ownerUserId) : DomainEventBase
    {
        public Guid CarId { get; } = carId;
        public Guid OwnerUserId { get; } = ownerUserId;
    }
}
