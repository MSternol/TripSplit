using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Domain.Common;

namespace TripSplit.Domain.Entities
{
    public sealed class TripShareLink : BaseEntity
    {
        public Guid TripId { get; private set; }
        public string Token { get; private set; } = "";
        public DateTime CreatedAtUtc { get; private set; }
        public DateTime? ExpiresAtUtc { get; private set; }
        public bool IsActive { get; private set; } = true;

        private TripShareLink() { }

        public TripShareLink(Guid tripId, string token, DateTime? expiresAtUtc = null)
        {
            TripId = tripId;
            Token = token;
            CreatedAtUtc = DateTime.UtcNow;
            ExpiresAtUtc = expiresAtUtc;
            IsActive = true;
        }

        public bool IsValidNowUtc(DateTime utcNow) =>
            IsActive && (ExpiresAtUtc is null || utcNow <= ExpiresAtUtc.Value);

        public void Deactivate() => IsActive = false;
    }
}
