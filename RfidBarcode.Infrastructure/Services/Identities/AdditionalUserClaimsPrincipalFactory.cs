using RfidBarcode.Domain.Entities.Identities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace RfidBarcode.Infrastructure.Services.Identities
{
    public class AdditionalUserClaimsPrincipalFactory
        : UserClaimsPrincipalFactory<ApplicationUser>
    {
        private readonly ApplicationDbContext _context;
        public AdditionalUserClaimsPrincipalFactory(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, optionsAccessor)
        {
            _context = context;
        }

        public async override Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            var principal = await base.CreateAsync(user);
            if (principal.Identity != null)
            {
                var identity = (ClaimsIdentity)principal.Identity;
                var userName = identity.Name;

                var role = await (from users in _context.Users
                                  join userRoles in _context.UserRoles on users.Id equals userRoles.UserId
                                  join roles in _context.Roles on userRoles.RoleId equals roles.Id
                                  where users.UserName == userName
                                  select new
                                  {
                                      Id = roles.Id,
                                      Name = roles.NormalizedName
                                  }).FirstOrDefaultAsync();

                if (role != null)
                {
                    identity.AddClaim(new Claim(IdentityExtended.ClaimRole, role.Name));
                    var access = await _context.AccessMenuRoles.Include(x => x.AccessMenu).Where(x => x.RoleId == role.Id)
                        .ToListAsync();
                    foreach (var acc  in access)
                    {
                        identity.AddClaim(new Claim(acc.AccessMenu.Id, "R"));
                    }
                }
            }

            return principal;
        }
    }
}
