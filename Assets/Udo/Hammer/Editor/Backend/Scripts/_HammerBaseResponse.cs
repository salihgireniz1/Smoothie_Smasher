using System.Net.Http;
using Newtonsoft.Json;

namespace UDO.Hammer.Editor.Backend
{
    // ReSharper disable once InconsistentNaming
    public class _HammerBaseResponse
    {
        [JsonIgnore] public HttpResponseMessage ResponseMessage { get; set; }
        [JsonIgnore] public string Content { get; set; }
    }
}