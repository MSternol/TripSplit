using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Domain.Entities;

namespace TripSplit.Domain.Repositories
{
    public interface ICarRepository : IRepository<Car>
    {
        Task<Car?> GetAsync(Guid id, Guid ownerUserId, CancellationToken ct = default);
        Task<IReadOnlyList<Car>> ListAsync(Guid ownerUserId, CancellationToken ct = default);
        Task<bool> ExistsAsync(Guid id, Guid ownerUserId, CancellationToken ct = default);
    }
}
