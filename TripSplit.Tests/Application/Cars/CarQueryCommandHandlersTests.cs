using AutoMapper;
using FluentAssertions;
using Moq;
using System.Threading;
using TripSplit.Application.Abstractions;
using TripSplit.Application.DTOs;
using TripSplit.Application.Features.Cars.DeleteCar;
using TripSplit.Application.Features.Cars.GetCarDetails;
using TripSplit.Application.Features.Cars.ListCars;
using TripSplit.Domain.Entities;
using TripSplit.Domain.Enums;
using TripSplit.Domain.Repositories;
using Xunit;

namespace TripSplit.Tests.Application.Cars
{
    public class CarQueryCommandHandlersTests
    {
        private static (IMapper mapper, Guid user) SetupMapperAndUser()
        {
            var mapper = TripSplit.Tests.Support.MapperFactory.Create();
            return (mapper, Guid.NewGuid());
        }

        [Fact]
        public async Task ListCars_Returns_Mapped_Dtos_For_Current_User()
        {
            var (mapper, userId) = SetupMapperAndUser();

            var current = new Mock<ICurrentUserService>();
            current.Setup(x => x.GetUserId()).Returns(userId);

            var repo = new Mock<ICarRepository>();
            repo.Setup(r => r.ListAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new[]
                {
                    new Car(userId, "Astra", FuelType.Petrol, 6.8, 50),
                    new Car(userId, "Octavia", FuelType.Diesel, 5.2, 55)
                });

            var handler = new ListCarsHandler(repo.Object, current.Object, mapper);
            var result = await handler.Handle(new ListCarsQuery(), CancellationToken.None);

            result.Should().HaveCount(2);
            result.Should().AllBeOfType<CarDto>();
        }

        [Fact]
        public async Task GetCarDetails_Respects_Ownership()
        {
            var (mapper, userId) = SetupMapperAndUser();
            var current = new Mock<ICurrentUserService>();
            current.Setup(x => x.GetUserId()).Returns(userId);

            var repo = new Mock<ICarRepository>();
            var id = Guid.NewGuid();

            repo.Setup(r => r.GetAsync(id, userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Car(userId, "Fabia", FuelType.Petrol, 6.0, 45));

            var handler = new GetCarDetailsHandler(repo.Object, current.Object, mapper);

            var dto = await handler.Handle(new GetCarDetailsQuery(id), CancellationToken.None);

            dto.Should().NotBeNull();
            dto!.Name.Should().Be("Fabia");
        }

        [Fact]
        public async Task DeleteCar_Removes_And_Saves()
        {
            var userId = Guid.NewGuid();
            var current = new Mock<ICurrentUserService>();
            current.Setup(x => x.GetUserId()).Returns(userId);

            var repo = new Mock<ICarRepository>();
            var uow = new Mock<IUnitOfWork>();
            uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var id = Guid.NewGuid();
            repo.Setup(r => r.GetAsync(id, userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Car(userId, "Fabia", FuelType.Petrol, 6.0, 45));

            var handler = new DeleteCarHandler(repo.Object, current.Object, uow.Object);

            var ok = await handler.Handle(new DeleteCarCommand(id), CancellationToken.None);

            ok.Should().BeTrue();
            repo.Verify(r => r.RemoveAsync(It.IsAny<Car>(), It.IsAny<CancellationToken>()), Times.Once);
            uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
