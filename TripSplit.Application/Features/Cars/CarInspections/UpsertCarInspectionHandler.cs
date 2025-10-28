using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Application.Abstractions;
using TripSplit.Domain.Entities;
using TripSplit.Domain.Repositories;

namespace TripSplit.Application.Features.Cars.CarInspections
{
    public sealed class UpsertCarInspectionHandler(
        ICarRepository cars,
        ICarInspectionRepository inspRepo,
        ICurrentUserService current,
        IUnitOfWork uow
    ) : IRequestHandler<UpsertCarInspectionCommand, Guid>
    {
        public async Task<Guid> Handle(UpsertCarInspectionCommand r, CancellationToken ct)
        {
            var car = await cars.GetAsync(r.CarId, current.GetUserId(), ct);
            if (car is null) return Guid.Empty;

            var existing = await inspRepo.GetByCarIdAsync(r.CarId, current.GetUserId(), ct);
            if (existing is null)
            {
                var entity = new CarInspection(r.CarId, r.ValidFrom, r.ValidTo);
                await inspRepo.AddAsync(entity, ct);
                await uow.SaveChangesAsync(ct);
                return entity.Id;
            }
            else
            {
                existing.Update(r.ValidFrom, r.ValidTo);
                await uow.SaveChangesAsync(ct);
                return existing.Id;
            }
        }
    }
}
