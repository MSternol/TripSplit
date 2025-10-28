using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TripSplit.Application.Abstractions;
using TripSplit.Domain.Entities;
using TripSplit.Domain.Repositories;

namespace TripSplit.Application.Features.Cars.CreateCar
{
    public sealed class CreateCarHandler(
        ICurrentUserService current,
        ICarRepository cars,
        IUnitOfWork uow
    ) : IRequestHandler<CreateCarCommand, Guid>
    {
        public async Task<Guid> Handle(CreateCarCommand r, CancellationToken ct)
        {
            var car = new Car(
                ownerUserId: current.GetUserId(),
                name: r.name,
                fuelType: r.fuelType,
                averageConsumptionLper100: r.averageConsumptionLper100,
                tankCapacityL: r.tankCapacityL
            );

            car.SetReminders(r.remindersEnabled, r.reminderLeadTime);

            await cars.AddAsync(car, ct);
            await uow.SaveChangesAsync(ct);

            return car.Id;
        }
    }
}
