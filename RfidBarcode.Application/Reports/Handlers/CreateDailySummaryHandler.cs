using AutoMapper;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using LinqKit;
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
                var q = from item in _context.Items
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
                        KodeI = x.KodeI,
                        KodeGeneral = x.KodeGeneral,
                        Kategori = x.Kategori,
                        Kode1 = x.Kode1,
                        Kode = x.Kode,
                        GR = x.GR
                    })
                    .Select(g => new DailySummaryVM
                    {
                        KP = g.Key.KP,
                        KodeI = g.Key.KodeI,
                        KodeGeneral = g.Key.KodeGeneral,
                        Kategori = g.Key.Kategori,
                        Kode1 = g.Key.Kode1,
                        Kode = g.Key.Kode,
                        SaR = 0,
                        SaYard = 0,
                        InR = g.Count(x => x.StockOut == null && x.TangalBuatBarcode > request.PreviousDate),
                        InYard = g.Sum(x => x.StockOut == null && x.TangalBuatBarcode > request.PreviousDate ? x.Yard : 0),
                        OutR = g.Count(x => x.StockOut > request.PreviousDate && x.StockOut <= request.CurrentDate),
                        OutYard = g.Sum(x => x.StockOut > request.PreviousDate && x.StockOut <= request.CurrentDate ? x.Yard : 0),
                        R = g.Count(),
                        Yard = g.Sum(x => x.Yard),
                        TS = 0,
                        GR = g.Key.GR,
                        SAK = "",
                        Total = "0",
                        GradeGroup = g.Key.GR.ToUpper() == "AXP" || g.Key.GR.ToUpper() == "JD" ? "A" :
                            g.Key.GR.ToUpper() == "ALK" || g.Key.GR.ToUpper() == "ABL" ? "AB" : g.Key.GR,
                        GradeGroupSeq = g.Key.GR.ToUpper() == "AXP" || g.Key.GR.ToUpper() == "JD" ? 1 :
                            g.Key.GR.ToUpper() == "ALK" || g.Key.GR.ToUpper() == "ABL" ? 2 : 3,
                    })
                    join stockParam in _context.StockParams
                        on new { Kategori = item.Kategori, KodeI = item.KodeI, Kode = item.Kode, GR = item.GR }
                        equals new { Kategori = stockParam.c2 ?? "", KodeI = stockParam.c3 ?? "", Kode = stockParam.c4 ?? "", GR = stockParam.c5 ?? "" }
                        into stockParams
                    from stockParam in stockParams.DefaultIfEmpty()
                    select new { x = item, StockParam = stockParam };

                var qry = q.Select(x => new DailySummaryVM
                {
                    KP = x.x.KP,
                    KodeI = x.x.KodeI,
                    KodeGeneral = x.x.KodeGeneral,
                    Kategori = x.x.Kategori,
                    Kode1 = x.x.Kode1,
                    Kode = x.x.Kode,
                    SaR = x.x.SaR,
                    SaYard = x.x.SaYard,
                    InR = x.x.InR,
                    InYard = x.x.InYard,
                    OutR = x.x.OutR,
                    OutYard = x.x.OutYard,
                    R = x.x.R,
                    Yard = x.x.Yard,
                    TS = x.x.TS,
                    GR = x.x.GR,
                    SAK = "",
                    Total = x.x.Total,
                    GradeGroup = x.x.GradeGroup,
                    GradeGroupSeq = x.x.GradeGroupSeq,
                    p1 = x.StockParam.p1 ?? "",
                    p2 = x.StockParam.p2 ?? "",
                    p3 = x.StockParam.p3 ?? "",
                    p4 = x.StockParam.p4 ?? "",
                    p5 = x.StockParam.p5 ?? "",
                    p6 = x.StockParam.p6 ?? "",
                    p7 = x.StockParam.p7 ?? "",
                    p8 = x.StockParam.p8 ?? ""
                });

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
