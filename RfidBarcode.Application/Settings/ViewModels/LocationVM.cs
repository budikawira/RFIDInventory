namespace RfidBarcode.Application.Settings.ViewModels
{
    public class LocationVM
    {
        public long Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public byte Type { get; set; }

        public int SkipStockOpname { get; set; }
    }
}
