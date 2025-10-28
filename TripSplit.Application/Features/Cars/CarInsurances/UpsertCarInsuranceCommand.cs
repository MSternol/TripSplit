using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripSplit.Application.Features.Cars.CarInsurances
{
    public sealed record UpsertCarInsuranceCommand(
        Guid CarId,
        string? Company,
        string? PolicyNumber,
        DateTime? ValidFrom,
        DateTime? ValidTo
    ) : IRequest<Guid>;
}
