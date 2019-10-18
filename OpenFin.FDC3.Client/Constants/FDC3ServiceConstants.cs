namespace OpenFin.FDC3.Constants
{
    internal static class Fdc3ServiceConstants
    {
        private const string StagingVersion = "0.2.0-alpha.20";

        internal const string ServiceChannel = "of-fdc3-service-v1";
        internal const string ServiceIdentityName = "fdc3-service";
        internal const string ServiceIdentyUuid = "fdc3-service";

#if DEBUG
        //internal const string ServiceManifestUrl = "https://cdn.openfin.co/services/openfin/fdc3/app.staging.json";
        internal const string ServiceManifestUrl = "https://cdn.openfin.co/services/openfin/fdc3/0.2.0-alpha.20/app.json";
#elif STAGING
        internal static string ServiceManifestUrl = $"https://cdn.openfin.co/services/openfin/fdc3/{StagingVersion}/app.json";
#else
        internal const string ServiceManifestUrl  = "https://cdn.openfin.co/services/openfin/fdc3/app.json";
#endif
    }
}