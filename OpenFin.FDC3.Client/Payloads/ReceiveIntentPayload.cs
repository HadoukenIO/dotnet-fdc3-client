using Newtonsoft.Json;

namespace OpenFin.FDC3.Payloads
{
    public class ReceiveIntentPayload : PayloadBase
    {
        [JsonProperty("intent")]
        public string Intent { get; set; }
    }
}