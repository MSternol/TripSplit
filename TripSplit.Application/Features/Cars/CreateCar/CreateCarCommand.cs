using System;
using MediatR;
using TripSplit.Domain.Enums;

namespace TripSplit.Application.Features.Cars.CreateCar
{
    public sealed record CreateCarCommand(
         string name,
         FuelType fuelType,
         double averageConsumptionLper100,
         double tankCapacityL,
         bool? remindersEnabled,
         ReminderLeadTime? reminderLeadTime
     ) : IRequest<Guid>;
}
