using OpenFin.FDC3.Channels;
using System;

namespace OpenFin.FDC3.Events
{
    public class ChannelWindowRemovedEvent : FDC3Event
    {
        public FDC3EventType Type => FDC3EventType.WindowRemoved;
        public Identity Identity { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ChannelBase Channel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ChannelBase PreviousChannel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}