using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TripSplit.Domain.Entities;

namespace TripSplit.Infrastructure.Persistence.Configurations
{
    internal sealed class CarInsuranceConfiguration : IEntityTypeConfiguration<CarInsurance>
    {
        public void Configure(EntityTypeBuilder<CarInsurance> e)
        {
            e.ToTable("CarInsurances");
            e.HasKey(x => x.Id);

            e.Property(x => x.Company).HasMaxLength(200);
            e.Property(x => x.PolicyNumber).HasMaxLength(100);
            e.Property(x => x.ValidFrom).HasColumnType("date");
            e.Property(x => x.ValidTo).HasColumnType("date");

            e.HasIndex(x => x.CarId).IsUnique();
            e.HasIndex(x => x.ValidTo).HasDatabaseName("IX_CarInsurances_ValidTo");
        }
    }
}
