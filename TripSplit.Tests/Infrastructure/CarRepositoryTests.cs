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
    public class CarRepositoryTests
    {
        [Fact]
        public async Task Add_Get_List_Exists_Remove_Work()
        {
            using var db = TestDb.NewContext();
            var repo = new CarRepository(db);

            var owner = Guid.NewGuid();
            var other = Guid.NewGuid();

            var c1 = new Car(owner, "Octavia", FuelType.Diesel, 5.2, 55);
            var c2 = new Car(owner, "Astra", FuelType.Petrol, 6.8, 50);
            var c3 = new Car(other, "Mazda", FuelType.Petrol, 7.1, 48);

            await repo.AddAsync(c1);
            await repo.AddAsync(c2);
            await repo.AddAsync(c3);
            await db.SaveChangesAsync();

            // Get
            (await repo.GetAsync(c1.Id, owner))!.Name.Should().Be("Octavia");
            (await repo.GetAsync(c1.Id, other)).Should().BeNull();

            // List (sort by Name)
            var listOwner = await repo.ListAsync(owner);
            listOwner.Select(x => x.Name).Should().Equal("Astra", "Octavia");

            // Exists
            (await repo.ExistsAsync(c2.Id, owner)).Should().BeTrue();
            (await repo.ExistsAsync(c2.Id, other)).Should().BeFalse();

            // Remove
            await repo.RemoveAsync(c2);
            await db.SaveChangesAsync();

            (await repo.ExistsAsync(c2.Id, owner)).Should().BeFalse();
        }
    }
}
