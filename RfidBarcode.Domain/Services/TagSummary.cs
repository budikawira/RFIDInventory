namespace RfidBarcode.Domain.Services
{
    public class TagSummary
    {
        public string Epc { get; set; } = null!;
        public List<TagSummaryData> Data { get; set; } = null!;

    }
}
