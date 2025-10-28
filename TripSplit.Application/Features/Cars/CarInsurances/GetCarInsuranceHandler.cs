using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Application.Abstractions;
using TripSplit.Application.DTOs;
using TripSplit.Domain.Repositories;

namespace TripSplit.Application.Features.Cars.CarInsurances
{
    public sealed class GetCarInsuranceHandler(
        ICarInsuranceRepository insRepo,
        ICurrentUserService current,
        ICarRepository cars
    ) : IRequestHandler<GetCarInsuranceQuery, CarInsuranceDto?>
    {
        public async Task<CarInsuranceDto?> Handle(GetCarInsuranceQuery r, CancellationToken ct)
        {
            var car = await cars.GetAsync(r.CarId, current.GetUserId(), ct);
            if (car is null) return null;

            var ins = await insRepo.GetByCarIdAsync(r.CarId, current.GetUserId(), ct);
            if (ins is null) return null;

            return new CarInsuranceDto
            {
                Id = ins.Id,
                CarId = ins.CarId,
                Company = ins.Company,
                PolicyNumber = ins.PolicyNumber,
                ValidFrom = ins.ValidFrom,
                ValidTo = ins.ValidTo
            };
        }
    }
}
