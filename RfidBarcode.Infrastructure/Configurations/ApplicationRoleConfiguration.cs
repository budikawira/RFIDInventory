using RfidBarcode.Domain.Entities.Identities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace RfidBarcode.Infrastructure.Configurations
{
    public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.HasData(
                new ApplicationRole
                {
                    Id = ApplicationRole.RoleAdministrator,
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR".ToUpper()
                },
                new ApplicationRole
                {
                    Id = ApplicationRole.RoleAdminFinish,
                    Name = "Admin Finish",
                    NormalizedName = "ADMIN FINISH".ToUpper()
                }, new ApplicationRole
                {
                    Id = ApplicationRole.RoleQcFinish, 
                    Name = "QC Finish",
                    NormalizedName = "QC FINISH"
                },
                new ApplicationRole
                {
                    Id = ApplicationRole.RoleGudangKain, 
                    Name = "Gudang Kain",
                    NormalizedName = "GUDANG KAIN"
                },

                new ApplicationRole
                {
                    Id = ApplicationRole.RoleQcGudangKain, 
                    Name = "QC Gudang Kain",
                    NormalizedName = "QC GUDANG KAIN"
                },

                new ApplicationRole
                {
                    Id = ApplicationRole.RoleAdminGudangKain, 
                    Name = "Admin Gudang Kain",
                    NormalizedName = "ADMIN GUDANG KAIN"
                },

                new ApplicationRole
                {
                    Id = ApplicationRole.RoleGudangJakarta, 
                    Name = "Gudang Jakarta",
                    NormalizedName = "GUDANG JAKARTA"
                },

                new ApplicationRole
                {
                    Id = ApplicationRole.RoleAdminGudangJakarta, 
                    Name = "Admin Gudang Jakarta",
                    NormalizedName = "ADMIN GUDANG JAKARTA"
                }
            );
        }
    }
}
