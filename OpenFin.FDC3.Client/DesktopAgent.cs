using Openfin.Desktop;
using OpenFin.FDC3.Constants;
using OpenFin.FDC3.Context;
using OpenFin.FDC3.Events;
using OpenFin.FDC3.Exceptions;
using OpenFin.FDC3.Handlers;
using OpenFin.FDC3.Intents;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace OpenFin.FDC3
{
    public class DesktopAgent
    {
        /// <summary>
        /// Fires when DesktopAgent initialization is completed successfully.
        /// Returns exception if a failure occurred during initialization.
        /// /// Must be set before calling Initialize().
        /// </summary>
        public static Action<Exception> InitializationComplete;

        private static Runtime runtimeInstance;
        public static string Uuid => runtimeInstance.Options.UUID;
        private static bool isInitialized = false;

        //internal static Dictionary<string, Action<ContextBase>> IntentListeners;

        static DesktopAgent()
        {
        }

        /// <summary>
        /// Provides access to channel functions (eg. getting/joining channels)
        /// </summary>
        public static ContextChannels CtxChannels
        {
            get
            {
                if (!isInitialized)
                    throw new OpenFinInitializationException("DesktopAgent must be initialized before attempting to use ContextChannels API.");

                return ContextChannels.Instance;
            }
        }     

        /// <summary>
        /// Adds a listener for incoming context broadcast from the Desktop Agent.
        /// </summary>
        /// <param name="handler">The handler to invoke when </param>
        public static void AddContextHandler(Action<ContextBase> handler)
        {
            FDC3Handlers.ContextHandlers += handler;            
        }

        /// <summary>
        /// Removed context broadcast listener
        /// </summary>
        /// <param name="handler">Handler to be removed</param>
        internal static void RemoveContextHandler(Action<ContextBase> handler)
        {
            FDC3Handlers.ContextHandlers -= handler;
        }

        /// <summary>
        /// Ads a listener for incoming intents from the agent
        /// </summary>
        /// <param name="intent">The intent to listen for</param>
        /// <param name="handler">The handler to be called when an intent in received</param>
        public static Task AddIntentListenerAsync(string intent, Action<ContextBase> handler)
        {
            var hasIntent = FDC3Handlers.HasIntentHandler(intent);

            if(FDC3Handlers.IntentHandlers.Any(x => x.Key == intent))
            {
                FDC3Handlers.IntentHandlers[intent] += handler;
            }
            else
            {
                FDC3Handlers.IntentHandlers.Add(intent, handler);
            }

            if(!hasIntent)
            {
                return Connection.AddIntentHandlerAsync(intent);
            }
            else
            {
                return Task.FromResult<object>(null);
            }            
        }

        /// <summary>
        /// Publishes context to other applications on the desktop
        /// </summary>
        /// <param name="context">The context to publish to other applications</param>
        /// <returns></returns>
        public static Task BroadcastAsync(ContextBase context)
        {
            return Connection.BroadcastAsync(context);
        }

        /// <summary>
        /// Obtain information about in intent
        /// </summary>
        /// <param name="intent">The name of the intent</param>
        /// <param name="context">Optional context about the intent</param>
        /// <returns>A single application intent</returns>
        public static Task<AppIntent> FindIntentAsync(string intent, ContextBase context = null)
        {
            return Connection.FindIntentAsync(intent, context);
        }

        /// <summary>
        /// Finds all intents by context
        /// </summary>
        /// <param name="context">The intent context</param>
        /// <returns></returns>
        public static Task<List<AppIntent>> FindIntentsByContextAsync(ContextBase context)
        {
            return Connection.FindIntentsByContextAsync(context);
        }

        /// <summary>
        /// Initialize client with the default Manifest URL
        /// </summary>
        public static void Initialize(string windowNameAlias = "")
        {
            var fdcManifestUri = new Uri(Fdc3ServiceConstants.ServiceManifestUrl);
            Initialize(fdcManifestUri, windowNameAlias);
        }

        /// <summary>
        /// Initialize the agent with a specified URL. The InitializationComplete Action delegate must be set before calling this function.
        /// </summary>
        /// <param name="manifestUri">The URI if the manifest</param>
        public static void Initialize(Uri manifestUri, string windowNameAlias = "")
        {
            if (InitializationComplete == null)
                throw new OpenFinInitializationException("InitializationComplete action delegate must be set before calling Initialize.");

#if DEBUG

            var runtimeOptions = RuntimeOptions.LoadManifest(System.IO.Directory.GetCurrentDirectory() + "\\app.json");
#else
            var runtimeOptions = RuntimeOptions.LoadManifest(manifestUri);
            #endif
            runtimeInstance = Runtime.GetRuntimeInstance(runtimeOptions);
            runtimeInstance.Options.RuntimeConnectTimeout = -1;
            //runtimeInstance.Options.Version = "12.69.43.12";

            runtimeInstance.Connect(() =>
            {
                var fdcService = runtimeInstance.CreateApplication(runtimeOptions.StartupApplicationOptions);
                
                fdcService.isRunning(ack =>
                {
                    if (!ack.HasAcked())
                    {
                        fdcService.run();                        
                    }

                    ///EventRouter.Instance.RegisterEmitterProvider("main", Func<)
                    
                    Connection.ConnectionInitializationComplete = exception =>
                    {
                        InitializationComplete?.Invoke(exception);
                        isInitialized = true;
                        
                    };

                    Connection.Initialize(runtimeInstance, windowNameAlias);
                });
            });
        }

        /// <summary>
        /// Launches/links to an application by name
        /// </summary>
        /// <param name="name">The application name</param>
        /// <param name="context">Additional optional properties to be passed when </param>
        /// <returns></returns>
        public static Task OpenAsync(string name, ContextBase context = null)
        {
            return Connection.OpenAsync(name, context);
        }

        /// <summary>
        /// Raises an intent to resolve
        /// </summary>
        /// <param name="intent">The intent to resolve</param>
        /// <param name="context">The context</param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Task<IntentResolution> RaiseIntentAsync(string intent, ContextBase context, string target)
        {
            return Connection.RaiseIntent(intent, context, target);
        }




        /// <summary>
        /// Removed all registered handlers for an intent
        /// </summary>
        /// <param name="intent">The name of the intent to remove listeners for</param>
        public static Task RemoveIntentListenerAsync(string intent)
        {
            if (FDC3Handlers.IntentHandlers.ContainsKey(intent))
            {
                FDC3Handlers.IntentHandlers.Remove(intent);
            }

            return Connection.RemoveIntentHandler(intent);
        }

        /// <summary>
        /// Removes a specific intent handler by the intent and handler
        /// </summary>
        /// <param name="intent">The name of the intent</param>
        /// <param name="handler">The handler to be removed</param>
        //public static void UnsubscribeIntentListener(string intent, Action<ContextBase> handler)
        //{
        //    Connection.UnsubscribeIntentListener(intent, handler);
        //}
    }
}