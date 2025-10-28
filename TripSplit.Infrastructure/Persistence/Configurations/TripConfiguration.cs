using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Domain.Entities;

namespace TripSplit.Infrastructure.Persistence.Configurations
{
    internal sealed class TripConfiguration : IEntityTypeConfiguration<Trip>
    {
        public void Configure(EntityTypeBuilder<Trip> e)
        {
            e.ToTable("Trips");
            e.HasKey(x => x.Id);

            e.Property(x => x.FuelPricePerL).HasPrecision(10, 2);
            e.Property(x => x.ParkingCost).HasPrecision(10, 2);
            e.Property(x => x.ExtraCosts).HasPrecision(10, 2);

            e.Property(x => x.FuelCostTotal).HasPrecision(12, 2);
            e.Property(x => x.TripCostTotal).HasPrecision(12, 2);
            e.Property(x => x.CostPerPerson).HasPrecision(12, 2);

            e.OwnsOne(x => x.Start, o =>
            {
                o.Property(p => p.Name).HasColumnName("StartName").HasMaxLength(256);
                o.Property(p => p.Latitude).HasColumnName("StartLat");
                o.Property(p => p.Longitude).HasColumnName("StartLon");
            });

            e.OwnsOne(x => x.End, o =>
            {
                o.Property(p => p.Name).HasColumnName("EndName").HasMaxLength(256);
                o.Property(p => p.Latitude).HasColumnName("EndLat");
                o.Property(p => p.Longitude).HasColumnName("EndLon");
            });

            e.HasIndex(x => x.OwnerUserId);
            e.HasIndex(x => new { x.OwnerUserId, x.StartedAt });
        }
    }
}
