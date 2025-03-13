using RfidBarcode.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.Runtime.ConstrainedExecution;

namespace RfidBarcode.Domain.Entities
{
    public class AccessMenu : BaseEntity
    {
        [Key]
        public string Id { get; set; } = null!;
        public string Description { get; set; } = null!;

        public static string UserManagement = "UM";
    }
}
