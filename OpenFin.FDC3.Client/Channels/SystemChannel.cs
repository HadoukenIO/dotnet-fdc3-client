namespace OpenFin.FDC3.Channels
{
    public class SystemChannel : ChannelBase
    {
        public DisplayMetadata VisualIdentity { get; }

        public SystemChannel(SystemChannelTransport transport, Connection connection) : base(transport.ChannelId, Channels.ChannelType.System, connection)
        {
            this.VisualIdentity = transport.VisualIdentity;
        }
    }
}