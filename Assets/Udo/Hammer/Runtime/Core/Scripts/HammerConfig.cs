#nullable enable
using System;
using Newtonsoft.Json;

// ReSharper disable StringLiteralTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
namespace Udo.Hammer.Runtime.Core
{
    [Serializable]
    public class HammerConfig
    {
        [JsonProperty("usercentrics_settingsId")]
        public string? usercentrics_settingsId { get; set; }

        [JsonProperty("usercentrics_isGeoRuleset")]
        public bool? usercentrics_isGeoRuleset { get; set; }

        [JsonProperty("adjust_appToken")] public string? adjust_appToken { get; set; }

        [JsonProperty("adjust_firstLaunchEventToken")]
        public string? adjust_firstLaunchEventToken { get; set; }

        [JsonProperty("appsflyer_devKey")] public string? appsflyer_devKey { get; set; }
        [JsonProperty("appsflyer_iosAppId")] public string? appsflyer_iosAppId { get; set; }

        [JsonProperty("ironsource_appKeyAndroid")]
        public string? ironsource_appKeyAndroid { get; set; }

        [JsonProperty("ironsource_appKeyIos")] public string? ironsource_appKeyIos { get; set; }

        [JsonProperty("ironsource_enableAdmob")]
        public bool? ironsource_enableAdmob { get; set; }

        [JsonProperty("ironsource_admobAppIdAndroid")]
        public string? ironsource_admobAppIdAndroid { get; set; }

        [JsonProperty("ironsource_admobAppIdIos")]
        public string? ironsource_admobAppIdIos { get; set; }

        [JsonProperty("firebase_googleServicesJson")]
        public string? firebase_googleServicesJson { get; set; }

        [JsonProperty("firebase_googleServicesPlist")]
        public string? firebase_googleServicesPlist { get; set; }

        [JsonProperty("gameanalytics_androidSecretKey")]
        public string? gameanalytics_androidSecretKey { get; set; }

        [JsonProperty("gameanalytics_androidGameKey")]
        public string? gameanalytics_androidGameKey { get; set; }

        [JsonProperty("gameanalytics_iosSecretKey")]
        public string? gameanalytics_iosSecretKey { get; set; }

        [JsonProperty("gameanalytics_iosGameKey")]
        public string? gameanalytics_iosGameKey { get; set; }

        [JsonProperty("maxsdk_adUnitIosRewarded")]
        public string? maxsdk_adUnitIosRewarded { get; set; }

        [JsonProperty("maxsdk_adUnitIosInterstitial")]
        public string? maxsdk_adUnitIosInterstitial { get; set; }

        [JsonProperty("maxsdk_adUnitIosBanner")]
        public string? maxsdk_adUnitIosBanner { get; set; }

        [JsonProperty("maxsdk_adUnitAndroidRewarded")]
        public string? maxsdk_adUnitAndroidRewarded { get; set; }

        [JsonProperty("maxsdk_adUnitAndroidInterstitial")]
        public string? maxsdk_adUnitAndroidInterstitial { get; set; }

        [JsonProperty("maxsdk_adUnitAndroidBanner")]
        public string? maxsdk_adUnitAndroidBanner { get; set; }

        [JsonProperty("maxsdk_applovinSdkKey")]
        public string? maxsdk_applovinSdkKey { get; set; }

        [JsonProperty("maxsdk_enableAdmob")] public bool? maxsdk_enableAdmob { get; set; }

        [JsonProperty("maxsdk_admobAppIdAndroid")]
        public string? maxsdk_admobAppIdAndroid { get; set; }

        [JsonProperty("maxsdk_admobAppIdIos")] public string? maxsdk_admobAppIdIos { get; set; }
        [JsonProperty("facebook_appName")] public string? facebook_appName { get; set; }
        [JsonProperty("facebook_clientToken")] public string? facebook_clientToken { get; set; }
        [JsonProperty("facebook_appId")] public string? facebook_appId { get; set; }
    }
}