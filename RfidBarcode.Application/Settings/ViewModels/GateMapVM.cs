namespace RfidBarcode.Application.Settings.ViewModels
{
    public class GateMapVM
    {
        public long Id { get; set; }

        public long GateId { get; set; }

        public string? Antenna { get; set; }

        public long? PrevLocationId { get; set; }

        public long NextLocationId { get; set; }

        public string PrevLocationName { get; set; } = null!;

        public string NextLocationName { get; set; } = null!;
    }
}
