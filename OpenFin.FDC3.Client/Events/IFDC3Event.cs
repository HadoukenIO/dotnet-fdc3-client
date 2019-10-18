using OpenFin.FDC3.Channels;

namespace OpenFin.FDC3.Events
{
    public class FDC3Event
    {
        public virtual FDC3EventType Type { get; }

        /// <summary>
        /// The window that has changed context
        /// </summary>
        public Identity Identity { get; set; }

        /// <summary>
        /// The current channel the window belongs to
        /// </summary>
        public ChannelBase Channel { get; set; }

        /// <summary>
        /// The previous channel the window belongs to
        /// </summary>
        public ChannelBase PreviousChannel { get; set; }

        public FDC3Event(Identity identity, ChannelBase channel, ChannelBase previousChannel)
        {
            Identity        = identity;
            Channel         = channel;
            PreviousChannel = previousChannel;
        }
    }
}