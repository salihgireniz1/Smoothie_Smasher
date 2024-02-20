using System;
using Newtonsoft.Json;

namespace UDO.Hammer.Editor.Backend
{
    // ReSharper disable once InconsistentNaming
    public class _HammerBucket
    {
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("owner")] public string Owner { get; set; }
        [JsonProperty("created_at")] public DateTime CreatedAt { get; set; }
        [JsonProperty("updated_at")] public DateTime UpdatedAt { get; set; }
        [JsonProperty("public")] public bool Public { get; set; }
    }
}