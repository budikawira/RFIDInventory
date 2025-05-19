using DocumentFormat.OpenXml.Vml.Office;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Application.Common.Libs;
using RfidBarcode.Application.Operationals.Requests;
using RfidBarcode.Application.Operationals.Responses;
using RfidBarcode.Domain.Entities;

namespace RfidBarcode.Application.Operationals.Handlers
{
    public class StockOpnameUploadHandler : IRequestHandler<StockOpnameUploadRequest, StockOpnameUploadResponse>
    {
        private readonly IApplicationDbContext _context;

        public StockOpnameUploadHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<StockOpnameUploadResponse> Handle(StockOpnameUploadRequest request, CancellationToken cancellationToken)
        {
            var res = new StockOpnameUploadResponse();
            using (var trans = await _context.Db.BeginTransactionAsync())
            {
                try
                {
                    var ids = new List<long>();
                    var scannedIds = new List<long>();
                    var misplacedIds = new List<long>();
                    var notScannedIds = new List<long>();
                    foreach (var tagId in request.TagIds)
                    {
                        var id = Helper.ParseItemTagId(tagId);
                        if (id != null)
                        {
                            ids.Add((long)id);
                            scannedIds.Add((long)id);
                        }
                        else
                        {
                            res.NokTagIds.Add(tagId, "Invalid Tag");
                        }
                    }
                    foreach (var tagId in request.Misplaced)
                    {
                        var id = Helper.ParseItemTagId(tagId);
                        if (id != null)
                        {
                            ids.Add((long)id);
                            misplacedIds.Add((long)id);
                        }
                        else
                        {
                            res.NokTagIds.Add(tagId, "Invalid Tag");
                        }
                    }
                    foreach (var tagId in request.NotScanned)
                    {
                        var id = Helper.ParseItemTagId(tagId);
                        if (id != null)
                        {
                            notScannedIds.Add((long)id);
                        }
                        else
                        {
                            res.NokTagIds.Add(tagId, "Invalid Tag");
                        }
                    }
                    //long userId = request.UserId;
                    string userName = await _context.Users.Where(x => x.Id == request.UserId)
                        .Select(x => x.UserName)
                        .FirstOrDefaultAsync() ?? "";

                    var location = await _context.Locations.Where(x => x.Id == request.LocationId)
                        .FirstOrDefaultAsync();

                    if (location == null)
                    {
                        res.Message = "Lokasi tidak sesuai!";
                        return res;
                    }


                    var stockOpname = new StockOpname()
                    {
                        LocationId = request.LocationId,
                        FinalLocationName = location.Name,
                        UserId = request.UserId
                    };
                    await _context.StockOpnames.AddAsync(stockOpname);
                    await _context.SaveChangesAsync(cancellationToken);

                    var items = await _context.Items.Include(x => x.Location)
                        .Include(x => x.ItemPrintLogs).Where(x => ids.Contains(x.Id)).ToListAsync();

                    foreach (var item in items)
                    {
                        var tagId = Helper.GetItemTagId(item);

                        var stockOpnameDetail = new StockOpnameDetail()
                        {
                            StockOpnameId = stockOpname.Id,
                            ItemId = item.Id,
                            TagId = tagId,
                            Note = misplacedIds.Contains(item.Id) ? "Misplaced" : "Scanned"
                        };
                        await _context.StockOpnameDetails.AddAsync(stockOpnameDetail);

                        var itemLocation = new TagLocation()
                        {
                            ItemId = item.Id,
                            Epc = tagId,
                            PrevLocationId = item.LocationId,
                            LocationId = location.Id,
                            FinalLocation = location.Name,
                            Note = userName,
                            Source = "so",
                            StockOpnameId = stockOpname.Id
                        };
                        await _context.TagLocations.AddAsync(itemLocation);
                        await _context.SaveChangesAsync(cancellationToken);
                        var prevLocationId = item.LocationId;
                        if (prevLocationId != location.Id)
                        {
                            var itemMovement = new ItemMovement()
                            {
                                ItemId = item.Id,
                                PrevLocationId = prevLocationId,
                                LocationId = location.Id,
                                PrevLocationName = item.Location != null ? item.Location.Name : null,
                                LocationName = location.Name,
                                Source = ItemMovement.SOURCE_STOCK_OPNAME,
                                TagLocationId = itemLocation.Id
                            };
                            await _context.ItemMovements.AddAsync(itemMovement);
                        }

                        item.LocationId = location.Id;
                        if (item.ItemPrintLogs.Count == 0)
                        {
                            //if item print is not recorded yet, then add it automatically
                            await _context.ItemPrintLogs.AddAsync(new ItemPrintLog() { ItemId = item.Id });
                        }
                        ids.Remove(item.Id);
                        res.OkTagIds.Add(tagId);
                    }

                    items = await _context.Items.Where(x => notScannedIds.Contains(x.Id)).ToListAsync();
                    foreach (var item in items)
                    {
                        var tagId = Helper.GetItemTagId(item);
                        var stockOpnameDetail = new StockOpnameDetail()
                        {
                            StockOpnameId = stockOpname.Id,
                            ItemId = item.Id,
                            TagId = tagId,
                            Note = "Not Scanned"
                        };
                        await _context.StockOpnameDetails.AddAsync(stockOpnameDetail);

                        res.OkTagIds.Add(tagId);
                    }


                    foreach (var id in ids)
                    {
                        var dummyItem = new Item() { Id = id };

                        var tagId = Helper.GetItemTagId(dummyItem);
                        var stockOpnameDetail = new StockOpnameDetail()
                        {
                            StockOpnameId = stockOpname.Id,
                            ItemId = null,
                            TagId = tagId,
                            Note = "Invalid Tag"
                        };
                        res.NokTagIds.Add(tagId, "Tag tidak ditemukan");
                        await _context.StockOpnameDetails.AddAsync(stockOpnameDetail);
                    }

                    await _context.SaveChangesAsync(cancellationToken);

                    await trans.CommitAsync();
                    res.Result = BaseResponse.RESULT_OK;
                    res.Message = "Berhasil upload stock opname!";
                }
                catch (Exception ex)
                {
                    await trans.RollbackAsync();
                    res.Message = "Exception : " + ex.Message;
                }
            }

            return res;
        }
    }
}
