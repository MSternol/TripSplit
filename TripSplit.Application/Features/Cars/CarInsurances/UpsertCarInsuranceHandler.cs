using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Application.Abstractions;
using TripSplit.Domain.Entities;
using TripSplit.Domain.Repositories;

namespace TripSplit.Application.Features.Cars.CarInsurances
{
    public sealed class UpsertCarInsuranceHandler(
        ICarRepository cars,
        ICarInsuranceRepository insRepo,
        ICurrentUserService current,
        IUnitOfWork uow
    ) : IRequestHandler<UpsertCarInsuranceCommand, Guid>
    {
        public async Task<Guid> Handle(UpsertCarInsuranceCommand r, CancellationToken ct)
        {
            var car = await cars.GetAsync(r.CarId, current.GetUserId(), ct);
            if (car is null) return Guid.Empty;

            var existing = await insRepo.GetByCarIdAsync(r.CarId, current.GetUserId(), ct);
            if (existing is null)
            {
                var entity = new CarInsurance(r.CarId, r.Company, r.PolicyNumber, r.ValidFrom, r.ValidTo);
                await insRepo.AddAsync(entity, ct);
                await uow.SaveChangesAsync(ct);
                return entity.Id;
            }
            else
            {
                existing.Update(r.Company, r.PolicyNumber, r.ValidFrom, r.ValidTo);
                await uow.SaveChangesAsync(ct);
                return existing.Id;
            }
        }
    }
}
