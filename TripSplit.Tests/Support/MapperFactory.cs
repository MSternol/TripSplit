using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Application.Mapping;

namespace TripSplit.Tests.Support
{
    public static class MapperFactory
    {
        public static IMapper Create()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddAutoMapper(cfg => { }, typeof(ApplicationMappingProfile).Assembly);
            var sp = services.BuildServiceProvider();
            return sp.GetRequiredService<IMapper>();
        }
    }
}
