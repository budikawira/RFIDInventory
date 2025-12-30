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
                    Name = "Adm Barcode",
                    NormalizedName = "Adm Barcode".ToUpper()
                }, new ApplicationRole
                {
                    Id = ApplicationRole.RoleQcFinish,
                    Name = "Adm Finish",
                    NormalizedName = "Adm Finish".ToUpper()
                },
                new ApplicationRole
                {
                    Id = ApplicationRole.RoleGudangKain,
                    Name = "Adm Gudang",
                    NormalizedName = "Adm Gudang".ToUpper()
                }

                //new ApplicationRole
                //{
                //    Id = ApplicationRole.RoleQcGudangKain, 
                //    Name = "QC Gudang Kain",
                //    NormalizedName = "QC GUDANG KAIN"
                //},

            //new ApplicationRole
            //{
            //    Id = ApplicationRole.RoleAdminGudangKain, 
            //    Name = "Admin Gudang Kain",
            //    NormalizedName = "ADMIN GUDANG KAIN"
            //},

            //new ApplicationRole
            //{
            //    Id = ApplicationRole.RoleGudangJakarta, 
            //    Name = "Gudang Jakarta",
            //    NormalizedName = "GUDANG JAKARTA"
            //},

            //new ApplicationRole
            //{
            //    Id = ApplicationRole.RoleAdminGudangJakarta, 
            //    Name = "Admin Gudang Jakarta",
            //    NormalizedName = "ADMIN GUDANG JAKARTA"
            //}
            );
        }
    }
}
