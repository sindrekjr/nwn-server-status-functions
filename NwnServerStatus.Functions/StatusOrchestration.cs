using System.Threading;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace NwnServerStatus.Functions
{
    public class StatusOrchestration
    {
        private readonly NwMasterService nwMasterService;

        public StatusOrchestration(NwMasterService nwMasterService)
        {
            this.nwMasterService = nwMasterService;
        }

        [FunctionName("StatusOrchestration")]
        public async Task<NwMasterApiResponse> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var input = context.GetInput<NwMasterApiResponse>();
            var status = await context.CallActivityAsync<NwMasterApiResponse>("StatusOrchestration_Poll", input);

            await context.CreateTimer(context.CurrentUtcDateTime.AddSeconds(5), CancellationToken.None);
            context.ContinueAsNew(status);

            return status;
        }

        [FunctionName("StatusOrchestration_Poll")]
        public async Task<NwMasterApiResponse> SayHello([ActivityTrigger] NwMasterApiResponse status, ILogger log) =>
            await nwMasterService.GetStatus(status.Host, status.Port);

        [FunctionName("StatusOrchestration_StartPolling")]
        public async Task<HttpResponseMessage> StartPolling(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "poll/{ip}/{port:int?}")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            string ip, int? port,
            ILogger log)
        {
            var status = await nwMasterService.GetStatus(ip, port ?? 5123);

            // var pollingParameters = await req.Content.ReadAsAsync<StartPollingRequestBody>();
            // var pollingParams = req.Content.ReadAsStream().ToStartPollingRequestBody();

            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("StatusOrchestration", status.KxPk, status);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}