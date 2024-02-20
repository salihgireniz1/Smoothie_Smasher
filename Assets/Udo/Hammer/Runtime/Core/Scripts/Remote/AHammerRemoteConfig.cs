using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Udo.Hammer.Runtime.Core
{
    [Serializable]
    public class AHammerRemoteConfig
    {
        public AHammerRemoteConfig(string key)
        {
            Key = key;
        }

        [JsonProperty("key")]
        [field: SerializeField]
        public string Key { get; set; }
    }
}