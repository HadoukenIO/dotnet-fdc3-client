using Newtonsoft.Json;

namespace OpenFin.FDC3.Channels
{
    public class SystemChannelTransport : ChannelTransport
    {
        public override ChannelType ChannelType => ChannelType.System;
        [JsonProperty("visualIdentity")]
        public DisplayMetadata VisualIdentity { get; set; }
    }
}