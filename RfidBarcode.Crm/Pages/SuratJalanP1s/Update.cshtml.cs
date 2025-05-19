using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RfidBarcode.Application.Common.BaseObjects;
using RfidBarcode.Application.Common.Interfaces;
using RfidBarcode.Application.Operationals.Requests;
using RfidBarcode.Application.Operationals.ViewModels;
using RfidBarcode.Application.Settings.Requests;
using RfidBarcode.Application.Settings.ViewModels;
using RfidBarcode.Domain.Entities;

namespace RfidBarcode.Crm.Pages.SuratJalanP1s
{
    public class UpdateModel : PageModel
    {
        public SuratJalanP1VM ViewModel { get; set; }

        [BindProperty]
        public long SuratJalanP1Id { get; set; }

        [BindProperty]
        public List<long> ItemIds { get; set; }

        public List<List<string>> ColItems { get; set; } = new List<List<string>>();

        public List<ItemVM> Items { get; set; } = new List<ItemVM>();

        public List<decimal> ColYards { get; set; }

        public List<string> ColKp { get; set; }

        public List<string> SuratJalanTypes { get; set; } = new List<string>()
        {
            SuratJalanP1.TYPE_P1,
            SuratJalanP1.TYPE_K4
        };

        public int TotalRoll { get; set; }
        public decimal TotalYard { get; set; }

        private readonly IMediator _mediator;

        public string Username { get; set; }

        public UpdateModel(IMediator mediator, IUserResolverService user)
        {
            _mediator = mediator;
            ViewModel = new SuratJalanP1VM();
            ItemIds = new List<long>();
            Username = user.GetUser();
        }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            var cmd = new GetSuratJalanP1Request(new SuratJalanP1VM() { Id = id });
            var res = await _mediator.Send(cmd);
            if (res.Result == BaseResponse.RESULT_OK && res.Data != null)
            {
                ViewModel = res.Data;
            }

            var request = new GetAllItemRequest() { Data = new ItemVM() { SuratJalanP1Id = id } };
            var response = await _mediator.Send(request);

            if (response.Data != null)
            {
                Items = response.Data;
                ColKp = new List<string>();
                var kps = Items
                    .GroupBy(x => x.Kp)
                    .Select(g => new
                    {
                        Kp = g.Key,
                        Count = g.Count()
                    }
                    ).OrderByDescending(x => x.Count);
                ColYards = new List<decimal>();

                foreach (var kp in kps)
                {
                    var items = Items.Where(x => x.Kp == kp.Kp).ToList();

                    var numCols = Math.Ceiling((decimal)items.Count / 5);

                    for (int i=0; i < numCols; i++)
                    {
                        decimal colYard = 0;
                        var colItem = new List<string>();
                        var offset = i * 5;
                        var endOffset = Math.Min(offset + 5, items.Count());
                        while (offset < endOffset)
                        {
                            var dt = items[offset];
                            colItem.Add(dt.Yard + " / " + dt.ConvertedLebar + " / " + dt.ConvertedK);
                            colYard += dt.Yard ?? 0;
                            offset++;
                        }
                        //force data to 5 rows
                        endOffset = i * 5 + 5;
                        for (int j = offset; j < endOffset; j++)
                        {
                            colItem.Add("");
                        }
                        ColYards.Add(colYard);
                        ColItems.Add(colItem);
                        ColKp.Add(kp.Kp);
                    }
                }

                //force to 5 columsn
                for (int i=ColItems.Count; i < 5; i++)
                {
                    var colItem = new List<string>() { "", "", "", "", "" };
                    ColItems.Add(colItem);
                    ColYards.Add(0);
                }

                TotalYard = 0;
                for (int i=0; i < 5; i++)
                {
                    TotalYard += ColYards[i];
                }
                TotalRoll = response.Data.Count;

            }
            

            return Page();
        }

        public async Task<IActionResult> OnPostRefreshDataAsync()
        {
            var response = new BaseDataTableResponse<ItemVM>();
            try
            {
                var request = new GetAllItemRequest() { Data = new ItemVM() { SuratJalanP1Id = SuratJalanP1Id } };
                request.InitFromDataTable(Request.Form);

                response = await _mediator.Send(request);
            }
            catch (Exception)
            {

            }

            return new OkObjectResult(response);
        }


        public async Task<IActionResult> OnPostRefreshDataModalAsync()
        {
            var response = new BaseDataTableResponse<ItemVM>();
            try
            {
                var request = new GetAllItemRequest() { ExcludedSuratJalanP1Id = SuratJalanP1Id };
                request.InitFromDataTable(Request.Form);

                response = await _mediator.Send(request);
            }
            catch (Exception)
            {

            }

            return new OkObjectResult(response);
        }

        public async Task<IActionResult> OnPostAddItemsAsync()
        {
            var cmd = new AddItemsForP1Request(SuratJalanP1Id, ItemIds);
            var response = await _mediator.Send(cmd);

            return new OkObjectResult(response);
        }

        public async Task<IActionResult> OnPostDeleteAsync(long id)
        {
            var list = new List<long>();
            list.Add(id);
            var cmd = new RemoveItemsForP1Request(list);
            var response = await _mediator.Send(cmd);

            return new OkObjectResult(response);
        }

        public async Task<IActionResult> OnPostFinalizeAsync(string type, string no)
        {
            var list = new List<long>();
            var cmd = new FinalizeP1Request(SuratJalanP1Id, type, no);
            var response = await _mediator.Send(cmd);

            return new OkObjectResult(response);
        }
    }
}
