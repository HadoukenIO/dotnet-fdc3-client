using Newtonsoft.Json.Linq;
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
    public partial class Connection
    {
        internal void AddChannelChangedEventListener(Action<ChannelChangedPayload> handler)
        {
            FDC3Handlers.ChannelChangedHandlers += handler;
        }

        internal Task AddChannelContextListenerAsync(string channelId)
        {
            return channelClient.DispatchAsync<Task>(ApiFromClientTopic.ChannelAddContextListener, new { id = channelId });
        }

        internal Task AddChannelEventListenerAsync(string channelId, FDC3EventType eventType)
        {
            return channelClient.DispatchAsync(ApiFromClientTopic.ChannelAddEventListener, new { id = channelId, eventType });
        }

        internal Task AddIntentHandlerAsync(string intent)
        {
            return channelClient.DispatchAsync(ApiFromClientTopic.AddIntentListener, new { intent });
        }

        /// <summary>
        /// Publishes context to other applications on the desktop
        /// </summary>
        /// <param name="context">The context to publish to other applications</param>
        /// <returns></returns>
        public Task BroadcastAsync(Context.ContextBase context)
        {
            return channelClient.DispatchAsync<Task>(ApiFromClientTopic.Broadcast, new { context = context });
        }

        internal Task ChannelBroadcastAsync(string channelId, ContextBase context)
        {
            return channelClient.DispatchAsync<Task>(ApiFromClientTopic.ChannelBroadcast, new { id = channelId, context });
        }

        /// <summary>
        /// Obtain information about in intent
        /// </summary>
        /// <param name="intent">The name of the intent</param>
        /// <param name="context">Optional context about the intent</param>
        /// <returns>A single application intent</returns>
        public Task<AppIntent> FindIntentAsync(string intent, ContextBase context)
        {
            return channelClient.DispatchAsync<AppIntent>(ApiFromClientTopic.FindIntent, new { intent, context });
        }

        /// <summary>
        /// Finds all intents by context
        /// </summary>
        /// <param name="context">The intent context</param>
        /// <returns></returns>
        public Task<List<AppIntent>> FindIntentsByContextAsync(ContextBase context)
        {
            return channelClient.DispatchAsync<List<AppIntent>>(ApiFromClientTopic.FindIntentsByContext, new { context });
        }

        public async Task<ChannelBase> GetChannelByIdAsync(string channelId)
        {
            var channelTransport = await channelClient.DispatchAsync<ChannelTransport>(ApiFromClientTopic.GetChannelById, new { id = channelId });
            return ChannelUtils.GetChannelObject(channelTransport, this);
        }

        public Task<List<Identity>> GetChannelMembersAsync(string channelId)
        {
            return channelClient.DispatchAsync<List<Identity>>(ApiFromClientTopic.ChannelGetMembers, new { id = channelId });
        }
        public async Task<ChannelBase> GetCurrentChannelAsync(Identity identity)
        {
            var channelTransport = await channelClient.DispatchAsync<ChannelTransport>(ApiFromClientTopic.GetCurrentChannel, new { identity });
            return ChannelUtils.GetChannelObject(channelTransport, this);
        }

        public Task<ContextBase> GetCurrentContextAsync(string channelId)
        {
            return channelClient.DispatchAsync<ContextBase>(ApiFromClientTopic.ChannelGetCurrentContext, new { id = channelId });
        }

        /// <summary>
        /// Gets all system channels.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<SystemChannel>> GetSystemChannelsAsync()
        {
            try
            {
                var transports = await channelClient.DispatchAsync<List<SystemChannelTransport>>(ApiFromClientTopic.GetSystemChannels, JValue.CreateUndefined());
                var channels = new List<SystemChannel>();
                transports.ForEach(transport => channels.Add(ChannelUtils.GetChannelObject(transport, this) as SystemChannel));                

                return channels;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        internal  Task JoinChannelAsync(string channelId, Identity identity = null)
        {
            if (identity == null)
            {
                return channelClient.DispatchAsync<Task>(ApiFromClientTopic.ChannelJoin, new { id = channelId });
            }

            return channelClient.DispatchAsync<Task>(ApiFromClientTopic.ChannelJoin, new { id = channelId, identity = identity });
        }

        /// <summary>
        /// Launches/links to an application by name
        /// </summary>
        /// <param name="name">The application name</param>
        /// <param name="context">Additional optional properties to be passed when </param>
        /// <returns></returns>
        public Task OpenAsync(string name, ContextBase context = null)
        {
            return channelClient.DispatchAsync<string>(ApiFromClientTopic.Open, new { name, context });
        }

        /// <summary>
        /// Raises an intent to resolve
        /// </summary>
        /// <param name="intent">The intent to resolve</param>
        /// <param name="context">The context</param>
        /// <param name="target"></param>
        /// <returns></returns>
        public Task<IntentResolution> RaiseIntent(string intent, ContextBase context, string target)
        {
            return channelClient.DispatchAsync<IntentResolution>(ApiFromClientTopic.RaiseIntent, new { intent, context, target });
        }

        internal  Task RemoveChannelContextListenerAsync(ChannelContextListener listener)
        {
            return channelClient.DispatchAsync<Task>(ApiFromClientTopic.ChannelRemoveContextListener, new { id = listener.Channel.ChannelId });
        }

        /// <summary>
        /// Removes all registered handlers for an intent
        /// </summary>
        /// <param name="intent">The name of the intent to remove listeners for</param>
        internal Task RemoveIntentHandler(string intent)
        {
            if (FDC3Handlers.IntentHandlers.ContainsKey(intent))
            {
                FDC3Handlers.IntentHandlers.Remove(intent);
            }

            return channelClient.DispatchAsync(ApiFromClientTopic.RemoveIntentListener, new { intent });
        }

        /// <summary>
        /// Ads a listener for incoming intents from the agent
        /// </summary>
        /// <param name="intent">The intent to listen for</param>
        /// <param name="handler">The handler to be called when an intent in received</param>
        public Task AddIntentListenerAsync(string intent, Action<ContextBase> handler)
        {
            var hasIntent = FDC3Handlers.HasIntentHandler(intent);

            if (FDC3Handlers.IntentHandlers.Any(x => x.Key == intent))
            {
                FDC3Handlers.IntentHandlers[intent] += handler;
            }
            else
            {
                FDC3Handlers.IntentHandlers.Add(intent, handler);
            }

            if (!hasIntent)
            {
                return channelClient.DispatchAsync(ApiFromClientTopic.AddIntentListener, new { intent }); ;
            }
            else
            {
                return Task.FromResult<object>(null);
            }
        }

        /// <summary>
        /// Adds a listener for incoming context broadcast from the Desktop Agent.
        /// </summary>
        /// <param name="handler">The handler to invoke when </param>
        public void AddContextHandler(Action<ContextBase> handler)
        {
            if(ContextHandlers != null)
            {
                channelClient.DispatchAsync(ApiFromClientTopic.AddContextListener, JValue.CreateUndefined());
            }

            ContextHandlers += handler;            
        }

        /// <summary>
        /// Removed context broadcast listener
        /// </summary>
        /// <param name="handler">Handler to be removed</param>
        internal void RemoveContextHandler(Action<ContextBase> handler)
        {
            ContextHandlers -= handler;

            if(ContextHandlers == null)
            {
                channelClient.DispatchAsync(ApiFromClientTopic.RemoveContextListener, JValue.CreateUndefined());
            }
        }
    }
}