using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Domain.Common;

namespace TripSplit.Domain.Events
{
    public sealed class CarUpdated(Guid carId) : DomainEventBase
    {
        public Guid CarId { get; } = carId;
    }
}
