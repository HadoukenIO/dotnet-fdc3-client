﻿using OpenFin.FDC3.Channels;
using OpenFin.FDC3.Constants;

namespace OpenFin.FDC3.Utils
{
    internal abstract class ChannelUtils
    {
        private static DefaultChannel defaultChannel;        

        internal static ChannelBase GetChannelObject(ChannelTransport channelTransport, Connection connection)
        {
            if (channelTransport == null)
            {
                if(defaultChannel == null)
                {
                    defaultChannel = new DefaultChannel(connection);
                }

                return defaultChannel;
            }
                

            ChannelBase channel;

            switch (channelTransport.ChannelType)
            {
                case ChannelType.Default:
                    if(defaultChannel == null)
                    {
                        defaultChannel = new DefaultChannel(connection);
                    }
                    channel = defaultChannel;
                    break;

                case ChannelType.System:
                    channel = new SystemChannel(channelTransport as SystemChannelTransport, connection);
                    break;

                default:
                    channel = null;
                    break;
            }

            return channel;
        }
    }
}