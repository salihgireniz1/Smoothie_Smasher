using Newtonsoft.Json;

namespace UDO.Hammer.Editor.Backend.Model.Custom
{
    // ReSharper disable once InconsistentNaming
    public class _HammerGameSdkVersionKeyServerEnriched : _HammerGameSdkVersionKeyServer
    {
        [JsonProperty("sdks")] public _HammerSdk Sdk { get; set; }
        [JsonProperty("sdks_versions")] public _HammerSdkVersion SdkVersion { get; set; }
        [JsonProperty("sdks_versions_keys")] public _HammerSdkVersionKey SdkVersionKey { get; set; }
    }
}