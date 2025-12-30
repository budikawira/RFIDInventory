using RfidBarcode.Domain.Common;
using RfidBarcode.Domain.Entities.Identities;
using System.ComponentModel.DataAnnotations;
using System.Runtime.ConstrainedExecution;

namespace RfidBarcode.Domain.Entities
{
    public class AccessMenu : BaseEntity
    {
        [Key]
        public string Id { get; set; } = null!;
        public string Description { get; set; } = null!;

        public List<AccessMenuRole> AccessMenuRoles { get; set; } = null!;

        public const string UserManagement = "UM";
        public const string RoleManagement = "RM";
        public const string InputBarcode = "IB";
        public const string SuratJalanInbound = "SJI";
        public const string SuratJalanOutbond = "SJO";
    }
}
