using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Application.Abstractions;
using TripSplit.Domain.Repositories;

namespace TripSplit.Application.Features.Cars.DeleteCar
{
    public sealed class DeleteCarHandler(
    ICarRepository cars,
    ICurrentUserService current,
    IUnitOfWork uow
) : IRequestHandler<DeleteCarCommand, bool>
    {
        public async Task<bool> Handle(DeleteCarCommand r, CancellationToken ct)
        {
            var entity = await cars.GetAsync(r.Id, current.GetUserId(), ct);
            if (entity is null) return false;

            await cars.RemoveAsync(entity, ct);
            await uow.SaveChangesAsync(ct);
            return true;
        }
    }
}
