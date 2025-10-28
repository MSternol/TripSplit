using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Domain.Entities;
using TripSplit.Domain.ValueObjects;
using TripSplit.Infrastructure.Repositories;
using TripSplit.Tests.Support;

namespace TripSplit.Tests.Infrastructure
{
    public class TripRepositoryTests
    {
        private static Location L(string n) => new(n, 0, 0);

        [Fact]
        public async Task Add_Get_List_Exists_Remove_Work()
        {
            using var db = TestDb.NewContext();
            var repo = new TripRepository(db);

            var owner = Guid.NewGuid();
            var other = Guid.NewGuid();

            var t1 = new Trip(owner, DateTime.UtcNow.AddDays(-1), L("Kraków"), L("Gdańsk"), 500, 6.5m, 6.8, null, 3, 10, 0);
            var t2 = new Trip(owner, DateTime.UtcNow, L("Kraków"), L("Warszawa"), 300, 6.5m, 6.0, null, 2, 0, 5);
            var t3 = new Trip(other, DateTime.UtcNow, L("A"), L("B"), 100, 6.5m, 5.0, null, 2, 0, 0);

            await repo.AddAsync(t1);
            await repo.AddAsync(t2);
            await repo.AddAsync(t3);
            await db.SaveChangesAsync();

            // Get by owner filter
            (await repo.GetAsync(t1.Id, owner))!.DistanceKm.Should().Be(500);
            (await repo.GetAsync(t1.Id, other)).Should().BeNull();

            // List: ordered DESC by StartedAt
            var listOwner = await repo.ListAsync(owner);
            listOwner.Select(x => x.Id).Should().ContainInOrder(t2.Id, t1.Id);

            // Exists
            (await repo.ExistsAsync(t2.Id, owner)).Should().BeTrue();
            (await repo.ExistsAsync(t2.Id, other)).Should().BeFalse();

            // GetById (bez ownera)
            (await repo.GetByIdAsync(t3.Id))!.OwnerUserId.Should().Be(other);

            // Remove
            await repo.RemoveAsync(t1);
            await db.SaveChangesAsync();
            (await repo.GetAsync(t1.Id, owner)).Should().BeNull();
        }
    }
}
