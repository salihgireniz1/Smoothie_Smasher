using Newtonsoft.Json;

namespace UDO.Hammer.Editor.Backend.Model
{
    // ReSharper disable once InconsistentNaming
    public class _HammerGameSdkVersion : _HammerCommonFields
    {
        [JsonProperty("game_uuid")] public string GameUuid { get; set; }
        [JsonProperty("sdk_uuid")] public string SdkUuid { get; set; }
        [JsonProperty("sdk_version_uuid")] public string SdkVersionUuid { get; set; }
    }
}