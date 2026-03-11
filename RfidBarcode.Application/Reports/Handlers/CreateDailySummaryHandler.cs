using AutoMapper;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Application.Reports.Requests;
using RfidBarcode.Application.Reports.ViewModels;
using System.Linq.Dynamic.Core;
using System.Reflection.PortableExecutable;

namespace RfidBarcode.Application.Reports.Handlers
{
    public class CreateDailySummaryHandler : BaseHandler, 
        IRequestHandler<CreateDailySummaryRequest, 
            BaseObjectResponse<Dictionary<string, List<DailySummaryVM>>>>
    {
        public CreateDailySummaryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<BaseObjectResponse<Dictionary<string, List<DailySummaryVM>>>> 
            Handle(CreateDailySummaryRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseObjectResponse<Dictionary<string, List<DailySummaryVM>>>();

            try
            {
                var count = _context.Items.Count();
                //check item without ScanP1
                var qry = _context.Items
                    .Select(x => new
                    {
                        KP = x.Kp,
                        Identitas = x.IdentitasBenang ?? "",
                        OZ = x.Oz ?? "",
                        KodeI = x.Kode1 ?? "",
                        KodeGeneral = x.Kode2 ?? "",
                        Kategori = x.Kode3 ?? "",
                        Kode = x.Kode ?? "",
                        Kode1 = x.Kode1 ?? "",
                        //K = x.Kode3 ?? "",
                        R = x.R ?? 0,
                        Yard = x.Yard ?? 0,
                        TS = 0,
                        P = 0,
                        GR = x.Grade ?? "",
                        SAK = "",
                        StockOut = x.OutScan,
                        TangalBuatBarcode = x.TanggalBuatBarcode,
                        LocationId = x.LocationId
                    })
                    .Where(x => (x.StockOut == null || x.StockOut >= request.PreviousDate) &&
                        x.LocationId != null &&
                        x.TangalBuatBarcode < request.CurrentDate)
                    .GroupBy(x => new
                    {
                        KP = x.KP,
                        Identitas = x.Identitas,
                        OZ = x.OZ,
                        KodeI = x.KodeI,
                        KodeGeneral = x.KodeGeneral,
                        Kategori = x.Kategori,
                        Kode1 = x.Kode1,
                        Kode = x.Kode,
                        //K = x.K,
                        GR = x.GR
                    })
                    .Select(g => new DailySummaryVM
                    {
                        KP = g.Key.KP,
                        Identitas = g.Key.Identitas,
                        OZ = g.Key.OZ,
                        KodeI = g.Key.KodeI,
                        KodeGeneral = g.Key.KodeGeneral,
                        Kategori = g.Key.Kategori,
                        Kode1 = g.Key.Kode1,
                        Kode = g.Key.Kode,
                        //K = g.Key.K,
                        SaR = 0,
                        SaYard = 0,
                        InR = g.Count(x => x.StockOut == null && x.TangalBuatBarcode > request.PreviousDate),
                        InYard = g.Sum(x => x.StockOut == null && x.TangalBuatBarcode > request.PreviousDate ? x.Yard : 0),
                        OutR = g.Count(x => x.StockOut > request.PreviousDate && x.StockOut <= request.CurrentDate),
                        OutYard = g.Sum(x => x.StockOut > request.PreviousDate && x.StockOut <= request.CurrentDate ? x.Yard : 0),
                        R = g.Count(),
                        Yard = g.Sum(x => x.Yard),
                        TS = 0, // Assuming no TS for this case
                        P = 0, // Assuming no P for this case
                        GR = g.Key.GR,
                        SAK = "", // Assuming SAK is not available in this context
                        Total = "0", // Placeholder for total, adjust as needed
                        GradeGroup = g.Key.GR == "AXP" || g.Key.GR == "JD" ? "A" : 
                            g.Key.GR == "ALK" || g.Key.GR == "ABL" ? "AB" : g.Key.GR,
                        GradeGroupSeq = g.Key.GR == "AXP" || g.Key.GR == "JD" ? 1 :
                            g.Key.GR == "ALK" || g.Key.GR == "ABL" ? 2 : 3,
                    })
                    .AsQueryable();

                if (!string.IsNullOrEmpty(request.Kode))
                {
                    qry = qry.Where(x => x.Kategori.Contains(request.Kode));
                }
                if (!string.IsNullOrEmpty(request.Grade))
                {
                    qry = qry.Where(x => x.GR.Contains(request.Grade));
                }

                var items = await qry.ToListAsync(cancellationToken);
                var rows = new Dictionary<string, List<DailySummaryVM>>();
                var keys = items.Select(x => new
                {
                    Kategori = x.Kategori,
                    GradeGroup = x.GradeGroup,
                    GradeGroupSeq = x.GradeGroupSeq,
                    GR = x.GR
                }).Distinct()
                    .OrderBy(x => x.Kategori)
                    .ThenBy(x => x.GradeGroup)
                    .ThenBy(x => x.GR)
                    .ToList();

                keys.ForEach(x =>
                    {
                        var list = items.Where(y => y.Kategori == x.Kategori && y.GR == x.GR)
                        .ToList();
                        if (list != null)
                        {
                            var key = $"{x.Kategori.PadRight(25)} {x.GradeGroup}";
                            rows[key] = list;
                        }
                    });
                Console.WriteLine($"Rows Count: {rows.Count}");
                response.Data = rows;
                response.Message = "";
                response.Result = BaseResponse.RESULT_OK;
            }
            catch (Exception)
            {
                response.Message = "An error occurred while creating the daily summary.";
            }
            return response;
        }

        
    }
}
