using System;
using MediatR;
using TripSplit.Domain.Enums;

namespace TripSplit.Application.Features.Cars.UpdateCar
{
    public sealed record UpdateCarCommand(
        Guid id,
        string? name = null,
        FuelType? fuelType = null,
        double? averageConsumptionLper100 = null,
        double? tankCapacityL = null,
        bool? remindersEnabled = null,
        ReminderLeadTime? reminderLeadTime = null
    ) : IRequest<bool>;
}
