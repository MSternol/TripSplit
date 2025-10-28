using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Application.Abstractions;

namespace TripSplit.Tests.Support
{
    public sealed class FakeCurrentUserService : ICurrentUserService
    {
        private readonly Guid _id;
        public FakeCurrentUserService(Guid id) => _id = id;
        public Guid GetUserId() => _id;
    }
}
