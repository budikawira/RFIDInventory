namespace RfidBarcode.Crm.Common.ViewModels
{
    public class ChartDataVM<T>
    {
        public T? Value { get; set; }
        public string Name { get; set; } = null!;
    }
}
