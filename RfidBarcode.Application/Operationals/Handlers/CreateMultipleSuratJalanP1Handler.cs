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
                        .Include(x => x.SuratJalanP1)
                        .Where(x => request.Ids.Contains(x.Id)).ToListAsync();

                    //check if there is any invalid items
                    //items that already paired with other K3
                    var invalidItems = items.Where(x => x.SuratJalanP1Id != null);
                    if (invalidItems.Count() > 0)
                    {
                        foreach (var item in invalidItems)
                        {
                            response.NokTagIds.Add(item.Id, item.SuratJalanP1 != null ? item.SuratJalanP1.No ?? "Draft" : "-");
                        }
                        response.Message = "Beberapa data sudah memiliki Surat Jalan";
                        return response;
                    }

                    var groups = items
                        .Select(x => new
                        {
                            Kode = (x.Grade != "ALK") ? x.Kode : "",
                            Kode1 = (x.Grade != "ALK") ? x.Kode1 : "",
                            Kode2 = (x.Grade != "ALK") ? x.Kode2 : "",
                            Kode3 = (x.Grade != "ALK") ? x.Kode3 : "",
                            Kode4 = (x.Grade != "ALK") ? x.Kode4 : "",
                            x.Grade,
                            x.LocationId,
                            x.SuratJalanP1Id
                        })
                        .GroupBy(x => new { x.Kode, x.Kode1, x.Kode2, x.Kode3, x.Kode4, x.Grade})
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

                        if (group.Grade == "ALK")
                        {
                            qry = qry.Where(x => x.Grade == "ALK");


                            var itemsInGroup = qry.ToList();

                            int i = 0;
                            while (i < itemsInGroup.Count)
                            {
                                var itemsToSent = itemsInGroup.Skip(i).Take(maxItem).ToList();
                                i += maxItem;
                                //create surat jalan
                                var userId = _user.GetUserId();
                                var k3 = new SuratJalanP1()
                                {
                                    Kode = group.Kode,
                                    Kode1 = group.Kode1,
                                    Kode2 = group.Kode2,
                                    Kode3 = group.Kode3,
                                    Kode4 = group.Kode4,
                                    Grade = group.Grade,
                                    UserId = _user.GetUserId(),
                                    //No = Helper.GenerateSuratJalanNo("P1", request.Year, request.Month, count),
                                    //FinalizeDate = DateTime.Now
                                };
                                await _context.SuratJalanP1s.AddAsync(k3);
                                await _context.SaveChangesAsync(cancellationToken);

                                var k3Vm = _mapper.Map<SuratJalanP1VM>(k3);

                                //create items
                                foreach (var itemK3 in itemsToSent)
                                {
                                    itemK3.SuratJalanP1Id = k3.Id;
                                    if (itemK3.SuratJalanP1 != null)
                                    {
                                        response.NokTagIds.Add(itemK3.Id, itemK3.SuratJalanP1!.No ?? "Failed");
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
                                    x.Grade != "ALK");

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
                            for (int kpIx = 0;  kpIx < normalizedKps.Count; kpIx++)
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
                                    var k3 = new SuratJalanP1()
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
                                        //No = Helper.GenerateSuratJalanNo("P1", request.Year, request.Month, count),
                                        //FinalizeDate = DateTime.Now
                                    };
                                    await _context.SuratJalanP1s.AddAsync(k3);
                                    await _context.SaveChangesAsync(cancellationToken);

                                    var k3Vm = _mapper.Map<SuratJalanP1VM>(k3);

                                    //create items
                                    foreach (var updatedItem in updatedItems)
                                    {
                                        updatedItem.SuratJalanP1Id = k3.Id;
                                        if (updatedItem.SuratJalanP1Id != null && updatedItem.SuratJalanP1 != null)
                                        {
                                            response.NokTagIds.Add(updatedItem.Id, updatedItem.SuratJalanP1.No ?? "Failed");
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
                                var k3 = new SuratJalanP1()
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
                                    // No = Helper.GenerateSuratJalanNo("P1", request.Year, request.Month, count),
                                    //FinalizeDate = DateTime.Now
                                };
                                await _context.SuratJalanP1s.AddAsync(k3);
                                await _context.SaveChangesAsync(cancellationToken);

                                var k3Vm = _mapper.Map<SuratJalanP1VM>(k3);

                                //create items
                                foreach (var updatedItem in updatedItems)
                                {
                                    updatedItem.SuratJalanP1Id = k3.Id;
                                    if (updatedItem.SuratJalanP1Id != null && updatedItem.SuratJalanP1 != null)
                                    {
                                        response.NokTagIds.Add(updatedItem.Id, updatedItem.SuratJalanP1.No ?? "Failed");
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
