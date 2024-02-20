using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
namespace Udo.Hammer.Runtime.Core
{
    [CreateAssetMenu(fileName = "HammerConfig", menuName = "UDO/Hammer/Runtime Config", order = 1)]
    public class HammerConfigObject : ScriptableObject
    {
        [Header("Usercentrics")] [SerializeField]
        private string _usercentrics_settingsId;
        [SerializeField] private bool _usercentrics_isGeoRuleset;

        [SerializeField] private HammerRemoteConfigBool _usercentrics_debugMode = new("usercentrics_debugMode", false);

        [Header("Adjust")] [SerializeField] private string _adjust_appToken;
        [SerializeField] private string _adjust_firstLaunchEventToken;
        [SerializeField] private HammerRemoteConfigBool _adjust_debugMode = new("adjust_debugMode", false);
        
        [Header("AppsFlyer")] [SerializeField] private string _appsflyer_devKey;
        [SerializeField] private string _appsflyer_iosAppId;
        [SerializeField] private HammerRemoteConfigBool _appsflyer_getConversionData = new("_appsflyer_getConversionData", true);
        [SerializeField] private HammerRemoteConfigBool _appsflyer_debugMode = new("_appsflyer_debugMode", false);

        [Header("IronSource")] [SerializeField]
        private string _ironsource_appKeyAndroid;

        [SerializeField] private string _ironsource_appKeyIos;
        [SerializeField] private bool _ironsource_enableAdmob;
        [SerializeField] private string _ironsource_admobAppIdAndroid;
        [SerializeField] private string _ironsource_admobAppIdIos;

        [Header("Firebase")] [SerializeField] private string _firebase_googleServicesJson;

        [SerializeField] private string _firebase_googleServicesPlist;

        [Header("GameAnalytics")] [SerializeField]
        private string _gameanalytics_androidSecretKey;

        [SerializeField] private string _gameanalytics_androidGameKey;
        [SerializeField] private string _gameanalytics_iosSecretKey;
        [SerializeField] private string _gameanalytics_iosGameKey;

        [Header("MaxSdk")] [SerializeField] private string _maxsdk_adUnitIosRewarded;

        [SerializeField] private string _maxsdk_adUnitIosInterstitial;
        [SerializeField] private string _maxsdk_adUnitIosBanner;
        [SerializeField] private string _maxsdk_adUnitAndroidRewarded;
        [SerializeField] private string _maxsdk_adUnitAndroidInterstitial;
        [SerializeField] private string _maxsdk_adUnitAndroidBanner;
        [SerializeField] private string _maxsdk_applovinSdkKey;
        [SerializeField] private bool _maxsdk_enableAdmob;
        [SerializeField] private string _maxsdk_admobAppIdAndroid;
        [SerializeField] private string _maxsdk_admobAppIdIos;

        [Header("FacebookSDK")] [SerializeField]
        private string _facebook_appName;

        [SerializeField] private string _facebook_clientToken;
        [SerializeField] private string _facebook_appId;

        public string usercentrics_settingsId
        {
            get => _usercentrics_settingsId;
            set => _usercentrics_settingsId = value;
        }

        public bool usercentrics_isGeoRuleset
        {
            get => _usercentrics_isGeoRuleset;
            set => _usercentrics_isGeoRuleset = value;
        }

        public bool usercentrics_debugMode
        {
            get => _usercentrics_debugMode.Value;
            set => _usercentrics_debugMode.Value = value;
        }

        public string adjust_appToken
        {
            get => _adjust_appToken;
            set => _adjust_appToken = value;
        }

        public string adjust_firstLaunchEventToken
        {
            get => _adjust_firstLaunchEventToken;
            set => _adjust_firstLaunchEventToken = value;
        }

        public bool adjust_debugMode
        {
            get => _adjust_debugMode.Value;
            set => _adjust_debugMode.Value = value;
        }

        public string appsflyer_devKey
        {
            get => _appsflyer_devKey;
            set => _appsflyer_devKey = value;
        }

        public string appsflyer_iosAppId
        {
            get => _appsflyer_iosAppId;
            set => _appsflyer_iosAppId = value;
        }

        public bool appsflyer_getConversionData
        {
            get => _appsflyer_getConversionData.Value;
            set => _appsflyer_getConversionData.Value = value;
        }

        public bool appsflyer_debugMode
        {
            get => _appsflyer_debugMode.Value;
            set => _appsflyer_debugMode.Value = value;
        }

        public string ironsource_appKeyAndroid
        {
            get => _ironsource_appKeyAndroid;
            set => _ironsource_appKeyAndroid = value;
        }

        public string ironsource_appKeyIos
        {
            get => _ironsource_appKeyIos;
            set => _ironsource_appKeyIos = value;
        }

        public bool ironsource_enableAdmob
        {
            get => _ironsource_enableAdmob;
            set => _ironsource_enableAdmob = value;
        }

        public string ironsource_admobAppIdAndroid
        {
            get => _ironsource_admobAppIdAndroid;
            set => _ironsource_admobAppIdAndroid = value;
        }

        public string ironsource_admobAppIdIos
        {
            get => _ironsource_admobAppIdIos;
            set => _ironsource_admobAppIdIos = value;
        }

        public string firebase_googleServicesJson
        {
            get => _firebase_googleServicesJson;
            set => _firebase_googleServicesJson = value;
        }

        public string firebase_googleServicesPlist
        {
            get => _firebase_googleServicesPlist;
            set => _firebase_googleServicesPlist = value;
        }

        public string gameanalytics_androidSecretKey
        {
            get => _gameanalytics_androidSecretKey;
            set => _gameanalytics_androidSecretKey = value;
        }

        public string gameanalytics_androidGameKey
        {
            get => _gameanalytics_androidGameKey;
            set => _gameanalytics_androidGameKey = value;
        }

        public string gameanalytics_iosSecretKey
        {
            get => _gameanalytics_iosSecretKey;
            set => _gameanalytics_iosSecretKey = value;
        }

        public string gameanalytics_iosGameKey
        {
            get => _gameanalytics_iosGameKey;
            set => _gameanalytics_iosGameKey = value;
        }

        public string maxsdk_adUnitIosRewarded
        {
            get => _maxsdk_adUnitIosRewarded;
            set => _maxsdk_adUnitIosRewarded = value;
        }

        public string maxsdk_adUnitIosInterstitial
        {
            get => _maxsdk_adUnitIosInterstitial;
            set => _maxsdk_adUnitIosInterstitial = value;
        }

        public string maxsdk_adUnitIosBanner
        {
            get => _maxsdk_adUnitIosBanner;
            set => _maxsdk_adUnitIosBanner = value;
        }

        public string maxsdk_adUnitAndroidRewarded
        {
            get => _maxsdk_adUnitAndroidRewarded;
            set => _maxsdk_adUnitAndroidRewarded = value;
        }

        public string maxsdk_adUnitAndroidInterstitial
        {
            get => _maxsdk_adUnitAndroidInterstitial;
            set => _maxsdk_adUnitAndroidInterstitial = value;
        }

        public string maxsdk_adUnitAndroidBanner
        {
            get => _maxsdk_adUnitAndroidBanner;
            set => _maxsdk_adUnitAndroidBanner = value;
        }

        public string maxsdk_applovinSdkKey
        {
            get => _maxsdk_applovinSdkKey;
            set => _maxsdk_applovinSdkKey = value;
        }

        public bool maxsdk_enableAdmob
        {
            get => _maxsdk_enableAdmob;
            set => _maxsdk_enableAdmob = value;
        }

        public string maxsdk_admobAppIdAndroid
        {
            get => _maxsdk_admobAppIdAndroid;
            set => _maxsdk_admobAppIdAndroid = value;
        }

        public string maxsdk_admobAppIdIos
        {
            get => _maxsdk_admobAppIdIos;
            set => _maxsdk_admobAppIdIos = value;
        }

        public string facebook_appName
        {
            get => _facebook_appName;
            set => _facebook_appName = value;
        }

        public string facebook_clientToken
        {
            get => _facebook_clientToken;
            set => _facebook_clientToken = value;
        }

        public string facebook_appId
        {
            get => _facebook_appId;
            set => _facebook_appId = value;
        }

// #if UNITY_EDITOR
//         private void OnValidate()
//         {
//             EditorUtility.RequestScriptReload();
//         }
// #endif

        public HammerConfig ToHammerConfig()
        {
            var hammerConfig = new HammerConfig
            {
                usercentrics_settingsId = usercentrics_settingsId,
                usercentrics_isGeoRuleset = usercentrics_isGeoRuleset,
                adjust_appToken = adjust_appToken,
                adjust_firstLaunchEventToken = adjust_firstLaunchEventToken,
                appsflyer_devKey = appsflyer_devKey,
                appsflyer_iosAppId = appsflyer_iosAppId,
                ironsource_appKeyAndroid = ironsource_appKeyAndroid,
                ironsource_appKeyIos = ironsource_appKeyIos,
                ironsource_enableAdmob = ironsource_enableAdmob,
                ironsource_admobAppIdAndroid = ironsource_admobAppIdAndroid,
                ironsource_admobAppIdIos = ironsource_admobAppIdIos,
                firebase_googleServicesJson = firebase_googleServicesJson,
                firebase_googleServicesPlist = firebase_googleServicesPlist,
                gameanalytics_androidSecretKey = gameanalytics_androidSecretKey,
                gameanalytics_androidGameKey = gameanalytics_androidGameKey,
                gameanalytics_iosSecretKey = gameanalytics_iosSecretKey,
                gameanalytics_iosGameKey = gameanalytics_iosGameKey,
                maxsdk_adUnitIosRewarded = maxsdk_adUnitIosRewarded,
                maxsdk_adUnitIosInterstitial = maxsdk_adUnitIosInterstitial,
                maxsdk_adUnitIosBanner = maxsdk_adUnitIosBanner,
                maxsdk_adUnitAndroidRewarded = maxsdk_adUnitAndroidRewarded,
                maxsdk_adUnitAndroidInterstitial = maxsdk_adUnitAndroidInterstitial,
                maxsdk_adUnitAndroidBanner = maxsdk_adUnitAndroidBanner,
                maxsdk_applovinSdkKey = maxsdk_applovinSdkKey,
                maxsdk_enableAdmob = maxsdk_enableAdmob,
                maxsdk_admobAppIdAndroid = maxsdk_admobAppIdAndroid,
                maxsdk_admobAppIdIos = maxsdk_admobAppIdIos,
                facebook_appName = facebook_appName,
                facebook_clientToken = facebook_clientToken,
                facebook_appId = facebook_appId
            };
            return hammerConfig;
        }

        public void FromHammerConfig(HammerConfig hammerConfig)
        {
            usercentrics_settingsId = hammerConfig.usercentrics_settingsId;
            usercentrics_isGeoRuleset = hammerConfig.usercentrics_isGeoRuleset ?? false;
            adjust_appToken = hammerConfig.adjust_appToken;
            adjust_firstLaunchEventToken = hammerConfig.adjust_firstLaunchEventToken;
            appsflyer_devKey = hammerConfig.appsflyer_devKey;
            appsflyer_iosAppId = hammerConfig.appsflyer_iosAppId;
            ironsource_appKeyAndroid = hammerConfig.ironsource_appKeyAndroid;
            ironsource_appKeyIos = hammerConfig.ironsource_appKeyIos;
            ironsource_enableAdmob = hammerConfig.ironsource_enableAdmob ?? false;
            ironsource_admobAppIdAndroid = hammerConfig.ironsource_admobAppIdAndroid;
            ironsource_admobAppIdIos = hammerConfig.ironsource_admobAppIdIos;
            firebase_googleServicesJson = hammerConfig.firebase_googleServicesJson;
            firebase_googleServicesPlist = hammerConfig.firebase_googleServicesPlist;
            gameanalytics_androidSecretKey = hammerConfig.gameanalytics_androidSecretKey;
            gameanalytics_androidGameKey = hammerConfig.gameanalytics_androidGameKey;
            gameanalytics_iosSecretKey = hammerConfig.gameanalytics_iosSecretKey;
            gameanalytics_iosGameKey = hammerConfig.gameanalytics_iosGameKey;
            maxsdk_adUnitIosRewarded = hammerConfig.maxsdk_adUnitIosRewarded;
            maxsdk_adUnitIosInterstitial = hammerConfig.maxsdk_adUnitIosInterstitial;
            maxsdk_adUnitIosBanner = hammerConfig.maxsdk_adUnitIosBanner;
            maxsdk_adUnitAndroidRewarded = hammerConfig.maxsdk_adUnitAndroidRewarded;
            maxsdk_adUnitAndroidInterstitial = hammerConfig.maxsdk_adUnitAndroidInterstitial;
            maxsdk_adUnitAndroidBanner = hammerConfig.maxsdk_adUnitAndroidBanner;
            maxsdk_applovinSdkKey = hammerConfig.maxsdk_applovinSdkKey;
            maxsdk_enableAdmob = hammerConfig.maxsdk_enableAdmob ?? false;
            maxsdk_admobAppIdAndroid = hammerConfig.maxsdk_admobAppIdAndroid;
            maxsdk_admobAppIdIos = hammerConfig.maxsdk_admobAppIdIos;
            facebook_appName = hammerConfig.facebook_appName;
            facebook_clientToken = hammerConfig.facebook_clientToken;
            facebook_appId = hammerConfig.facebook_appId;
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            AssetDatabase.Refresh();
#endif
        }

        [ContextMenu("Clear")]
        public void Clear()
        {
            usercentrics_settingsId = null;
            usercentrics_isGeoRuleset = false;
            adjust_appToken = null;
            adjust_firstLaunchEventToken = null;
            appsflyer_devKey = null;
            appsflyer_iosAppId = null;
            ironsource_appKeyAndroid = null;
            ironsource_appKeyIos = null;
            ironsource_enableAdmob = false;
            ironsource_admobAppIdAndroid = null;
            ironsource_admobAppIdIos = null;
            firebase_googleServicesJson = null;
            firebase_googleServicesPlist = null;
            gameanalytics_androidSecretKey = null;
            gameanalytics_androidGameKey = null;
            gameanalytics_iosSecretKey = null;
            gameanalytics_iosGameKey = null;
            maxsdk_adUnitIosRewarded = null;
            maxsdk_adUnitIosInterstitial = null;
            maxsdk_adUnitIosBanner = null;
            maxsdk_adUnitAndroidRewarded = null;
            maxsdk_adUnitAndroidInterstitial = null;
            maxsdk_adUnitAndroidBanner = null;
            maxsdk_applovinSdkKey = null;
            maxsdk_enableAdmob = false;
            maxsdk_admobAppIdAndroid = null;
            maxsdk_admobAppIdIos = null;
            facebook_appName = null;
            facebook_clientToken = null;
            facebook_appId = null;
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
    }
}