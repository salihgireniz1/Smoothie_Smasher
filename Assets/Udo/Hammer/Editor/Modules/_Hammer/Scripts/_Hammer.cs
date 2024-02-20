using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.SceneManagement;
using Object = UnityEngine.Object;

namespace Udo.Hammer.Editor.Modules._Hammer
{
    // ReSharper disable once InconsistentNaming
    [InitializeOnLoad]
    public static class _Hammer
    {
        private const string ZipName = "Hammer";
        private const string ZipNameAnalytics = "Hammerlytics";

        private static readonly List<_HammerFileOrFolderDetails> Paths = new()
        {
            new _HammerFileOrFolderDetails(new[] { "Udo", "Hammer", "Editor", "Modules" }, "_" + ZipName, false),
            new _HammerFileOrFolderDetails(new[] { "Udo", "Hammer", "Editor", "Modules" }, "_" + ZipNameAnalytics, false),
            new _HammerFileOrFolderDetails(new[] { "Udo", "Hammer", "Editor" }, "Backend", false),
            new _HammerFileOrFolderDetails(new[] { "Udo", "Hammer", "Runtime" }, "Core", false),
            new _HammerFileOrFolderDetails(new[] { "Udo", "Hammer", "Runtime" }, "_" + ZipNameAnalytics, false),
            new _HammerFileOrFolderDetails(new[] { "Plugins", "Android" }, "AndroidManifest.xml", true),
            new _HammerFileOrFolderDetails(new[] { "Plugins", "Android" }, "gradleTemplate.properties", true),
            new _HammerFileOrFolderDetails(new[] { "Plugins", "Android" }, "mainTemplate.gradle", true),
            new _HammerFileOrFolderDetails(new[] { "Plugins", "Android" }, "HammerSdkDependencies.xml", true),
            new _HammerFileOrFolderDetails(new[] { "Plugins", "Android" }, "hammer.androidlib", false),
        };

        static _Hammer()
        {
            // if (_HammerModulesHelper.ShouldUpdateRuntime())
            // {
            // }

            _HammerModulesHelper.CreateLibrary(ZipName, Paths);

            ResetEditorSettings();
        }

        private static async void ResetEditorSettings()
        {
            await Task.Delay(5000);
            _HammerModulesHelper.GetEditorConfig().createZips = false;
            _HammerModulesHelper.GetEditorConfig().shouldUpdateRuntime = false;
            EditorUtility.SetDirty(_HammerModulesHelper.GetEditorConfig());
            AssetDatabase.Refresh();
        }

        [MenuItem("UDO/Settings/HammerEditor")]
        public static void SelectHammerEditor()
        {
            Selection.activeObject = AssetDatabase.LoadAssetAtPath<Object>(Path.Combine("Assets", "Udo", "Hammer",
                "Editor", "Backend", "Scripts", "UI", "_HammerConfig.asset"));
        }

        [MenuItem("UDO/Settings/HammerRuntime")]
        public static void SelectHammerRuntime()
        {
            Selection.activeObject = _HammerModulesHelper.GetRuntimeConfig();
        }

        [MenuItem("UDO/Settings/AndroidManifest.xml")]
        public static void SelectAndroidManifest()
        {
            Selection.activeObject =
                AssetDatabase.LoadAssetAtPath<Object>(Path.Combine("Assets", "Plugins", "Android",
                    "AndroidManifest.xml"));
        }

        [MenuItem("UDO/HammerBootstrapScene")]
        public static void SelectHammerBootstrapScene()
        {
            Selection.activeObject =
                AssetDatabase.LoadAssetAtPath<Object>(
                    AssetDatabase.GUIDToAssetPath("ff0508853489c444fa76314521e60801"));
            var shouldOpen = EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            if (shouldOpen)
            {
                EditorSceneManager.OpenScene(AssetDatabase.GUIDToAssetPath("ff0508853489c444fa76314521e60801"));
            }
        }

        [MenuItem("UDO/Danger Zone/Uninstall Hammer SDK Completely")]
        public static void UninstallHammerSdk()
        {
            var assembly = Assembly.GetAssembly(typeof(_Hammer));
            Type type;
            MethodInfo methodInfo;

            try
            {
                type = assembly.GetType("Udo.Hammer.Editor.Modules._Adjust._HammerAdjust");
                methodInfo = type.GetMethod("Uninstall", BindingFlags.Static | BindingFlags.Public);
                methodInfo?.Invoke(null, Array.Empty<object>());
            }
            catch (Exception)
            {
                // ignored
            }

            try
            {
                type = assembly.GetType("Udo.Hammer.Editor.Modules._AppsFlyer._HammerAppsFlyer");
                methodInfo = type.GetMethod("Uninstall", BindingFlags.Static | BindingFlags.Public);
                methodInfo?.Invoke(null, Array.Empty<object>());
            }
            catch (Exception)
            {
                // ignored
            }

            try
            {
                type = assembly.GetType("Udo.Hammer.Editor.Modules._DevToDev._HammerDevToDev");
                methodInfo = type.GetMethod("Uninstall", BindingFlags.Static | BindingFlags.Public);
                methodInfo?.Invoke(null, Array.Empty<object>());
            }
            catch (Exception)
            {
                // ignored
            }

            try
            {
                type = assembly.GetType(
                    "Udo.Hammer.Editor.Modules._ExternalDependencyManager._HammerExternalDependencyManager");
                methodInfo = type.GetMethod("Uninstall", BindingFlags.Static | BindingFlags.Public);
                methodInfo?.Invoke(null, Array.Empty<object>());
            }
            catch (Exception)
            {
                // ignored
            }

            try
            {
                type = assembly.GetType("Udo.Hammer.Editor.Modules._FacebookSDK._HammerFacebookSDK");
                methodInfo = type.GetMethod("Uninstall", BindingFlags.Static | BindingFlags.Public);
                methodInfo?.Invoke(null, Array.Empty<object>());
            }
            catch (Exception)
            {
                // ignored
            }

            try
            {
                type = assembly.GetType("Udo.Hammer.Editor.Modules._Firebase._HammerFirebase");
                methodInfo = type.GetMethod("Uninstall", BindingFlags.Static | BindingFlags.Public);
                methodInfo?.Invoke(null, Array.Empty<object>());
            }
            catch (Exception)
            {
                // ignored
            }

            try
            {
                type = assembly.GetType("Udo.Hammer.Editor.Modules._GameAnalytics._HammerGameAnalytics");
                methodInfo = type.GetMethod("Uninstall", BindingFlags.Static | BindingFlags.Public);
                methodInfo?.Invoke(null, Array.Empty<object>());
            }
            catch (Exception)
            {
                // ignored
            }

            try
            {
                type = assembly.GetType("Udo.Hammer.Editor.Modules._IronSource._HammerIronSource");
                methodInfo = type.GetMethod("Uninstall", BindingFlags.Static | BindingFlags.Public);
                methodInfo?.Invoke(null, Array.Empty<object>());
            }
            catch (Exception)
            {
                // ignored
            }

            try
            {
                type = assembly.GetType("Udo.Hammer.Editor.Modules._MaxSdk._HammerMaxSdk");
                methodInfo = type.GetMethod("Uninstall", BindingFlags.Static | BindingFlags.Public);
                methodInfo?.Invoke(null, Array.Empty<object>());
            }
            catch (Exception)
            {
                // ignored
            }

            try
            {
                type = assembly.GetType("Udo.Hammer.Editor.Modules._Usercentrics._HammerUsercentrics");
                methodInfo = type.GetMethod("Uninstall", BindingFlags.Static | BindingFlags.Public);
                methodInfo?.Invoke(null, Array.Empty<object>());
            }
            catch (Exception)
            {
                // ignored
            }

            _HammerModulesHelper.UninstallSdk(Paths);
        }
    }
}