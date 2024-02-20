using System;
using Newtonsoft.Json;

namespace UDO.Hammer.Editor.Backend
{
    // ReSharper disable once InconsistentNaming
    [Serializable]
    public class _HammerSession
    {
        [JsonProperty("access_token")] public string AccessToken { get; set; }
        [JsonProperty("expires_in")] public int ExpiresIn { get; set; }
        [JsonProperty("refresh_token")] public string RefreshToken { get; set; }
        [JsonProperty("token_type")] public string TokenType { get; set; }
        [JsonProperty("user")] public _HammerUser User { get; set; }
        [JsonProperty("created_at")] public DateTime CreatedAt { get; private set; } = DateTime.Now;

        public DateTime ExpiresAt()
        {
            return new DateTime(CreatedAt.Ticks).AddSeconds(ExpiresIn);
        }
    }
}