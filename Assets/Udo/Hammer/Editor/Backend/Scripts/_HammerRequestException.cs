using System;
using System.Net.Http;

namespace UDO.Hammer.Editor.Backend
{
    // ReSharper disable once InconsistentNaming
    public class _HammerRequestException : Exception
    {
        public _HammerRequestException(HttpResponseMessage response, _HammerErrorResponse error) : base(error.Message)
        {
            Response = response;
            Error = error;
        }

        public HttpResponseMessage Response { get; }
        public _HammerErrorResponse Error { get; }
    }
}