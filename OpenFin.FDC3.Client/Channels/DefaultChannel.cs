using OpenFin.FDC3.Constants;

namespace OpenFin.FDC3.Channels
{
    public class DefaultChannel : ChannelBase
    {
        public DefaultChannel(Connection connection) : base(ChannelConstants.DefaultChannelId, ChannelType.Default, connection)
        {
        }
    }
}