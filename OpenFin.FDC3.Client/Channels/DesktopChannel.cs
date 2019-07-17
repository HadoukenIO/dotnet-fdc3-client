namespace OpenFin.FDC3.Channels
{
    public class DesktopChannel : ChannelBase
    {
        public string Name { get; }
        public int Color { get; }

        public DesktopChannel(DesktopChannelTransport transport) : base(transport.ChannelId, Channels.ChannelType.Desktop)
        {
            this.Name = transport.Name;
            this.Color = transport.Color;
        }
    }
}