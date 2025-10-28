using FluentAssertions;
using Moq;
using TripSplit.Application.Abstractions;
using TripSplit.Application.Features.Cars.CreateCar;
using TripSplit.Domain.Entities;
using TripSplit.Domain.Enums;
using TripSplit.Domain.Repositories;
using Xunit;

namespace TripSplit.Tests.Application.Cars
{
    public class CreateCarHandlerTests
    {
        [Fact]
        public async Task Creates_Car_And_Returns_Id()
        {
            // arrange
            var currentUserId = Guid.NewGuid();
            var current = new Mock<ICurrentUserService>();
            current.Setup(x => x.GetUserId()).Returns(currentUserId);

            var repo = new Mock<ICarRepository>();
            repo.Setup(r => r.AddAsync(It.IsAny<Car>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var handler = new CreateCarHandler(current.Object, repo.Object, uow.Object);

            // act
            var id = await handler.Handle(
                new CreateCarCommand(
                    name: "Fabia",
                    fuelType: FuelType.Petrol,
                    averageConsumptionLper100: 6.2,
                    tankCapacityL: 45,
                    remindersEnabled: null,
                    reminderLeadTime: null
                ),
                CancellationToken.None
            );

            // assert
            id.Should().NotBeEmpty();
            repo.Verify(r => r.AddAsync(It.IsAny<Car>(), It.IsAny<CancellationToken>()), Times.Once);
            uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
