using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RfidBarcode.Domain.Entities.Identities
{
    public class ApplicationRole : IdentityRole<Int64>
    {
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = null!;

        public const Int64 RoleAdministrator = 1;
        public const Int64 RoleAdminFinish = 2;
        public const Int64 RoleQcFinish = 3; 
        public const Int64 RoleGudangKain = 4;
        public const Int64 RoleQcGudangKain = 5;
        public const Int64 RoleAdminGudangKain = 6;
        public const Int64 RoleGudangJakarta = 7;
        public const Int64 RoleAdminGudangJakarta = 8;
    }
}
