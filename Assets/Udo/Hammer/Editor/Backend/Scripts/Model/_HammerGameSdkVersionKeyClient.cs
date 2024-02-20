using Newtonsoft.Json;

namespace UDO.Hammer.Editor.Backend.Model
{
    // ReSharper disable once InconsistentNaming
    public class _HammerGameSdkVersionKeyClient : _HammerCommonFields
    {
        [JsonProperty("game_uuid")] public string GameUuid { get; set; }
        [JsonProperty("sdk_uuid")] public string SdkUuid { get; set; }
        [JsonProperty("sdk_version_uuid")] public string SdkVersionUuid { get; set; }
        [JsonProperty("sdk_version_key_uuid")] public string SdkVersionKeyUuid { get; set; }
        [JsonProperty("client_value")] public string ClientValue { get; set; }
    }
}