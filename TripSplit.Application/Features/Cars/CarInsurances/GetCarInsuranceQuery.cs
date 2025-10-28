using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Application.DTOs;

namespace TripSplit.Application.Features.Cars.CarInsurances
{
    public sealed record GetCarInsuranceQuery(Guid CarId) : IRequest<CarInsuranceDto?>;
}
