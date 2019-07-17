using Newtonsoft.Json;

namespace OpenFin.FDC3.Events
{
    public class EventTransportTarget
    {
        public string Type { get; set; }

        [JsonProperty("id")]
        public string ChannelId { get; set; }
    }

    
}