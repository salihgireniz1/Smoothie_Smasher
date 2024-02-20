#if UNITY_IOS
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEditor.iOS.Xcode.Extensions;

// ReSharper disable StringLiteralTypo
// ReSharper disable IdentifierTypo
namespace Udo.Hammer.Editor.Modules._Usercentrics
{
    // ReSharper disable once InconsistentNaming
    public static class _HammerUsercentricsPostProcessorIos
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
                    "Usercentrics.xcframework",
                    "UsercentricsUI.xcframework",
                };

                return dynamicLibrariesToEmbed;
            }
        }
        
        private const string LegacyResourcesDirectoryName = "Resources";
        private const string HammerResourcesDirectoryName = "HammerResources";
        
        private const string DefaultUserTrackingDescriptionEn =
 "Pressing \\\"Allow\\\" uses device info for more relevant ad content";
        private const string DefaultUserTrackingDescriptionDe =
 "\\\"Erlauben\\\" drücken benutzt Gerätinformationen für relevantere Werbeinhalte";
        private const string DefaultUserTrackingDescriptionEs =
 "Presionando \\\"Permitir\\\", se usa la información del dispositivo para obtener contenido publicitario más relevante";
        private const string DefaultUserTrackingDescriptionFr =
 "\\\"Autoriser\\\" permet d'utiliser les infos du téléphone pour afficher des contenus publicitaires plus pertinents";
        private const string DefaultUserTrackingDescriptionJa = "\\\"許可\\\"をクリックすることで、デバイス情報を元により最適な広告を表示することができます";
        private const string DefaultUserTrackingDescriptionKo = "\\\"허용\\\"을 누르면 더 관련성 높은 광고 콘텐츠를 제공하기 위해 기기 정보가 사용됩니다";
        private const string DefaultUserTrackingDescriptionZhHans = "点击\\\"允许\\\"以使用设备信息获得更加相关的广告内容";
        private const string DefaultUserTrackingDescriptionZhHant = "點擊\\\"允許\\\"以使用設備信息獲得更加相關的廣告內容";

        [PostProcessBuild(int.MaxValue - 2)]
        public static void MaxPostProcessPbxProject(BuildTarget buildTarget, string buildPath)
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

            LocalizeUserTrackingDescriptionIfNeeded(DefaultUserTrackingDescriptionDe, "de", buildPath, project, unityMainTargetGuid);
            LocalizeUserTrackingDescriptionIfNeeded(DefaultUserTrackingDescriptionEn, "en", buildPath, project, unityMainTargetGuid);
            LocalizeUserTrackingDescriptionIfNeeded(DefaultUserTrackingDescriptionEs, "es", buildPath, project, unityMainTargetGuid);
            LocalizeUserTrackingDescriptionIfNeeded(DefaultUserTrackingDescriptionFr, "fr", buildPath, project, unityMainTargetGuid);
            LocalizeUserTrackingDescriptionIfNeeded(DefaultUserTrackingDescriptionJa, "ja", buildPath, project, unityMainTargetGuid);
            LocalizeUserTrackingDescriptionIfNeeded(DefaultUserTrackingDescriptionKo, "ko", buildPath, project, unityMainTargetGuid);
            LocalizeUserTrackingDescriptionIfNeeded(DefaultUserTrackingDescriptionZhHans, "zh-Hans", buildPath, project, unityMainTargetGuid);
            LocalizeUserTrackingDescriptionIfNeeded(DefaultUserTrackingDescriptionZhHant, "zh-Hant", buildPath, project, unityMainTargetGuid);

            project.WriteToFile(projectPath);
        }
        
        private static void LocalizeUserTrackingDescriptionIfNeeded(string localizedUserTrackingDescription, string localeCode, string buildPath, PBXProject project, string targetGuid)
        {
            // Use the legacy resources directory name if the build is being appended (the "Resources" directory already exists if it is an incremental build).
            var resourcesDirectoryName =
 Directory.Exists(Path.Combine(buildPath, LegacyResourcesDirectoryName)) ? LegacyResourcesDirectoryName : HammerResourcesDirectoryName;
            var resourcesDirectoryPath = Path.Combine(buildPath, resourcesDirectoryName);
            var localeSpecificDirectoryName = localeCode + ".lproj";
            var localeSpecificDirectoryPath = Path.Combine(resourcesDirectoryPath, localeSpecificDirectoryName);
            var infoPlistStringsFilePath = Path.Combine(localeSpecificDirectoryPath, "InfoPlist.strings");

            // Create intermediate directories as needed.
            if (!Directory.Exists(resourcesDirectoryPath))
            {
                Directory.CreateDirectory(resourcesDirectoryPath);
            }

            if (!Directory.Exists(localeSpecificDirectoryPath))
            {
                Directory.CreateDirectory(localeSpecificDirectoryPath);
            }

            var localizedDescriptionLine =
 "\"NSUserTrackingUsageDescription\" = \"" + localizedUserTrackingDescription + "\";\n";
            // File already exists, update it in case the value changed between builds.
            if (File.Exists(infoPlistStringsFilePath))
            {
                var output = new List<string>();
                var lines = File.ReadAllLines(infoPlistStringsFilePath);
                var keyUpdated = false;
                foreach (var line in lines)
                {
                    if (line.Contains("NSUserTrackingUsageDescription"))
                    {
                        output.Add(localizedDescriptionLine);
                        keyUpdated = true;
                    }
                    else
                    {
                        output.Add(line);
                    }
                }

                if (!keyUpdated)
                {
                    output.Add(localizedDescriptionLine);
                }

                File.WriteAllText(infoPlistStringsFilePath, string.Join("\n", output.ToArray()) + "\n");
            }
            // File doesn't exist, create one.
            else
            {
                File.WriteAllText(infoPlistStringsFilePath, "/* Localized versions of Info.plist keys - Generated by Hammer SDK */\n" + localizedDescriptionLine);
            }

            var localeSpecificDirectoryRelativePath = Path.Combine(resourcesDirectoryName, localeSpecificDirectoryName);
            var guid =
 project.AddFolderReference(localeSpecificDirectoryRelativePath, localeSpecificDirectoryRelativePath);
            project.AddFileToBuild(targetGuid, guid);
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
                
                // if ("UsercentricsUI.xcframework".Equals(dynamicLibraryToSearch))
                // {
                //     var infoPlistPath =  dynamicLibraryAbsolutePath + "/ios-arm64/" + dynamicLibraryToSearch.Substring(0, dynamicLibraryToSearch.IndexOf('.')) + ".framework/Info.plist";
                //     if (File.Exists(infoPlistPath))
                //     {
                //         PlistDocument plist = new PlistDocument();
                //         plist.ReadFromString(File.ReadAllText(infoPlistPath));
                //         PlistElementDict rootDict = plist.root;
                //         var buildKey = "CFBundleShortVersionString";
                //         if (rootDict.values.TryGetValue(buildKey, out var plistElement))
                //         {
                //             var length = plistElement.AsString().IndexOf("-unity");
                //             if (length > 0)
                //             {
                //                 var fixedVersion = plistElement.AsString().Substring(0, plistElement.AsString().IndexOf("-unity"));
                //                 rootDict.SetString(buildKey, fixedVersion);
                //                 File.WriteAllText(infoPlistPath, plist.WriteToString());
                //             }
                //         }
                //     }
                // }
                
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