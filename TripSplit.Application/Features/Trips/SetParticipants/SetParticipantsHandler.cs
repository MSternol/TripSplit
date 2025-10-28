using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Application.Abstractions;
using TripSplit.Domain.Entities;
using TripSplit.Domain.Repositories;
using TripSplit.Domain.ValueObjects;

namespace TripSplit.Application.Features.Trips.SetParticipants
{
    public sealed class SetParticipantsHandler(
    ICurrentUserService current,
    ITripRepository trips,
    ITripParticipantRepository participants,
    IUnitOfWork uow
) : IRequestHandler<SetParticipantsCommand, bool>
    {
        public async Task<bool> Handle(SetParticipantsCommand r, CancellationToken ct)
        {
            var trip = await trips.GetAsync(r.TripId, current.GetUserId(), ct);
            if (trip is null) return false;

            var list = new List<TripParticipant>();

            void AddIfNotNull(int slot, (string FirstName, string LastName)? x)
            {
                if (x is null) return;
                var name = new PersonName(x.Value.FirstName, x.Value.LastName);
                if (!name.IsEmpty) list.Add(new TripParticipant(trip.Id, slot, name));
            }

            AddIfNotNull(0, r.Driver);
            AddIfNotNull(1, r.Passenger1);
            AddIfNotNull(2, r.Passenger2);
            AddIfNotNull(3, r.Passenger3);
            AddIfNotNull(4, r.Passenger4);

            await participants.RemoveAllAsync(trip.Id, ct);
            if (list.Count > 0)
                await participants.UpsertRangeAsync(list, ct);

            await uow.SaveChangesAsync(ct);
            return true;
        }
    }
}
