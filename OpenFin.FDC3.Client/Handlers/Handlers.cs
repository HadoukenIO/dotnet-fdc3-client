using OpenFin.FDC3.Channels;
using OpenFin.FDC3.Context;
using OpenFin.FDC3.Events;
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
        internal static Dictionary<string, Action<ContextBase>> IntentHandlers = new Dictionary<string, Action<ContextBase>>();
        internal static Dictionary<string, Dictionary<FDC3EventType, Action<FDC3Event>>> FDC3ChannelEventHandlers = new Dictionary<string, Dictionary<FDC3EventType, Action<FDC3Event>>>();

        internal static bool HasIntentHandler(string intent)
        {
            return IntentHandlers.Any(x => x.Key == intent);
        }

        internal static bool HasContextListener(string channelId)
        {
            return ChannelContextHandlers.Any(x => x.Channel.ChannelId == channelId);
        }

        internal static bool HasEventListener(string channelId)
        {
            return FDC3ChannelEventHandlers.Any(x => x.Key == channelId);
        }
    }
}