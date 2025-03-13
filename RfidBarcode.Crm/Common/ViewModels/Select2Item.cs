namespace RfidBarcode.Crm.Common.ViewModels
{
    public class Select2Item
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public Select2Item(string text, string value) { 
            Text = text;
            Value = value;
        }
    }
}
