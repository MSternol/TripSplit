using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Application.DTOs;

namespace TripSplit.Application.Features.Cars.CarInspections
{
    public sealed record GetCarInspectionQuery(Guid CarId) : IRequest<CarInspectionDto?>;
}
