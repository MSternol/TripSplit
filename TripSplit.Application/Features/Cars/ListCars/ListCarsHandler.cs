using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TripSplit.Application.Abstractions;
using TripSplit.Application.DTOs;
using TripSplit.Domain.Repositories;

namespace TripSplit.Application.Features.Cars.ListCars
{
    public sealed class ListCarsHandler(
        ICarRepository cars,
        ICurrentUserService current,
        IMapper mapper
    ) : IRequestHandler<ListCarsQuery, IReadOnlyList<CarDto>>
    {
        public async Task<IReadOnlyList<CarDto>> Handle(ListCarsQuery r, CancellationToken ct)
        {
            var items = await cars.ListAsync(current.GetUserId(), ct);
            return items.Select(mapper.Map<CarDto>).ToList();
        }
    }
}
