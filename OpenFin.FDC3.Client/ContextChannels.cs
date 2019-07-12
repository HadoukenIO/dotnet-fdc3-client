using OpenFin.FDC3.Channels;
using OpenFin.FDC3.Constants;
using OpenFin.FDC3.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenFin.FDC3
{
    public sealed class ContextChannels
    {
        private static readonly ContextChannels _instance = new ContextChannels();
        private static Dictionary<string, ChannelBase> channelLookup = new Dictionary<string, ChannelBase>();

        internal static ContextChannels Instance => _instance;

        private ContextChannels()
        {
            channelLookup = new Dictionary<string, ChannelBase>();
            channelLookup[ChannelConstants.DefaultChannelId] = ChannelUtils.DefaultChannel;
        }

        public async static Task<IEnumerable<DesktopChannel>> GetDesktopChannelsAsync()
        {
            var channels = await Connection.GetDesktopChannelsAsync();
            return channels;
        }

        public async static Task<ChannelBase> GetChannelByIdAsync(string channelId)
        {
            var transport = await Connection.GetChannelByIdAsync(channelId);

            if (channelLookup.Any(x => x.Key == channelId))
                return channelLookup[channelId];

            channelLookup[channelId] = ChannelUtils.GetChannelObject(transport);

            return channelLookup[channelId];
        }

        public async static Task<ChannelBase> GetCurrentChannelAsync(Identity identity)
        {
            var channel = await Connection.GetCurrentChannelAsync(identity);
            var channelId = channel.ChannelId;

            if (channelLookup.Any(x => x.Key == channelId))
                return channelLookup[channelId];

            channelLookup[channelId] = channel;

            return channel;
        }
    }
}