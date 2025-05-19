using RfidBarcode.Application.Users.ViewModels;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using RfidBarcode.Domain.Entities;
using DocumentFormat.OpenXml.Office2010.Excel;
using RfidBarcode.Application.Operationals.ViewModels;

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

        //public static string GenerateSuratJalanNo(string type, int year, int month, int count)
        //{
        //    return type + "/" + month.ToString("D2") + "/" + year.ToString() + "/" + count.ToString("D4");
        //}
    }
}
