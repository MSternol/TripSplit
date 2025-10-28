using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Application.Abstractions;
using TripSplit.Domain.Repositories;

namespace TripSplit.Application.Features.Cars.CarInsurances
{
    public sealed class DeleteCarInsuranceHandler(
        ICarRepository cars,
        ICarInsuranceRepository insRepo,
        ICurrentUserService current,
        IUnitOfWork uow
    ) : IRequestHandler<DeleteCarInsuranceCommand, bool>
    {
        public async Task<bool> Handle(DeleteCarInsuranceCommand r, CancellationToken ct)
        {
            var userId = current.GetUserId();
            var car = await cars.GetAsync(r.CarId, userId, ct);
            if (car is null) return false;

            var existing = await insRepo.GetByCarIdAsync(r.CarId, userId, ct);
            if (existing is null) return true;

            await insRepo.RemoveAsync(existing, ct);
            await uow.SaveChangesAsync(ct);
            return true;
        }
    }
}
