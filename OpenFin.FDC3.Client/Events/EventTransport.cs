using OpenFin.FDC3.Channels;
using OpenFin.FDC3.Utils; 

namespace OpenFin.FDC3.Events
{
    public class EventTransport<T> where T : FDC3Event
    {
        public string Type { get; set; }
        public Identity Identity { get; set; }
        public ChannelTransport Channel { get; set; }
        public ChannelTransport PreviousChannel { get; set; }
        public EventTransportTarget Target { get; set; }
        public T ToEvent<T>() where T : FDC3Event
        {
            return new FDC3Event
            {
                Identity = Identity,
                Channel = ChannelUtils.GetChannelObject(this.Channel),
                PreviousChannel = ChannelUtils.GetChannelObject(PreviousChannel)
            } as T;
        }
    }

    public class EventTransportTarget
    {
        public string Type { get; set; }
        public string ChannelId { get; set; }
    }

    
}