using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Application.Abstractions;
using TripSplit.Application.DTOs;
using TripSplit.Domain.Repositories;

namespace TripSplit.Application.Features.Cars.CarInspections
{
    public sealed class GetCarInspectionHandler(
        ICarInspectionRepository inspRepo,
        ICurrentUserService current,
        ICarRepository cars
    ) : IRequestHandler<GetCarInspectionQuery, CarInspectionDto?>
    {
        public async Task<CarInspectionDto?> Handle(GetCarInspectionQuery r, CancellationToken ct)
        {
            var car = await cars.GetAsync(r.CarId, current.GetUserId(), ct);
            if (car is null) return null;

            var insp = await inspRepo.GetByCarIdAsync(r.CarId, current.GetUserId(), ct);
            if (insp is null) return null;

            return new CarInspectionDto
            {
                Id = insp.Id,
                CarId = insp.CarId,
                ValidFrom = insp.ValidFrom,
                ValidTo = insp.ValidTo
            };
        }
    }
}
