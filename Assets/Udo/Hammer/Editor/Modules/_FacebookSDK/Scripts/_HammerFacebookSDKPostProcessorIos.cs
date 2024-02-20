#if UNITY_IOS
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEditor.iOS.Xcode.Extensions;

namespace Udo.Hammer.Editor.Modules._FacebookSDK
{
    // ReSharper disable once InconsistentNaming
    public static class _HammerFacebookSDKPostProcessorIos
    {
        private const string TargetUnityIphonePodfileLine = "target 'Unity-iPhone' do";
        
#if !UNITY_2019_3_OR_NEWER
        private const string UnityMainTargetName = "Unity-iPhone";
#endif
        
        private static List<string> DynamicLibrariesToEmbed
        {
            get
            {
                var dynamicLibrariesToEmbed = new List<string>()
                {
                    "FBAEMKit.xcframework",
                    "FBSDKCoreKit.xcframework",
                    "FBSDKCoreKit_Basics.xcframework",
                    "FBSDKGamingServicesKit.xcframework",
                    "FBSDKLoginKit.xcframework",
                    "FBSDKShareKit.xcframework",
                };

                return dynamicLibrariesToEmbed;
            }
        }
        
        [PostProcessBuildAttribute(int.MaxValue - 3)]
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
            EmbedDynamicLibrariesIfNeeded(buildPath, project, unityMainTargetGuid);
            
            project.WriteToFile(projectPath);
        }

        private static void EmbedDynamicLibrariesIfNeeded(string buildPath, PBXProject project, string targetGuid)
        {
            // Check that the Pods directory exists (it might not if a publisher is building with Generate Podfile setting disabled in EDM).
            var podsDirectory = Path.Combine(buildPath, "Pods");
            if (!Directory.Exists(podsDirectory)) return;

            var dynamicLibraryPathsPresentInProject = new List<string>();
            foreach (var dynamicLibraryToSearch in DynamicLibrariesToEmbed)
            {
                // both .framework and .xcframework are directories, not files
                var directories =
 Directory.GetDirectories(podsDirectory, dynamicLibraryToSearch, SearchOption.AllDirectories);
                if (directories.Length <= 0) continue;

                var dynamicLibraryAbsolutePath = directories[0];
                var index = dynamicLibraryAbsolutePath.LastIndexOf("Pods");
                var relativePath = dynamicLibraryAbsolutePath.Substring(index);
                dynamicLibraryPathsPresentInProject.Add(relativePath);
            }

            if (dynamicLibraryPathsPresentInProject.Count <= 0) return;

#if UNITY_2019_3_OR_NEWER
            // Embed framework only if the podfile does not contain target `Unity-iPhone`.
//            if (!ContainsUnityIphoneTargetInPodfile(buildPath))
//            {
                foreach (var dynamicLibraryPath in dynamicLibraryPathsPresentInProject)
                {
                    var fileGuid = project.AddFile(dynamicLibraryPath, dynamicLibraryPath);
                    project.AddFileToEmbedFrameworks(targetGuid, fileGuid);
                }
//            }
#else
            string runpathSearchPaths;
#if UNITY_2018_2_OR_NEWER
            runpathSearchPaths = project.GetBuildPropertyForAnyConfig(targetGuid, "LD_RUNPATH_SEARCH_PATHS");
#else
            runpathSearchPaths = "$(inherited)";          
#endif
            runpathSearchPaths += string.IsNullOrEmpty(runpathSearchPaths) ? "" : " ";

            // Check if runtime search paths already contains the required search paths for dynamic libraries.
            if (runpathSearchPaths.Contains("@executable_path/Frameworks")) return;

            runpathSearchPaths += "@executable_path/Frameworks";
            project.SetBuildProperty(targetGuid, "LD_RUNPATH_SEARCH_PATHS", runpathSearchPaths);
#endif
        }

//#if UNITY_2019_3_OR_NEWER
//        private static bool ContainsUnityIphoneTargetInPodfile(string buildPath)
//        {
//            var podfilePath = Path.Combine(buildPath, "Podfile");
//            if (!File.Exists(podfilePath)) return false;
//
//            var lines = File.ReadAllLines(podfilePath);
//            return lines.Any(line => line.Contains(TargetUnityIphonePodfileLine));
//        }
//#endif
    }
}
#endif