using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Domain.Common;
using TripSplit.Domain.ValueObjects;

namespace TripSplit.Domain.Entities
{
    public sealed class TripParticipant : BaseEntity
    {
        public Guid TripId { get; private set; }

        public int SlotIndex { get; private set; }

        public PersonName Name { get; private set; }

        private TripParticipant() { }

        public TripParticipant(Guid tripId, int slotIndex, PersonName name)
        {
            TripId = tripId;
            SlotIndex = Math.Clamp(slotIndex, 0, 4);
            Name = name ?? new PersonName("", "");
        }

        public void UpdateName(PersonName name) => Name = name ?? new PersonName("", "");
    }
}
