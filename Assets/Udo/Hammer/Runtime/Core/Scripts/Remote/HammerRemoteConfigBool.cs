using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Udo.Hammer.Runtime.Core
{
    [Serializable]
    public class HammerRemoteConfigBool : AHammerRemoteConfig
    {
        public HammerRemoteConfigBool(string key, bool value) : base(key)
        {
            Value = value;
        }

        [JsonProperty("value")]
        [field: SerializeField]
        public bool Value { get; set; }
    }
}