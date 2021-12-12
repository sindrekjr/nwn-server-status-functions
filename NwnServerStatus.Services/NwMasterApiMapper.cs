namespace NwnServerStatus.Services
{
    public static class NwMasterApiMapper
    {
        public static ServerStatus MapToServerStatus(this NwMasterApiResponse nwMasterApiResponse) =>
            new ServerStatus
            {
                ServerId = nwMasterApiResponse.KxPk,
                ServerName = nwMasterApiResponse.SessionName,
                Passworded = nwMasterApiResponse.Passworded,
                Players = nwMasterApiResponse.CurrentPlayers,
                Uptime = Math.Abs(nwMasterApiResponse.FirstSeen - nwMasterApiResponse.LastAdvertisement) * 1000,
                LastSeen = nwMasterApiResponse.LastAdvertisement,
                Latency = nwMasterApiResponse.Latency,
                Host = nwMasterApiResponse.Host,
                Port = nwMasterApiResponse.Port,
            };
    }
}