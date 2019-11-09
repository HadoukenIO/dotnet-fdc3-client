namespace OpenFin.FDC3.Channels
{
    /// <summary>
    /// User-facing channel to be used within a channel selector component
    /// </summary>
    public class SystemChannel : ChannelBase
    {
        /// <summary>
        /// The visual metadata for this channel.
        /// </summary>
        public DisplayMetadata VisualIdentity { get; }

        /// <summary>
        /// Creates a SystemChannel object
        /// </summary>
        /// <param name="transport"></param>
        /// <param name="connection"></param>
        public SystemChannel(SystemChannelTransport transport, Connection connection) : base(transport.ChannelId, Channels.ChannelType.System, connection)
        {
            this.VisualIdentity = transport.VisualIdentity;
        }
    }
}