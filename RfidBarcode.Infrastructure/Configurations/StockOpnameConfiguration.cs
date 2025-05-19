using RfidBarcode.Domain.Entities;
using RfidBarcode.Domain.Entities.Identities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace RfidBarcode.Infrastructure.Configurations
{
    public class StockOpnameConfiguration : IEntityTypeConfiguration<StockOpname>
    {
        public void Configure(EntityTypeBuilder<StockOpname> builder)
        {
            builder.HasMany(x => x.StockOpnameDetails)
                .WithOne(y => y.StockOpname)
                .HasForeignKey(x => x.StockOpnameId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Location)
                .WithMany()
                .HasForeignKey(x => x.LocationId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
