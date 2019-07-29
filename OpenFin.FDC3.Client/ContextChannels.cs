using OpenFin.FDC3.Channels;
using OpenFin.FDC3.Constants;
using OpenFin.FDC3.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenFin.FDC3
{
    public class ContextChannels
    {        
        //private Dictionary<string, ChannelBase> channelLookup = new Dictionary<string, ChannelBase>();

        //private Connection connection;
        //public ContextChannels(Connection connection)
        //{
        //    this.connection = connection;
        //    channelLookup = new Dictionary<string, ChannelBase>();
        //    channelLookup[ChannelConstants.DefaultChannelId] = ChannelUtils.DefaultChannel;
        //}

        //public async Task<IEnumerable<DesktopChannel>> GetDesktopChannelsAsync()
        //{
        //    var channels = await connection.GetDesktopChannelsAsync();
        //    return channels;
        //}

        //public async Task<ChannelBase> GetChannelByIdAsync(string channelId)
        //{
        //    var transport = await connection.GetChannelByIdAsync(channelId);

        //    if (channelLookup.Any(x => x.Key == channelId))
        //        return channelLookup[channelId];

        //    channelLookup[channelId] = ChannelUtils.GetChannelObject(transport, this);

        //    return channelLookup[channelId];
        //}

        //public async Task<ChannelBase> GetCurrentChannelAsync(Identity identity)
        //{
        //    var channel = await connection.GetCurrentChannelAsync(identity);
        //    var channelId = channel.ChannelId;

        //    if (channelLookup.Any(x => x.Key == channelId))
        //        return channelLookup[channelId];

        //    channelLookup[channelId] = channel;

        //    return channel;
        //}
    }
}