using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace Udo.Hammer.Editor.Backend
{
    // ReSharper disable once InconsistentNaming
    [InitializeOnLoad]
    public static class _Hammer
    {
        private const string IOSMinimumOSVersion = "12.0";
        private const AndroidSdkVersions AndroidTargetOSVersion = (AndroidSdkVersions)33;
        private const AndroidSdkVersions AndroidMinimumOSVersion = AndroidSdkVersions.AndroidApiLevel24;
        private const ScriptingImplementation AndroidScriptingBackend = ScriptingImplementation.IL2CPP;
        private const ApiCompatibilityLevel AndroidApiCompatibilityLevel = ApiCompatibilityLevel.NET_Unity_4_8;

        private const Il2CppCompilerConfiguration AndroidIL2CPPCompilerConfiguration =
            Il2CppCompilerConfiguration.Release;

        private const AndroidArchitecture AndroidTargetArchitectures =
            AndroidArchitecture.ARMv7 | AndroidArchitecture.ARM64;

        private const AndroidTargetDevices AndroidTargetDevice = AndroidTargetDevices.PhonesTabletsAndTVDevicesOnly;
        private const bool AndroidUseDefaultGraphicsAPIs = false;
        private static readonly GraphicsDeviceType[] AndroidGraphicsAPIs = { GraphicsDeviceType.OpenGLES3 };

        static _Hammer()
        {
            if (!IOSMinimumOSVersion.Equals(PlayerSettings.iOS.targetOSVersionString))
            {
                PlayerSettings.iOS.targetOSVersionString = IOSMinimumOSVersion;
                Debug.Log("PlayerSettings.iOS.targetOSVersionString = " + IOSMinimumOSVersion);
            }

            if (!AndroidMinimumOSVersion.Equals(PlayerSettings.Android.minSdkVersion))
            {
                PlayerSettings.Android.minSdkVersion = AndroidMinimumOSVersion;
                Debug.Log("PlayerSettings.Android.minSdkVersion = " + AndroidMinimumOSVersion);
            }

            if (!AndroidTargetOSVersion.Equals(PlayerSettings.Android.targetSdkVersion))
            {
                PlayerSettings.Android.targetSdkVersion = AndroidTargetOSVersion;
                Debug.Log("PlayerSettings.Android.targetSdkVersion = " + AndroidTargetOSVersion);
            }

            if (!AndroidScriptingBackend.Equals(PlayerSettings.GetScriptingBackend(BuildTargetGroup.Android)))
            {
                PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, AndroidScriptingBackend);
                Debug.Log("PlayerSettings.SetScriptingBackend(" + BuildTargetGroup.Android + ", " +
                          AndroidScriptingBackend + ");");
            }

            if (!AndroidApiCompatibilityLevel.Equals(PlayerSettings.GetApiCompatibilityLevel(BuildTargetGroup.Android)))
            {
                PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Android, AndroidApiCompatibilityLevel);
                Debug.Log("PlayerSettings.SetApiCompatibilityLevel(" + BuildTargetGroup.Android + ", " +
                          AndroidApiCompatibilityLevel + ");");
            }

            if (!AndroidIL2CPPCompilerConfiguration.Equals(
                    PlayerSettings.GetIl2CppCompilerConfiguration(BuildTargetGroup.Android)))
            {
                PlayerSettings.SetIl2CppCompilerConfiguration(BuildTargetGroup.Android,
                    AndroidIL2CPPCompilerConfiguration);
                Debug.Log("PlayerSettings.SetIl2CppCompilerConfiguration(" + BuildTargetGroup.Android + ", " +
                          AndroidIL2CPPCompilerConfiguration + ");");
            }

            if (!AndroidTargetArchitectures.Equals(PlayerSettings.Android.targetArchitectures))
            {
                PlayerSettings.Android.targetArchitectures = AndroidTargetArchitectures;
                Debug.Log("PlayerSettings.Android.targetArchitectures = " + AndroidTargetArchitectures);
            }

            if (!AndroidTargetDevice.Equals(PlayerSettings.Android.androidTargetDevices))
            {
                PlayerSettings.Android.androidTargetDevices = AndroidTargetDevice;
                Debug.Log("PlayerSettings.Android.androidTargetDevices = " + AndroidTargetDevice);
            }

            if (!AndroidUseDefaultGraphicsAPIs.Equals(PlayerSettings.GetUseDefaultGraphicsAPIs(BuildTarget.Android)))
            {
                PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.Android, AndroidUseDefaultGraphicsAPIs);
                Debug.Log("PlayerSettings.SetUseDefaultGraphicsAPIs(" + BuildTarget.Android + ", " +
                          AndroidUseDefaultGraphicsAPIs + ");");
            }

            if (!ArrayUtility.ArrayEquals(AndroidGraphicsAPIs, PlayerSettings.GetGraphicsAPIs(BuildTarget.Android)))
            {
                PlayerSettings.SetGraphicsAPIs(BuildTarget.Android, AndroidGraphicsAPIs);
                Debug.Log("PlayerSettings.SetGraphicsAPIs(" + BuildTarget.Android + ", " + AndroidGraphicsAPIs + ");");
            }
        }
    }
}