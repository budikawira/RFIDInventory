using System.Security.Claims;
using System.Security.Principal;

public static class IdentityExtended
{
    public static string ClaimRole = "role";
    public static string ClaimExternalId = "ext";
    public static string ClaimUserId = "id";
    public static string ClaimDeviceId = "dev";


    public static string AccessAll = "all";
    public static string AccessRead = "read";

    public enum Access
    {
        All,
        Read,
        None
    }
    public enum Role
    {
        Admin,
        Management,
        Supervisor,
        Operator,
        Unknown
    }
    public static Guid GetExternalId(this IIdentity identity)
    {

        IEnumerable<Claim> claims = ((ClaimsIdentity)identity).Claims;
        Guid externalId = Guid.Empty;
        var id = claims.Where(p => p.Type == ClaimExternalId).FirstOrDefault()?.Value;

        Guid.TryParse(id, out externalId);

        return externalId;
    }

    public static Int64 GetUserId(this IIdentity identity)
    {

        IEnumerable<Claim> claims = ((ClaimsIdentity)identity).Claims;
        Int64 userId = 0;
        var id = claims.Where(p => p.Type == ClaimUserId).FirstOrDefault()?.Value;

        Int64.TryParse(id, out userId);

        return userId;
    }

    public static string GetClaim(this IIdentity identity, string claim)
    {
        IEnumerable<Claim> claims = ((ClaimsIdentity)identity).Claims;
        var res = claims.Where(c => c.Type == claim).FirstOrDefault();
        return res == null ? "" : res.Value;
    }


    public static Role GetUserRole(this IIdentity identity)
    {
        IEnumerable<Claim> claims = ((ClaimsIdentity)identity).Claims;
        var count = claims.Where(p => p.Type == "role").Count();
        var role = claims.Where(p => p.Type == "role").FirstOrDefault()?.Value; ;
        if ("ADMINISTRATOR".Equals(role)) return Role.Admin;
        else if ("MANAGEMENT".Equals(role)) return Role.Management;
        else if ("SUPERVISOR".Equals(role)) return Role.Supervisor;
        else if ("OPERATOR".Equals(role)) return Role.Operator;

        return Role.Unknown;
    }

}

