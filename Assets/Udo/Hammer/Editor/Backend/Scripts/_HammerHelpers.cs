using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace UDO.Hammer.Editor.Backend
{
    // ReSharper disable once InconsistentNaming
    internal static class _HammerHelpers
    {
        public static async Task<T> MakeRequest<T>(HttpMethod method, string url, object data = null,
            Dictionary<string, string> headers = null)
        {
            var baseResponse = await MakeRequest(method, url, data, headers);
            return JsonConvert.DeserializeObject<T>(baseResponse.Content);
        }

        private static async Task<_HammerBaseResponse> MakeRequest(HttpMethod method, string url, object data = null,
            Dictionary<string, string> headers = null)
        {
            var builder = new UriBuilder(url);

            var query = HttpUtility.ParseQueryString(builder.Query);
            if (data != null && method == HttpMethod.Get)
                if (data is Dictionary<string, string> reqParams)
                    foreach (var param in reqParams)
                        query[param.Key] = param.Value;

            builder.Query = query.ToString();

            using (var requestMessage = new HttpRequestMessage(method, builder.Uri))
            {
                if (data != null && method != HttpMethod.Get)
                    requestMessage.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8,
                        "application/json");

                if (headers != null)
                    foreach (var kvp in headers)
                        requestMessage.Headers.TryAddWithoutValidation(kvp.Key, kvp.Value);

                using (var client = new HttpClient())
                {
                    using (var response = await client.SendAsync(requestMessage))
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        if (response.IsSuccessStatusCode)
                            return new _HammerBaseResponse { Content = content, ResponseMessage = response };
                        var obj = new _HammerErrorResponse
                        {
                            Content = content,
                            Message = content
                        };
                        throw new _HammerRequestException(response, obj);
                    }
                }
            }
        }

        public static async Task<MemoryStream> DownloadDataAsync(this HttpClient client, Uri uri,
            Dictionary<string, string> headers = null, IProgress<float> progress = null,
            CancellationToken cancellationToken = default)
        {
            var destination = new MemoryStream();
            var message = new HttpRequestMessage(HttpMethod.Get, uri);
            if (headers != null)
                foreach (var header in headers)
                    message.Headers.Add(header.Key, header.Value);

            float GetProgressPercentage(float totalBytes, float currentBytes)
            {
                return totalBytes / currentBytes * 100f;
            }

            using (var response =
                   await client.SendAsync(message, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
            {
                var contentLength = response.Content.Headers.ContentLength;
                using (var download = await response.Content.ReadAsStreamAsync())
                {
                    if (progress is null || !contentLength.HasValue)
                    {
                        await download.CopyToAsync(destination, cancellationToken);
                        return destination;
                    }

                    var progressWrapper = new Progress<long>(totalBytes =>
                        progress.Report(GetProgressPercentage(totalBytes, contentLength.Value)));
                    await download.CopyToAsync(destination, 81920, progressWrapper, cancellationToken);
                }
            }

            return destination;
        }

        private static async Task CopyToAsync(this Stream source, Stream destination, int bufferSize,
            IProgress<long> progress = null, CancellationToken cancellationToken = default)
        {
            if (bufferSize < 0)
                throw new ArgumentOutOfRangeException(nameof(bufferSize));
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (!source.CanRead)
                throw new InvalidOperationException($"'{nameof(source)}' is not readable.");
            if (destination == null)
                throw new ArgumentNullException(nameof(destination));
            if (!destination.CanWrite)
                throw new InvalidOperationException($"'{nameof(destination)}' is not writable.");

            var buffer = new byte[bufferSize];
            long totalBytesRead = 0;
            int bytesRead;
            while ((bytesRead =
                       await source.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false)) != 0)
            {
                await destination.WriteAsync(buffer, 0, bytesRead, cancellationToken).ConfigureAwait(false);
                totalBytesRead += bytesRead;
                progress?.Report(totalBytesRead);
            }
        }
    }
}