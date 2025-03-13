using RfidBarcode.Application.Common.BaseObjects.AppConfig;

namespace RfidBarcode.Application.Common
{
    public class AppConfig
    {
        public string Operator { get; set; } = null!;
        public Param Unit { get; set; } = null!;
        public List<Param> ListBusiness { get; set; } = null!;
        public string PrefixPlat { get; set; } = null!;
        public string ImageLogo { get; set; } = null!;
        public string ImageCar { get; set; } = null!;
        public List<ReceiptData>? ReceiptIn { get; set; }
        public List<ReceiptData>? ReceiptOut { get; set; }
        public List<ReceiptData>? ReceiptFines { get; set; }
        public List<ReceiptData>? ReceiptInOut { get; set; }
    }
}
