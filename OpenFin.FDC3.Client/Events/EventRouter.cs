using OpenFin.FDC3.Handlers;
using OpenFin.FDC3.Utils;
using System;
using System.Collections.Generic;

namespace OpenFin.FDC3.Events
{
    public class EventRouter
    {
        private EventRouter()
        {           

        }

        public static EventRouter Instance = new EventRouter();
        

        public void DispatchEvent<T>(EventTransport<T> eventTransport) where T : FDC3Event
        {
            var @event = eventTransport.ToEvent();

            //if(FDC3Handlers.FDC3ChannelEventHandlers.ContainsKey(eventTransport.Target.Type) && FDC3Handlers.FDC3ChannelEventHandlers[eventTransport.Target.Type].ContainsKey(@event.))
            //var handler = FDC3Handlers.FDC3ChannelEventHandlers[eventTransport.Target.Type][@event.Type];
            //handler?.Invoke(@event);
        }
    }
}