using Newtonsoft.Json;

namespace OpenFin.FDC3.Channels
{
    public class ChannelTransport
    {
        [JsonProperty("type")]
        public virtual TransportType TransportType { get; }

        [JsonProperty("id")]
        public string ChannelId { get; set; }
    }
}