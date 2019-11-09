using Newtonsoft.Json;

namespace OpenFin.FDC3.Channels
{
    /// <summary>
    /// 
    /// </summary>
    public class ChannelTransport
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("type")]
        public virtual ChannelType ChannelType { get; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("id")]
        public string ChannelId { get; set; }
    }
}