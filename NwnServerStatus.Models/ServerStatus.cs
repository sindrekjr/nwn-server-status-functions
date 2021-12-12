namespace NwnServerStatus.Models
{
    public class ServerStatus
    {
        public string ServerId { get; set; }
        public string ServerName { get; set; }
        public bool Passworded { get; set; }
        public int Players { get; set; }
        public int Uptime { get; set; }
        public int LastSeen { get; set; }
        public int Latency { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }
}