namespace RfidBarcode.Application.Common
{
    public class ReceiptData
    {
        public int Sequence { get; set; }
        public string ReceiptType { get; set; } = null!;
        public string? Header { get; set; }
        public string? Footer { get; set; }
    }
}
