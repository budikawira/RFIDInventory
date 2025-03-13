using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using RfidBarcode.Domain.Common;

namespace RfidBarcode.Domain.Entities
{
    public class Gate : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Name { get; set; } = null!;

        public string ClientId { get; set; } = null!;

        public List<GateMap> GateMaps { get; set; } = null!;
    }
}
