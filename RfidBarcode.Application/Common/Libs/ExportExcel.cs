using ClosedXML.Excel;
using System.Drawing;
using System.Reflection;

namespace RfidBarcode.Application.Common.Exports
{
    public class ExportExcel<T> where T : class
    {
        public string? Title;
        public string[] SubTitles;
        public string[] Headers;
        public string[] Columns;
        public int[] DataTypes;

        public int HeaderMerge = -1;

        public const int TYPE_STRING = 0;
        public const int TYPE_INT = 1;
        public const int TYPE_LONG = 2;
        public const int TYPE_DOUBLE = 3;
        public const int TYPE_DECIMAL = 4;

        public ExportExcel(string? title, string[] subTitles,
            string[] headers, string[] columns,
            int[] dataTypes)
        {
            Title = title;
            SubTitles = subTitles;
            Headers = headers;
            Columns = columns;
            DataTypes = dataTypes;
        }

        public XLWorkbook ExportFile(List<T> list, PropertyInfo[] propertyInfos)
        {
            XLWorkbook wb = new XLWorkbook();
            try
            {
                var ws = wb.Worksheets.Add("Report");

                var propertyNames = new Dictionary<String, int>();
                var ix = 0;
                foreach (var property in propertyInfos)
                {
                    propertyNames.Add(property.Name, ix++);
                }

                var rowIndex = 1;
                if (!string.IsNullOrEmpty(Title))
                {
                    ws.Cell(rowIndex, 1).Value = Title;

                    rowIndex++;
                }
                for (int i = 0; i < SubTitles.Length; i++)
                {
                    ws.Cell(rowIndex + i, 1).Value = SubTitles[i];
                }
                rowIndex += SubTitles.Length;

                if (HeaderMerge == -1)
                {
                    for (int i = 1; i < rowIndex; i++)
                    {
                        ws.Range(ws.Cell(i, 1), ws.Cell(i, Headers.Length)).Merge();
                    }
                }

                if (!string.IsNullOrEmpty(Title))
                {
                    rowIndex++;
                }

                for (int i = 0; i < Headers.Length; i++)
                {
                    ws.Cell(rowIndex, i + 1).Value = Headers[i];
                }
                var rowHeader = ws.Range(ws.Cell(rowIndex, 1), ws.Cell(rowIndex, Headers.Length));
                rowIndex++;

                for (int i = 0; i < list.Count; i++)
                {
                    for (int j = 0; j < Columns.Length; j++)
                    {
                        if (!string.IsNullOrEmpty(Columns[j]))
                        {
                            int col = propertyNames.GetValueOrDefault(Columns[j]);
                            var value = propertyInfos[col].GetValue(list[i]);
                            if (value != null)
                            {
                                switch (DataTypes[j])
                                {
                                    case TYPE_STRING:
                                        ws.Cell(i + rowIndex, j + 1).Value = (string)value; break;
                                    case TYPE_INT:
                                        ws.Cell(i + rowIndex, j + 1).Value = (int)value; break;
                                    case TYPE_LONG:
                                        ws.Cell(i + rowIndex, j + 1).Value = (long)value; break;
                                    case TYPE_DOUBLE:
                                        ws.Cell(i + rowIndex, j + 1).Value = (double)value; break;
                                    case TYPE_DECIMAL:
                                        ws.Cell(i + rowIndex, j + 1).Value = (decimal)value; break;
                                }
                            }
                        }

                    }
                }
                var rowTable = ws.Range(ws.Cell(rowIndex - 1, 1), ws.Cell(rowIndex + list.Count - 1, Headers.Length));

                rowTable.Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                rowTable.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                rowTable.Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                rowTable.Style.Border.RightBorder = XLBorderStyleValues.Medium;
                rowHeader.Style.Fill.BackgroundColor = XLColor.LightBlue;
                rowHeader.Style.Font.Bold = true;

                if (!string.IsNullOrEmpty(Title))
                {
                    ws.Cell(1, 1).Style.Font.FontSize = 18;
                    ws.Cell(1, 1).Style.Font.Bold = true;
                }
                //header
                ws.Columns(1, Headers.Length).AdjustToContents();

            }
            catch (Exception)
            {
                throw;
            }
            return wb;
        }
    }
}
