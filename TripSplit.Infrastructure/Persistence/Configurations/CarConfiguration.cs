using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TripSplit.Domain.Entities;

namespace TripSplit.Infrastructure.Persistence.Configurations
{
    internal sealed class CarConfiguration : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> e)
        {
            e.ToTable("Cars");
            e.HasKey(x => x.Id);

            e.Property(x => x.OwnerUserId).IsRequired();
            e.Property(x => x.Name).IsRequired().HasMaxLength(200);
            e.Property(x => x.FuelType).IsRequired();
            e.Property(x => x.AverageConsumptionLper100).IsRequired();
            e.Property(x => x.TankCapacityL).IsRequired();

            e.Property(x => x.RemindersEnabled);
            e.Property(x => x.ReminderLeadTime).HasConversion<int>();

            e.HasIndex(x => new { x.OwnerUserId, x.Name }).IsUnique(false);

            e.HasOne(x => x.Insurance)
             .WithOne()
             .HasForeignKey<CarInsurance>(i => i.CarId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.Inspection)
             .WithOne()
             .HasForeignKey<CarInspection>(i => i.CarId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
