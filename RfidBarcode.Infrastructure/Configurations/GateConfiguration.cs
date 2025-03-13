using RfidBarcode.Domain.Entities;
using RfidBarcode.Domain.Entities.Identities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace RfidBarcode.Infrastructure.Configurations
{
    public class GateConfiguration : IEntityTypeConfiguration<Gate>
    {
        public void Configure(EntityTypeBuilder<Gate> builder)
        {
            builder.HasMany(x => x.GateMaps)
                .WithOne(y => y.Gate)
                .HasForeignKey(x => x.GateId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
