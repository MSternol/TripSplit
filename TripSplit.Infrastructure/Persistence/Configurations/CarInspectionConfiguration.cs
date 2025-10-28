using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TripSplit.Domain.Entities;

namespace TripSplit.Infrastructure.Persistence.Configurations
{
    internal sealed class CarInspectionConfiguration : IEntityTypeConfiguration<CarInspection>
    {
        public void Configure(EntityTypeBuilder<CarInspection> e)
        {
            e.ToTable("CarInspections");
            e.HasKey(x => x.Id);

            e.Property(x => x.ValidFrom).HasColumnType("date");
            e.Property(x => x.ValidTo).HasColumnType("date");

            e.HasIndex(x => x.CarId).IsUnique();
            e.HasIndex(x => x.ValidTo).HasDatabaseName("IX_CarInspections_ValidTo");
        }
    }
}
