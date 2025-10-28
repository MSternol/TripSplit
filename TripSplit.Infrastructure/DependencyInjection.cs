using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TripSplit.Domain.Repositories;
using TripSplit.Infrastructure.Persistence;
using TripSplit.Infrastructure.Repositories;

namespace TripSplit.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            var connString = config.GetConnectionString("Default");
            if (string.IsNullOrWhiteSpace(connString))
                throw new InvalidOperationException("ConnectionStrings:Default is missing.");

            services.AddDbContextPool<AppDbContext>(options =>
            {
                options.UseSqlServer(connString, sql =>
                {
                    sql.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    sql.CommandTimeout(30);
                });

                options.EnableSensitiveDataLogging(false);
                options.EnableDetailedErrors(false);
            }, poolSize: 256);

            services.AddScoped<ITripRepository, TripRepository>();
            services.AddScoped<ICarRepository, CarRepository>();
            services.AddScoped<ICarInsuranceRepository, CarInsuranceRepository>();
            services.AddScoped<ICarInspectionRepository, CarInspectionRepository>();
            services.AddScoped<ITripParticipantRepository, TripParticipantRepository>();
            services.AddScoped<ITripShareLinkRepository, TripShareLinkRepository>();
            services.AddScoped<IUnitOfWork, EfUnitOfWork>();

            services.AddHealthChecks().AddDbContextCheck<AppDbContext>("db", tags: new[] { "ready" });

            return services;
        }
    }
}
