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

namespace TripSplit.Application.Features.Trips.GetTripDetails
{
    public sealed class GetTripDetailsHandler(
    ITripRepository trips,
    ICurrentUserService current,
    IMapper mapper
) : IRequestHandler<GetTripDetailsQuery, TripDto?>
    {
        public async Task<TripDto?> Handle(GetTripDetailsQuery r, CancellationToken ct)
        {
            var entity = await trips.GetAsync(r.Id, current.GetUserId(), ct);
            return entity is null ? null : mapper.Map<TripDto>(entity);
        }
    }
}
