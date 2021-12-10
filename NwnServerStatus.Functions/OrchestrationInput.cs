namespace NwnServerStatus.Functions
{
    public class OrchestrationInput : StartPollingRequestBody
    {
        public NwMasterApiResponse PreviousResponse { get; set; }
    }
}