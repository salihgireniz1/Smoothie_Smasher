using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UDO.Hammer.Editor.Backend.Model.Custom;
using Udo.Hammer.Editor.Backend.UI;
using UnityEditor;

namespace UDO.Hammer.Editor.Backend
{
    // ReSharper disable once InconsistentNaming
    public class _HammerClient
    {
        private const string ApiUrl = "https://fwmsrhwaqdewjyyoulok.supabase.co";

        private const string ApiKey =
            "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImZ3bXNyaHdhcWRld2p5eW91bG9rIiwicm9sZSI6ImFub24iLCJpYXQiOjE2NjYzNTgzNzIsImV4cCI6MTk4MTkzNDM3Mn0.PubYw_4E_rpivUKWIfUAxSl_zCgC8vZwfOG-KOwYiI0";

        private const string BucketName = "udohammer-sdks";

        private const string FileNameConfigObject = "_HammerConfig.asset";

        private static readonly string[] PathsToCombineRootFolder =
        {
            "Assets", "Udo", "Hammer", "Editor", "Backend", "Scripts", "UI"
        };

        private static _HammerClient _instance;
        private string _authUrl;
        private _HammerBucket _bucketSdks;
        private _HammerEditorWindowConfigObject _configObject;

        private _HammerSession _currentSession;
        private _HammerUser _currentUser;
        private string _functionsUrl;

        private _HammerClientOptions _options;
        private string _realtimeUrl;
        private string _restUrl;
        private string _schema;
        private string _storageUrl;

        private static _HammerClient Instance
        {
            get
            {
                if (_instance != null) return _instance;

                _instance = new _HammerClient
                {
                    _options = new _HammerClientOptions(),
                    _authUrl = string.Format(_HammerClientOptions.AuthUrlFormat, ApiUrl),
                    _restUrl = string.Format(_HammerClientOptions.RestUrlFormat, ApiUrl),
                    _realtimeUrl = string.Format(_HammerClientOptions.RealtimeUrlFormat, ApiUrl).Replace("http", "ws"),
                    _storageUrl = string.Format(_HammerClientOptions.StorageUrlFormat, ApiUrl),
                    _schema = _HammerClientOptions.Schema
                };

                var isPlatform = new Regex(@"(supabase\.co)|(supabase\.in)").Match(ApiUrl);
                if (isPlatform.Success)
                {
                    var parts = ApiUrl.Split('.');
                    _instance._functionsUrl = $"{parts[0]}.functions.{parts[1]}.{parts[2]}";
                }
                else
                {
                    _instance._functionsUrl = string.Format(_HammerClientOptions.FunctionsUrlFormat, ApiUrl);
                }

                _instance._options.Headers.Add("apiKey", ApiKey);
                _instance._options.Headers.Add("Authorization", $"Bearer {ApiKey}");

                var rootFolderPath = Path.Combine(PathsToCombineRootFolder);
                var pathHammerEditorConfigObject = Path.Combine(rootFolderPath, FileNameConfigObject);
                _instance._configObject =
                    AssetDatabase.LoadAssetAtPath<_HammerEditorWindowConfigObject>(pathHammerEditorConfigObject);

                return _instance;
            }
        }

        public static async Task<_HammerUser> SignInWithEmail(string email = null, string password = null)
        {
            if (IsLoggedIn()) return Instance._currentUser;

            email ??= Instance._configObject.email;
            if (email == null) throw new ArgumentNullException(nameof(email));

            password ??= Instance._configObject.password;
            if (password == null) throw new ArgumentNullException(nameof(password));

            var body = new Dictionary<string, object> { { "email", email }, { "password", password } };
            Instance._currentSession = await _HammerHelpers.MakeRequest<_HammerSession>(HttpMethod.Post,
                $"{Instance._authUrl}/token?grant_type=password", body, Instance._options.Headers);
            if (Instance._currentSession != null)
                Instance._options.Headers["Authorization"] = $"Bearer {Instance._currentSession.AccessToken}";

            Instance._currentUser = Instance._currentSession?.User;
            return Instance._currentUser;
        }

        private static bool IsLoggedIn()
        {
            return Instance._currentSession != null && Instance._currentUser != null &&
                   Instance._currentSession.ExpiresIn > 0;
        }

        public static async Task<string> DownloadSdk(string remotePath, string localPath,
            EventHandler<float> onProgress = null)
        {
            await SignInWithEmail();
            await GetSdkBucket();

            using (var client = new HttpClient())
            {
                var uri = new Uri($"{Instance._storageUrl}/object/{Instance._bucketSdks.Id}/{remotePath}");
                var progress = new Progress<float>();
                if (onProgress != null) progress.ProgressChanged += onProgress;

                var stream = await client.DownloadDataAsync(uri, Instance._options.Headers, progress);
                await using (var fileStream = new FileStream(localPath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    stream.WriteTo(fileStream);
                }

                return localPath;
            }
        }

        private static async Task<_HammerBucket> GetSdkBucket()
        {
            await SignInWithEmail();

            var buckets = await ListBuckets();
            foreach (var bucket in buckets)
                if (bucket.Name.Equals(BucketName))
                    Instance._bucketSdks = bucket;

            return Instance._bucketSdks;
        }

        private static async Task<List<_HammerBucket>> ListBuckets()
        {
            await SignInWithEmail();

            var result = await _HammerHelpers.MakeRequest<List<_HammerBucket>>(HttpMethod.Get,
                $"{Instance._storageUrl}/bucket", null, Instance._options.Headers);
            return result;
        }

        public static async Task<List<_HammerGameSdkVersionKeyServerEnriched>> GetKeys(string gameId = null)
        {
            await SignInWithEmail();

            gameId ??= Instance._configObject.gameId;
            if (gameId == null) throw new ArgumentNullException(nameof(gameId));

            var result = await _HammerHelpers.MakeRequest<List<_HammerGameSdkVersionKeyServerEnriched>>(HttpMethod.Get,
                $"{Instance._restUrl}/games_sdks_versions_keys_server?game_uuid=eq.{gameId}&active=eq.true&select=*,sdks_versions_keys(*),sdks(*),sdks_versions(*)",
                null,
                Instance._options.Headers);
            return result;
        }

        public static async Task<List<_HammerGameSdkVersionEnriched>> GetSdks(string gameId = null)
        {
            await SignInWithEmail();

            gameId ??= Instance._configObject.gameId;
            if (gameId == null) throw new ArgumentNullException(nameof(gameId));

            var result = await _HammerHelpers.MakeRequest<List<_HammerGameSdkVersionEnriched>>(HttpMethod.Get,
                $"{Instance._restUrl}/games_sdks_versions?game_uuid=eq.{gameId}&active=eq.true&select=*,sdks(*),sdks_versions(*)",
                null,
                Instance._options.Headers);
            return result;
        }
    }
}