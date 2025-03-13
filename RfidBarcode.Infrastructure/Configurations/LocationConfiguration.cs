using RfidBarcode.Domain.Entities;
using RfidBarcode.Domain.Entities.Identities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace RfidBarcode.Infrastructure.Configurations
{
    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            //builder.HasMany(x => x.GateMapNext)
            //    .WithOne(y => y.NextLocation)
            //    .HasForeignKey(y => y.NextLocationId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //builder.HasMany(x => x.GateMapPrev)
            //    .WithOne(y => y.PrevLocation)
            //    .HasForeignKey(y => y.PrevLocationId)
            //    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
