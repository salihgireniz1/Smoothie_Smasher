using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Udo.Hammer.Runtime.Core
{
    [Serializable]
    public class HammerRemoteConfigLong : AHammerRemoteConfig
    {
        public HammerRemoteConfigLong(string key, long value) : base(key)
        {
            Value = value;
        }

        [JsonProperty("value")]
        [field: SerializeField]
        public long Value { get; set; }
    }
}