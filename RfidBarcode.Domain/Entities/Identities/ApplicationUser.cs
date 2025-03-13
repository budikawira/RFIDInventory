using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RfidBarcode.Domain.Entities.Identities
{
    public class ApplicationUser : IdentityUser<Int64>
    {

        public virtual ICollection<IdentityUserClaim<Int64>>? Claims { get; set; }
        public virtual ICollection<IdentityUserLogin<Int64>>? Logins { get; set; }
        public virtual ICollection<IdentityUserToken<Int64>>? Tokens { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = null!;

        public static Int64 IdAdmin = 1;


    }
}
