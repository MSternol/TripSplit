using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Application.Abstractions;
using TripSplit.Domain.Repositories;

namespace TripSplit.Application.Features.Cars.CarInspections
{
    public sealed class DeleteCarInspectionHandler(
        ICarRepository cars,
        ICarInspectionRepository inspRepo,
        ICurrentUserService current,
        IUnitOfWork uow
    ) : IRequestHandler<DeleteCarInspectionCommand, bool>
    {
        public async Task<bool> Handle(DeleteCarInspectionCommand r, CancellationToken ct)
        {
            var userId = current.GetUserId();
            var car = await cars.GetAsync(r.CarId, userId, ct);
            if (car is null) return false;

            var existing = await inspRepo.GetByCarIdAsync(r.CarId, userId, ct);
            if (existing is null) return true;

            await inspRepo.RemoveAsync(existing, ct);
            await uow.SaveChangesAsync(ct);
            return true;
        }
    }
}
