using System;
using TripSplit.Domain.Common;

namespace TripSplit.Domain.Entities
{
    public sealed class CarInspection : BaseEntity
    {
        public Guid CarId { get; private set; }
        public DateTime? ValidFrom { get; private set; }
        public DateTime? ValidTo { get; private set; }

        private CarInspection() { }

        public CarInspection(Guid carId, DateTime? from, DateTime? to)
        {
            CarId = carId;
            ValidFrom = from;
            ValidTo = to;
        }

        public void Update(DateTime? from, DateTime? to)
        {
            if (from.HasValue) ValidFrom = from;
            if (to.HasValue) ValidTo = to;
        }
    }

}
