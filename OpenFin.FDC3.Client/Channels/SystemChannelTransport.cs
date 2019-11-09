using Newtonsoft.Json;

namespace OpenFin.FDC3.Channels
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemChannelTransport : ChannelTransport
    {
        /// <summary>
        /// The channel type.
        /// </summary>
        public override ChannelType ChannelType => ChannelType.System;
        
        /// <summary>
        /// The metadata to be used to display this channel.
        /// </summary>
        [JsonProperty("visualIdentity")]        
        public DisplayMetadata VisualIdentity { get; set; }
    }
}