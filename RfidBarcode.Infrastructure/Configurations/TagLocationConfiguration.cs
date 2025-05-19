using RfidBarcode.Domain.Entities;
using RfidBarcode.Domain.Entities.Identities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace RfidBarcode.Infrastructure.Configurations
{
    public class TagLocationConfiguration : IEntityTypeConfiguration<TagLocation>
    {
        public void Configure(EntityTypeBuilder<TagLocation> builder)
        {
            builder.HasOne(x => x.Item)
                .WithMany(y => y.TagLocations)
                .IsRequired(false)
                .HasForeignKey(x => x.ItemId)
                .OnDelete(DeleteBehavior.SetNull);
            
            builder.HasOne(x => x.Location)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.LocationId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(x => x.PrevLocation)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.PrevLocationId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(x => x.StockOpname)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.StockOpnameId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
