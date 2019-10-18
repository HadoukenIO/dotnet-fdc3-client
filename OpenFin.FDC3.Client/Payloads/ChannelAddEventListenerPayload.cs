using Newtonsoft.Json;
using OpenFin.FDC3.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFin.FDC3.Payloads
{
    public class ChannelAddEventListenerPayload
    {
        [JsonProperty("id")]
        public string ChannelId { get; set; }
        public FDC3EventType EventType { get; set; }
    }
}
