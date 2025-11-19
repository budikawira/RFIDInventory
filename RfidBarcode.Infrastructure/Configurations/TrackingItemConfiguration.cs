using RfidBarcode.Domain.Entities;
using RfidBarcode.Domain.Entities.Identities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace RfidBarcode.Infrastructure.Configurations
{
    public class TrackingItemConfiguration : IEntityTypeConfiguration<TrackingItem>
    {
        public void Configure(EntityTypeBuilder<TrackingItem> builder)
        {
            builder.Property(x => x.Yard)
                .HasColumnType("decimal(18,2)");
            builder.Property(x => x.Kg)
                .HasColumnType("decimal(18,2)");
            //builder.Property(x => x.Point)
            //    .HasColumnType("decimal(18,2)");
        }
    }
}
