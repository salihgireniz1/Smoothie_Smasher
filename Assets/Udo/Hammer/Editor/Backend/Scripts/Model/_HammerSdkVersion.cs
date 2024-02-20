using Newtonsoft.Json;

namespace UDO.Hammer.Editor.Backend.Model
{
    // ReSharper disable once InconsistentNaming
    public class _HammerSdkVersion : _HammerCommonFields
    {
        [JsonProperty("sdk_uuid")] public string SdkUuid { get; set; }
        [JsonProperty("sdk_version")] public string SdkVersion { get; set; }
    }
}