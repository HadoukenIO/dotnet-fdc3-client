using Newtonsoft.Json;
using OpenFin.FDC3.Context;

namespace OpenFin.FDC3.Payloads
{
    public class HandleChannelContextPayload
    {
        [JsonProperty("channel")]
        public string ChannelId { get; set; }
        public ContextBase Context { get; set; }
    }
}