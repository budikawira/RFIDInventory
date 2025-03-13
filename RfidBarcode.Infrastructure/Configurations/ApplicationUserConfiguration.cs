using RfidBarcode.Domain.Entities.Identities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace RfidBarcode.Infrastructure.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
//            var hasher = new PasswordHasher<IdentityUser>();
//            //Seeding the User to AspNetUsers table
//            builder.HasData(
//                new ApplicationUser
//                {
//                    Id =ApplicationUser.IdAdmin,
//                    UserName = "admin",
//                    NormalizedUserName = "ADMIN",
//#pragma warning disable CS8625
//                    PasswordHash = hasher.HashPassword(null, "admin"),
//#pragma warning restore CS8625
//                    SecurityStamp = Guid.NewGuid().ToString()
//                }
//            );
        }
    }
}
