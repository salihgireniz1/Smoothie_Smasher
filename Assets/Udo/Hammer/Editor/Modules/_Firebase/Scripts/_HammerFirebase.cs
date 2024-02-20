using System;
using System.Collections.Generic;
using System.IO;
using Udo.Hammer.Editor.Modules._Hammer;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Udo.Hammer.Editor.Modules._Firebase
{
    // ReSharper disable once InconsistentNaming
    [InitializeOnLoad]
    public static class _HammerFirebase
    {
        private const string ZipName = "Firebase";

        private static readonly List<_HammerFileOrFolderDetails> Paths = new()
        {
            new _HammerFileOrFolderDetails(new[] { "Udo", "Hammer", "Editor", "Modules" }, "_" + ZipName, false),
            new _HammerFileOrFolderDetails(new[] { "Udo", "Hammer", "Runtime" }, "_" + ZipName, false),
            new _HammerFileOrFolderDetails(Array.Empty<string>(), ZipName, false),
            new _HammerFileOrFolderDetails(new[] { "Editor Default Resources" }, ZipName, false),
            new _HammerFileOrFolderDetails(new[] { "Plugins", "iOS" }, ZipName, false),
            new _HammerFileOrFolderDetails(new[] { "Plugins", "tvOS" }, ZipName, false),
            new _HammerFileOrFolderDetails(new[] { "Plugins", "Android" }, "FirebaseApp.androidlib", false),
            new _HammerFileOrFolderDetails(new[] { "Plugins", "Android" }, "FirebaseCrashlytics.androidlib", false),
            new _HammerFileOrFolderDetails(new[] { "GeneratedLocalRepo" }, ZipName, false),
            new _HammerFileOrFolderDetails(Array.Empty<string>(), "google-services.json", true),
            new _HammerFileOrFolderDetails(Array.Empty<string>(), "GoogleService-Info.plist", true),
        };

        static _HammerFirebase()
        {
            if (_HammerModulesHelper.ShouldUpdateRuntime())
            {
                var hammerConfigObject = _HammerModulesHelper.GetRuntimeConfig();

                _HammerModulesHelper.CreateFile(Path.Combine("Assets", "GoogleService-Info.plist"),
                    hammerConfigObject.firebase_googleServicesPlist);
                _HammerModulesHelper.CreateFile(Path.Combine("Assets", "google-services.json"),
                    hammerConfigObject.firebase_googleServicesJson);
            }

            _HammerModulesHelper.CreateLibrary(ZipName, Paths);
        }

        [MenuItem("UDO/Danger Zone/Uninstall Firebase")]
        public static void Uninstall()
        {
            _HammerModulesHelper.UninstallSdk(Paths);
        }

        [MenuItem("UDO/Settings/FirebaseIos")]
        public static void SelectFirebaseIosSettings()
        {
            Selection.activeObject =
                AssetDatabase.LoadAssetAtPath<Object>(Path.Combine("Assets", "GoogleService-Info.plist"));
        }

        [MenuItem("UDO/Settings/FirebaseAndroid")]
        public static void SelectFirebaseAndroidSettings()
        {
            Selection.activeObject =
                AssetDatabase.LoadAssetAtPath<TextAsset>(Path.Combine("Assets", "google-services.json"));
        }
    }
}