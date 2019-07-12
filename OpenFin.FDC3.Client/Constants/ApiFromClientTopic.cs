namespace OpenFin.FDC3.Constants
{
    internal static class ApiFromClientTopic
    {
        internal const string Open = "OPEN";
        internal const string FindIntent = "FIND-INTENT";
        internal const string FindIntentsByContext = "FIND-INTENTS-BY-CONTEXT";
        internal const string Broadcast = "BROADCAST";
        internal const string RaiseIntent = "RAISE-INTENT";
        internal const string AddIntentListener = "ADD-INTENT-LISTENER";
        internal const string RemoveIntentListener = "REMOVE-INTENT-LISTENER";
        internal const string GetDesktopChannels = "GET-DESKTOP-CHANNELS";
        internal const string GetChannelById = "GET-CHANNEL-BY-ID";
        internal const string GetCurrentChannel = "GET-CURRENT-CHANNEL";
        internal const string ChannelGetMembers = "CHANNEL-GET-MEMBERS";
        internal const string ChannelJoin = "CHANNEL-JOIN";
        internal const string ChannelBroadcast = "CHANNEL-BROADCAST";
        internal const string ChannelGetCurrentContext = "CHANNEL-GET-CURRENT-CONTEXT";
        internal const string ChannelAddContextListener = "CHANNEL-ADD-CONTEXT-LISTENER";
        internal const string ChannelRemoveContextListener = "CHANNEL-REMOVE-CONTEXT-LISTENER";
        internal const string ChannelAddEventListener = "CHANNEL-ADD-EVENT-LISTENER";
    }
}