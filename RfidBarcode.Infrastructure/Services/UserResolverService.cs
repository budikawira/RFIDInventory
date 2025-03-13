using RfidBarcode.Application.Common.Interfaces;

namespace RfidBarcode.Infrastructure.Services
{
    public class UserResolverService : IUserResolverService
    {
        private readonly IHttpContextAccessor _context;
        public UserResolverService(IHttpContextAccessor context)
        {
            _context = context;
        }

        public string GetUser()
        {
            if (_context.HttpContext != null &&
                _context.HttpContext.User != null
                && _context.HttpContext.User.Identity != null)
            {
                if (_context.HttpContext.User?.Identity?.Name != null)
                {
                    return _context.HttpContext.User?.Identity?.Name!;
                }
            }

            return "system";
        }
        

        public Guid GetExternalId()
        {
            if (_context.HttpContext != null &&
                _context.HttpContext.User != null
                && _context.HttpContext.User.Identity != null)
            {
                return _context.HttpContext.User.Identity.GetExternalId();
            }
            return Guid.Empty;
        }

        public Int64 GetUserId()
        {
            if (_context.HttpContext != null &&
                _context.HttpContext.User != null
                && _context.HttpContext.User.Identity != null)
            {
                return _context.HttpContext.User.Identity.GetUserId();
            }
            return 0;
        }

        public IdentityExtended.Role GetRole()
        {
            if (_context.HttpContext != null &&
                _context.HttpContext.User != null
                && _context.HttpContext.User.Identity != null)
            {
                return _context.HttpContext.User?.Identity?.GetUserRole() ?? IdentityExtended.Role.Unknown;
            }

            return IdentityExtended.Role.Unknown;
        }

        private string GetClaimValue(string policy)
        {
            if (_context.HttpContext != null &&
                _context.HttpContext.User != null
                && _context.HttpContext.User.Identity != null)
            {
                return _context.HttpContext.User.Identity.GetClaim(policy);
            }

            return "";
        }

        public bool HasReadAccess(string policy)
        {
            var val = GetClaimValue(policy);
            if (val.Length > 0) return true;

            return false;
        }

        public bool HasWriteAccess(string policy)
        {
            var val = GetClaimValue(policy);
            if (val.CompareTo("W") == 0) return true;

            return false;
        }
    }
}
