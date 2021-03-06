﻿using Newtonsoft.Json;

namespace OpenFin.FDC3.Events
{
    public class EventTransportTarget
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}