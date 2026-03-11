using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.IdentityModel.Tokens;
using RfidBarcode.Application.Operationals.ViewModels;
using RfidBarcode.Application.Reports.ViewModels;
using RfidBarcode.Application.Users.ViewModels;
using RfidBarcode.Domain.Entities;
using System.Data;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace RfidBarcode.Application.Common.Libs
{
    public class Helper
    {
        public static string GenerateJSONWebToken(IConfiguration _config, long userId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>();
            claims.Add(new Claim(IdentityExtended.ClaimUserId, userId.ToString()));

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddDays(7),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static string GenerateStaticJSONWebToken(IConfiguration _config)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["StaticJwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>();

            var token = new JwtSecurityToken(_config["StaticJwt:Issuer"],
              _config["StaticJwt:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(5),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static string GenerateSuratJalanNo(string type, string code, int sequence)
        {
            return $"{type}/{code}/{sequence:D5}";
        }

        public static bool ValidateSuratJalanColumns(List<ItemVM> rows)
        {
            var kps = rows.GroupBy(x => x.Kp)
                .Select(g => new
                {
                    Kp = g.Key,
                    Count = g.Count()
                }).OrderByDescending(x => x.Count).ToList();
            if (kps.Count > 5)
            {
                return false;
            }

            var col = 0;
            foreach (var kp in kps)
            {

                var additionalCol = (int)Math.Ceiling(((decimal)kp.Count / 5));
                col += additionalCol;
            }

            if (col > 5)
            {
                return false;
            }


            return true;
        }

        public static DateTime? ParseDate(string? str)
        {
            DateTime result;

            try
            {
                var success = DateTime.TryParseExact(str, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
                if (success)
                {
                    return result;
                }
            }
            catch (Exception) { }

            return null;
        }

        public static int? ParseInt(string? str)
        {
            int result;
            if (int.TryParse(str, out result))
            {
                return result;
            }
            return null;
        }

        public static IEnumerable<Claim> GetClaimsFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            return jwtToken.Claims;
        }

        public static string GetK3L(string Kode3)
        {
            if (Kode3 == "C")
            {
                return "20-D-001123";
            }
            else if (Kode3 == "CR" || Kode3 == "SC")
            {
                return "20-D-001130";
            }
            else if (Kode3 == "P" || Kode3 == "PR" || Kode3 == "SP")
            {
                return "20-D-001131";
            }
            else if (Kode3 == "ST" || Kode3 == "T")
            {
                return "20-D-001830";
            }

            return "";
        }

        public static string GetEpc(long ItemId)
        {
            return string.Format("0505{0:X20}", ItemId);
        }

        public static string GetQr(Item item)
        {
            var sb = new StringBuilder();
            sb.Append(item.Merk ?? "");
            sb.Append(";");
            sb.Append(item.Kp ?? "");
            sb.Append(";");
            sb.Append(item.Kode1 ?? "");
            sb.Append(";");
            sb.Append(item.Kode2 ?? "");
            sb.Append(";");
            sb.Append(item.Kode3 ?? "");
            sb.Append(";");
            sb.Append(item.Kode4 ?? "");
            sb.Append(";");
            sb.Append(item.Grade ?? "");
            sb.Append(";");
            sb.Append(item.Yard != null ? item.Yard.ToString() : "");
            sb.Append(";");
            sb.Append(item.Kg != null ? item.Kg.ToString() : "");
            sb.Append(";");
            sb.Append(item.Lebar ?? "");
            sb.Append(";");
            sb.Append(item.K ?? "");
            sb.Append(";");
            sb.Append(item.SerialNumber ?? "");
            sb.Append(";");
            sb.Append(item.Inisial ?? "");
            return sb.ToString();
        }

        public static long? ParseItemTagId(string tagId)
        {
            if (tagId.Length == 24)
            {
                if (tagId.Substring(0, 4) == "0505")
                {
                    long data;
                    if (long.TryParse(tagId.Substring(4), NumberStyles.HexNumber, null, out data))
                    {
                        return data;
                    }
                }
            }

            return null;
        }

        public static string GetItemTagId(Item item)
        {
            if (!string.IsNullOrEmpty(item.Epc))
            {
                return item.Epc;
            }
            return string.Format("0505{0:X20}", item.Id);
        }

        public static XLWorkbook? CreateExcelDailyReport(Dictionary<string, List<DailySummaryVM>> rows)
        {
            XLWorkbook wb = new XLWorkbook();
            try
            {
                var ws = wb.Worksheets.Add("Report");
                var colSA = 1;
                var colIn = 1;
                var colOut = 1;
                var colSaldoAkhir = 1;
                var colKode = 1;
                int rowIndex = 1;
                var lastCol = 0;

                //create header
                var colIndex = 1;
                ws.Cell(rowIndex, colIndex++).Value = "KP";
                ws.Cell(rowIndex, colIndex++).Value = "Identitas";
                ws.Cell(rowIndex, colIndex++).Value = "OZ";
                ws.Cell(rowIndex, colIndex++).Value = "I";
                ws.Cell(rowIndex, colIndex++).Value = "Kode General"; //Kode General
                ws.Cell(rowIndex, colIndex++).Value = "Kategori";
                colKode = colIndex;
                ws.Cell(rowIndex, colIndex++).Value = "Kode";
                //ws.Cell(rowIndex, colIndex++).Value = "K";
                colSA = colIndex;
                ws.Cell(rowIndex, colIndex++).Value = "SA";
                ws.Cell(rowIndex, colIndex++).Value = "SA";
                ws.MergedRanges.Add(ws.Range(rowIndex, colIndex - 2, rowIndex, colIndex - 1));
                colIn = colIndex;
                ws.Cell(rowIndex, colIndex++).Value = "In";
                ws.Cell(rowIndex, colIndex++).Value = "In";
                ws.MergedRanges.Add(ws.Range(rowIndex, colIndex - 2, rowIndex, colIndex - 1));
                colOut = colIndex;
                ws.Cell(rowIndex, colIndex++).Value = "Out";
                ws.Cell(rowIndex, colIndex++).Value = "Out";
                ws.MergedRanges.Add(ws.Range(rowIndex, colIndex - 2, rowIndex, colIndex - 1));
                colSaldoAkhir = colIndex;
                ws.Cell(rowIndex, colIndex++).Value = "Saldo Akhir";
                ws.Cell(rowIndex, colIndex++).Value = "Saldo Akhir";
                ws.MergedRanges.Add(ws.Range(rowIndex, colIndex - 2, rowIndex, colIndex - 1));
                ws.Cell(rowIndex, colIndex++).Value = "T.S.";
                ws.Cell(rowIndex, colIndex++).Value = "P";
                ws.Cell(rowIndex, colIndex++).Value = "GR";
                ws.Cell(rowIndex, colIndex++).Value = "";
                ws.Cell(rowIndex, colIndex++).Value = "S A K";
                ws.Cell(rowIndex, colIndex).Value = "";
                lastCol = colIndex;

                rowIndex++;
                colIndex = 1;
                while (colIndex < colSA)
                {
                    ws.MergedRanges.Add(ws.Range(ws.Cell(rowIndex - 1, colIndex), ws.Cell(rowIndex, colIndex)));
                    colIndex++;
                }
                ws.Cell(rowIndex, colSA).Value = "R";
                ws.Cell(rowIndex, colSA + 1).Value = "Yds";
                ws.Cell(rowIndex, colIn).Value = "R";
                ws.Cell(rowIndex, colIn + 1).Value = "Yds";
                ws.Cell(rowIndex, colOut).Value = "R";
                ws.Cell(rowIndex, colOut + 1).Value = "Yds";
                ws.Cell(rowIndex, colSaldoAkhir).Value = "R";
                ws.Cell(rowIndex, colSaldoAkhir + 1).Value = "Yds";
                colIndex = colSaldoAkhir + 2;
                while (colIndex <= lastCol)
                {
                    ws.MergedRanges.Add(ws.Range(ws.Cell(rowIndex - 1, colIndex), ws.Cell(rowIndex, colIndex)));
                    colIndex++;
                }

                var rowHeader = ws.Range(ws.Cell(rowIndex - 1, 1), ws.Cell(rowIndex, lastCol));
                rowHeader.Style.Fill.BackgroundColor = XLColor.LightBlue;
                rowHeader.Style.Font.Bold = true;
                rowHeader.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;


                rowIndex++;

                string? previousKategori = "";
                var startYs = -1;
                var endYs = -1;
                var kodeGeneral = string.Empty;
                decimal ts = 0;
                var colTs = 0;
                foreach (var key in rows.Keys)
                {
                    var list = rows[key];
                    if (list == null || list.Count == 0)
                        continue;
                    if (previousKategori != list[0].Kategori)
                    {
                        ws.Cell(rowIndex, colKode - 1).Value = list[0].Kategori;
                        rowIndex++;
                    }

                    ws.Cell(rowIndex, colKode - 1).Value = list[0].Kategori;
                    ws.Cell(rowIndex, colKode).Value = key;
                    ws.Cell(rowIndex, colKode).Style.Fill.BackgroundColor = XLColor.Yellow;
                    rowIndex++;
                    foreach (var row in list)
                    {
                        colIndex = 1;


                        var SaR = row.R - row.InR + row.OutR;
                        var SaYard = row.Yard - row.InYard + row.OutYard;
                        ws.Cell(rowIndex, colIndex++).Value = row.KP;
                        ws.Cell(rowIndex, colIndex++).Value = row.Identitas;
                        ws.Cell(rowIndex, colIndex++).Value = row.OZ;
                        ws.Cell(rowIndex, colIndex++).Value = row.KodeI;
                        ws.Cell(rowIndex, colIndex++).Value = row.KodeGeneral;
                        ws.Cell(rowIndex, colIndex++).Value = row.Kategori;
                        ws.Cell(rowIndex, colIndex++).Value = row.Kode;
                        //ws.Cell(rowIndex, colIndex++).Value = row.K;
                        //SA
                        ws.Cell(rowIndex, colIndex++).Value = SaR > 0 ? SaR : "";
                        ws.Cell(rowIndex, colIndex++).Value = SaYard > 0 ? SaYard : "";
                        //In
                        ws.Cell(rowIndex, colIndex++).Value = row.InR > 0 ? row.InR : "";
                        ws.Cell(rowIndex, colIndex++).Value = row.InYard > 0 ? row.InYard : "";
                        //Out
                        ws.Cell(rowIndex, colIndex++).Value = row.OutR > 0 ? row.OutR : "";
                        ws.Cell(rowIndex, colIndex++).Value = row.OutYard > 0 ? row.OutYard : "";
                        //YDS
                        ws.Cell(rowIndex, colIndex++).Value = row.R > 0 ? row.R : "";
                        ws.Cell(rowIndex, colIndex++).Value = row.Yard > 0 ? row.Yard : "";
                        //T.S.
                        colTs = colIndex;
                        if (kodeGeneral != row.KodeGeneral)
                        {
                            if (startYs > 0 && endYs > 0)
                            {
                                //merge previous YS
                                ws.Range(ws.Cell(startYs, colTs), ws.Cell(endYs, colTs)).Merge();
                                ws.Cell(startYs, colTs).Value = ts;
                            }

                            startYs = rowIndex;
                            endYs = rowIndex;
                            ts = row.Yard;
                            kodeGeneral = row.KodeGeneral;
                        }
                        else
                        {
                            ts += row.Yard;
                            endYs = rowIndex;
                        }
                        colIndex++; //TS
                        //ws.Cell(rowIndex, colIndex++).Value = ts;
                        //P
                        ws.Cell(rowIndex, colIndex++).Value = row.P;
                        //GR
                        ws.Cell(rowIndex, colIndex++).Value = row.GR;
                        //SAK
                        ws.Cell(rowIndex, colIndex++).Value = row.SAK;
                        //Total
                        ws.Cell(rowIndex, colIndex).Value = row.Total;
                        rowIndex++;
                    }

                    if (startYs > 0 && endYs > 0)
                    {
                        ws.Range(ws.Cell(startYs, colTs), ws.Cell(endYs, colTs)).Merge();
                        ws.Cell(startYs, colTs).Value = ts;
                        startYs = -1;
                        kodeGeneral = string.Empty;
                    }

                    colIndex = 6;
                    ws.Cell(rowIndex, colIndex++).Value = list[0].Kategori;
                    ws.Cell(rowIndex, colIndex++).Value = $"TOTAL {list[0].Kategori}";
                    ws.Cell(rowIndex, colIndex).FormulaA1 =
                        $"=SUM({ws.Cell(rowIndex - list.Count, colIndex).Address.ToString()}:{ws.Cell(rowIndex - 1, colIndex).Address.ToString()})";
                    colIndex++;
                    ws.Cell(rowIndex, colIndex).FormulaA1 =
                        $"=SUM({ws.Cell(rowIndex - list.Count, colIndex).Address.ToString()}:{ws.Cell(rowIndex - 1, colIndex).Address.ToString()})";
                    colIndex++;
                    ws.Cell(rowIndex, colIndex).FormulaA1 =
                        $"=SUM({ws.Cell(rowIndex - list.Count, colIndex).Address.ToString()}:{ws.Cell(rowIndex - 1, colIndex).Address.ToString()})";
                    colIndex++;
                    ws.Cell(rowIndex, colIndex).FormulaA1 =
                        $"=SUM({ws.Cell(rowIndex - list.Count, colIndex).Address.ToString()}:{ws.Cell(rowIndex - 1, colIndex).Address.ToString()})";
                    colIndex++;
                    ws.Cell(rowIndex, colIndex).FormulaA1 =
                        $"=SUM({ws.Cell(rowIndex - list.Count, colIndex).Address.ToString()}:{ws.Cell(rowIndex - 1, colIndex).Address.ToString()})";
                    colIndex++;
                    ws.Cell(rowIndex, colIndex).FormulaA1 =
                        $"=SUM({ws.Cell(rowIndex - list.Count, colIndex).Address.ToString()}:{ws.Cell(rowIndex - 1, colIndex).Address.ToString()})";
                    colIndex++;
                    ws.Cell(rowIndex, colIndex).FormulaA1 =
                        $"=SUM({ws.Cell(rowIndex - list.Count, colIndex).Address.ToString()}:{ws.Cell(rowIndex - 1, colIndex).Address.ToString()})";
                    colIndex++;
                    ws.Cell(rowIndex, colIndex).FormulaA1 =
                        $"=SUM({ws.Cell(rowIndex - list.Count, colIndex).Address.ToString()}:{ws.Cell(rowIndex - 1, colIndex).Address.ToString()})";


                    ws.Range(ws.Cell(rowIndex, 6), ws.Cell(rowIndex, colIndex)).Style.Font.SetBold(true);
                    rowIndex += 2;
                }

                ws.Columns(1, lastCol).AdjustToContents();
                for (int i = 1; i <= lastCol; i++)
                {
                    var w = ws.Column(i).Width;
                    if (w < 8)
                    {
                        ws.Column(i).Width = 8;
                    }
                }

                var rowTable = ws.Range(ws.Cell(1, 1), ws.Cell(rowIndex-1, lastCol));
                rowTable.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                rowTable.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                rowTable.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                rowTable.Style.Border.RightBorder = XLBorderStyleValues.Medium;
            }
            catch (Exception)
            {
                return null;
            }

            return wb;
        }

    }
}
