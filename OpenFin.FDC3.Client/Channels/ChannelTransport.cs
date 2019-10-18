using Newtonsoft.Json;

namespace OpenFin.FDC3.Channels
{
    public class ChannelTransport
    {
        [JsonProperty("type")]
        public virtual ChannelType ChannelType { get; }

        [JsonProperty("id")]
        public string ChannelId { get; set; }
    }
}