using System;
using UnityEngine;

namespace Udo.Hammer.Runtime.Core
{
    public abstract class HammerSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static readonly Lazy<T> LazyInstance = new(CreateSingleton);

        public static T Instance => LazyInstance.Value;

        private void Awake()
        {
            AwakeImpl();
        }

        private static T CreateSingleton()
        {
            var ownerObject = new GameObject($"{typeof(T).Name} (singleton)");
            var instance = ownerObject.AddComponent<T>();
            DontDestroyOnLoad(ownerObject);
            return instance;
        }

        protected abstract void AwakeImpl();
    }
}