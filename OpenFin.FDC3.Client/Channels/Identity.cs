using Newtonsoft.Json;

namespace OpenFin.FDC3.Channels
{
    public class Identity
    {
        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}