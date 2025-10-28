using AutoMapper;
using FluentAssertions;
using TripSplit.Application.DTOs;
using TripSplit.Application.Mapping;
using TripSplit.Domain.Entities;
using TripSplit.Domain.Enums;
using TripSplit.Domain.ValueObjects;
using TripSplit.Tests.Support;
using Xunit;

namespace TripSplit.Tests.Application.Mapping
{
    public class ApplicationMappingProfileTests
    {
        private readonly IMapper _mapper = TripSplit.Tests.Support.MapperFactory.Create();

        [Fact]
        public void Trip_Maps_To_TripDto()
        {
            var trip = new Trip(Guid.NewGuid(), DateTime.UtcNow,
                new Location("Kraków", 50, 19), new Location("Gdańsk", 54, 18.6),
                500, 6.5m, 6.8, null, 4, 10, 5);

            var dto = _mapper.Map<TripSplit.Application.DTOs.TripDto>(trip);

            dto.StartName.Should().Be("Kraków");
            dto.EndName.Should().Be("Gdańsk");
            dto.DistanceKm.Should().Be(500);
            dto.CostPerPerson.Should().Be(trip.CostPerPerson);
        }

        [Fact]
        public void Car_Maps_To_CarDto()
        {
            var car = new Car(Guid.NewGuid(), "Octavia", FuelType.Diesel, 5.1, 55);
            var dto = _mapper.Map<TripSplit.Application.DTOs.CarDto>(car);
            dto.Name.Should().Be("Octavia");
            dto.AverageConsumptionLper100.Should().Be(5.1);
        }
    }
}
