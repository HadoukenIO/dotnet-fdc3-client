using OpenFin.FDC3.Channels;
using OpenFin.FDC3.Context;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenFin.FDC3.Handlers
{
    internal sealed class FDC3Handlers
    {
        internal static Action<ChannelChangedPayload> ChannelChangedHandlers;
        internal static Action<ContextBase> ContextHandlers;
        internal static List<ChannelContextListener> ChannelContextHandlers = new List<ChannelContextListener>();
        internal static List<IntentHandler> IntentHandlers = new List<IntentHandler>();

        internal static bool HasIntentHandler(string intent)
        {
            return IntentHandlers.Any(x => x.Intent == intent);
        }
    }
}