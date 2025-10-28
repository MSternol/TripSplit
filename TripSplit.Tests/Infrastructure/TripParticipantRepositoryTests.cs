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
    public class TripParticipantRepositoryTests
    {
        [Fact]
        public async Task UpsertRange_Updates_Existing_And_Inserts_New_And_List_Orders_By_Slot()
        {
            using var db = TestDb.NewContext();
            var tripId = Guid.NewGuid();

            db.TripParticipants.Add(new TripParticipant(tripId, 0, new PersonName("Jan", "K")));
            db.TripParticipants.Add(new TripParticipant(tripId, 2, new PersonName("Ola", "Z")));
            await db.SaveChangesAsync();

            var repo = new TripParticipantRepository(db);

            var incoming = new[]
            {
            new TripParticipant(tripId, 0, new PersonName("Janusz","Kowal")),
            new TripParticipant(tripId, 1, new PersonName("Ala","Makota")),
            new TripParticipant(tripId, 2, new PersonName("Aleksandra","Z"))
        };

            await repo.UpsertRangeAsync(incoming);
            await db.SaveChangesAsync();

            var list = await repo.ListAsync(tripId);
            list.Select(x => x.SlotIndex).Should().Equal(0, 1, 2);
            list[0].Name.ToString().Should().Be("Janusz Kowal");
            list[1].Name.ToString().Should().Be("Ala Makota");
            list[2].Name.ToString().Should().Be("Aleksandra Z");
        }

        [Fact]
        public async Task RemoveAll_Removes_All_For_Trip()
        {
            using var db = TestDb.NewContext();
            var tripId = Guid.NewGuid();

            db.TripParticipants.Add(new TripParticipant(tripId, 0, new PersonName("A", "B")));
            db.TripParticipants.Add(new TripParticipant(tripId, 1, new PersonName("C", "D")));
            await db.SaveChangesAsync();

            var repo = new TripParticipantRepository(db);
            await repo.RemoveAllAsync(tripId);
            await db.SaveChangesAsync();

            (await repo.ListAsync(tripId)).Should().BeEmpty();
        }
    }
}
