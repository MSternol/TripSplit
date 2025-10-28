using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Domain.Entities;
using TripSplit.Infrastructure.Identity;

namespace TripSplit.Infrastructure.Persistence
{
    public sealed class AppDbContext : IdentityDbContext<
        AppUser,
        AppRole, 
        Guid, 
        IdentityUserClaim<Guid>,
        IdentityUserRole<Guid>,
        IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>,
        IdentityUserToken<Guid>> 
    {
        public DbSet<Trip> Trips => Set<Trip>();
        public DbSet<Car> Cars => Set<Car>();
        public DbSet<CarInsurance> CarInsurances => Set<CarInsurance>();
        public DbSet<CarInspection> CarInspections => Set<CarInspection>();
        public DbSet<TripParticipant> TripParticipants => Set<TripParticipant>();
        public DbSet<TripShareLink> TripShareLinks => Set<TripShareLink>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);

            b.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            b.Entity<AppUser>(e =>
            {
                e.Property(x => x.FirstName).HasMaxLength(100);
                e.Property(x => x.LastName).HasMaxLength(100);
            });
        }
    }
}
