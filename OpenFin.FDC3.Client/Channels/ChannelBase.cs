using OpenFin.FDC3.Context;
using OpenFin.FDC3.Events;
using OpenFin.FDC3.Handlers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenFin.FDC3.Channels
{
    /// <summary>
    /// The base class for context channels.
    /// </summary>
    public abstract class ChannelBase
    {
        private Connection connection;
        public string ChannelId { get; private set; }
        public readonly ChannelType ChannelType;

        protected ChannelBase(string channelId, ChannelType channelType, Connection connection)
        {
            ChannelId = channelId;
            ChannelType = channelType;
            this.connection = connection;
        }

        /// <summary>
        /// Returns a collection of all windows connected to this channel.
        /// </summary>
        /// <returns></returns>
        public Task<List<Identity>> GetMembersAsync()
        {
            return connection.GetChannelMembersAsync(this.ChannelId);
        }

        /// <summary>
        /// Returns the last context that was broadcast on this channel. All channels initially have no context, until a window is added to the channel and then broadcasts.
        /// The context of a channel will be captured regardless of how it's set on the channel.
        /// </summary>
        /// <returns>
        /// If no contexts have been passed to the channel this method returns null. Context is set to its initial context-less state when a channel is cleared of all windows.
        /// </returns>
        public Task<ContextBase> GetCurrentContextAsync()
        {
            return connection.GetCurrentContextAsync(this.ChannelId);
        }

        /// <summary>
        /// Adds the provided window to this channel.
        /// If this channel has a current context, the context will be passed to the window through its context listener upon joining this channel.
        /// </summary>
        /// <param name="identity">The window to be added to this channel</param>
        /// <returns></returns>
        public Task JoinAsync(Identity identity = null)
        {
            return connection.JoinChannelAsync(this.ChannelId, identity);
        }

        /// <summary>
        /// Broadcasts the given context on this channel.
        /// Note that this function ca be used without first joining the channel, allowing applciations to broadcast on channels they are not members.
        /// This broadcast will be receied by all windows that are members of this channel, except for the window that makes the broadcast.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task BroadcastAsync(ContextBase context)
        {
            return connection.ChannelBroadcastAsync(this.ChannelId, context);
        }

        /// <summary>
        /// Adds the event that is fired when a window broadcasts on this channel
        /// </summary>
        /// <param name="listener"></param>
        /// <returns></returns>
        public Task AddContextListenerAsync(Action<ContextBase> listener)
        {
            var channelContextListener = new ChannelContextListener
            {
                Channel = this,
                Handler = listener
            };

            var hasAny = FDC3Handlers.HasContextListener(this.ChannelId);

            FDC3Handlers.ChannelContextHandlers.Add(channelContextListener);

            if (!hasAny)
                return connection.AddChannelContextListenerAsync(this.ChannelId);
            else
                return new TaskCompletionSource<object>(null).Task;
        }

        /// <summary>
        /// Removes the event fired when a window
        /// </summary>
        /// <param name="listener"></param>
        public void RemoveContextListener(ChannelContextListener listener)
        {
            FDC3Handlers.ChannelContextHandlers.RemoveAll(x => x.Channel.ChannelId == listener.Channel.ChannelId);
            connection.RemoveChannelContextListenerAsync(listener);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="eventHandler"></param>
        /// <returns></returns>
        public Task AddEventListenerAsync(FDC3EventType eventType, Action<FDC3Event> eventHandler)
        {
            var hasAny = FDC3Handlers.HasEventListener(this.ChannelId);
            FDC3Handlers.FDC3ChannelEventHandlers[this.ChannelId].Add(eventType, eventHandler);

            if (!hasAny)
                return connection.AddChannelEventListenerAsync(this.ChannelId, eventType);
            else
                return new TaskCompletionSource<object>(null).Task;
        }

        public Task UnsubscribeEventListenerAsync(FDC3EventType eventType, Action<FDC3Event> eventHandler)
        {
            FDC3Handlers.FDC3ChannelEventHandlers[this.ChannelId][eventType] -= eventHandler;
            FDC3Handlers.FDC3ChannelEventHandlers[this.ChannelId].Remove(eventType);

            if (FDC3Handlers.FDC3ChannelEventHandlers[this.ChannelId][eventType] == null)
            {
                return connection.RemoveFDC3EventListenerAsync(this.ChannelId, eventType);
            }
            else
            {
                return new TaskCompletionSource<object>().Task;
            }

            return null;
        }
    }
}