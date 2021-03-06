﻿using System;
using Newtonsoft.Json;
using OpenFin.FDC3.Channels;
using OpenFin.FDC3.Utils;

namespace OpenFin.FDC3.Events
{
    public class EventTransport<T> where T : FDC3Event
    {
        public FDC3EventType Type { get; set; }
        public Identity Identity { get; set; }
        public ChannelTransport Channel { get; set; }
        public ChannelTransport PreviousChannel { get; set; }
        [JsonConverter(typeof(EventTargetConverter))]
        public EventTransportTarget Target { get; set; }
        public FDC3Event ToEvent(Connection connection)
        {
            var channel = ChannelUtils.GetChannelObject(this.Channel, connection);
            var previousChannel = ChannelUtils.GetChannelObject(PreviousChannel, connection);

            switch (Type)
            {
                case FDC3EventType.ChannelChanged:
                    return new ChannelChangedEvent(Identity,channel, previousChannel) ;
                case FDC3EventType.WindowAdded:
                    return new ChannelWindowAddedEvent(Identity, channel, previousChannel);
                case FDC3EventType.WindowRemoved:
                    return new ChannelWindowRemovedEvent(Identity, channel, previousChannel);
                default:
                    throw new System.ArgumentException("unrecognized event type.");
            }            
        }
    }    
}