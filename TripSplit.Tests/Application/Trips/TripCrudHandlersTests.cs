using AutoMapper;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using TripSplit.Application.Abstractions;
using TripSplit.Application.DTOs;
using TripSplit.Application.Features.Trips.CreateTrip;
using TripSplit.Application.Features.Trips.DeleteTrip;
using TripSplit.Application.Features.Trips.GetTripDetails;
using TripSplit.Application.Features.Trips.ListTrips;
using TripSplit.Domain.Entities;
using TripSplit.Domain.Repositories;
using TripSplit.Domain.ValueObjects;

namespace TripSplit.Tests.Application.Trips
{
    public class TripCrudHandlersTests
    {
        private static IMapper Mapper() => TripSplit.Tests.Support.MapperFactory.Create();


        [Fact]
        public async Task CreateTrip_Adds_And_Saves_Returns_Id()
        {
            var userId = Guid.NewGuid();
            var current = new Mock<ICurrentUserService>();
            current.Setup(x => x.GetUserId()).Returns(userId);

            var repo = new Mock<ITripRepository>();
            var uow = new Mock<IUnitOfWork>();
            uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var handler = new CreateTripHandler(current.Object, repo.Object, uow.Object);

            var id = await handler.Handle(new CreateTripCommand(
                startName: "Kraków", endName: "Gdańsk",
                startLat: 50, startLon: 19, endLat: 54, endLon: 18.6,
                distanceKm: 500, fuelPricePerL: 6.5m, averageConsumptionLper100: 6.8,
                litersUsed: null, peopleCount: 4, parkingCost: 10, extraCosts: 5, carId: null),
                CancellationToken.None);

            id.Should().NotBeEmpty();
            repo.Verify(r => r.AddAsync(It.IsAny<Trip>(), It.IsAny<CancellationToken>()), Times.Once);
            uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetTripDetails_Maps_When_Owned()
        {
            var mapper = Mapper();
            var userId = Guid.NewGuid();
            var current = new Mock<ICurrentUserService>();
            current.Setup(x => x.GetUserId()).Returns(userId);

            var repo = new Mock<ITripRepository>();

            var trip = new Trip(userId, DateTime.UtcNow,
                new Location("A", 0, 0), new Location("B", 1, 1),
                100, 6m, 6.5, null, 2, 0, 0);

            repo.Setup(r => r.GetAsync(trip.Id, userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(trip);

            var handler = new GetTripDetailsHandler(repo.Object, current.Object, mapper);
            var dto = await handler.Handle(new GetTripDetailsQuery(trip.Id), CancellationToken.None);

            dto.Should().NotBeNull();
            dto!.StartName.Should().Be("A");
            dto.EndName.Should().Be("B");
        }

        [Fact]
        public async Task ListTrips_Maps_All_For_User()
        {
            var mapper = Mapper();
            var userId = Guid.NewGuid();
            var current = new Mock<ICurrentUserService>();
            current.Setup(x => x.GetUserId()).Returns(userId);

            var repo = new Mock<ITripRepository>();
            repo.Setup(r => r.ListAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Trip>
                {
                new Trip(userId, DateTime.UtcNow, new Location("A",0,0), new Location("B",1,1), 10,6,6,null,2,0,0),
                new Trip(userId, DateTime.UtcNow, new Location("C",0,0), new Location("D",1,1), 20,6,6,null,2,0,0),
                });

            var handler = new ListTripsHandler(repo.Object, current.Object, mapper);
            var list = await handler.Handle(new ListTripsQuery(), CancellationToken.None);

            list.Should().HaveCount(2).And.AllBeOfType<TripDto>();
        }

        [Fact]
        public async Task DeleteTrip_Removes_And_Saves()
        {
            var userId = Guid.NewGuid();
            var current = new Mock<ICurrentUserService>();
            current.Setup(x => x.GetUserId()).Returns(userId);

            var repo = new Mock<ITripRepository>();
            var uow = new Mock<IUnitOfWork>();
            uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var trip = new Trip(userId, DateTime.UtcNow, new Location("A", 0, 0), new Location("B", 1, 1), 10, 6, 6, null, 2, 0, 0);
            repo.Setup(r => r.GetAsync(trip.Id, userId, It.IsAny<CancellationToken>())).ReturnsAsync(trip);

            var handler = new DeleteTripHandler(repo.Object, current.Object, uow.Object);
            var ok = await handler.Handle(new DeleteTripCommand(trip.Id), CancellationToken.None);

            ok.Should().BeTrue();
            repo.Verify(r => r.RemoveAsync(trip, It.IsAny<CancellationToken>()), Times.Once);
            uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
