using RfidBarcode.Domain.Common;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RfidBarcode.Domain.Entities.Identities
{
    public class AccessMenuRole : BaseEntity
    {
        [Key]
        public Int64 Id { get; set; }

        public string AccessMenuId { get; set; } = null!;

        public Int64 RoleId { get; set; }

        public ApplicationRole Role { get; set; } = null!;
        public AccessMenu AccessMenu { get; set; } = null!;
    }
}
