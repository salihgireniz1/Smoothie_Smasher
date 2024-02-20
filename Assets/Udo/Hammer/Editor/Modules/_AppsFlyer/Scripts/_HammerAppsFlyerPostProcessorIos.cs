#if UNITY_IOS
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

namespace Udo.Hammer.Editor.Modules._AppsFlyer
{
    // ReSharper disable once InconsistentNaming
    public static class _HammerAppsFlyerPostProcessorIos
    {
        [PostProcessBuild(int.MaxValue - 3)]
        public static void PostProcessPlist(BuildTarget buildTarget, string path)
        {
            var plistPath = Path.Combine(path, "Info.plist");
            var plist = new PlistDocument();
            plist.ReadFromFile(plistPath);

            plist.root.SetString("NSAdvertisingAttributionReportEndpoint", "https://appsflyer-skadnetwork.com/");

            plist.WriteToFile(plistPath);
        }
    }
}
#endif