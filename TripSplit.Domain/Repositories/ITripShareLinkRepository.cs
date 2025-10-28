using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Domain.Entities;

namespace TripSplit.Domain.Repositories
{
    public interface ITripShareLinkRepository
    {
        Task AddAsync(TripShareLink link, CancellationToken ct = default);
        Task<TripShareLink?> GetByTokenAsync(string token, CancellationToken ct = default);
        Task DeactivateAsync(TripShareLink link, CancellationToken ct = default);
    }
}
