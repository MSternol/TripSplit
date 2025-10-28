using System;
using TripSplit.Domain.Common;

namespace TripSplit.Domain.Entities
{
    public sealed class CarInsurance : BaseEntity
    {
        public Guid CarId { get; private set; }
        public string? Company { get; private set; }
        public string? PolicyNumber { get; private set; }
        public DateTime? ValidFrom { get; private set; }
        public DateTime? ValidTo { get; private set; }

        private CarInsurance() { }

        public CarInsurance(Guid carId, string? company, string? policy, DateTime? from, DateTime? to)
        {
            CarId = carId;
            Company = TrimOrNull(company);
            PolicyNumber = TrimOrNull(policy);
            ValidFrom = from;
            ValidTo = to;
        }

        public void Update(string? company, string? policy, DateTime? from, DateTime? to)
        {
            if (company is not null) Company = TrimOrNull(company);
            if (policy is not null) PolicyNumber = TrimOrNull(policy);
            if (from.HasValue) ValidFrom = from;
            if (to.HasValue) ValidTo = to;
        }

        private static string? TrimOrNull(string? s)
            => string.IsNullOrWhiteSpace(s) ? null : s.Trim();
    }

}
