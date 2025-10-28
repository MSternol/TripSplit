using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace TripSplit.Tests.Support
{
    public static class TestDb
    {
        public static AppDbContext NewContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"TripSplitTests_{Guid.NewGuid()}")
                .EnableSensitiveDataLogging()
                .Options;

            var ctx = new AppDbContext(options);
            ctx.Database.EnsureCreated();
            return ctx;
        }
    }
}
