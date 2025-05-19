using RfidBarcode.Domain.Entities;
using RfidBarcode.Domain.Entities.Identities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace RfidBarcode.Infrastructure.Configurations
{
    public class ItemMovementConfiguration : IEntityTypeConfiguration<ItemMovement>
    {
        public void Configure(EntityTypeBuilder<ItemMovement> builder)
        {
            builder.HasOne(x => x.Item)
                .WithMany(y => y.ItemMovements)
                .HasForeignKey(x => x.ItemId)
                .OnDelete(DeleteBehavior.Cascade);
            
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

            //builder.HasOne(x => x.TagLocation)
            //    .WithMany()
            //    .IsRequired(false)
            //    .HasForeignKey(x => x.TagLocationId)
            //    .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
