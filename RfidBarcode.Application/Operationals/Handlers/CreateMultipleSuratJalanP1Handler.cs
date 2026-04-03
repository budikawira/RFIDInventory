using AutoMapper;
using DocumentFormat.OpenXml.Office.Word;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Application.Common.Libs;
using RfidBarcode.Application.Operationals.Requests;
using RfidBarcode.Application.Operationals.Responses;
using RfidBarcode.Application.Operationals.ViewModels;
using RfidBarcode.Domain.Entities;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Text;
using Item = RfidBarcode.Domain.Entities.Item;

namespace RfidBarcode.Application.Operationals.Handlers
{
    public class CreateMultipleSuratJalanP1Handler : BaseHandler, IRequestHandler<CreateMultipleSuratJalanP1Request,
        CreateMultipleSuratJalanP1Response>
    {
        private readonly IUserResolverService _user;

        public CreateMultipleSuratJalanP1Handler(IApplicationDbContext context, IMapper mapper, IUserResolverService user)
        {
            _context = context;
            _mapper = mapper;
            _user = user;
        }

        public async Task<CreateMultipleSuratJalanP1Response> Handle(CreateMultipleSuratJalanP1Request request, CancellationToken cancellationToken)
        {
            var maxItem = 25;
            var response = new CreateMultipleSuratJalanP1Response();

            using (var trx = await _context.Db.BeginTransactionAsync())
            {
                try
                {
                    var items = await _context.Items
                        .Include(x => x.InSuratJalan)
                        .Include(x => x.OutSuratJalan)
                        .Where(x => request.Ids.Contains(x.Id)).ToListAsync();

                    //check if there is any invalid items
                    //items that already paired with other Surat Jalan Outbond
                    if (request.Mode == CreateMultipleSuratJalanP1Request.MODE_OUTBOND)
                    {
                        #region Outbond
                        var invalidItems = items.Where(x => x.OutSuratJalanId != null);
                        if (invalidItems.Count() > 0)
                        {
                            foreach (var item in invalidItems)
                            {
                                response.NokTagIds.Add(item.Id, item.OutSuratJalan != null ? item.OutSuratJalan.No ?? "Draft Outbond" : "-");
                            }
                            response.Message = "Beberapa data sudah memiliki Surat Jalan Outbond";
                            return response;
                        }

                        var groups = items
                            .Select(x => new
                            {
                                Kode = (x.Grade != null && x.Grade.ToUpper() == "AXP") ? x.Kode : "",
                                Kode1 = (x.Grade != null && x.Grade.ToUpper() == "AXP") ? x.Kode1 : "",
                                Kode2 = (x.Grade != null && x.Grade.ToUpper() == "AXP") ? x.Kode2 : "",
                                Kode3 = (x.Grade != null && x.Grade.ToUpper() == "AXP") ? x.Kode3 : "",
                                Kode4 = (x.Grade != null && x.Grade.ToUpper() == "AXP") ? x.Kode4 : "",
                                x.Grade,
                                x.LocationId,
                                x.InSuratJalanId
                            })
                            .GroupBy(x => new { x.Kode, x.Kode1, x.Kode2, x.Kode3, x.Kode4, x.Grade })
                            .Select(x => new
                            {
                                x.Key.Kode,
                                x.Key.Kode1,
                                x.Key.Kode2,
                                x.Key.Kode3,
                                x.Key.Kode4,
                                x.Key.Grade
                            })
                            .ToList();

                        foreach (var group in groups)
                        {
                            var qry = items.AsQueryable();

                            if (group.Grade != null && group.Grade.ToUpper() != "AXP")
                            {
                                qry = qry.Where(x => x.Grade == group.Grade);


                                var itemsInGroup = qry.ToList();

                                int i = 0;
                                while (i < itemsInGroup.Count)
                                {
                                    var itemsToSent = itemsInGroup.Skip(i).Take(maxItem).ToList();
                                    i += maxItem;
                                    //create surat jalan
                                    var userId = _user.GetUserId();
                                    var k3 = new SuratJalan()
                                    {
                                        Kode = group.Kode,
                                        Kode1 = group.Kode1,
                                        Kode2 = group.Kode2,
                                        Kode3 = group.Kode3,
                                        Kode4 = group.Kode4,
                                        Grade = group.Grade,
                                        UserId = _user.GetUserId(),
                                        SuratJalanType = SuratJalanType.TYPE_OUTBOND,
                                        //No = Helper.GenerateSuratJalanNo("P1", request.Year, request.Month, count),
                                        //FinalizeDate = DateTime.Now
                                    };
                                    await _context.SuratJalans.AddAsync(k3);
                                    await _context.SaveChangesAsync(cancellationToken);

                                    var k3Vm = _mapper.Map<SuratJalanVM>(k3);

                                    //create items
                                    foreach (var itemK3 in itemsToSent)
                                    {
                                        itemK3.OutSuratJalanId = k3.Id;
                                        if (itemK3.OutSuratJalan != null)
                                        {
                                            response.NokTagIds.Add(itemK3.Id, itemK3.OutSuratJalan!.No ?? "Failed");
                                        }
                                        else
                                        {
                                            response.OkTagIds.Add(itemK3.Id, "Ok");
                                        }
                                    }

                                    await _context.SaveChangesAsync(cancellationToken);
                                }
                            }
                            else
                            {
                                qry = qry.Where(x => x.Kode == group.Kode
                                        && x.Grade == group.Grade && //x.Kp == group.Kp &&
                                        (x.Grade != null && x.Grade.ToUpper() == "AXP"));

                                var itemsInGroup = qry.ToList();
                                var kps = itemsInGroup.GroupBy(x => x.Kp)
                                    .Select(g => new
                                    {
                                        Kp = g.Key,
                                        Count = g.Count()
                                    }).OrderByDescending(x => x.Count).ToList();

                                var normalizedKps = new List<int>();
                                foreach (var kp in kps)
                                {
                                    if (kp.Count <= 5)
                                    {
                                        normalizedKps.Add(kp.Count);
                                    }
                                    else
                                    {
                                        var count = kp.Count;
                                        while (count > 0)
                                        {
                                            normalizedKps.Add(Math.Min(5, count));
                                            count -= 5;
                                        }
                                    }
                                }

                                List<Item> updatedItems = new List<Item>();
                                int offset = 0;
                                int col = 0;
                                for (int kpIx = 0; kpIx < normalizedKps.Count; kpIx++)
                                {
                                    var normalizedKp = normalizedKps[kpIx];
                                    for (int i = 0; i < normalizedKp; i++)
                                    {
                                        updatedItems.Add(itemsInGroup[i + offset]);
                                    }
                                    offset += normalizedKp;
                                    col++;
                                    if (col == 5)
                                    {
                                        col = 0;
                                        var userId = _user.GetUserId();
                                        var k3 = new SuratJalan()
                                        {
                                            //Kp = group.Kp,
                                            Kode = group.Kode,
                                            Kode1 = group.Kode1,
                                            Kode2 = group.Kode2,
                                            Kode3 = group.Kode3,
                                            Kode4 = group.Kode4,
                                            Grade = group.Grade,
                                            //Type = request.SuratJalanType,
                                            UserId = _user.GetUserId(),
                                            SuratJalanType = SuratJalanType.TYPE_OUTBOND,
                                            //No = Helper.GenerateSuratJalanNo("P1", request.Year, request.Month, count),
                                            //FinalizeDate = DateTime.Now
                                        };
                                        await _context.SuratJalans.AddAsync(k3);
                                        await _context.SaveChangesAsync(cancellationToken);

                                        var k3Vm = _mapper.Map<SuratJalanVM>(k3);

                                        //create items
                                        foreach (var updatedItem in updatedItems)
                                        {
                                            updatedItem.OutSuratJalanId = k3.Id;
                                            if (updatedItem.OutSuratJalanId != null && updatedItem.OutSuratJalan != null)
                                            {
                                                response.NokTagIds.Add(updatedItem.Id,
                                                    updatedItem.OutSuratJalan.No ?? "Failed");
                                            }
                                            else
                                            {
                                                response.OkTagIds.Add(updatedItem.Id, k3.No ?? "Ok");
                                            }
                                        }

                                        await _context.SaveChangesAsync(cancellationToken);
                                        updatedItems.Clear();
                                    }
                                }

                                if (updatedItems.Count > 0)
                                {
                                    var userId = _user.GetUserId();
                                    var k3 = new SuratJalan()
                                    {
                                        //Kp = group.Kp,
                                        Kode = group.Kode,
                                        Kode1 = group.Kode1,
                                        Kode2 = group.Kode2,
                                        Kode3 = group.Kode3,
                                        Kode4 = group.Kode4,
                                        Grade = group.Grade,
                                        //Type = request.SuratJalanType,
                                        UserId = _user.GetUserId(),
                                        SuratJalanType = SuratJalanType.TYPE_OUTBOND,
                                        // No = Helper.GenerateSuratJalanNo("P1", request.Year, request.Month, count),
                                        //FinalizeDate = DateTime.Now
                                    };
                                    await _context.SuratJalans.AddAsync(k3);
                                    await _context.SaveChangesAsync(cancellationToken);

                                    var k3Vm = _mapper.Map<SuratJalanVM>(k3);

                                    //create items
                                    foreach (var updatedItem in updatedItems)
                                    {
                                        updatedItem.OutSuratJalanId = k3.Id;
                                        if (updatedItem.OutSuratJalanId != null && updatedItem.OutSuratJalan != null)
                                        {
                                            response.NokTagIds.Add(updatedItem.Id, updatedItem.OutSuratJalan.No ?? "Failed");
                                        }
                                        else
                                        {
                                            response.OkTagIds.Add(updatedItem.Id, k3.No ?? "Ok");
                                        }
                                    }

                                    await _context.SaveChangesAsync(cancellationToken);
                                }
                            }

                        }
                        #endregion
                    } 
                    else
                    {
                        #region Inbound
                        var invalidItems = items.Where(x => x.OutSuratJalanId != null || x.InSuratJalanId != null);
                        if (invalidItems.Count() > 0)
                        {
                            foreach (var item in invalidItems)
                            {
                                var status = item.InSuratJalan != null ? item.InSuratJalan.No ?? "Draft Inbound" : 
                                    (item.OutSuratJalan != null ? item.OutSuratJalan.No ?? "Draft Outbond" : "-");
                                response.NokTagIds.Add(item.Id, status);
                            }
                            response.Message = "Beberapa data sudah memiliki Surat Jalan Inbound atau Outbond";
                            return response;
                        }

                        var groups = items
                            .Select(x => new
                            {
                                Kode = (x.Grade != null && x.Grade.ToUpper() == "AXP") ? x.Kode : "",
                                Kode1 = (x.Grade != null && x.Grade.ToUpper() == "AXP") ? x.Kode1 : "",
                                Kode2 = (x.Grade != null && x.Grade.ToUpper() == "AXP")  ? x.Kode2 : "",
                                Kode3 = (x.Grade != null && x.Grade.ToUpper() == "AXP") ? x.Kode3 : "",
                                Kode4 = (x.Grade != null && x.Grade.ToUpper() == "AXP") ? x.Kode4 : "",
                                x.Grade,
                                x.LocationId,
                                x.InSuratJalanId
                            })
                            .GroupBy(x => new { x.Kode, x.Kode1, x.Kode2, x.Kode3, x.Kode4, x.Grade })
                            .Select(x => new
                            {
                                x.Key.Kode,
                                x.Key.Kode1,
                                x.Key.Kode2,
                                x.Key.Kode3,
                                x.Key.Kode4,
                                x.Key.Grade
                            })
                            .ToList();

                        foreach (var group in groups)
                        {
                            var qry = items.AsQueryable();

                            if (group.Grade != "AXP")
                            {
                                qry = qry.Where(x => x.Grade == group.Grade);


                                var itemsInGroup = qry.ToList();

                                int i = 0;
                                while (i < itemsInGroup.Count)
                                {
                                    var itemsToSent = itemsInGroup.Skip(i).Take(maxItem).ToList();
                                    i += maxItem;
                                    //create surat jalan
                                    var userId = _user.GetUserId();
                                    var k3 = new SuratJalan()
                                    {
                                        Kode = group.Kode,
                                        Kode1 = group.Kode1,
                                        Kode2 = group.Kode2,
                                        Kode3 = group.Kode3,
                                        Kode4 = group.Kode4,
                                        Grade = group.Grade,
                                        UserId = _user.GetUserId(),
                                        SuratJalanType = SuratJalanType.TYPE_INBOUND,
                                        //No = Helper.GenerateSuratJalanNo("P1", request.Year, request.Month, count),
                                        //FinalizeDate = DateTime.Now
                                    };
                                    await _context.SuratJalans.AddAsync(k3);
                                    await _context.SaveChangesAsync(cancellationToken);

                                    var k3Vm = _mapper.Map<SuratJalanVM>(k3);

                                    //create items
                                    foreach (var itemK3 in itemsToSent)
                                    {
                                        itemK3.InSuratJalanId = k3.Id;
                                        if (itemK3.InSuratJalan != null)
                                        {
                                            response.NokTagIds.Add(itemK3.Id, itemK3.OutSuratJalan!.No ?? "Failed");
                                        }
                                        else
                                        {
                                            response.OkTagIds.Add(itemK3.Id, "Ok");
                                        }
                                    }

                                    await _context.SaveChangesAsync(cancellationToken);
                                }
                            }
                            else
                            {
                                qry = qry.Where(x => x.Kode == group.Kode
                                        && x.Grade == group.Grade && //x.Kp == group.Kp &&
                                        x.Grade == "AXP");

                                var itemsInGroup = qry.ToList();
                                var kps = itemsInGroup.GroupBy(x => x.Kp)
                                    .Select(g => new
                                    {
                                        Kp = g.Key,
                                        Count = g.Count()
                                    }).OrderByDescending(x => x.Count).ToList();

                                var normalizedKps = new List<int>();
                                foreach (var kp in kps)
                                {
                                    if (kp.Count <= 5)
                                    {
                                        normalizedKps.Add(kp.Count);
                                    }
                                    else
                                    {
                                        var count = kp.Count;
                                        while (count > 0)
                                        {
                                            normalizedKps.Add(Math.Min(5, count));
                                            count -= 5;
                                        }
                                    }
                                }

                                List<Item> updatedItems = new List<Item>();
                                int offset = 0;
                                int col = 0;
                                for (int kpIx = 0; kpIx < normalizedKps.Count; kpIx++)
                                {
                                    var normalizedKp = normalizedKps[kpIx];
                                    for (int i = 0; i < normalizedKp; i++)
                                    {
                                        updatedItems.Add(itemsInGroup[i + offset]);
                                    }
                                    offset += normalizedKp;
                                    col++;
                                    if (col == 5)
                                    {
                                        col = 0;
                                        var userId = _user.GetUserId();
                                        var k3 = new SuratJalan()
                                        {
                                            //Kp = group.Kp,
                                            Kode = group.Kode,
                                            Kode1 = group.Kode1,
                                            Kode2 = group.Kode2,
                                            Kode3 = group.Kode3,
                                            Kode4 = group.Kode4,
                                            Grade = group.Grade,
                                            //Type = request.SuratJalanType,
                                            UserId = _user.GetUserId(),
                                            SuratJalanType = SuratJalanType.TYPE_INBOUND,
                                            //No = Helper.GenerateSuratJalanNo("P1", request.Year, request.Month, count),
                                            //FinalizeDate = DateTime.Now
                                        };
                                        await _context.SuratJalans.AddAsync(k3);
                                        await _context.SaveChangesAsync(cancellationToken);

                                        var k3Vm = _mapper.Map<SuratJalanVM>(k3);

                                        //create items
                                        foreach (var updatedItem in updatedItems)
                                        {
                                            updatedItem.OutSuratJalanId = k3.Id;
                                            if (updatedItem.OutSuratJalanId != null && updatedItem.OutSuratJalan != null)
                                            {
                                                response.NokTagIds.Add(updatedItem.Id,
                                                    updatedItem.OutSuratJalan.No ?? "Failed");
                                            }
                                            else
                                            {
                                                response.OkTagIds.Add(updatedItem.Id, k3.No ?? "Ok");
                                            }
                                        }

                                        await _context.SaveChangesAsync(cancellationToken);
                                        updatedItems.Clear();
                                    }
                                }

                                if (updatedItems.Count > 0)
                                {
                                    var userId = _user.GetUserId();
                                    var k3 = new SuratJalan()
                                    {
                                        //Kp = group.Kp,
                                        Kode = group.Kode,
                                        Kode1 = group.Kode1,
                                        Kode2 = group.Kode2,
                                        Kode3 = group.Kode3,
                                        Kode4 = group.Kode4,
                                        Grade = group.Grade,
                                        //Type = request.SuratJalanType,
                                        UserId = _user.GetUserId(),
                                        SuratJalanType = SuratJalanType.TYPE_INBOUND,
                                        // No = Helper.GenerateSuratJalanNo("P1", request.Year, request.Month, count),
                                        //FinalizeDate = DateTime.Now
                                    };
                                    await _context.SuratJalans.AddAsync(k3);
                                    await _context.SaveChangesAsync(cancellationToken);

                                    var k3Vm = _mapper.Map<SuratJalanVM>(k3);

                                    //create items
                                    foreach (var updatedItem in updatedItems)
                                    {
                                        updatedItem.InSuratJalanId = k3.Id;
                                        if (updatedItem.InSuratJalanId != null && updatedItem.InSuratJalan != null)
                                        {
                                            response.NokTagIds.Add(updatedItem.Id, updatedItem.InSuratJalan.No ?? "Failed");
                                        }
                                        else
                                        {
                                            response.OkTagIds.Add(updatedItem.Id, k3.No ?? "Ok");
                                        }
                                    }

                                    await _context.SaveChangesAsync(cancellationToken);
                                }
                            }

                        }
                        #endregion
                    }


                    if (response.NokTagIds.Count == 0)
                    {
                        await trx.CommitAsync(cancellationToken);
                        response.Result = BaseResponse.RESULT_OK;
                        response.Message = "Data berhasil dibuat!";
                    }

                }
                catch (Exception ex)
                {
                    await trx.RollbackAsync();
                    response.Message = "Exception: " + ex.Message;
                }
            }

            

            return response;
        }
    }
}
