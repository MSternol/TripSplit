using Microsoft.EntityFrameworkCore;
using TripSplit.Domain.Entities;
using TripSplit.Domain.Repositories;
using TripSplit.Infrastructure.Persistence;

namespace TripSplit.Infrastructure.Repositories
{
    public sealed class CarRepository : ICarRepository
    {
        private readonly AppDbContext _db;
        public CarRepository(AppDbContext db) => _db = db;

        public Task AddAsync(Car entity, CancellationToken ct = default)
            => _db.Cars.AddAsync(entity, ct).AsTask();

        public Task RemoveAsync(Car entity, CancellationToken ct = default)
        {
            _db.Cars.Remove(entity);
            return Task.CompletedTask;
        }

        public Task<Car?> GetAsync(Guid id, Guid ownerUserId, CancellationToken ct = default)
            => _db.Cars
                  .Include(x => x.Insurance)
                  .Include(x => x.Inspection)
                  .FirstOrDefaultAsync(x => x.Id == id && x.OwnerUserId == ownerUserId, ct);

        public async Task<IReadOnlyList<Car>> ListAsync(Guid ownerUserId, CancellationToken ct = default)
            => await _db.Cars.AsNoTracking()
                .Include(x => x.Insurance)
                .Include(x => x.Inspection)
                .Where(x => x.OwnerUserId == ownerUserId)
                .OrderBy(x => x.Name)
                .ToListAsync(ct);

        public Task<bool> ExistsAsync(Guid id, Guid ownerUserId, CancellationToken ct = default)
            => _db.Cars.AsNoTracking()
                       .AnyAsync(x => x.Id == id && x.OwnerUserId == ownerUserId, ct);
    }
}
