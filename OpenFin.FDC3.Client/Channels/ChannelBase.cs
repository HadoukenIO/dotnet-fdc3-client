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
        public string ChannelId { get; private set; }
        public readonly string ChannelType;

        protected ChannelBase(string channelId, string channelType)
        {
            ChannelId = channelId;
            ChannelType = channelType;
        }

        /// <summary>
        /// Returns a collection of all windows connected to this channel.
        /// </summary>
        /// <returns></returns>
        public Task<List<Identity>> GetMembersAsync()
        {
            return Connection.GetChannelMembersAsync(this.ChannelId);
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
            return Connection.GetCurrentContextAsync(this.ChannelId);
        }

        /// <summary>
        /// Adds the provided window to this channel.
        /// If this channel has a current context, the context will be passed to the window through its context listener upon joining this channel.
        /// </summary>
        /// <param name="identity">The window to be added to this channel</param>
        /// <returns></returns>
        public Task JoinAsync(Identity identity = null)
        {
            return Connection.JoinChannelAsync(this.ChannelId, identity);
        }

        public Task BroadcastAsync(ContextBase context)
        {
            return Connection.ChannelBroadcastAsync(this.ChannelId, context);
        }

        public Task<ChannelContextListener> AddContextListenerAsync(Action<ContextBase> listener)
        {
            return Connection.AddChannelContextListenerAsync(this, listener);
        }

        public void RemoveContextListener(ChannelContextListener listener)
        {
            FDC3Handlers.ChannelContextHandlers.RemoveAll(x => x.Channel.ChannelId == listener.Channel.ChannelId);
            Connection.RemoveChannelContextListenerAsync(listener);
        }

        public Task AddEventListnerAsync(FDC3EventType eventType, Action<FDC3Event> eventHandler)
        {
            return Connection.AddEventListenerAsync(this.ChannelId, eventType);
        }
    }
}