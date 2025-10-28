using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Domain.Entities;
using TripSplit.Domain.Repositories;
using TripSplit.Infrastructure.Persistence;

namespace TripSplit.Infrastructure.Repositories
{
    public sealed class TripParticipantRepository(AppDbContext db) : ITripParticipantRepository
    {
        public async Task<IReadOnlyList<TripParticipant>> ListAsync(Guid tripId, CancellationToken ct = default)
            => await db.TripParticipants.AsNoTracking().Where(x => x.TripId == tripId)
                   .OrderBy(x => x.SlotIndex).ToListAsync(ct);

        public async Task UpsertRangeAsync(IEnumerable<TripParticipant> items, CancellationToken ct = default)
        {
            foreach (var item in items)
            {
                var existing = await db.TripParticipants
                    .FirstOrDefaultAsync(x => x.TripId == item.TripId && x.SlotIndex == item.SlotIndex, ct);
                if (existing is null)
                    await db.TripParticipants.AddAsync(item, ct);
                else
                    existing.UpdateName(item.Name);
            }
        }

        public async Task RemoveAllAsync(Guid tripId, CancellationToken ct = default)
        {
            var all = await db.TripParticipants.Where(x => x.TripId == tripId).ToListAsync(ct);
            db.TripParticipants.RemoveRange(all);
        }
    }
}
