using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace NwnServerStatus.Functions
{
    public class PollingOrchestration
    {
        private readonly NwMasterService nwMasterService;

        public PollingOrchestration(NwMasterService nwMasterService)
        {
            this.nwMasterService = nwMasterService;
        }

        [FunctionName("PollingOrchestration")]
        public async Task RunPollingCycle([OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var input = context.GetInput<PollingOrchestrationInput>();
            var status = await context.CallActivityAsync<ServerStatus>("PollingOrchestration_Poll", (input.Host, input.Port));
            context.SetCustomStatus(status);

            await context.CreateTimer(context.CurrentUtcDateTime.AddSeconds(5), CancellationToken.None);
            context.ContinueAsNew(input);
        }

        [FunctionName("PollingOrchestration_Poll")]
        public async Task<ServerStatus> Poll([ActivityTrigger] (string ip, int port) input, ILogger log) =>
            await nwMasterService.GetServerStatus(input.ip, input.port);

        [FunctionName("PollingOrchestration_Start")]
        public async Task<HttpResponseMessage> StartPolling(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "poll/{host}/{port:int?}")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            string host, int port,
            ILogger log)
        {
            var instanceId = $"{host}:{port}";
            if (await starter.GetStatusAsync(instanceId) == null)
            {
                await starter.StartNewAsync("PollingOrchestration", instanceId, new { host, port });
                log.LogInformation($"Started orchestration with ID = '{instanceId}'.");
            }

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }

    internal class PollingOrchestrationInput
    {
        public string Host { get; set; }
        public int Port { get; set; }
    }
}