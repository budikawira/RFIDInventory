using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;

namespace RfidBarcode.Application.Operationals.ViewModels
{
    public class ItemVM : BaseViewModel
    {
        public long Id { get; set; }
        public string Merk { get; set; } = null!;
        public string Kp { get; set; } = null!;
        //public string? Ib { get; set; }
        public string? Kode { get; set; }
        public string? Kode1 { get; set; }
        public string? Kode2 { get; set; }
        public string? Kode3 { get; set; }
        public string? Kode4 { get; set; }
        public string? Oz { get; set; }
        public string? Grade { get; set; }
        public string? Point { get; set; }
        public decimal? Yard { get; set; }
        public decimal? Kg { get; set; }
        public string? Lebar { get; set; }
        public string? K { get; set; }
        public string? SusutLusi { get; set; }
        public string? SerialNumber { get; set; }
        public string? K3l { get; set; }
        public string? Inisial { get; set; }
        public long UserId { get; set; }
        //public int? Koreksi { get; set; }

        public long? QcFinishUserId { get; set; }
        public DateTime? QcFinish { get; set; }
        public DateTime? TanggalBuatBarcode { get; set; }
        //public string? PointGrade { get; set; } = "";
        public long? InSuratJalanId { get; set; }
        public long? InScanUserId { get; set; }
        public long? OutSuratJalanId { get; set; }
        public long? OutScanUserId { get; set; }
        public DateTime? InScan { get; set; }
        public DateTime? OutScan { get; set; }
        //public string? T01 { get; set; }

        public string Lot { get { return ""; } }

        public long? TrackingItemId { get; set; }

        public int PrintCount { get; set; }

        public string TagId { get { return Epc ?? ""; } }
        public string? Epc { get; set; }
        public string? Qr { get; set; }
        public long? LocationId { get; set; }
        public string? LocationName { get; set; }
        public byte? LocationType { get; set; }

        public string? InSuratJalan { get; set; }
        public string? OutSuratJalan { get; set; }

        public string TanggalBuatBarcodeString { get { 
                return TanggalBuatBarcode != null ? TanggalBuatBarcode.Value.ToString("yyyy-MM-dd") : ""; 
            } 
        }

        public string? PrintStatus { get; set; }

        public string ConvertedLebar
        {
            get
            {
                if (Lebar != null && Lebar.Length >= 2)
                {
                    if (Lebar.Substring(0,2).CompareTo("L ") == 0)
                    {
                        return Lebar.Substring(2);
                    }
                }
                return Lebar ?? "";
            }
        }

        public string ConvertedK
        {
            get
            {
                if (K != null)
                {
                    var index = K.IndexOf("/");
                    if (index > 0)
                    {
                        return K.Substring(0, index);
                    }
                }

                return K ?? "";
            }
        }

        public const string PRINT_STATUS_DONE = "Sudah Print";
        public const string PRINT_STATUS_PENDING = "Belum Print";

        public static Dictionary<int, string> ListStockStatus = new Dictionary<int, string>()
        {
            { STOCK_STATUS_ALL, "Semua" },
            { STOCK_STATUS_IN_STOCK, "Dalam Stok" },
            { STOCK_STATUS_SHIPPED, "Terkirim" },
            { STOCK_STATUS_NOT_RECEIVED, "Belum Diterima" },
        };

        public const int STOCK_STATUS_ALL = 0;
        public const int STOCK_STATUS_IN_STOCK = 1;
        public const int STOCK_STATUS_SHIPPED = 2;
        public const int STOCK_STATUS_NOT_RECEIVED = 3;
    }
}
