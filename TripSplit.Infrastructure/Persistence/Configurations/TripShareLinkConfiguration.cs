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
    internal sealed class TripShareLinkConfiguration : IEntityTypeConfiguration<TripShareLink>
    {
        public void Configure(EntityTypeBuilder<TripShareLink> e)
        {
            e.ToTable("TripShareLinks");
            e.HasKey(x => x.Id);

            e.Property(x => x.TripId).IsRequired();
            e.Property(x => x.Token).IsRequired().HasMaxLength(128);
            e.HasIndex(x => x.Token).IsUnique();

            e.Property(x => x.IsActive).IsRequired();
            e.Property(x => x.CreatedAtUtc).IsRequired();
        }
    }
}
