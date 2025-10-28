using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripSplit.Domain.ValueObjects
{
    public readonly struct DistanceKm : IEquatable<DistanceKm>
    {
        public double Value { get; }
        public DistanceKm(double value) { Value = value < 0 ? 0 : Math.Round(value, 3); }
        public static implicit operator double(DistanceKm d) => d.Value;
        public static DistanceKm From(double value) => new(value);
        public override int GetHashCode() => Value.GetHashCode();
        public bool Equals(DistanceKm other) => Value.Equals(other.Value);
        public override bool Equals(object? obj) => obj is DistanceKm d && Equals(d);
        public override string ToString() => $"{Value:0.###}";
    }
}
