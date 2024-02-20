using System;
using System.Collections.Generic;
using Udo.Hammer.Editor.Modules._Hammer;
using Unity.Usercentrics;
using UnityEditor;
using Object = UnityEngine.Object;

// ReSharper disable StringLiteralTypo
// ReSharper disable IdentifierTypo
namespace Udo.Hammer.Editor.Modules._Usercentrics
{
    // ReSharper disable once InconsistentNaming
    [InitializeOnLoad]
    public static class _HammerUsercentrics
    {
        private const string ZipName = "Usercentrics";
        private const string GuidPrefabUsercentrics = "cf74ce6dd372d499f9cd1c0a2ed26f72";
        private const string GuidPrefabAtt = "9be3c5746e037411c8f7559bd7c54677";

        private static readonly List<_HammerFileOrFolderDetails> Paths = new()
        {
            new _HammerFileOrFolderDetails(new[] { "Udo", "Hammer", "Editor", "Modules" }, "_" + ZipName, false),
            new _HammerFileOrFolderDetails(new[] { "Udo", "Hammer", "Runtime" }, "_" + ZipName, false),
            new _HammerFileOrFolderDetails(Array.Empty<string>(), ZipName, false),
        };

        static _HammerUsercentrics()
        {
            using (var editingScope =
                   new PrefabUtility.EditPrefabContentsScope(AssetDatabase.GUIDToAssetPath(GuidPrefabUsercentrics)))
            {
                if (_HammerModulesHelper.ShouldUpdateRuntime())
                {
                    var hammerConfigObject = _HammerModulesHelper.GetRuntimeConfig();
                    var prefabRoot = editingScope.prefabContentsRoot;
                    var usercentrics = prefabRoot.GetComponent<Usercentrics>();
                    if (hammerConfigObject.usercentrics_isGeoRuleset)
                    {
                        usercentrics.GeoRuleSetID = hammerConfigObject.usercentrics_settingsId;                        
                    }
                    else
                    {
                        usercentrics.SettingsID = hammerConfigObject.usercentrics_settingsId;                        
                    }
                    usercentrics.Options.TimeoutMillis = 30000;
                    usercentrics.Options.DebugMode = hammerConfigObject.usercentrics_debugMode;
                    usercentrics.Options.ConsentMediation = false;

                    using (var editingScope2 =
                           new PrefabUtility.EditPrefabContentsScope(
                               AssetDatabase.GUIDToAssetPath(GuidPrefabAtt)))
                    {
                        var prefabRoot2 = editingScope2.prefabContentsRoot;
                        var att = prefabRoot2.GetComponent<AppTrackingTransparency>();
                        att.IsATTEnabled = true;
                        att.PopupMessage = "Pressing \\\"Allow\\\" uses device info for more relevant ad content";
                    }

                    var autoInitialize = prefabRoot.GetComponent<AutoInitialize>();
                    autoInitialize.Enabled = false;
                }

                _HammerModulesHelper.CreateLibrary(ZipName, Paths);
            }
        }

        [MenuItem("UDO/Danger Zone/Uninstall Usercentrics")]
        public static void Uninstall()
        {
            _HammerModulesHelper.UninstallSdk(Paths);
        }

        [MenuItem("UDO/Settings/Usercentrics")]
        public static void SelectUsercentricsSettings()
        {
            Selection.activeObject =
                AssetDatabase.LoadAssetAtPath<Object>(
                    AssetDatabase.GUIDToAssetPath(GuidPrefabUsercentrics));
        }
    }
}