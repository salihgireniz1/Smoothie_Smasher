using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Udo.Hammer.Runtime.Core
{
    [Serializable]
    public class HammerRemoteConfigInteger : AHammerRemoteConfig
    {
        public HammerRemoteConfigInteger(string key, int value) : base(key)
        {
            Value = value;
        }

        [JsonProperty("value")]
        [field: SerializeField]
        public int Value { get; set; }
    }
}