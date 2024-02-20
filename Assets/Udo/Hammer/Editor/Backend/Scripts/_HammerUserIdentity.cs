using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace UDO.Hammer.Editor.Backend
{
    // ReSharper disable once InconsistentNaming
    [Serializable]
    public class _HammerUserIdentity
    {
        [JsonProperty("created_at")] public DateTime CreatedAt { get; set; }
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("identity_data")] public Dictionary<string, object> IdentityData { get; set; } = new();
        [JsonProperty("last_sign_in_at")] public DateTime LastSignInAt { get; set; }
        [JsonProperty("provider")] public string Provider { get; set; }
        [JsonProperty("updated_at")] public DateTime? UpdatedAt { get; set; }
        [JsonProperty("user_id")] public string UserId { get; set; }
    }
}