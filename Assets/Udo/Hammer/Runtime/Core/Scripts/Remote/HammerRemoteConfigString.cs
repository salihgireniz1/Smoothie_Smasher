using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Udo.Hammer.Runtime.Core
{
    [Serializable]
    public class HammerRemoteConfigString : AHammerRemoteConfig
    {
        public HammerRemoteConfigString(string key, string value) : base(key)
        {
            Value = value;
        }

        [JsonProperty("value")]
        [field: SerializeField]
        public string Value { get; set; }
    }
}