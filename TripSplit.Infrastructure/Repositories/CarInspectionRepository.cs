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
    public sealed class CarInspectionRepository : ICarInspectionRepository
    {
        private readonly AppDbContext _db;
        public CarInspectionRepository(AppDbContext db) => _db = db;

        public Task AddAsync(CarInspection entity, CancellationToken ct = default)
            => _db.CarInspections.AddAsync(entity, ct).AsTask();

        public Task RemoveAsync(CarInspection entity, CancellationToken ct = default)
        {
            _db.CarInspections.Remove(entity);
            return Task.CompletedTask;
        }

        public Task<CarInspection?> GetAsync(Guid id, CancellationToken ct = default)
            => _db.CarInspections.FirstOrDefaultAsync(x => x.Id == id, ct);

        public Task<bool> ExistsAsync(Guid id, CancellationToken ct = default)
            => _db.CarInspections.AnyAsync(x => x.Id == id, ct);

        public async Task<IReadOnlyList<CarInspection>> ListAsync(CancellationToken ct = default)
            => await _db.CarInspections.AsNoTracking().ToListAsync(ct);

        public async Task<CarInspection?> GetByCarIdAsync(Guid carId, Guid ownerUserId, CancellationToken ct = default)
            => await _db.CarInspections
                        .Where(i => i.CarId == carId)
                        .Join(_db.Cars, i => i.CarId, c => c.Id, (i, c) => new { i, c })
                        .Where(x => x.c.OwnerUserId == ownerUserId)
                        .Select(x => x.i)
                        .FirstOrDefaultAsync(ct);

        public Task<bool> ExistsForCarAsync(Guid carId, Guid ownerUserId, CancellationToken ct = default)
            => _db.CarInspections
                  .Where(i => i.CarId == carId)
                  .Join(_db.Cars, i => i.CarId, c => c.Id, (i, c) => c.OwnerUserId)
                  .AnyAsync(uid => uid == ownerUserId, ct);
    }
}
