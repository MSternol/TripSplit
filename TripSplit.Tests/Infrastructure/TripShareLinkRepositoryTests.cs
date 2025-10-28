using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Domain.Entities;
using TripSplit.Infrastructure.Repositories;
using TripSplit.Tests.Support;

namespace TripSplit.Tests.Infrastructure
{
    public class TripShareLinkRepositoryTests
    {
        [Fact]
        public async Task Add_Get_Deactivate_And_IsValidNow()
        {
            using var db = TestDb.NewContext();
            var repo = new TripShareLinkRepository(db);

            var link = new TripShareLink(Guid.NewGuid(), token: "abc123", expiresAtUtc: DateTime.UtcNow.AddHours(1));
            await repo.AddAsync(link);
            await db.SaveChangesAsync();

            var fetched = await repo.GetByTokenAsync("abc123");
            fetched.Should().NotBeNull();
            fetched!.IsValidNowUtc(DateTime.UtcNow).Should().BeTrue();

            await repo.DeactivateAsync(fetched);
            fetched.IsValidNowUtc(DateTime.UtcNow).Should().BeFalse();
        }

        [Fact]
        public async Task Expired_Link_Is_Not_Valid()
        {
            using var db = TestDb.NewContext();
            var repo = new TripShareLinkRepository(db);

            var link = new TripShareLink(Guid.NewGuid(), "expired", DateTime.UtcNow.AddMinutes(-1));
            await repo.AddAsync(link);
            await db.SaveChangesAsync();

            var fetched = await repo.GetByTokenAsync("expired");
            fetched!.IsValidNowUtc(DateTime.UtcNow).Should().BeFalse();
        }
    }
}
