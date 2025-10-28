using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripSplit.Domain.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T entity, CancellationToken ct = default);
        Task RemoveAsync(T entity, CancellationToken ct = default);
    }
}
