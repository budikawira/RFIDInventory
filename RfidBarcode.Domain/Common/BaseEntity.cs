using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RfidBarcode.Domain.Common
{
    public class BaseEntity : IBaseEntity
    {
        public DateTime? CreatedDate { get; set; }

        public string CreatedBy { get; set; } = null!;

        public DateTime? LastUpdateDate { get; set; }

        public string LastUpdateBy { get; set; } = null!;

        public virtual string GetSequenceName()
        {
            return "";
        }
    }
}
