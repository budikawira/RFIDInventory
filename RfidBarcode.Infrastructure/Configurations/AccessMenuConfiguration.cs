using RfidBarcode.Domain.Entities;
using RfidBarcode.Domain.Entities.Identities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace RfidBarcode.Infrastructure.Configurations
{
    public class AccessMenuConfiguration : IEntityTypeConfiguration<AccessMenu>
    {
        public void Configure(EntityTypeBuilder<AccessMenu> builder)
        {
           
            //Seeding the User to AspNetUsers table
            builder.HasData(
                new AccessMenu
                {
                    Id = AccessMenu.UserManagement,
                    Description = "User Management",
                    CreatedBy = "system",
                    CreatedDate = new DateTime(2025,5,12),
                    LastUpdateBy = "system",
                    LastUpdateDate = new DateTime(2025, 5, 12)
                }
            );
            builder.HasData(
                new AccessMenu
                {
                    Id = AccessMenu.RoleManagement,
                    Description = "Role Management",
                    CreatedBy = "system",
                    CreatedDate = new DateTime(2025, 12, 30),
                    LastUpdateBy = "system",
                    LastUpdateDate = new DateTime(2025, 12, 30)
                }
            );
            builder.HasData(
                new AccessMenu
                {
                    Id = AccessMenu.InputBarcode,
                    Description = "Input Barcode",
                    CreatedBy = "system",
                    CreatedDate = new DateTime(2025, 12, 30),
                    LastUpdateBy = "system",
                    LastUpdateDate = new DateTime(2025, 12, 30)
                }
            );
            builder.HasData(
                new AccessMenu
                {
                    Id = AccessMenu.SuratJalanInbound,
                    Description = "Surat Jalan Inbound",
                    CreatedBy = "system",
                    CreatedDate = new DateTime(2025, 12, 30),
                    LastUpdateBy = "system",
                    LastUpdateDate = new DateTime(2025, 12, 30)
                }
            );
            builder.HasData(
                new AccessMenu
                {
                    Id = AccessMenu.SuratJalanOutbond,
                    Description = "Surat Jalan Outbond",
                    CreatedBy = "system",
                    CreatedDate = new DateTime(2025, 12, 30),
                    LastUpdateBy = "system",
                    LastUpdateDate = new DateTime(2025, 12, 30)
                }
            );
        }
    }
}
