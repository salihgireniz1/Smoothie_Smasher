using Newtonsoft.Json;

namespace UDO.Hammer.Editor.Backend.Model
{
    // ReSharper disable once InconsistentNaming
    public class _HammerSdk : _HammerCommonFields
    {
        [JsonProperty("sdk_name")] public string SdkName { get; set; }
    }
}