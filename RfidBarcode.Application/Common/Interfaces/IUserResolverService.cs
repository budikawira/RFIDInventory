namespace RfidBarcode.Application.Common.Interfaces
{
    public interface IUserResolverService
    {
        public string GetUser(); //get the username
        public bool HasReadAccess(string policy);
        public bool HasWriteAccess(string policy);

        public Guid GetExternalId(); //for ldap authentication

        public Int64 GetUserId();

        public IdentityExtended.Role GetRole();
    }
}
