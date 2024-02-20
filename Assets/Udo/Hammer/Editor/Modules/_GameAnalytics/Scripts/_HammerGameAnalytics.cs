using System;
using System.Collections.Generic;
using System.IO;
using GameAnalyticsSDK.Setup;
using Udo.Hammer.Editor.Modules._Hammer;
using UnityEditor;
using UnityEngine;

namespace Udo.Hammer.Editor.Modules._GameAnalytics
{
    // ReSharper disable once InconsistentNaming
    [InitializeOnLoad]
    public static class _HammerGameAnalytics
    {
        private const string ZipName = "GameAnalytics";

        private static readonly List<_HammerFileOrFolderDetails> Paths = new()
        {
            new _HammerFileOrFolderDetails(new[] { "Udo", "Hammer", "Editor", "Modules" }, "_" + ZipName, false),
            new _HammerFileOrFolderDetails(new[] { "Udo", "Hammer", "Runtime" }, "_" + ZipName, false),
            new _HammerFileOrFolderDetails(Array.Empty<string>(), ZipName, false),
            new _HammerFileOrFolderDetails(new[] { "Resources" }, ZipName, false),
        };

        static _HammerGameAnalytics()
        {
            if (_HammerModulesHelper.ShouldUpdateRuntime())
            {
                var hammerConfigObject = _HammerModulesHelper.GetRuntimeConfig();

                var settingsGa =
                    AssetDatabase.LoadAssetAtPath<Settings>(Path.Combine("Assets", "Resources", "GameAnalytics",
                        "Settings.asset"));

                for (var index = settingsGa.Platforms.Count; index > 0; index--)
                {
                    settingsGa.RemovePlatformAtIndex(index - 1);
                }

                settingsGa.AddPlatform(RuntimePlatform.Android);
                settingsGa.UpdateGameKey(0, hammerConfigObject.gameanalytics_androidGameKey);
                settingsGa.UpdateSecretKey(0, hammerConfigObject.gameanalytics_androidSecretKey);

                settingsGa.AddPlatform(RuntimePlatform.IPhonePlayer);
                settingsGa.UpdateGameKey(1, hammerConfigObject.gameanalytics_iosGameKey);
                settingsGa.UpdateSecretKey(1, hammerConfigObject.gameanalytics_iosSecretKey);

                settingsGa.UsePlayerSettingsBuildNumber = true;
                settingsGa.SubmitErrors = true;
                settingsGa.SubmitFpsAverage = true;
                settingsGa.SubmitFpsCritical = true;
                settingsGa.NativeErrorReporting = true;
                settingsGa.InfoLogBuild = false;
                settingsGa.InfoLogEditor = false;
                settingsGa.VerboseLogBuild = false;

                EditorUtility.SetDirty(settingsGa);
            }

            _HammerModulesHelper.CreateLibrary(ZipName, Paths);
        }

        [MenuItem("UDO/Danger Zone/Uninstall GameAnalytics")]
        public static void Uninstall()
        {
            _HammerModulesHelper.UninstallSdk(Paths);
        }

        [MenuItem("UDO/Settings/GameAnalytics")]
        public static void SelectGameAnalyticsSettings()
        {
            Selection.activeObject =
                AssetDatabase.LoadAssetAtPath<Settings>(Path.Combine("Assets", "Resources", "GameAnalytics",
                    "Settings.asset"));
        }
    }
}