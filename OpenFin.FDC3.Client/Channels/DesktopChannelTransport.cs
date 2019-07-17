using OpenFin.FDC3.Constants;

namespace OpenFin.FDC3.Channels
{
    public class DesktopChannelTransport : ChannelTransport
    {
        public override TransportType TransportType => TransportType.Desktop;
        public string Name { get; set; }
        public int Color { get; set; }
    }
}