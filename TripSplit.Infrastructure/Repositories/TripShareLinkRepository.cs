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
    public sealed class TripShareLinkRepository(AppDbContext db) : ITripShareLinkRepository
    {
        public async Task AddAsync(TripShareLink link, CancellationToken ct = default)
            => await db.TripShareLinks.AddAsync(link, ct);

        public Task<TripShareLink?> GetByTokenAsync(string token, CancellationToken ct = default)
            => db.TripShareLinks.FirstOrDefaultAsync(x => x.Token == token, ct);

        public Task DeactivateAsync(TripShareLink link, CancellationToken ct = default)
        {
            link.Deactivate();
            return Task.CompletedTask;
        }
    }
}
