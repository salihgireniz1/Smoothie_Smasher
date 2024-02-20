using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UDO.Hammer.Editor.Backend;
using Udo.Hammer.Runtime.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Udo.Hammer.Editor.Backend.UI
{
    // ReSharper disable once InconsistentNaming
    public class _HammerEditorWindow : EditorWindow
    {
        private const string FileNameStylesheet = "_HammerEditorWindow.uss";
        private const string FileNameVisualTreeAsset = "_HammerEditorWindow.uxml";
        private const string FileNameConfigObject = "_HammerConfig.asset";

        private static readonly string[] PathsToCombineRootFolder =
        {
            "Assets", "Udo", "Hammer", "Editor", "Backend", "Scripts", "UI"
        };

        private static readonly string[] PathsToCombineHammerConfig =
        {
            "Assets", "Udo", "Hammer", "Runtime", "Core", "HammerConfig.asset"
        };

        private _HammerEditorWindowConfigObject _configObject;

        private readonly bool _debug = false;
        private StyleSheet _styleSheet;
        private Button _udoButtonSyncConfig;
        private Button _udoButtonSyncFile;

        private TextField _udoTextEmail;
        private TextField _udoTextGameId;
        private TextField _udoTextPassword;
        private Toggle _udoToggleCreateZips;
        private VisualTreeAsset _visualTreeAsset;

        public void CreateGUI()
        {
            Initialize();
        }

        [MenuItem("UDO/Hammer &#1")]
        public static void ShowWindow()
        {
            var window = GetWindow<_HammerEditorWindow>();
            window.titleContent = new GUIContent("UDO Hammer");
            var size = new Vector2(450, 150);
            window.minSize = size;
            window.maxSize = size;
            window.maximized = true;
            window.Show();
        }

        private void Initialize()
        {
            var rootFolderPath = Path.Combine(PathsToCombineRootFolder);

            var pathStyleSheet = Path.Combine(rootFolderPath, FileNameStylesheet);
            _styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(pathStyleSheet);
            rootVisualElement.styleSheets.Add(_styleSheet);

            var pathVisualTreeAsset = Path.Combine(rootFolderPath, FileNameVisualTreeAsset);
            _visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(pathVisualTreeAsset);
            _visualTreeAsset.CloneTree(rootVisualElement);

            var pathHammerEditorConfigObject = Path.Combine(rootFolderPath, FileNameConfigObject);
            _configObject =
                AssetDatabase.LoadAssetAtPath<_HammerEditorWindowConfigObject>(pathHammerEditorConfigObject);

            _udoTextGameId = rootVisualElement.Q<TextField>("udo-text-gameId");
            _udoTextGameId.value = _configObject.gameId;
            _udoTextGameId.RegisterValueChangedCallback(e => { _configObject.gameId = e.newValue; });

            _udoButtonSyncFile = rootVisualElement.Q<Button>("udo-button-syncFile");
            _udoButtonSyncFile.clicked += UdoButtonSyncFileOnClicked;

            _udoButtonSyncConfig = rootVisualElement.Q<Button>("udo-button-syncConfig");
            _udoButtonSyncConfig.clicked += UdoButtonSyncConfigOnClicked;

            _udoToggleCreateZips = rootVisualElement.Q<Toggle>("udo-toggle-createZips");
            _udoToggleCreateZips.value = _configObject.createZips;
            _udoToggleCreateZips.RegisterValueChangedCallback(e => { _configObject.createZips = e.newValue; });
            _udoToggleCreateZips.visible = _debug;

            _udoTextEmail = rootVisualElement.Q<TextField>("udo-text-email");
            _udoTextEmail.value = _configObject.email;
            _udoTextEmail.RegisterValueChangedCallback(e => { _configObject.email = e.newValue; });
            _udoTextEmail.visible = _debug;

            _udoTextPassword = rootVisualElement.Q<TextField>("udo-text-password");
            _udoTextPassword.value = _configObject.password;
            _udoTextPassword.RegisterValueChangedCallback(e => { _configObject.password = e.newValue; });
            _udoTextPassword.visible = _debug;
        }

        private static void CreateModulesFolder()
        {
            var rootFolder = Path.GetDirectoryName(Application.dataPath);
            var hammerFolder = Path.Combine(rootFolder, "Hammer");
            if (!Directory.Exists(hammerFolder))
            {
                Directory.CreateDirectory(hammerFolder);
            }
            var gitignoreFile = Path.Combine(hammerFolder, ".gitignore");
            if (!File.Exists(gitignoreFile))
            {
                File.WriteAllText(gitignoreFile, "*\n!.gitignore");
            }
            var tmpFolder = Path.Combine(hammerFolder, "tmp");
            if (!Directory.Exists(tmpFolder))
            {
                Directory.CreateDirectory(tmpFolder);
            }
            else
            {
                Directory.Delete(tmpFolder, true);
                Directory.CreateDirectory(tmpFolder);
            }
        }
        
        public static void MoveDirectory(string source, string target)
        {
            var sourcePath = source.TrimEnd('\\', ' ');
            var targetPath = target.TrimEnd('\\', ' ');
            var files = Directory.EnumerateFiles(sourcePath, "*", SearchOption.AllDirectories)
                .GroupBy(s=> Path.GetDirectoryName(s));
            foreach (var folder in files)
            {
                var targetFolder = folder.Key.Replace(sourcePath, targetPath);
                Directory.CreateDirectory(targetFolder);
                foreach (var file in folder)
                {
                    var targetFile = Path.Combine(targetFolder, Path.GetFileName(file));
                    if (File.Exists(targetFile)) File.Delete(targetFile);
                    File.Move(file, targetFile);
                }
            }
            Directory.Delete(source, true);
        }
        
        private async void UdoButtonSyncFileOnClicked()
        {
            CreateModulesFolder();
            
            try
            {
                _udoButtonSyncFile.SetEnabled(false);
                _udoButtonSyncConfig.SetEnabled(false);
            
                var gameId = "" + _configObject.gameId;
                var createZips = _configObject.createZips;
                var password = "" + _configObject.password;
                var email = "" + _configObject.email;
                var shouldUpdateRuntime = _configObject.shouldUpdateRuntime;
            
                var rootFolder = Path.GetDirectoryName(Application.dataPath);
                var outputFolder = Path.Combine(rootFolder, "Hammer", "tmp");
                var sdks = await _HammerClient.GetSdks();
                foreach (var sdk in sdks)
                {
                    Debug.Log("Download Start: " + sdk.Sdk.SdkName);
                    var url = sdk.SdkUuid + "/" + sdk.SdkVersionUuid + "/" + sdk.Sdk.SdkName + ".zip";
                    var localPath = Path.Combine(outputFolder, sdk.Sdk.SdkName + ".zip");
                    var path = await _HammerClient.DownloadSdk(url, localPath, (_, f) => { Debug.Log(f); });
                    path = path.Replace("\\", "/");
                    Debug.Log("Download Complete: " + path);
                    ZipFile.ExtractToDirectory(path, outputFolder, true);
                    Debug.Log("Extracted From Zip");
                    File.Delete(localPath);
                }

                MoveDirectory(outputFolder, Application.dataPath);
                _configObject.gameId = gameId;
                _configObject.createZips = createZips;
                _configObject.password = password;
                _configObject.email = email;
                _configObject.shouldUpdateRuntime = shouldUpdateRuntime;
                _configObject.SetTimeSyncFile(DateTime.Now);

                _udoTextGameId.value = gameId;
            }
            finally
            {
                EditorUtility.SetDirty(_configObject);
                AssetDatabase.Refresh();
                EditorUtility.RequestScriptReload();
                _udoButtonSyncFile.SetEnabled(true);
                _udoButtonSyncConfig.SetEnabled(true);
            }
        }

        private async void UdoButtonSyncConfigOnClicked()
        {
            try
            {
                _udoButtonSyncFile.SetEnabled(false);
                _udoButtonSyncConfig.SetEnabled(false);

                var path = Path.Combine(PathsToCombineHammerConfig);
                var hammerConfigObject = AssetDatabase.LoadAssetAtPath<HammerConfigObject>(path);
                var hammerConfig = hammerConfigObject.ToHammerConfig();
                var jsonClient = JsonConvert.SerializeObject(hammerConfig);
                var result = JsonConvert.DeserializeObject<JToken>(jsonClient);
                if (result != null)
                {
                    var keys = await _HammerClient.GetKeys();

                    foreach (var key in keys) result[key.SdkVersionKey.SdkKey] = key.ServerValue;

                    hammerConfig = result.ToObject<HammerConfig>();
                    hammerConfigObject.FromHammerConfig(hammerConfig);
                }

                _configObject.SetTimeSyncConfig(DateTime.Now);
            }
            finally
            {
                _configObject.shouldUpdateRuntime = true;
                _configObject.createZips = true;
                EditorUtility.SetDirty(_configObject);
                AssetDatabase.Refresh();
                EditorUtility.RequestScriptReload();
                _udoButtonSyncFile.SetEnabled(true);
                _udoButtonSyncConfig.SetEnabled(true);
            }
        }
    }
}