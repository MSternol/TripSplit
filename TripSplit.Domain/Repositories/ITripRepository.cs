using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Domain.Entities;

namespace TripSplit.Domain.Repositories
{
    public interface ITripRepository : IRepository<Trip>
    {
        Task<Trip?> GetAsync(Guid id, Guid ownerUserId, CancellationToken ct = default);
        Task<IReadOnlyList<Trip>> ListAsync(Guid ownerUserId, CancellationToken ct = default);
        Task<bool> ExistsAsync(Guid id, Guid ownerUserId, CancellationToken ct = default);
        Task<Trip?> GetByIdAsync(Guid id, CancellationToken ct = default);
    }
}
