using DocumentFormat.OpenXml.Presentation;

namespace RfidBarcode.Application.Reports.ViewModels
{
    public class DailySummaryVM
    {
        public string KP { get; set; } = null!;
        //public string Identitas { get; set; } = null!;
        //public string OZ { get; set; } = null!;

        public string KodeI { get; set; } = null!;
        public string KodeGeneral { get; set; } = null!;
        public string Kategori { get; set; } = null!;
        public string Kode1 { get; set; } = null!;
        public string Kode { get; set; } = null!;
        //public string K { get; set; } = null!;
        public int SaR { get; set; }
        public decimal SaYard { get; set; }
        public int InR { get; set; }
        public decimal InYard { get; set; }
        public int OutR { get; set; }
        public decimal OutYard { get; set; }
        public int R { get; set; }
        public decimal Yard { get; set; }
        public decimal TS { get; set; }
        //public decimal P { get; set; }
        public string GR { get; set; } = null!;
        public string SAK { get; set; } = null!;
        public string Total { get; set; } = null!;
        
        public string? GradeGroup { get; set; } = null!;
        public int GradeGroupSeq { get; set; } = 0;

        public string p1 { get; set; } = "";
        public string p2 { get; set; } = "";
        public string p3 { get; set; } = "";
        public string p4 { get; set; } = "";
        public string p5 { get; set; } = "";
        public string p6 { get; set; } = "";
        public string p7 { get; set; } = "";
        public string p8 { get; set; } = "";
    }
}
