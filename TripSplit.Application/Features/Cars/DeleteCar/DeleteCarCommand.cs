using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripSplit.Application.Features.Cars.DeleteCar
{
    public sealed record DeleteCarCommand(Guid Id) : IRequest<bool>;
}
