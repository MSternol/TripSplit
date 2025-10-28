using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Application.DTOs;

namespace TripSplit.Application.Features.Cars.ListCars
{
    public sealed record ListCarsQuery() : IRequest<IReadOnlyList<CarDto>>;
}
