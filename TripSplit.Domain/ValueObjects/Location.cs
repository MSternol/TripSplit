using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripSplit.Domain.ValueObjects
{
    public sealed class Location : IEquatable<Location>
    {
        public string Name { get; }
        public double Latitude { get; }
        public double Longitude { get; }


        public Location(string name, double latitude, double longitude)
        {
            Name = name ?? string.Empty;
            Latitude = latitude;
            Longitude = longitude;
        }


        public bool Equals(Location? other)
        => other is not null && Latitude.Equals(other.Latitude) && Longitude.Equals(other.Longitude) && Name == other.Name;


        public override bool Equals(object? obj) => Equals(obj as Location);
        public override int GetHashCode() => HashCode.Combine(Name, Latitude, Longitude);
    }
}
