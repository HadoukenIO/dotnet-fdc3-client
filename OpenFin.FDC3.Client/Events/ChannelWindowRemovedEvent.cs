using OpenFin.FDC3.Channels;
using System;

namespace OpenFin.FDC3.Events
{
    public class ChannelWindowRemovedEvent : FDC3Event
    {
        public ChannelWindowRemovedEvent(Identity identity, ChannelBase channel, ChannelBase previousChannel) : base(identity, channel, previousChannel)
        {
        }

        public override FDC3EventType Type => FDC3EventType.WindowRemoved;       
    }
}