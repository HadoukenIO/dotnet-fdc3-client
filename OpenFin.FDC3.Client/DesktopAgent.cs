using Openfin.Desktop;
using OpenFin.FDC3.Constants;
using OpenFin.FDC3.Exceptions;
using System;

namespace OpenFin.FDC3
{
    public class FDC3
    {
        private static bool isInitialized = false;
        private static Runtime runtimeInstance;

        /// <summary>
        /// Fires when initialization has completed successfully.
        /// Must be set before calling Initialize().
        /// </summary>
        public static Action OnInitialized;      
                
        public static string Uuid => runtimeInstance?.Options?.UUID;      

        /// <summary>
        /// Initialize client with the default manifest URL
        /// </summary>
        public static void Initialize()
        {
            if (isInitialized)
                return;

            if (OnInitialized == null)
                throw new OpenFinInitializationException("OnInitialized action delegate must be set before calling Initialize.");

            var fdcManifestUri = new Uri(Fdc3ServiceConstants.ServiceManifestUrl);
            var runtimeOptions = RuntimeOptions.LoadManifest(fdcManifestUri);
            completeInitialization(runtimeOptions);
        }

        /// <summary>
        /// Initialize the agent with a specified URL. The OnInitialized Action delegate must be set before calling this function.
        /// </summary>
        /// <param name="manifestUri">The URI of the path of the manifest.</param>
        public static void Initialize(string filePath)
        {
            if (isInitialized)
                return;

            if (OnInitialized == null)
                throw new OpenFinInitializationException("OnInitialized action delegate must be set before calling Initialize.");

            var runtimeOptions = RuntimeOptions.LoadManifest(filePath);
            completeInitialization(runtimeOptions);
        }

        private static void completeInitialization(RuntimeOptions runtimeOptions)
        {
            runtimeInstance = Runtime.GetRuntimeInstance(runtimeOptions);

            runtimeInstance.Connect(() =>
            {
                var fdcService = runtimeInstance.CreateApplication(runtimeOptions.StartupApplicationOptions);

                fdcService.isRunning(ack =>
                {
                    if (!ack.HasAcked())
                    {
                        fdcService.run();
                    }

                    ConnectionManager.RuntimeInstance = runtimeInstance;

                    runtimeInstance.System.getRuntimeInfo(info =>
                    {
                        ConnectionManager.RuntimeInfo = info;

                        isInitialized = true;
                        OnInitialized.Invoke();
                    });                   
                });
            });
        }
    }
}