using AutoMapper;
using ClosedXML.Excel;
using MediatR;
using Newtonsoft.Json;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Application.Reports.Requests;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace RfidBarcode.Application.Reports.Handlers
{
    public class ImportDailySummaryHandler : BaseHandler, 
        IRequestHandler<ImportDailySummaryRequest, BaseResponse>
    {
        private readonly IUserResolverService _user;
        
        public ImportDailySummaryHandler(IApplicationDbContext context, IMapper mapper, 
            IUserResolverService user)
        {
            _context = context;
            _mapper = mapper;
            _user = user;
        }

        public async Task<BaseResponse> Handle(ImportDailySummaryRequest request, 
            CancellationToken cancellationToken)
        {
            var response = new BaseResponse();
            
            var errorList = new List<string>();
            IFormFile file = request.File;
            string fileName = file.FileName;
            
            // Calculate file hash for duplicate detection
            using var stream = file.OpenReadStream();
            using var sha = SHA256.Create();
            var hash = BitConverter.ToString(sha.ComputeHash(stream))
                .Replace("-", "").ToLower();

            try
            {
                using (var workbook = new XLWorkbook(file.OpenReadStream()))
                {
                    var ws = workbook.Worksheet(1);
                    var rowCount = ws.LastRowUsed()?.RowNumber();

                    // Start from row 3 (assuming row 1-2 are headers)
                    var gradeGroup = "";
                    for (int row = 3; row <= rowCount; row++)
                    {
                        try
                        {
                            var kategori = ws.Cell(row, 6).GetValue<string>();

                            // Skip empty rows
                            if (kategori == null || kategori!.ToString().Length == 0)
                            {
                                continue;
                            }
                            Console.WriteLine($"Processing row {row}: Kategori={kategori}");
                            var kode = ws.Cell(row, 7).GetValue<string>();

                            var kategoryStyle = ws.Cell(row, 6).Style.Font.Bold;
                            if (kategoryStyle && kode.StartsWith("TOTAL"))
                            {
                                continue; //this is total row
                            }

                            var kodeColor = ws.Cell(row, 7).Style.Fill.BackgroundColor;
                            if (kodeColor == XLColor.Yellow)
                            {
                                gradeGroup = kode;
                                Console.WriteLine($"Row {row} is GradeGroup: {gradeGroup}");
                                continue;
                            }

                            var kp = ws.Cell(row, 1).GetValue<string>();
                            if (string.IsNullOrEmpty(kp) && string.IsNullOrEmpty(kode))
                            {
                                continue;
                            }

                            var param1 = ws.Cell(row, 2).GetValue<string>();
                            var oz = ws.Cell(row, 3).GetValue<string>();
                            var kodeI = ws.Cell(row, 4).GetValue<string>();
                            var kodeGeneral = ws.Cell(row, 7).GetValue<string>();
                            var param2 = ws.Cell(row, 8).GetValue<string>();
                            var p = ws.Cell(row, 18).GetValue<string>();
                            var gr = ws.Cell(row, 19).GetValue<string>();
                            var param3 = ws.Cell(row, 20).GetValue<string>();
                            var param4 = ws.Cell(row, 22).GetValue<string>();
                            var param5 = ws.Cell(row, 23).GetValue<string>();
                            var param6 = ws.Cell(row, 24).GetValue<string>();


                            var entity = await _context.StockParams
                                .Where(x => 
                                    x.c1 == gradeGroup &&
                                    x.c2 == kategori &&
                                    x.c3 == kodeI && 
                                    x.c4 == kodeGeneral &&
                                    x.c5 == gr
                                ).FirstOrDefaultAsync(cancellationToken);

                            if (entity == null)
                            {
                                entity = new Domain.Entities.StockParam
                                {
                                    c1 = gradeGroup,
                                    c2 = kategori,
                                    c3 = kodeI,
                                    c4 = kodeGeneral,
                                    c5 = gr,
                                    p1 = param1,
                                    p2 = oz,
                                    p3 = param2,
                                    p4 = p,
                                    p5 = param3,
                                    p6 = param4,
                                    p7 = param5,
                                    p8 = param6
                                };
                                _context.StockParams.Add(entity);
                            }
                            else
                            {
                                entity.p1 = param1;
                                entity.p2 = oz;
                                entity.p3 = param2;
                                entity.p4 = p;
                                entity.p5 = param3;
                                entity.p6 = param4;
                                entity.p7 = param5;
                                entity.p8 = param6;
                            }

                            await _context.SaveChangesAsync(cancellationToken);
                            response.Result = BaseResponse.RESULT_OK;
                        }
                        catch (Exception ex)
                        {
                            errorList.Add($"Error line-{row}: {ex.Message}");
                        }
                    }

                    if (errorList.Count == 0)
                    {
                        response.Result = BaseResponse.RESULT_OK;
                        response.Message = "Berhasil import data!";

                    }
                    else
                    {
                        response.Message = string.Join("\n", errorList);
                    }
                }
            }
            catch (Exception ex)
            {
                response.Message = "Exception: " + ex.Message;
            }

            return response;
        }
    }
}