using OpenFin.FDC3.Context;
using System;

namespace OpenFin.FDC3.Handlers
{
    public class IntentHandler
    {
        public Action<ContextBase> Handler;
        public string Intent { get; set; }
    }
}