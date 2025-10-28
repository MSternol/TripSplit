using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Domain.Entities;
using TripSplit.Domain.Enums;
using TripSplit.Infrastructure.Repositories;
using TripSplit.Tests.Support;

namespace TripSplit.Tests.Infrastructure
{
    public class EfUnitOfWorkTests
    {
        [Fact]
        public async Task SaveChanges_Writes_To_Database()
        {
            using var db = TestDb.NewContext();
            var uow = new EfUnitOfWork(db);
            var cars = new CarRepository(db);

            var car = new Car(Guid.NewGuid(), "Fabia", FuelType.Petrol, 6.1, 45);
            await cars.AddAsync(car);

            var written = await uow.SaveChangesAsync();
            written.Should().BeGreaterThan(0);

            (await cars.ExistsAsync(car.Id, car.OwnerUserId)).Should().BeTrue();
        }
    }
}
