namespace OpenFin.FDC3.Channels
{
    public class DesktopChannel : ChannelBase
    {
        public string Name { get; }
        public int Color { get; }

        public DesktopChannel(DesktopChannelTransport transport, Connection connection) : base(transport.ChannelId, Channels.ChannelType.Desktop, connection)
        {
            this.Name = transport.Name;
            this.Color = transport.Color;
        }
    }
}