using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace OpenFin.FDC3.Events
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum FDC3EventType
    {
        [EnumMember(Value = "channel-changed")]
        ChannelChanged,

        [EnumMember(Value = "window-added")]
        WindowAdded,

        [EnumMember(Value = "window-removed")]
        WindowRemoved
    }
}