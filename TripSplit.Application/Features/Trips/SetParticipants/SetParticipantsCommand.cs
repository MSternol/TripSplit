using MediatR;

namespace TripSplit.Application.Features.Trips.SetParticipants
{
    public sealed record SetParticipantsCommand(
    Guid TripId,
    (string FirstName, string LastName)? Driver,
    (string FirstName, string LastName)? Passenger1,
    (string FirstName, string LastName)? Passenger2,
    (string FirstName, string LastName)? Passenger3,
    (string FirstName, string LastName)? Passenger4
) : IRequest<bool>;
}