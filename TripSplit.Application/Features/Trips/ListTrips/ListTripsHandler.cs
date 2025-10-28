using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Application.Abstractions;
using TripSplit.Application.DTOs;
using TripSplit.Domain.Repositories;

namespace TripSplit.Application.Features.Trips.ListTrips
{
    public sealed class ListTripsHandler(
    ITripRepository trips,
    ICurrentUserService current,
    IMapper mapper
) : IRequestHandler<ListTripsQuery, IReadOnlyList<TripDto>>
    {
        public async Task<IReadOnlyList<TripDto>> Handle(ListTripsQuery r, CancellationToken ct)
        {
            var items = await trips.ListAsync(current.GetUserId(), ct);
            return items.Select(mapper.Map<TripDto>).ToList();
        }
    }
}
