using OpenFin.FDC3.Constants;

namespace OpenFin.FDC3.Channels
{
    public class DesktopChannelTransport : ChannelTransport
    {
        public override string TransportType => ChannelTransportTypes.Desktop;
        public string Name { get; set; }
        public int Color { get; set; }
    }
}