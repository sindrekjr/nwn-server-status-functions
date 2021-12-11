namespace NwnServerStatus.Services
{
    public class NwMasterService
    {
        private readonly string baseUri = "https://api.nwn.beamdog.net/v1/servers";
        private readonly HttpClient client;

        public NwMasterService(IHttpClientFactory httpClientFactory)
        {
            this.client = httpClientFactory.CreateClient();
        }

        public async Task<NwMasterApiResponse> GetStatus(string ip, int port)
        {
            var response = await client.GetAsync(new Uri($"{baseUri}/{ip}/{port}"));
            return JsonSerializer.Deserialize<NwMasterApiResponse>(await response.Content.ReadAsStringAsync());
        }
    }
}