using Newtonsoft.Json;

namespace OpenFin.FDC3.Channels
{
    /// <summary>
    ///  Defines the suggested visual appearance of a system channel when presented in an app, for example, as part of a channel selector.
    /// </summary>
    public class DisplayMetadata
    {
        /// <summary>
        /// A user-readable name for this channel, e.g: "Red"
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The color that should be associated within this channel when displaying this channel in a UI, e.g: `#FF0000`.
        /// </summary>
        [JsonProperty("color")]
        public string Color { get; set; }

        /// <summary>
        /// A URL of an image that can be used to display this channel.
        /// </summary>
        [JsonProperty("glyph")]
        public string Glyph { get; set; }
    }
}
