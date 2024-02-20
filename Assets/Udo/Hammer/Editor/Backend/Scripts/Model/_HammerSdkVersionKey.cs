using Newtonsoft.Json;

namespace UDO.Hammer.Editor.Backend.Model
{
    // ReSharper disable once InconsistentNaming
    public class _HammerSdkVersionKey : _HammerCommonFields
    {
        [JsonProperty("sdk_uuid")] public string SdkUuid { get; set; }
        [JsonProperty("sdk_version_uuid")] public string SdkVersionUuid { get; set; }
        [JsonProperty("sdk_key")] public string SdkKey { get; set; }
        [JsonProperty("sdk_default_value")] public string SdkDefaultValue { get; set; }
        [JsonProperty("sdk_value_type")] public string SdkValueType { get; set; }
    }
}