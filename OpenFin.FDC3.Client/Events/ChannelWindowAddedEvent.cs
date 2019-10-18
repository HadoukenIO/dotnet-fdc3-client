using OpenFin.FDC3.Channels;

namespace OpenFin.FDC3.Events
{
    public class ChannelWindowAddedEvent : FDC3Event
    {
        public ChannelWindowAddedEvent(Identity identity, ChannelBase channel, ChannelBase previousChannel) : base(identity, channel, previousChannel)
        {
        }

        public override FDC3EventType Type => FDC3EventType.WindowAdded;
    }
}