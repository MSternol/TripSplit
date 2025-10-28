using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripSplit.Application.Features.Cars.CarInspections
{
    public sealed record UpsertCarInspectionCommand(
        Guid CarId,
        DateTime? ValidFrom,
        DateTime? ValidTo
    ) : IRequest<Guid>;
}
