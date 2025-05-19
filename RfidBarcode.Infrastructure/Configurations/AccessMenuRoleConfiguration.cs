using RfidBarcode.Domain.Entities;
using RfidBarcode.Domain.Entities.Identities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace RfidBarcode.Infrastructure.Configurations
{
    public class AccessMenuRoleConfiguration : IEntityTypeConfiguration<AccessMenuRole>
    {
        public void Configure(EntityTypeBuilder<AccessMenuRole> builder)
        {

            //Seeding the User to AspNetUsers table
            builder.HasData(
                new AccessMenuRole
                {
                    Id = 1,
                    RoleId = ApplicationRole.RoleAdministrator, //Administrator
                    AccessMenuId = AccessMenu.UserManagement,
                    CreatedBy = "system",
                    CreatedDate = new DateTime(2025, 5, 12),
                    LastUpdateBy = "system",
                    LastUpdateDate = new DateTime(2025, 5, 12)
                }
            );

            builder.HasOne(x => x.Role)
                .WithMany()
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.AccessMenu)
                .WithMany()
                .HasForeignKey(x => x.AccessMenuId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
