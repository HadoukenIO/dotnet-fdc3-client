using Openfin.Desktop;
using Openfin.Desktop.Messaging;
using OpenFin.FDC3.Constants;
using OpenFin.FDC3.Context;
using OpenFin.FDC3.Events;
using OpenFin.FDC3.Handlers;
using OpenFin.FDC3.Payloads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenFin.FDC3
{
    public partial class Connection 
    {       
        private string connectionAlias { get; }

        private ChannelClient channelClient;

        Action<ContextBase> ContextHandlers;

        internal Connection(string alias)
        {         
            connectionAlias = alias;            
        }

        internal Task InitializeAsync()
        {

            if (!string.IsNullOrEmpty(ConnectionManager.RuntimeInfo.FDC3ChannelName))
            {
                channelClient = ConnectionManager.RuntimeInstance.InterApplicationBus.Channel.CreateClient(ConnectionManager.RuntimeInfo.FDC3ChannelName, connectionAlias);
            }
            else
            {                
                channelClient = ConnectionManager.RuntimeInstance.InterApplicationBus.Channel.CreateClient(Fdc3ServiceConstants.ServiceChannel, connectionAlias);
            }

            registerChannelTopics();

            return channelClient.ConnectAsync();
        }

        public Task DisconnectAsync()
        {
            return channelClient.DisconnectAsync();
        }

        private void registerChannelTopics()
        {
            if (channelClient == null)
            {
                throw new NullReferenceException("ChannelClient must be created before registering topics.");
            }

            channelClient.RegisterTopic<ReceiveIntentPayload>(ApiToClientTopic.ReceiveIntent, payload =>
            {
                if (FDC3Handlers.IntentHandlers.ContainsKey(payload.Intent))
                {
                    FDC3Handlers.IntentHandlers[payload.Intent].Invoke(payload.Context);
                }
            });

            channelClient.RegisterTopic<HandleChannelContextPayload>(ApiToClientTopic.ChannelReceiveContext, payload =>
            {
                foreach (var listener in FDC3Handlers.ChannelContextHandlers.Where(x => x.Channel.ChannelId == payload.ChannelId))
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
                EventRouter.Instance.DispatchEvent(@event, this);
            });

            channelClient.RegisterTopic<ReceiveContextPayload>(ApiToClientTopic.ReceiveContext, payload =>
            {
                ContextHandlers?.Invoke(payload.Context);
            });
        }     
    }
}