using RfidBarcode.Domain.Entities;
using RfidBarcode.Domain.Entities.Identities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace RfidBarcode.Infrastructure.Configurations
{
    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.HasOne(x => x.TrackingItem)
                .WithMany(y => y.Items)
                .IsRequired(false)
                .HasForeignKey(x => x.TrackingItemId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(x => x.ItemPrintLogs)
                .WithOne(y => y.Item)
                .HasForeignKey(y => y.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.TagLocations)
                .WithOne(y => y.Item)
                .HasForeignKey(y => y.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.StockOpnameDetails)
                .WithOne(y => y.Item)
                .HasForeignKey(y => y.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Yard)
                .HasColumnType("decimal(18,2)");
            builder.Property(x => x.Kg)
                .HasColumnType("decimal(18,2)");
            //builder.Property(x => x.Point)
            //    .HasColumnType("decimal(18,2)");

            builder.HasOne(x => x.Location)
                .WithMany(y => y.Items)
                .IsRequired(false)
                .HasForeignKey(x => x.LocationId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(x => x.SuratJalanP1)
                .WithMany(y => y.Items)
                .IsRequired(false)
                .HasForeignKey(x => x.SuratJalanP1Id)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
