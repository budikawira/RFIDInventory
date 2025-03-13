namespace RfidBarcode.Domain.Services
{
    public class GateData
    {
        public List<TagSummary> Data { get; set; } = null!;
        public long Time { get; set; }
    }
}
