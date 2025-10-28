using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripSplit.Domain.ValueObjects
{
    public sealed class PersonName : IEquatable<PersonName>
    {
        public string FirstName { get; }
        public string LastName { get; }

        public PersonName(string firstName, string lastName)
        {
            FirstName = (firstName ?? string.Empty).Trim();
            LastName = (lastName ?? string.Empty).Trim();
        }

        public bool IsEmpty => string.IsNullOrWhiteSpace(FirstName) && string.IsNullOrWhiteSpace(LastName);

        public bool Matches(string firstName, string lastName)
            => string.Equals(FirstName, (firstName ?? "").Trim(), StringComparison.OrdinalIgnoreCase)
            && string.Equals(LastName, (lastName ?? "").Trim(), StringComparison.OrdinalIgnoreCase);

        public bool Equals(PersonName? other)
            => other is not null && Matches(other.FirstName, other.LastName);

        public override bool Equals(object? obj) => Equals(obj as PersonName);
        public override int GetHashCode() => HashCode.Combine(FirstName.ToLowerInvariant(), LastName.ToLowerInvariant());
        public override string ToString() => $"{FirstName} {LastName}".Trim();
    }
}
