using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RfidBarcode.Domain.Entities.Identities
{
    public class ApplicationUserRole : IdentityUserRole<Int64>
    {
        public virtual ApplicationUser User { get; set; } = null!;
        public virtual ApplicationRole Role { get; set; } = null!;

    }
}
