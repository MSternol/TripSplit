using System;
using MediatR;
using TripSplit.Application.DTOs;

namespace TripSplit.Application.Features.Cars.GetCarDetails
{
    public sealed record GetCarDetailsQuery(Guid Id) : IRequest<CarDto?>;
}
