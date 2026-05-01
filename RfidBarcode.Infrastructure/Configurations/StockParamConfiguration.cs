using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RfidBarcode.Domain.Entities;

namespace RfidBarcode.Infrastructure.Configurations
{
    public class StockParamConfiguration : IEntityTypeConfiguration<StockParam>
    {
        public void Configure(EntityTypeBuilder<StockParam> builder)
        {
            // Set collation for each property individually
            builder.Property(e => e.c1)
                   .UseCollation("utf8mb4_general_ci");

            builder.Property(e => e.c2)
                   .UseCollation("utf8mb4_general_ci");

            builder.Property(e => e.c3)
                   .UseCollation("utf8mb4_general_ci");

            builder.Property(e => e.c4)
                   .UseCollation("utf8mb4_general_ci");

            builder.Property(e => e.c5)
                   .UseCollation("utf8mb4_general_ci");

            // Composite index on c1-c5
            builder.HasIndex(e => new { e.c1, e.c2, e.c3, e.c4, e.c5 })
                .HasDatabaseName("IX_StockParam_c1_c2_c3_c4_c5");
        }
    }
}