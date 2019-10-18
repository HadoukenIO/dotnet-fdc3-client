using OpenFin.FDC3.Context;
using System;

namespace OpenFin.FDC3.Channels
{
    public abstract class ContextListener
    {
        public Action<ContextBase> Handler { get; set; }
    }
}