using System;
using System.Collections.Generic;

namespace UDO.Hammer.Editor.Backend
{
    // ReSharper disable once InconsistentNaming
    [Serializable]
    public class _HammerClientOptions
    {
        public const string Schema = "public";
        public const string AuthUrlFormat = "{0}/auth/v1";
        public const string RestUrlFormat = "{0}/rest/v1";
        public const string RealtimeUrlFormat = "{0}/realtime/v1";
        public const string StorageUrlFormat = "{0}/storage/v1";
        public const string FunctionsUrlFormat = "{0}/functions/v1";
        public readonly Dictionary<string, string> Headers = new();
    }
}