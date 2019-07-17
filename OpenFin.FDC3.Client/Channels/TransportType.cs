using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OpenFin.FDC3.Channels
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TransportType
    {
        [EnumMember(Value = "default")]
        Default,
        [EnumMember(Value = "desktop")]
        Desktop
    }
}
