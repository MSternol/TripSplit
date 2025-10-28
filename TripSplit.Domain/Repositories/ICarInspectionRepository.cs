using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Domain.Entities;

namespace TripSplit.Domain.Repositories
{
    public interface ICarInspectionRepository : IRepository<CarInspection>
    {
        Task<CarInspection?> GetByCarIdAsync(Guid carId, Guid ownerUserId, CancellationToken ct = default);
        Task<bool> ExistsForCarAsync(Guid carId, Guid ownerUserId, CancellationToken ct = default);
    }
}
