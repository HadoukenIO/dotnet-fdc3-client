using Newtonsoft.Json;
using OpenFin.FDC3.Context;

namespace OpenFin.FDC3.Payloads
{
    public class ReceiveContextPayload
    {
        [JsonProperty("context")]
        public ContextBase Context { get; set; }
    }
}