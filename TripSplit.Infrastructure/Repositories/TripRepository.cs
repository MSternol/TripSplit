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
    public sealed class TripRepository : ITripRepository
    {
        private readonly AppDbContext _db;
        public TripRepository(AppDbContext db) => _db = db;

        public async Task AddAsync(Trip entity, CancellationToken ct = default)
            => await _db.Trips.AddAsync(entity, ct);

        public Task RemoveAsync(Trip entity, CancellationToken ct = default)
        {
            _db.Trips.Remove(entity);
            return Task.CompletedTask;
        }

        public Task<Trip?> GetAsync(Guid id, Guid ownerUserId, CancellationToken ct = default)
            => _db.Trips.FirstOrDefaultAsync(x => x.Id == id && x.OwnerUserId == ownerUserId, ct);

        public async Task<IReadOnlyList<Trip>> ListAsync(Guid ownerUserId, CancellationToken ct = default)
            => await _db.Trips
                        .AsNoTracking()
                        .Where(x => x.OwnerUserId == ownerUserId)
                        .OrderByDescending(x => x.StartedAt)
                        .ToListAsync(ct);

        public Task<bool> ExistsAsync(Guid id, Guid ownerUserId, CancellationToken ct = default)
            => _db.Trips.AsNoTracking().AnyAsync(x => x.Id == id && x.OwnerUserId == ownerUserId, ct);

        public Task<Trip?> GetByIdAsync(Guid id, CancellationToken ct = default)
            => _db.Trips.FirstOrDefaultAsync(x => x.Id == id, ct);

    }
}
