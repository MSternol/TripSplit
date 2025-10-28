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
    internal sealed class TripParticipantConfiguration : IEntityTypeConfiguration<TripParticipant>
    {
        public void Configure(EntityTypeBuilder<TripParticipant> e)
        {
            e.ToTable("TripParticipants");
            e.HasKey(x => x.Id);

            e.Property(x => x.TripId).IsRequired();
            e.Property(x => x.SlotIndex).IsRequired();

            e.HasIndex(x => new { x.TripId, x.SlotIndex }).IsUnique();

            e.OwnsOne(x => x.Name, o =>
            {
                o.Property(p => p.FirstName).HasColumnName("FirstName").HasMaxLength(100);
                o.Property(p => p.LastName).HasColumnName("LastName").HasMaxLength(100);
            });
        }
    }
}

