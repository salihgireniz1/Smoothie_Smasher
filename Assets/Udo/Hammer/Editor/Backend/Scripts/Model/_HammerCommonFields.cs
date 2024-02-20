using System;
using Newtonsoft.Json;

namespace UDO.Hammer.Editor.Backend.Model
{
    // ReSharper disable once InconsistentNaming
    public class _HammerCommonFields
    {
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("created_at")] public DateTime CreatedAt { get; set; }
        [JsonProperty("created_by")] public string CreatedBy { get; set; }
        [JsonProperty("version")] public int Version { get; set; }
        [JsonProperty("updated_at")] public DateTime UpdatedAt { get; set; }
        [JsonProperty("updated_by")] public string UpdatedBy { get; set; }
        [JsonProperty("public_id")] public string PublicId { get; set; }
        [JsonProperty("active")] public bool Active { get; set; }
    }
}