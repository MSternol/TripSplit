using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripSplit.Application.Abstractions
{
    public interface ICurrentUserService
    {
        Guid GetUserId();
    }
}
