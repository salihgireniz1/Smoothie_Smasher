using System;
using System.Collections.Generic;
using Udo.Hammer.Editor.Modules._Hammer;
using UnityEditor;

namespace Udo.Hammer.Editor.Modules._ExternalDependencyManager
{
    // ReSharper disable once InconsistentNaming
    [InitializeOnLoad]
    public static class _HammerExternalDependencyManager
    {
        private const string ZipName = "ExternalDependencyManager";

        private static readonly List<_HammerFileOrFolderDetails> Paths = new()
        {
            new _HammerFileOrFolderDetails(new[] { "Udo", "Hammer", "Editor", "Modules" }, "_" + ZipName, false),
            new _HammerFileOrFolderDetails(Array.Empty<string>(), ZipName, false),
        };

        static _HammerExternalDependencyManager()
        {
            // if (_HammerModulesHelper.ShouldUpdateRuntime())
            // {
            // }

            _HammerModulesHelper.CreateLibrary(ZipName, Paths);
        }

        [MenuItem("UDO/Danger Zone/Uninstall ExternalDependencyManager")]
        public static void Uninstall()
        {
            _HammerModulesHelper.UninstallSdk(Paths);
        }
    }
}