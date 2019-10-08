using Newtonsoft.Json;

namespace OpenFin.FDC3.Channels
{
    public class DisplayMetadata
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("color")]
        public string Color { get; set; }
        [JsonProperty("glyph")]
        public string Glyph { get; set; }
    }
}
