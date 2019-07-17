using OpenFin.FDC3.Channels;

namespace OpenFin.FDC3.Events
{
    public class ChannelChangedEvent : FDC3Event
    {
        public override FDC3EventType Type => FDC3EventType.ChannelChanged;

        public ChannelChangedEvent(Identity identity, ChannelBase channel, ChannelBase previousChannel) : base(identity, channel, previousChannel)
        {            
        }
    }
}