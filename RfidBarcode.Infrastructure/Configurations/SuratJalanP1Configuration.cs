using RfidBarcode.Domain.Entities;
using RfidBarcode.Domain.Entities.Identities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace RfidBarcode.Infrastructure.Configurations
{
    public class SuratJalanP1Configuration : IEntityTypeConfiguration<SuratJalanP1>
    {
        public void Configure(EntityTypeBuilder<SuratJalanP1> builder)
        {

        }
    }
}
