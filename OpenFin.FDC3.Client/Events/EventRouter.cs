using OpenFin.FDC3.Utils;
using System;
using System.Collections.Generic;

namespace OpenFin.FDC3.Events
{
    public class EventRouter
    {
        private readonly Dictionary<string, Action<string>> emitterProviders;
        private readonly Dictionary<FDC3EventType, Func<EventTransport<FDC3Event>, FDC3Event>> deserializers;

        private EventRouter()
        {
            emitterProviders = new Dictionary<string, Action<string>>();          
            
        }

        public static EventRouter Instance = new EventRouter();

        public void DispatchEvent(EventTransport<FDC3Event> @event)
        {            
        }
}