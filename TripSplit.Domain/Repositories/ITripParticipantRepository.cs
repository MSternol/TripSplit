using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Domain.Entities;

namespace TripSplit.Domain.Repositories
{
    public interface ITripParticipantRepository
    {
        Task<IReadOnlyList<TripParticipant>> ListAsync(Guid tripId, CancellationToken ct = default);
        Task UpsertRangeAsync(IEnumerable<TripParticipant> items, CancellationToken ct = default);
        Task RemoveAllAsync(Guid tripId, CancellationToken ct = default);
    }
}
