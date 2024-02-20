using System;
using System.Collections.Generic;
using AppsFlyerConnector;
using AppsFlyerSDK;
using Udo.Hammer.Runtime.Core;
using UnityEngine;

namespace Udo.Hammer.Runtime._AppsFlyer
{
    public class HammerAppsFlyer : MonoBehaviour, IHammerThirdPartySdk, IAppsFlyerConversionData
    {
        private bool _hasConsent;
        private string _devKey;
        private string _appId;
        private bool _appsflyerGetConversionData;

        public string DpsTemplateIdOrName()
        {
            return "Gx9iMF__f";
        }

        private void Awake()
        {
            Core.Hammer.ActionAttributionInit = Initialize;

            Core.Hammer.ActionAttributionAdRevenueEventIronSource = SendEvent_AD_REVENUE_IRONSOURCE;
            Core.Hammer.ActionAttributionAdRevenueEventMaxSdk = SendEvent_AD_REVENUE_MAXSDK;
            
            // AppsFlyer.OnRequestResponse += OnRequestResponse;
            // AppsFlyer.OnDeepLinkReceived += OnDeepLinkReceived;
            // AppsFlyer.OnInAppResponse += OnInAppResponse;
        }

        public void Initialize(Func<string> funcPrivacyGetUserId,
            Func<string, bool> funcPrivacyGetConsentByDpsTemplateIdOrName,
            Func<bool> funcPrivacyGetCcpaFlag, Func<bool> funcPrivacyGetCoppaFlag, Func<bool> funcPrivacyGetAttFlag,
            Transform hammerTransform)
        {
            if (Application.isEditor)
            {
                return;
            }

            Debug.Log(gameObject.name + ".Initialize start");

            _hasConsent = funcPrivacyGetConsentByDpsTemplateIdOrName(DpsTemplateIdOrName());

            _devKey = Core.Hammer.HammerConfigObject.appsflyer_devKey;
            _appsflyerGetConversionData = Core.Hammer.HammerConfigObject.appsflyer_getConversionData;
#if UNITY_ANDROID
            _appId = null;
#elif UNITY_IOS
            _appId = Core.Hammer.HammerConfigObject.appsflyer_iosAppId;
#endif
            AppsFlyer.initSDK(_devKey, _appId, _appsflyerGetConversionData ? this : null);
            AppsFlyer.setIsDebug(Core.Hammer.HammerConfigObject.appsflyer_debugMode);
            if (_hasConsent)
            {
                AppsFlyer.setCustomerUserId(funcPrivacyGetUserId());
            }
            else
            {
                AppsFlyer.anonymizeUser(_hasConsent);
            }

            //todo okan remote config
            // Debug.Log(gameObject.name + ".Initialize AppsFlyerPurchaseConnector start");
            // AppsFlyerPurchaseConnector.init(this, AppsFlyerConnector.Store.GOOGLE);       
            // AppsFlyerPurchaseConnector.setIsSandbox(Core.Hammer.HammerConfigObject.appsflyer_debugMode);
            // AppsFlyerPurchaseConnector.setAutoLogPurchaseRevenue(
            //     AppsFlyerAutoLogPurchaseRevenueOptions.AppsFlyerAutoLogPurchaseRevenueOptionsAutoRenewableSubscriptions,
            //     AppsFlyerAutoLogPurchaseRevenueOptions.AppsFlyerAutoLogPurchaseRevenueOptionsInAppPurchases);
            // AppsFlyerPurchaseConnector.setPurchaseRevenueValidationListeners(true);
            // AppsFlyerPurchaseConnector.build();
            // AppsFlyerPurchaseConnector.startObservingTransactions();
            // Debug.Log(gameObject.name + ".Initialize AppsFlyerPurchaseConnector end");
            
            Debug.Log(gameObject.name + ".Initialize AppsFlyerAdRevenue start");
            AppsFlyerAdRevenue.start();
            AppsFlyerAdRevenue.setIsDebug(Core.Hammer.HammerConfigObject.appsflyer_debugMode);
            Debug.Log(gameObject.name + ".Initialize AppsFlyerAdRevenue end");
            
            Debug.Log(gameObject.name + ".Initialize AppsFlyer start");
            AppsFlyer.startSDK();
            Debug.Log(gameObject.name + ".Initialize AppsFlyer end");

            Debug.Log(gameObject.name + ".Initialize end");
        }
        
        // ReSharper disable once InconsistentNaming
        // ReSharper disable once UnusedMember.Global
        public void didReceivePurchaseRevenueValidationInfo(string validationInfo)
        {
            Debug.Log(gameObject.name + ".didReceivePurchaseRevenueValidationInfo validationInfo: " + validationInfo);
            AppsFlyer.AFLog("didReceivePurchaseRevenueValidationInfo", validationInfo);
        }

        // private void OnRequestResponse(object sender, EventArgs e)
        // {
        //     Debug.Log(gameObject.name + ".OnRequestResponse start");
        //     
        //     var args = e as AppsFlyerRequestEventArgs;
        // }
        //
        // private void OnDeepLinkReceived(object sender, EventArgs e)
        // {
        //     Debug.Log(gameObject.name + ".OnDeepLinkReceived start");
        //     
        //     var args = e as DeepLinkEventsArgs;
        // }
        //
        // private void OnInAppResponse(object sender, EventArgs e)
        // {
        //     Debug.Log(gameObject.name + ".OnInAppResponse start");
        //     
        //     var args = e as AppsFlyerRequestEventArgs;
        // }

        public void onConversionDataSuccess(string conversionData)
        {
            Debug.Log(gameObject.name + ".onConversionDataSuccess conversionData: " + conversionData);
            //todo okan hammerlytics
        }

        public void onConversionDataFail(string error)
        {
            Debug.Log(gameObject.name + ".onConversionDataFail error: " + error);
            //todo okan hammerlytics
        }

        public void onAppOpenAttribution(string attributionData)
        {
            Debug.Log(gameObject.name + ".onAppOpenAttribution attributionData: " + attributionData);
            //todo okan hammerlytics
        }

        public void onAppOpenAttributionFailure(string error)
        {
            Debug.Log(gameObject.name + ".onAppOpenAttributionFailure error: " + error);
            //todo okan hammerlytics
        }

        private void SendEvent_LOGIN()
        {
            Debug.Log(gameObject.name + ".SendEvent_LOGIN");
            AppsFlyer.sendEvent(AFInAppEvents.LOGIN, null);
        }

        private void SendEvent_COMPLETE_REGISTRATION(string registrationMethod)
        {
            Debug.Log(gameObject.name + ".SendEvent_COMPLETE_REGISTRATION registrationMethod: " + registrationMethod);
            var eventValues = new Dictionary<string, string>
                { { AFInAppEvents.REGSITRATION_METHOD, registrationMethod } };
            AppsFlyer.sendEvent(AFInAppEvents.COMPLETE_REGISTRATION, eventValues);
        }

        private void SendEvent_PURCHASE(string revenue, string currency, string quantity, string contentID,
            string orderID, string receiptID)
        {
            Debug.Log(gameObject.name + ".SendEvent_COMPLETE_REGISTRATION revenue: " + revenue + ", currency: " +
                      currency + ", quantity: " + quantity + ", contentID: " + contentID + ", orderID: " + orderID +
                      ", receiptID: " + receiptID);
            var eventValues = new Dictionary<string, string>
            {
                { AFInAppEvents.REVENUE, revenue },
                { AFInAppEvents.CURRENCY, currency },
                { AFInAppEvents.QUANTITY, quantity },
                { AFInAppEvents.CONTENT_ID, contentID },
                { AFInAppEvents.ORDER_ID, orderID },
                { AFInAppEvents.RECEIPT_ID, receiptID }
            };
            AppsFlyer.sendEvent(AFInAppEvents.PURCHASE, eventValues);
        }

        private void SendEvent_LEVEL_ACHIEVED(string level, string score)
        {
            Debug.Log(gameObject.name + ".SendEvent_LEVEL_ACHIEVED level: " + level + ", score: " + score);
            var eventValues = new Dictionary<string, string>
            {
                { AFInAppEvents.LEVEL, level },
                { AFInAppEvents.SCORE, score }
            };
            AppsFlyer.sendEvent(AFInAppEvents.LEVEL_ACHIEVED, eventValues);
        }

        private void SendEvent_TUTORIAL_COMPLETION(string success, string tutorialID, string tutorialName)
        {
            Debug.Log(gameObject.name + ".SendEvent_TUTORIAL_COMPLETION success: " + success + ", tutorialID: " +
                      tutorialID + ", tutorialName: " + tutorialName);
            var eventValues = new Dictionary<string, string>
            {
                { AFInAppEvents.SUCCESS, success },
                { "af_tutorial_id", tutorialID },
                { "af_content", tutorialName }
            };
            AppsFlyer.sendEvent(AFInAppEvents.TUTORIAL_COMPLETION, eventValues);
        }

        private void SendEvent_SHARE(string description, string platform)
        {
            Debug.Log(gameObject.name + ".SendEvent_SHARE description: " + description + ", platform: " + platform);
            var eventValues = new Dictionary<string, string>
            {
                { AFInAppEvents.DESCRIPTION, description },
                { "platform", platform }
            };
            AppsFlyer.sendEvent(AFInAppEvents.SHARE, eventValues);
        }

        private void SendEvent_INVITE(string description)
        {
            Debug.Log(gameObject.name + ".SendEvent_INVITE description: " + description);
            var eventValues = new Dictionary<string, string> { { AFInAppEvents.DESCRIPTION, description } };
            AppsFlyer.sendEvent(AFInAppEvents.INVITE, eventValues);
        }

        private void SendEvent_BONUS_CLAIMED(string bonusType)
        {
            Debug.Log(gameObject.name + ".SendEvent_BONUS_CLAIMED bonusType: " + bonusType);
            var eventValues = new Dictionary<string, string> { { "bonus_type", bonusType } };
            AppsFlyer.sendEvent("bonus_claimed", eventValues);
        }

        private void SendEvent_AD_REVENUE_MAXSDK(string monetizationNetwork, double eventRevenue, string revenueCurrency = "USD")
        {
            Debug.Log(gameObject.name + ".SendEvent_AD_REVENUE_MAXSDK monetizationNetwork: " + monetizationNetwork +
                      " , eventRevenue: " + eventRevenue + " , revenueCurrency: " + revenueCurrency);
            SendEvent_AD_REVENUE(monetizationNetwork, AppsFlyerAdRevenueMediationNetworkType.AppsFlyerAdRevenueMediationNetworkTypeApplovinMax, eventRevenue, revenueCurrency);
        }

        private void SendEvent_AD_REVENUE_IRONSOURCE(string monetizationNetwork, double eventRevenue, string revenueCurrency = "USD")
        {
            Debug.Log(gameObject.name + ".SendEvent_AD_REVENUE_IRONSOURCE monetizationNetwork: " + monetizationNetwork +
                      " , eventRevenue: " + eventRevenue + " , revenueCurrency: " + revenueCurrency);
            SendEvent_AD_REVENUE(monetizationNetwork, AppsFlyerAdRevenueMediationNetworkType.AppsFlyerAdRevenueMediationNetworkTypeIronSource, eventRevenue, revenueCurrency);
        }

        private void SendEvent_AD_REVENUE(string monetizationNetwork,
            AppsFlyerAdRevenueMediationNetworkType mediationNetwork, double eventRevenue,
            string revenueCurrency = "USD")
        {
            // Debug.Log(gameObject.name + ".SendEvent_AD_REVENUE monetizationNetwork: " + monetizationNetwork +
            //           " , mediationNetwork: " + mediationNetwork + " , eventRevenue: " + eventRevenue +
            //           " , revenueCurrency: " + revenueCurrency);
            Debug.Log(gameObject.name + ".SendEvent_AD_REVENUE start");
            var additionalParameters = new Dictionary<string, string>
            {
                // { "custom", "foo" },
                // { "custom_2", "bar" },
                // { "af_quantity", "1" }
            };
            AppsFlyerAdRevenue.logAdRevenue(monetizationNetwork, mediationNetwork, eventRevenue, revenueCurrency,
                additionalParameters);
            Debug.Log(gameObject.name + ".SendEvent_AD_REVENUE end");
        }
    }
}