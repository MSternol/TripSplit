using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Domain.ValueObjects;

namespace TripSplit.Tests.Domain
{
    public class ValueObjectsTests
    {
        [Fact]
        public void PersonName_Equality_IsCaseInsensitive_AndTrims()
        {
            var a = new PersonName("  Jan ", "KOWALSKI ");
            var b = new PersonName("jan", "kOwAlSkI");

            a.Equals(b).Should().BeTrue();
            a.GetHashCode().Should().Be(b.GetHashCode());
            a.ToString().Should().Be("Jan KOWALSKI");
        }

        [Fact]
        public void DistanceKm_RoundsAndClamps()
        {
            var d1 = new DistanceKm(12.34567);
            d1.Value.Should().Be(12.346);

            var d2 = new DistanceKm(-5);
            d2.Value.Should().Be(0);
        }
    }
}
