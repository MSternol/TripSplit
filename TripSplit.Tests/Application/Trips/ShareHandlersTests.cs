using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Application.Features.Trips.Share;
using TripSplit.Domain.Entities;
using TripSplit.Domain.Repositories;
using TripSplit.Domain.ValueObjects;

namespace TripSplit.Tests.Application.Trips
{
    public class ShareHandlersTests
    {
        [Fact]
        public async Task GenerateShareLink_Creates_Token_And_Saves()
        {
            var currentUser = Guid.NewGuid();

            var current = new Mock<TripSplit.Application.Abstractions.ICurrentUserService>();
            current.Setup(c => c.GetUserId()).Returns(currentUser);

            var trips = new Mock<ITripRepository>();
            var links = new Mock<ITripShareLinkRepository>();
            var uow = new Mock<IUnitOfWork>();
            uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var trip = new Trip(currentUser, DateTime.UtcNow, new Location("A", 0, 0), new Location("B", 1, 1), 10, 6, 6, null, 2, 0, 0);
            trips.Setup(t => t.GetAsync(trip.Id, currentUser, It.IsAny<CancellationToken>())).ReturnsAsync(trip);

            var handler = new GenerateShareLinkHandler(current.Object, trips.Object, links.Object, uow.Object);
            var token = await handler.Handle(new GenerateShareLinkCommand(trip.Id, DateTime.UtcNow.AddHours(1)), CancellationToken.None);

            token.Should().NotBeNullOrWhiteSpace();
            links.Verify(l => l.AddAsync(It.Is<TripShareLink>(x => x.Token == token), It.IsAny<CancellationToken>()), Times.Once);
            uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ViewSharedTrip_Returns_TripDto_When_Token_Valid_And_Name_Matches_Or_NoParticipants()
        {
            var token = "t123";
            var trip = new Trip(Guid.NewGuid(), DateTime.UtcNow, new Location("A", 0, 0), new Location("B", 1, 1), 10, 6, 6, null, 2, 0, 0);
            var link = new TripShareLink(trip.Id, token, DateTime.UtcNow.AddHours(1));

            var links = new Mock<ITripShareLinkRepository>();
            links.Setup(l => l.GetByTokenAsync(token, It.IsAny<CancellationToken>())).ReturnsAsync(link);

            var trips = new Mock<ITripRepository>();
            trips.Setup(t => t.GetByIdAsync(trip.Id, It.IsAny<CancellationToken>())).ReturnsAsync(trip);

            var parts = new Mock<ITripParticipantRepository>();
            parts.Setup(p => p.ListAsync(trip.Id, It.IsAny<CancellationToken>())).ReturnsAsync(new List<TripParticipant>
        {
            new TripParticipant(trip.Id, 0, new PersonName("Jan","Kowalski"))
        });

            var handler = new ViewSharedTripHandler(links.Object, trips.Object, parts.Object);

            // dopasowanie po znormalizowanym tekście (EqualsLoose)
            var dto = await handler.Handle(new ViewSharedTripQuery(token, "jan", "KOWALSKI"), CancellationToken.None);

            dto.Should().NotBeNull();
            dto!.Id.Should().Be(trip.Id);
        }

        [Fact]
        public async Task DeactivateShareLink_Turns_Off_And_Saves()
        {
            var token = "abc";
            var link = new TripShareLink(Guid.NewGuid(), token, DateTime.UtcNow.AddHours(1));

            var links = new Mock<ITripShareLinkRepository>();
            links.Setup(l => l.GetByTokenAsync(token, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(link);

            links.Setup(l => l.DeactivateAsync(It.IsAny<TripShareLink>(), It.IsAny<CancellationToken>()))
                 .Callback<TripShareLink, CancellationToken>((lk, _) => lk.Deactivate())
                 .Returns(Task.CompletedTask);

            var uow = new Mock<IUnitOfWork>();
            uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var handler = new DeactivateShareLinkHandler(links.Object, uow.Object);

            var ok = await handler.Handle(new DeactivateShareLinkCommand(token), CancellationToken.None);

            ok.Should().BeTrue();
            link.IsActive.Should().BeFalse();
            uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
