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
                    CreatedDate = DateTime.Now,
                    LastUpdateBy = "system",
                    LastUpdateDate = DateTime.Now
                }
            );
        }
    }
}
