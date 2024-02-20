using System;
using System.Collections.Generic;
using System.IO;
using Udo.Hammer.Editor.Modules._Hammer;
using UnityEditor;

namespace Udo.Hammer.Editor.Modules._MaxSdk
{
    // ReSharper disable once InconsistentNaming
    [InitializeOnLoad]
    public static class _HammerMaxSdk
    {
        private const string ZipName = "MaxSdk";

        private static readonly List<_HammerFileOrFolderDetails> Paths = new()
        {
            new _HammerFileOrFolderDetails(new[] { "Udo", "Hammer", "Editor", "Modules" }, "_" + ZipName, false),
            new _HammerFileOrFolderDetails(new[] { "Udo", "Hammer", "Runtime" }, "_" + ZipName, false),
            new _HammerFileOrFolderDetails(Array.Empty<string>(), ZipName, false),
        };

        static _HammerMaxSdk()
        {
            if (_HammerModulesHelper.ShouldUpdateRuntime())
            {
                var hammerConfigObject = _HammerModulesHelper.GetRuntimeConfig();

                var applovinSettings =
                    AssetDatabase.LoadAssetAtPath<AppLovinSettings>(Path.Combine("Assets", "MaxSdk", "Resources",
                        "AppLovinSettings.asset"));
                applovinSettings.SdkKey = hammerConfigObject.maxsdk_applovinSdkKey;
                applovinSettings.SetAttributionReportEndpoint = false;
                applovinSettings.ConsentFlowEnabled = false;
                applovinSettings.QualityServiceEnabled = true;
                applovinSettings.AdMobAndroidAppId = hammerConfigObject.maxsdk_admobAppIdAndroid;
                applovinSettings.AdMobIosAppId = hammerConfigObject.maxsdk_admobAppIdIos;
                EditorUtility.SetDirty(applovinSettings);
            }

            _HammerModulesHelper.CreateLibrary(ZipName, Paths);
        }

        [MenuItem("UDO/Danger Zone/Uninstall MaxSdk")]
        public static void Uninstall()
        {
            _HammerModulesHelper.UninstallSdk(Paths);
        }

        [MenuItem("UDO/Settings/AppLovinSettings")]
        public static void SelectIronSourceMediationSettings()
        {
            Selection.activeObject =
                AssetDatabase.LoadAssetAtPath<AppLovinSettings>(Path.Combine("Assets", "MaxSdk", "Resources",
                    "AppLovinSettings.asset"));
        }
    }
}