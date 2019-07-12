using Openfin.Desktop;
using OpenFin.FDC3.Constants;
using OpenFin.FDC3.Context;
using OpenFin.FDC3.Events;
using OpenFin.FDC3.Payloads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenFin.FDC3
{
    internal static partial class Connection
    {
        internal static void Initialize(Runtime runtimeInstance, string windowNameAlias = "")
        {
            intentListeners = new Dictionary<string, Action<ContextBase>>();
            channelClient = runtimeInstance.InterApplicationBus.Channel.CreateClient(Fdc3ServiceConstants.ServiceChannel, windowNameAlias);

            registerChannelTopics();

            channelClient.ConnectAsync().ContinueWith(x =>
            {
                ConnectionInitializationComplete?.Invoke(x.Exception);
            });
        }

        private static void registerChannelTopics()
        {
            if (channelClient == null)
            {
                throw new NullReferenceException("ChannelClient must be created before registering topics.");
            }

            channelClient.RegisterTopic<RaiseIntentPayload>(ApiToClientTopic.Intent, payload =>
            {
                if (DesktopAgent.IntentListeners.ContainsKey(payload.Intent))
                {
                    DesktopAgent.IntentListeners[payload.Intent].Invoke(payload.Context);
                }
            });

            channelClient.RegisterTopic<HandleChannelContextPayload>(ApiToClientTopic.HandleChannelContext, payload =>
            {
                foreach (var listener in channelContextListeners.Where(x => x.Channel.ChannelId == payload.ChannelId))
                {
                    listener.Handler.Invoke(payload.Context);
                }
            });

            channelClient.RegisterTopic<object>(ApiToClientTopic.Warn, payload =>
            {
                Console.WriteLine(payload);
            });

            channelClient.RegisterTopic<EventTransport<FDC3Event>>(ApiToClientTopic.Event, @event =>
            {
                EventRouter.Instance.DispatchEvent(@event);
            });

            channelClient.RegisterTopic<ContextBase>(ApiToClientTopic.Context, payload =>
            {
                contextListeners.Invoke(payload);
            });
        }

        internal static Task AddEventListenerAsync(string channelId, FDC3EventType eventType)
        {
            return channelClient.DispatchAsync(ApiFromClientTopic.ChannelAddEventListener, new { id = channelId, eventType });
        }

        internal static Task AddIntentHandlerAsync(string intent)
        {
            return channelClient.DispatchAsync(ApiFromClientTopic.AddIntentListener, new { intent });
        }
    }
}