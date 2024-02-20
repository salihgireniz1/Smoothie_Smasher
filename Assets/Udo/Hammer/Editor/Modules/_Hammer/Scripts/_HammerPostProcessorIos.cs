#if UNITY_IOS
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

namespace Udo.Hammer.Editor.Modules._Hammer
{
    // ReSharper disable once InconsistentNaming
    public static class _HammerPostProcessorIos
    {
        
        [PostProcessBuildAttribute(int.MaxValue)]
        public static void PostProcessPbxProject(BuildTarget buildTarget, string buildPath)
        {
            var projectPath = PBXProject.GetPBXProjectPath(buildPath);
            var project = new PBXProject();
            project.ReadFromFile(projectPath);

#if UNITY_2019_3_OR_NEWER
            var unityMainTargetGuid = project.GetUnityMainTargetGuid();
            var unityFrameworkTargetGuid = project.GetUnityFrameworkTargetGuid();
#else
            var unityMainTargetGuid = project.TargetGuidByName(UnityMainTargetName);
            var unityFrameworkTargetGuid = project.TargetGuidByName(UnityMainTargetName);
#endif

            project.AddBuildProperty(unityMainTargetGuid, "OTHER_LDFLAGS", "-ObjC");
            project.SetBuildProperty(unityMainTargetGuid, "CLANG_ENABLE_MODULES", "YES");
            project.SetBuildProperty(unityMainTargetGuid, "GCC_ENABLE_OBJC_EXCEPTIONS", "YES");
            project.SetBuildProperty(unityMainTargetGuid, "ENABLE_BITCODE", "NO");
            project.SetBuildProperty(unityMainTargetGuid, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "YES");
            
#if UNITY_2018_2_OR_NEWER
            project.SetBuildProperty(unityFrameworkTargetGuid, "SWIFT_VERSION", "5.0");
#endif
            project.SetBuildProperty(unityFrameworkTargetGuid, "CLANG_ENABLE_MODULES", "YES");
            project.SetBuildProperty(unityFrameworkTargetGuid, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "NO");
            
            var workspaceSettingsPath = Path.Combine(projectPath,
                "Unity-iPhone.xcodeproj/project.xcworkspace/xcshareddata/" +
                "WorkspaceSettings.xcsettings");
            if (File.Exists(workspaceSettingsPath))
            {
                // Read the plist document, and find the root element
                var workspaceSettings = new PlistDocument();
                workspaceSettings.ReadFromFile(workspaceSettingsPath);
                var root = workspaceSettings.root;

                // Modify the document as necessary.
                var workspaceSettingsChanged = false;

                // Remove the BuildSystemType entry because it specifies the
                // legacy Xcode build system, which is deprecated
                if (root.values.ContainsKey("BuildSystemType"))

                {
                    root.values.Remove("BuildSystemType");

                    workspaceSettingsChanged = true;
                }

                // If actual changes to the document occurred, write the result
                // back to disk.
                if (workspaceSettingsChanged)
                {
                    workspaceSettings.WriteToFile(workspaceSettingsPath);
                }
            }

            project.WriteToFile(projectPath);
        }
    }
}
#endif