using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Common.Libs;
using RfidBarcode.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;

namespace RfidBarcode.Application.Operationals.ViewModels
{
    public class SuratJalanVM : SuratJalan
    {

        public string Status
        {
            get
            {
                if (FinalizeDate != null)
                {
                    return "Final";
                }

                return "Draft";
            }
        }

        public string FinalizeDateString
        {
            get
            {
                if (FinalizeDate != null)
                {
                    return FinalizeDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                }

                return "-";
            }
        }


        public string ConfirmDateString
        {
            get
            {
                if (ConfirmDate != null)
                {
                    return ConfirmDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                }

                return "-";
            }
        }

        public string CreatedDateString
        {
            get
            {
                if (CreatedDate != null)
                {
                    return CreatedDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                }

                return "-";
            }
        }

        public string? GetNoCode()
        {
            if (!string.IsNullOrEmpty(No))
            {
                var parts = No.Split('/');
                if (parts.Length >= 2)
                {
                    return parts[1];
                }
            }
            return null;
        }
    }
}
