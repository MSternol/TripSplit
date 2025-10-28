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
    public sealed class CarInsuranceRepository : ICarInsuranceRepository
    {
        private readonly AppDbContext _db;
        public CarInsuranceRepository(AppDbContext db) => _db = db;

        public Task AddAsync(CarInsurance entity, CancellationToken ct = default)
            => _db.CarInsurances.AddAsync(entity, ct).AsTask();

        public Task RemoveAsync(CarInsurance entity, CancellationToken ct = default)
        {
            _db.CarInsurances.Remove(entity);
            return Task.CompletedTask;
        }

        public Task<CarInsurance?> GetAsync(Guid id, CancellationToken ct = default)
            => _db.CarInsurances.FirstOrDefaultAsync(x => x.Id == id, ct);

        public Task<bool> ExistsAsync(Guid id, CancellationToken ct = default)
            => _db.CarInsurances.AnyAsync(x => x.Id == id, ct);

        public async Task<IReadOnlyList<CarInsurance>> ListAsync(CancellationToken ct = default)
            => await _db.CarInsurances.AsNoTracking().ToListAsync(ct);

        public async Task<CarInsurance?> GetByCarIdAsync(Guid carId, Guid ownerUserId, CancellationToken ct = default)
            => await _db.CarInsurances
                        .Where(i => i.CarId == carId)
                        .Join(_db.Cars, i => i.CarId, c => c.Id, (i, c) => new { i, c })
                        .Where(x => x.c.OwnerUserId == ownerUserId)
                        .Select(x => x.i)
                        .FirstOrDefaultAsync(ct);

        public Task<bool> ExistsForCarAsync(Guid carId, Guid ownerUserId, CancellationToken ct = default)
            => _db.CarInsurances
                  .Where(i => i.CarId == carId)
                  .Join(_db.Cars, i => i.CarId, c => c.Id, (i, c) => c.OwnerUserId)
                  .AnyAsync(uid => uid == ownerUserId, ct);
    }
}
