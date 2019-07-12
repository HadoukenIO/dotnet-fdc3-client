using OpenFin.FDC3.Context;

namespace OpenFin.FDC3.Payloads
{
    public class HandleChannelContextPayload
    {
        public string ChannelId { get; set; }
        public ContextBase Context { get; set; }
    }
}