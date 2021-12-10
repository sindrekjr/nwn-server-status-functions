using System.IO;
using System.Text.Json;

namespace NwnServerStatus.Functions
{
    public class StartPollingRequestBody
    {
        public string CallbackAddress { get; set; }
        public bool Forever { get; set; } = false;
    }

    public static class StartPollingRequestBodyMapper
    {
        private static readonly JsonSerializerOptions serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        public static StartPollingRequestBody ToStartPollingRequestBody(this Stream stream) =>
            JsonSerializer.Deserialize<StartPollingRequestBody>(new StreamReader(stream).ReadToEnd(), serializerOptions);
    }
}