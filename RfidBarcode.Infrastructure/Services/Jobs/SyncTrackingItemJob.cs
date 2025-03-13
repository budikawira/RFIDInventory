using MediatR;
using Quartz;
using RfidBarcode.Application.Operationals.Requests;

namespace RfidBarcode.Infrastructure.Services.Jobs
{
    public class SyncTrackingItemJob : IJob
    {
        private readonly IMediator _mediator;

        public SyncTrackingItemJob(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var cmd = new SyncTrackingItemRequest(100);
                var res = await _mediator.Send(cmd);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception SysTrackingItemJob : " + ex.Message);
            }
        }
    }
}
