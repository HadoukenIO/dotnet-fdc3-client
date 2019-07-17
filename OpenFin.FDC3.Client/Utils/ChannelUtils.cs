using OpenFin.FDC3.Channels;
using OpenFin.FDC3.Constants;

namespace OpenFin.FDC3.Utils
{
    internal abstract class ChannelUtils
    {
        private static DefaultChannel defaultChannel = new DefaultChannel();
        public static DefaultChannel DefaultChannel { get { return defaultChannel; } }

        internal static ChannelBase GetChannelObject(ChannelTransport channelTransport)
        {
            if (channelTransport == null)
                return defaultChannel;

            ChannelBase channel;

            switch (channelTransport.TransportType)
            {
                case TransportType.Default:
                    channel = defaultChannel;
                    break;

                case TransportType.Desktop:
                    channel = new DesktopChannel(channelTransport as DesktopChannelTransport);
                    break;

                default:
                    channel = null;
                    break;
            }

            return channel;
        }
    }
}