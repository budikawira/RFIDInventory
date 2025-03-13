using RfidBarcode.Domain.Entities;
using RfidBarcode.Domain.Entities.Identities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace RfidBarcode.Infrastructure.Configurations
{
    public class GateMapConfiguration : IEntityTypeConfiguration<GateMap>
    {
        public void Configure(EntityTypeBuilder<GateMap> builder)
        {
            builder.HasOne(x => x.NextLocation)
                .WithMany(y => y.GateMapNext)
                .HasForeignKey(x => x.NextLocationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.PrevLocation)
                .WithMany(y => y.GateMapPrev)
                .HasForeignKey(x => x.PrevLocationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
