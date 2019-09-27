using Openfin.Desktop;
using OpenFin.FDC3.Constants;
using OpenFin.FDC3.Exceptions;
using System;

namespace OpenFin.FDC3
{
    public class FDC3
    {
        /// <summary>
        /// Fires when DesktopAgent initialization is completed successfully.
        /// Returns exception if a failure occurred during initialization.
        /// /// Must be set before calling Initialize().
        /// </summary>
        public static Action InitializationComplete;
        private static bool isInitialized = false;
        private static Runtime runtimeInstance;
        public static string Uuid => runtimeInstance.Options.UUID;       

        /// <summary>
        /// Initialize client with the default Manifest URL
        /// </summary>
        public static void Initialize()
        {
            if (isInitialized)
                return;

            var fdcManifestUri = new Uri(Fdc3ServiceConstants.ServiceManifestUrl);
            var runtimeOptions = RuntimeOptions.LoadManifest(fdcManifestUri);
            completeInitialization(runtimeOptions);
        }

        /// <summary>
        /// Initialize the agent with a specified URL. The InitializationComplete Action delegate must be set before calling this function.
        /// </summary>
        /// <param name="manifestUri">The URI of the path of the manifest.</param>
        public static void Initialize(string filePath)
        {
            if (isInitialized)
                return; 

            var runtimeOptions = RuntimeOptions.LoadManifest(filePath);
            completeInitialization(runtimeOptions);
        }

        private static void completeInitialization(RuntimeOptions runtimeOptions)
        {
            if (InitializationComplete == null)
                throw new OpenFinInitializationException("InitializationComplete action delegate must be set before calling Initialize.");

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
                    isInitialized = true;
                    InitializationComplete.Invoke();                       
                });
            });
        }
    }
}