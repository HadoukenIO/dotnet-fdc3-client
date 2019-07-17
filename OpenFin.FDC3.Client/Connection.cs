using Newtonsoft.Json.Linq;
using Openfin.Desktop.Messaging;
using OpenFin.FDC3.Channels;
using OpenFin.FDC3.Constants;
using OpenFin.FDC3.Context;
using OpenFin.FDC3.Events;
using OpenFin.FDC3.Handlers;
using OpenFin.FDC3.Intents;
using OpenFin.FDC3.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenFin.FDC3
{
    internal static partial class Connection
    {
        internal static Action<Exception> ConnectionInitializationComplete;

        //private static Action<ChannelChangedPayload> channelChangedHandlers;
        //private static Action<ContextBase> contextListeners;
        private static ChannelClient channelClient;
        

        internal static Task<List<Identity>> GetChannelMembersAsync(string channelId)
        {
            return channelClient.DispatchAsync<List<Identity>>(ApiFromClientTopic.ChannelGetMembers, new { id = channelId });
        }

        internal static void AddChannelChangedEventListener(Action<ChannelChangedPayload> handler)
        {
            FDC3Handlers.ChannelChangedHandlers += handler;
        }

        internal static Task AddChannelContextListenerAsync(string channelId)
        {   
            return channelClient.DispatchAsync<Task>(ApiFromClientTopic.ChannelAddContextListener, new { id = channelId });           
        }               

        internal static Task BroadcastAsync(Context.ContextBase context)
        {
            return channelClient.DispatchAsync<Task>(ApiFromClientTopic.Broadcast, new { context = context });
        }

        internal static Task ChannelBroadcastAsync(string channelId, ContextBase context)
        {
            return channelClient.DispatchAsync<Task>(ApiFromClientTopic.ChannelBroadcast, new { id = channelId, context });
        }

        internal static Task<AppIntent> FindIntentAsync(string intent, ContextBase context)
        {
            return channelClient.DispatchAsync<AppIntent>(ApiFromClientTopic.FindIntent, new { intent, context });
        }

        internal static Task<List<AppIntent>> FindIntentsByContextAsync(ContextBase context)
        {
            return channelClient.DispatchAsync<List<AppIntent>>(ApiFromClientTopic.FindIntentsByContext, new { context });
        }

        internal static Task<ChannelTransport> GetChannelByIdAsync(string channelId)
        {
            return channelClient.DispatchAsync<ChannelTransport>(ApiFromClientTopic.GetChannelById, new { id = channelId });
        }

        internal async static Task<ChannelBase> GetCurrentChannelAsync(Identity identity)
        {
            var channelTransport = await channelClient.DispatchAsync<ChannelTransport>(ApiFromClientTopic.GetCurrentChannel, new { identity });
            return ChannelUtils.GetChannelObject(channelTransport);
        }

        internal static Task<ContextBase> GetCurrentContextAsync(string channelId)
        {
            return channelClient.DispatchAsync<ContextBase>(ApiFromClientTopic.ChannelGetCurrentContext, new { id = channelId });
        }

        internal async static Task<IEnumerable<DesktopChannel>> GetDesktopChannelsAsync()
        {
            var transports = await channelClient.DispatchAsync<List<DesktopChannelTransport>>(ApiFromClientTopic.GetDesktopChannels, JValue.CreateUndefined());
            var channels = new List<DesktopChannel>();

            foreach (var transport in transports)
            {
                var channel = ChannelUtils.GetChannelObject(transport) as DesktopChannel;
                channels.Add(channel);
            }

            return channels;
        }

        internal static Task JoinChannelAsync(string channelId, Identity identity = null)
        {
            if (identity == null)
            {
                return channelClient.DispatchAsync<Task>(ApiFromClientTopic.ChannelJoin, new { id = channelId });
            }

            return channelClient.DispatchAsync<Task>(ApiFromClientTopic.ChannelJoin, new { id = channelId, identity = identity });
        }

        internal static Task OpenAsync(string name, ContextBase context = null)
        {
            return channelClient.DispatchAsync<string>(ApiFromClientTopic.Open, new { name, context });
        }

        internal static Task<IntentResolution> RaiseIntent(string intent, ContextBase context, string target)
        {
            return channelClient.DispatchAsync<IntentResolution>(ApiFromClientTopic.RaiseIntent, new { intent, context, target });
        }

        internal static Task RemoveChannelContextListenerAsync(ChannelContextListener listener)
        {
            return channelClient.DispatchAsync<Task>(ApiFromClientTopic.ChannelRemoveContextListener, new { id = listener.Channel.ChannelId });
        }

        internal static Task RemoveIntentHandler(string intent)
        {
            return channelClient.DispatchAsync(ApiFromClientTopic.RemoveIntentListener, new { intent });
        }

        internal static Task AddChannelEventListenerAsync(string channelId, FDC3EventType eventType)
        {            
            return channelClient.DispatchAsync(ApiFromClientTopic.ChannelAddEventListener, new { id = channelId, eventType });
        }

        internal static Task AddIntentHandlerAsync(string intent)
        {
            return channelClient.DispatchAsync(ApiFromClientTopic.AddIntentListener, new { intent });
        }
    }
}