using Newtonsoft.Json;

namespace UDO.Hammer.Editor.Backend.Model.Custom
{
    // ReSharper disable once InconsistentNaming
    public class _HammerGameSdkVersionEnriched : _HammerGameSdkVersion
    {
        [JsonProperty("sdks")] public _HammerSdk Sdk { get; set; }
        [JsonProperty("sdks_versions")] public _HammerSdkVersion SdkVersion { get; set; }
    }
}