using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace UDO.Hammer.Editor.Backend
{
    // ReSharper disable once InconsistentNaming
    [Serializable]
    public class _HammerUser
    {
        [JsonProperty("action_link")] public string? ActionLink { get; set; }
        [JsonProperty("app_metadata")] public Dictionary<string, object> AppMetadata { get; set; } = new();
        [JsonProperty("aud")] public string Aud { get; set; }
        [JsonProperty("confirmation_sent_at")] public DateTime? ConfirmationSentAt { get; set; }
        [JsonProperty("confirmed_at")] public DateTime? ConfirmedAt { get; set; }
        [JsonProperty("created_at")] public DateTime CreatedAt { get; set; }
        [JsonProperty("email")] public string? Email { get; set; }
        [JsonProperty("email_confirmed_at")] public DateTime? EmailConfirmedAt { get; set; }
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("identities")] public List<_HammerUserIdentity> Identities { get; set; } = new();
        [JsonProperty("invited_at")] public DateTime? InvitedAt { get; set; }
        [JsonProperty("last_sign_in_at")] public DateTime? LastSignInAt { get; set; }
        [JsonProperty("phone")] public string? Phone { get; set; }
        [JsonProperty("phone_confirmed_at")] public DateTime? PhoneConfirmedAt { get; set; }
        [JsonProperty("recovery_sent_at")] public DateTime? RecoverySentAt { get; set; }
        [JsonProperty("role")] public string? Role { get; set; }
        [JsonProperty("updated_at")] public DateTime? UpdatedAt { get; set; }
        [JsonProperty("user_metadata")] public Dictionary<string, object> UserMetadata { get; set; } = new();
    }
}