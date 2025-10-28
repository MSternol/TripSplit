using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TripSplit.Application.Abstractions;
using TripSplit.Application.DTOs;
using TripSplit.Domain.Repositories;

namespace TripSplit.Application.Features.Cars.GetCarDetails
{
    public sealed class GetCarDetailsHandler(
        ICarRepository cars,
        ICurrentUserService current,
        IMapper mapper
    ) : IRequestHandler<GetCarDetailsQuery, CarDto?>
    {
        public async Task<CarDto?> Handle(GetCarDetailsQuery r, CancellationToken ct)
        {
            var car = await cars.GetAsync(r.Id, current.GetUserId(), ct);
            return car is null ? null : mapper.Map<CarDto>(car);
        }
    }
}
