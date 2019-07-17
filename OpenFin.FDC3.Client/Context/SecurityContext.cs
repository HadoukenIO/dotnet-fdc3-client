using Newtonsoft.Json;
using OpenFin.FDC3.Constants;

namespace OpenFin.FDC3.Context
{
    public class SecurityContext : ContextBase
    {
        [JsonProperty("type")]
        public override string Type => ContextTypes.Security;
    }
}