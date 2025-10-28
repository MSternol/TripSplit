using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TripSplit.Application.Abstractions;
using TripSplit.Domain.Repositories;

namespace TripSplit.Application.Features.Cars.UpdateCar
{
    public sealed class UpdateCarHandler(
        ICarRepository cars,
        ICurrentUserService current,
        IUnitOfWork uow
    ) : IRequestHandler<UpdateCarCommand, bool>
    {
        public async Task<bool> Handle(UpdateCarCommand r, CancellationToken ct)
        {
            var car = await cars.GetAsync(r.id, current.GetUserId(), ct);
            if (car is null) return false;

            car.UpdateBasics(r.name, r.fuelType, r.averageConsumptionLper100, r.tankCapacityL);
            car.SetReminders(r.remindersEnabled, r.reminderLeadTime);

            await uow.SaveChangesAsync(ct);
            return true;
        }
    }
}
