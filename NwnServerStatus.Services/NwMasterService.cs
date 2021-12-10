namespace NwnServerStatus.Services
{
    public class NwMasterService
    {
        private readonly HttpClient client;

        public NwMasterService(IHttpClientFactory httpClientFactory)
        {
            this.client = httpClientFactory.CreateClient();
        }
    }
}