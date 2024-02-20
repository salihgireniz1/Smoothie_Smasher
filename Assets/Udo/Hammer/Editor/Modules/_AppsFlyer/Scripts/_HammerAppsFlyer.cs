using System;
using System.Collections.Generic;
using Udo.Hammer.Editor.Modules._Hammer;
using UnityEditor;

namespace Udo.Hammer.Editor.Modules._AppsFlyer
{
    // ReSharper disable once InconsistentNaming
    [InitializeOnLoad]
    public static class _HammerAppsFlyer
    {
        private const string ZipName = "AppsFlyer";

        private static readonly List<_HammerFileOrFolderDetails> Paths = new()
        {
            new _HammerFileOrFolderDetails(new[] { "Udo", "Hammer", "Editor", "Modules" }, "_" + ZipName, false),
            new _HammerFileOrFolderDetails(new[] { "Udo", "Hammer", "Runtime" }, "_" + ZipName, false),
            new _HammerFileOrFolderDetails(Array.Empty<string>(), ZipName, false),
            new _HammerFileOrFolderDetails(new[] { "Resources" }, "BillingMode.json", true)
        };

        static _HammerAppsFlyer()
        {
            _HammerModulesHelper.CreateLibrary(ZipName, Paths);
        }

        [MenuItem("UDO/Danger Zone/Uninstall AppsFlyer")]
        public static void Uninstall()
        {
            _HammerModulesHelper.UninstallSdk(Paths);
        }
    }
}