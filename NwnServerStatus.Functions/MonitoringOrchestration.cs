using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace NwnServerStatus.Functions
{
    public static class MonitoringOrchestration
    {
        [FunctionName("MonitoringOrchestration")]
        public static async Task<ServerStatus> RunMonitoringCycle(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var input = context.GetInput<ServerStatus>();
            var status = await context.CallActivityAsync<ServerStatus>("MonitoringOrchestration_GetStatus", $"{input.Host}:{input.Port}");

            if (IsStatusChanged(status, input))
            {
                await context.CreateTimer(context.CurrentUtcDateTime.AddSeconds(10), CancellationToken.None);
                context.ContinueAsNew(status);
            }

            return status;
        }

        [FunctionName("MonitoringOrchestration_GetStatus")]
        public static async Task<ServerStatus> GetStatus(
            [ActivityTrigger] string instanceId,
            [DurableClient] IDurableOrchestrationClient client)
        {
            var pollingOrchestration = await client.GetStatusAsync(instanceId);
            return JsonSerializer.Deserialize<ServerStatus>(pollingOrchestration.CustomStatus.ToString());
        }

        [FunctionName("MonitoringOrchestration_Start")]
        public static async Task<HttpResponseMessage> StartStatusMonitoring(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "monitor/{host}/{port:int?}")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            string host, int port,
            ILogger log)
        {
            var existingPollingInstanceId = $"{host}:{port}";
            var existingPollingOrchestration = await starter.GetStatusAsync(existingPollingInstanceId);
            if (existingPollingOrchestration == null)
            {
                await starter.StartNewAsync("PollingOrchestration", existingPollingInstanceId, new { host, port });
                log.LogInformation($"Started polling orchestration with ID = '{existingPollingInstanceId}'.");
            }

            string instanceId = await starter.StartNewAsync("MonitoringOrchestration", null, new { host, port });
            log.LogInformation($"Started monitoring orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }

        private static bool IsStatusChanged(ServerStatus current, ServerStatus previous) =>
            previous.Players != default && current.Players != previous.Players;
    }
}