using System;
using System.Collections.Generic;
using System.IO;
using Facebook.Unity.Editor;
using Facebook.Unity.Settings;
using Udo.Hammer.Editor.Modules._Hammer;
using UnityEditor;
using UnityEngine;

namespace Udo.Hammer.Editor.Modules._FacebookSDK
{
    // ReSharper disable once InconsistentNaming
    [InitializeOnLoad]
    public static class _HammerFacebookSDK
    {
        private const string ZipName = "FacebookSDK";

        private static readonly List<_HammerFileOrFolderDetails> Paths = new()
        {
            new _HammerFileOrFolderDetails(new[] { "Udo", "Hammer", "Editor", "Modules" }, "_" + ZipName, false),
            new _HammerFileOrFolderDetails(new[] { "Udo", "Hammer", "Runtime" }, "_" + ZipName, false),
            new _HammerFileOrFolderDetails(Array.Empty<string>(), ZipName, false),
            new _HammerFileOrFolderDetails(new[] { "Editor" }, "DisableBitcode.cs", true),
            new _HammerFileOrFolderDetails(new[] { "StreamingAssets" }, "meta.mp4", true),
            new _HammerFileOrFolderDetails(new[] { "StreamingAssets" }, "meta-logo.png", true),
        };

        static _HammerFacebookSDK()
        {
            if (_HammerModulesHelper.ShouldUpdateRuntime())
            {
                var hammerConfigObject = _HammerModulesHelper.GetRuntimeConfig();

                if ((UnityEngine.Object) FacebookSettings.NullableInstance == (UnityEngine.Object) null)
                {
                    var instance = ScriptableObject.CreateInstance<FacebookSettings>();
                    var path1 = Path.Combine(Application.dataPath, "FacebookSDK/SDK/Resources");
                    if (!Directory.Exists(path1))
                        Directory.CreateDirectory(path1);
                    var path2 = Path.Combine(Path.Combine("Assets", "FacebookSDK/SDK/Resources"), "FacebookSettings.asset");
                    AssetDatabase.CreateAsset((UnityEngine.Object) instance, path2);
                }
                
                FacebookSettings.AppIds[0] = hammerConfigObject.facebook_appId;
                FacebookSettings.AppLabels[0] = hammerConfigObject.facebook_appName;
                FacebookSettings.ClientTokens[0] = hammerConfigObject.facebook_clientToken;
                EditorUtility.SetDirty((UnityEngine.Object) FacebookSettings.Instance);
                AssetDatabase.Refresh();
                ManifestMod.GenerateManifest();
            }

            _HammerModulesHelper.CreateLibrary(ZipName, Paths);
        }

        [MenuItem("UDO/Danger Zone/Uninstall FacebookSDK")]
        public static void Uninstall()
        {
            _HammerModulesHelper.UninstallSdk(Paths);
        }

        [MenuItem("UDO/Settings/Facebook")]
        public static void SelectFacebookSettings()
        {
            Selection.activeObject = (UnityEngine.Object) FacebookSettings.Instance;
        }
    }
}