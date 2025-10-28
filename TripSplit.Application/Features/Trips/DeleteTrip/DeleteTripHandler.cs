using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Application.Abstractions;
using TripSplit.Domain.Repositories;

namespace TripSplit.Application.Features.Trips.DeleteTrip
{
    public sealed class DeleteTripHandler(
     ITripRepository trips,
     ICurrentUserService current,
     IUnitOfWork uow
 ) : IRequestHandler<DeleteTripCommand, bool>
    {
        public async Task<bool> Handle(DeleteTripCommand r, CancellationToken ct)
        {
            var entity = await trips.GetAsync(r.Id, current.GetUserId(), ct);
            if (entity is null) return false;

            await trips.RemoveAsync(entity, ct);
            await uow.SaveChangesAsync(ct);
            return true;
        }
    }
}
