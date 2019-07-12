using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace OpenFin.FDC3.Events
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum FDC3EventType
    {
        [JsonProperty("channel-changed")]
        ChannelChanged,

        [JsonProperty("window-added")]
        WindowAdded,

        [JsonProperty("window-removed")]
        WindowRemoved
    }
}