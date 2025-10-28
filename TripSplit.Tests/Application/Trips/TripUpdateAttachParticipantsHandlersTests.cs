using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Application.Abstractions;
using TripSplit.Application.Features.Trips.AttachCarToTrip;
using TripSplit.Application.Features.Trips.SetParticipants;
using TripSplit.Application.Features.Trips.UpdateTripCosts;
using TripSplit.Domain.Entities;
using TripSplit.Domain.Repositories;
using TripSplit.Domain.ValueObjects;

namespace TripSplit.Tests.Application.Trips
{
    public class TripUpdateAttachParticipantsHandlersTests
    {
        [Fact]
        public async Task UpdateTripCosts_Recalculates_And_Saves()
        {
            var user = Guid.NewGuid();
            var current = new Mock<ICurrentUserService>();
            current.Setup(x => x.GetUserId()).Returns(user);

            var repo = new Mock<ITripRepository>();
            var uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var trip = new Trip(user, DateTime.UtcNow, new Location("A", 0, 0), new Location("B", 1, 1), 100, 6, 6, null, 2, 0, 0);
            repo.Setup(r => r.GetAsync(trip.Id, user, It.IsAny<CancellationToken>())).ReturnsAsync(trip);

            var handler = new UpdateTripCostsHandler(repo.Object, current.Object, uow.Object);

            var ok = await handler.Handle(new UpdateTripCostsCommand(
                trip.Id, FuelPricePerL: 6.99m, ParkingCost: 10m, ExtraCosts: 0m,
                AverageConsumptionLper100: 7.2, LitersUsed: null, PeopleCount: 3, DistanceKm: 150),
                CancellationToken.None);

            ok.Should().BeTrue();
            trip.PeopleCount.Should().Be(3);
            uow.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task AttachCarToTrip_Sets_Snapshot_And_Saves()
        {
            var user = Guid.NewGuid();
            var current = new Mock<ICurrentUserService>();
            current.Setup(x => x.GetUserId()).Returns(user);

            var repo = new Mock<ITripRepository>();
            var uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var trip = new Trip(user, DateTime.UtcNow, new Location("A", 0, 0), new Location("B", 1, 1), 100, 6, 6, null, 2, 0, 0);
            repo.Setup(r => r.GetAsync(trip.Id, user, It.IsAny<CancellationToken>())).ReturnsAsync(trip);

            var handler = new AttachCarToTripHandler(repo.Object, current.Object, uow.Object);
            var ok = await handler.Handle(new AttachCarToTripCommand(trip.Id, Guid.NewGuid(), "Octavia", 5.8), CancellationToken.None);

            ok.Should().BeTrue();
            trip.CarName.Should().Be("Octavia");
            uow.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SetParticipants_Replaces_List_And_Saves()
        {
            var user = Guid.NewGuid();
            var current = new Mock<ICurrentUserService>();
            current.Setup(x => x.GetUserId()).Returns(user);

            var trips = new Mock<ITripRepository>();
            var parts = new Mock<ITripParticipantRepository>();
            var uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var trip = new Trip(user, DateTime.UtcNow, new Location("A", 0, 0), new Location("B", 1, 1), 100, 6, 6, null, 2, 0, 0);
            trips.Setup(r => r.GetAsync(trip.Id, user, It.IsAny<CancellationToken>())).ReturnsAsync(trip);

            var handler = new SetParticipantsHandler(current.Object, trips.Object, parts.Object, uow.Object);

            var ok = await handler.Handle(new SetParticipantsCommand(
                trip.Id,
                Driver: ("Jan", "Kowalski"),
                Passenger1: ("Ala", "Makota"),
                Passenger2: null, Passenger3: null, Passenger4: null),
                CancellationToken.None);

            ok.Should().BeTrue();
            parts.Verify(p => p.RemoveAllAsync(trip.Id, It.IsAny<CancellationToken>()), Times.Once);
            parts.Verify(p => p.UpsertRangeAsync(It.Is<IEnumerable<TripParticipant>>(list => list.Count() == 2),
                It.IsAny<CancellationToken>()), Times.Once);
            uow.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
