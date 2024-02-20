using System;
using System.Collections;
using Udo.Hammer.Runtime.Core;
using UnityEngine;

namespace Udo.Hammer.Runtime._MaxSdk
{
    public class HammerMaxSdk : MonoBehaviour, IHammerThirdPartySdkAsyncInitialization
    {
        private bool _hasConsent;
        private bool _initialized;
        private string _sdkKey;
        private string _adUnitIdBanner;
        private string _adUnitIdInterstitial;
        private string _adUnitIdRewarded;

        public string DpsTemplateIdOrName()
        {
            return "fHczTMzX8";
        }

        private void OnEnable()
        {
            MaxSdkCallbacks.OnSdkInitializedEvent += OnSdkInitializedEvent;

            //BANNER
            MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnAdLoadedEventBanner;
            MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnAdLoadFailedEventBanner;
            MaxSdkCallbacks.Banner.OnAdClickedEvent += OnAdClickedEventBanner;
            MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnAdRevenuePaidEventBanner;
            MaxSdkCallbacks.Banner.OnAdReviewCreativeIdGeneratedEvent += OnAdReviewCreativeIdGeneratedEventBanner;
            MaxSdkCallbacks.Banner.OnAdExpandedEvent += OnAdExpandedEventBanner;
            MaxSdkCallbacks.Banner.OnAdCollapsedEvent += OnAdCollapsedEventBanner;

            //INTERSTITIAL
            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnAdLoadedEventInterstitial;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnAdLoadFailedEventInterstitial;
            MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnAdDisplayedEventInterstitial;
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnAdDisplayFailedEventInterstitial;
            MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnAdClickedEventInterstitial;
            MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnAdRevenuePaidEventInterstitial;
            MaxSdkCallbacks.Interstitial.OnAdReviewCreativeIdGeneratedEvent +=
                OnAdReviewCreativeIdGeneratedEventInterstitial;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnAdHiddenEventInterstitial;

            //REWARDED
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnAdLoadedEventRewarded;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnAdLoadFailedEventRewarded;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnAdDisplayedEventRewarded;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnAdDisplayFailedEventRewarded;
            MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnAdClickedEventRewarded;
            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnAdRevenuePaidEventRewarded;
            MaxSdkCallbacks.Rewarded.OnAdReviewCreativeIdGeneratedEvent += OnAdReviewCreativeIdGeneratedEventRewarded;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnAdReceivedRewardEventRewarded;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnAdHiddenEventRewarded;
        }

        private void OnDisable()
        {
            //BANNER
            MaxSdkCallbacks.Banner.OnAdLoadedEvent -= OnAdLoadedEventBanner;
            MaxSdkCallbacks.Banner.OnAdLoadFailedEvent -= OnAdLoadFailedEventBanner;
            MaxSdkCallbacks.Banner.OnAdClickedEvent -= OnAdClickedEventBanner;
            MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent -= OnAdRevenuePaidEventBanner;
            MaxSdkCallbacks.Banner.OnAdReviewCreativeIdGeneratedEvent -= OnAdReviewCreativeIdGeneratedEventBanner;
            MaxSdkCallbacks.Banner.OnAdExpandedEvent -= OnAdExpandedEventBanner;
            MaxSdkCallbacks.Banner.OnAdCollapsedEvent -= OnAdCollapsedEventBanner;

            //INTERSTITIAL
            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent -= OnAdLoadedEventInterstitial;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent -= OnAdLoadFailedEventInterstitial;
            MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent -= OnAdDisplayedEventInterstitial;
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent -= OnAdDisplayFailedEventInterstitial;
            MaxSdkCallbacks.Interstitial.OnAdClickedEvent -= OnAdClickedEventInterstitial;
            MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent -= OnAdRevenuePaidEventInterstitial;
            MaxSdkCallbacks.Interstitial.OnAdReviewCreativeIdGeneratedEvent -=
                OnAdReviewCreativeIdGeneratedEventInterstitial;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent -= OnAdHiddenEventInterstitial;

            //REWARDED
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent -= OnAdLoadedEventRewarded;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent -= OnAdLoadFailedEventRewarded;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent -= OnAdDisplayedEventRewarded;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent -= OnAdDisplayFailedEventRewarded;
            MaxSdkCallbacks.Rewarded.OnAdClickedEvent -= OnAdClickedEventRewarded;
            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent -= OnAdRevenuePaidEventRewarded;
            MaxSdkCallbacks.Rewarded.OnAdReviewCreativeIdGeneratedEvent -= OnAdReviewCreativeIdGeneratedEventRewarded;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent -= OnAdReceivedRewardEventRewarded;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent -= OnAdHiddenEventRewarded;
        }

        private void Awake()
        {
            Core.Hammer.ActionMediationInitAsync = InitializeAsync;
            Core.Hammer.ActionMediationShowBanner = ShowBanner;
            Core.Hammer.ActionMediationDestroyBanner = DestroyBanner;
            Core.Hammer.ActionMediationHasInterstitial = HasInterstitial;
            Core.Hammer.ActionMediationShowInterstitial = ShowInterstitial;
            Core.Hammer.ActionMediationHasRewarded = HasRewarded;
            Core.Hammer.ActionMediationShowRewarded = ShowRewarded;
        }

        public void Initialize(Func<string> funcPrivacyGetUserId, Func<string, bool> funcPrivacyGetConsentByDpsTemplateIdOrName,
            Func<bool> funcPrivacyGetCcpaFlag,
            Func<bool> funcPrivacyGetCoppaFlag, Func<bool> funcPrivacyGetAttFlag, Transform hammerTransform)
        {
            throw new NotImplementedException();
        }

        public IEnumerator InitializeAsync(Func<string> funcPrivacyGetUserId,
            Func<string, bool> funcPrivacyGetConsentByDpsTemplateIdOrName,
            Func<bool> funcPrivacyGetCcpaFlag, Func<bool> funcPrivacyGetCoppaFlag, Func<bool> funcPrivacyGetAttFlag,
            Transform hammerTransform)
        {
            if (Application.isEditor)
            {
                yield break;
            }

            Debug.Log(gameObject.name + ".Initialize start");

            _hasConsent = funcPrivacyGetConsentByDpsTemplateIdOrName(DpsTemplateIdOrName());
            if (_hasConsent)
            {
                MaxSdk.SetUserId(funcPrivacyGetUserId());
            }

            MaxSdk.SetHasUserConsent(_hasConsent);
            MaxSdk.SetDoNotSell(funcPrivacyGetCcpaFlag());
            //todo okan COPPA
            // MaxSdk.SetIsAgeRestrictedUser(funcPrivacyGetCoppaFlag());

            _sdkKey = Core.Hammer.HammerConfigObject.maxsdk_applovinSdkKey;
#if UNITY_ANDROID
            _adUnitIdBanner = Core.Hammer.HammerConfigObject.maxsdk_adUnitAndroidBanner;
            _adUnitIdInterstitial = Core.Hammer.HammerConfigObject.maxsdk_adUnitAndroidInterstitial;
            _adUnitIdRewarded = Core.Hammer.HammerConfigObject.maxsdk_adUnitAndroidRewarded;
#endif
#if UNITY_IOS
            _adUnitIdBanner = Core.Hammer.HammerConfigObject.maxsdk_adUnitIosBanner;
            _adUnitIdInterstitial = Core.Hammer.HammerConfigObject.maxsdk_adUnitIosInterstitial;
            _adUnitIdRewarded = Core.Hammer.HammerConfigObject.maxsdk_adUnitIosRewarded;
#endif

            MaxSdk.SetSdkKey(_sdkKey);
            MaxSdk.InitializeSdk();
            yield return new WaitUntil(() => _initialized);
            Debug.Log(gameObject.name + ".Initialize end");
        }

        private void OnSdkInitializedEvent(MaxSdkBase.SdkConfiguration obj)
        {
            if (Application.isEditor)
            {
                return;
            }
            
            //todo okan remote config
            // MaxSdk.ShowMediationDebugger();
            
            _initialized = true;

            Debug.Log(gameObject.name + ".OnSdkInitializedEvent");
        }

        #region BANNER

        private void ShowBanner()
        {
            if (Application.isEditor)
            {
                return;
            }

            Debug.Log(gameObject.name + ".ShowBanner");

            MaxSdk.SetBannerExtraParameter(_adUnitIdBanner, "adaptive_banner", "false");
            MaxSdk.SetBannerBackgroundColor(_adUnitIdBanner, Color.white);
            MaxSdk.CreateBanner(_adUnitIdBanner, MaxSdkBase.BannerPosition.BottomCenter);
        }

        private void DestroyBanner()
        {
            if (Application.isEditor)
            {
                return;
            }

            Debug.Log(gameObject.name + ".DestroyBanner");

            MaxSdk.DestroyBanner(_adUnitIdBanner);
        }

        #region BANNER_CALLBACKS

        private void OnAdLoadedEventBanner(string arg1, MaxSdkBase.AdInfo arg2)
        {
            if (Application.isEditor)
            {
                return;
            }

            Debug.Log(gameObject.name + ".OnAdLoadedEventBanner");

            MaxSdk.ShowBanner(_adUnitIdBanner);
        }

        private void OnAdLoadFailedEventBanner(string arg1, MaxSdkBase.ErrorInfo arg2)
        {
            if (Application.isEditor)
            {
                return;
            }

            Debug.Log(gameObject.name + ".OnAdLoadFailedEventBanner");
        }

        private void OnAdClickedEventBanner(string arg1, MaxSdkBase.AdInfo arg2)
        {
            if (Application.isEditor)
            {
                return;
            }

            Debug.Log(gameObject.name + ".OnAdClickedEventBanner");
        }

        private void OnAdRevenuePaidEventBanner(string arg1, MaxSdkBase.AdInfo arg2)
        {
            if (Application.isEditor)
            {
                return;
            }

            Debug.Log(gameObject.name + ".OnAdRevenuePaidEventBanner adInfo:" + arg2.ToString());
            
            //todo okan hammerlytics
            // Core.Hammer.Hammerlytics.ActionHammerlyticsOnAdRevenueEarned?.Invoke("MAX", arg2.AdFormat, arg2.AdUnitIdentifier, arg2.Placement, arg2.Revenue, arg2.NetworkName, arg2.NetworkPlacement, arg2.DspName, arg2.CreativeIdentifier);
            Core.Hammer.ActionAttributionAdRevenueEventMaxSdk?.Invoke(arg2.NetworkName, arg2.Revenue, "USD");
        }

        private void OnAdReviewCreativeIdGeneratedEventBanner(string arg1, string arg2, MaxSdkBase.AdInfo arg3)
        {
            if (Application.isEditor)
            {
                return;
            }

            Debug.Log(gameObject.name + ".OnAdReviewCreativeIdGeneratedEventBanner");
        }

        private void OnAdExpandedEventBanner(string arg1, MaxSdkBase.AdInfo arg2)
        {
            if (Application.isEditor)
            {
                return;
            }

            Debug.Log(gameObject.name + ".OnAdExpandedEventBanner");
        }

        private void OnAdCollapsedEventBanner(string arg1, MaxSdkBase.AdInfo arg2)
        {
            if (Application.isEditor)
            {
                return;
            }

            Debug.Log(gameObject.name + ".OnAdCollapsedEventBanner");
        }

        #endregion

        #endregion

        #region INTERSTITIAL

        private Action _interstitialOnLoadSuccess;
        private Action<string> _interstitialOnLoadFail;
        private Action _interstitialOnShowSuccess;
        private Action<string> _interstitialOnShowFail;

        private void HasInterstitial(Action onSuccess = null, Action<string> onFail = null)
        {
            if (Application.isEditor)
            {
                onSuccess?.Invoke();
                return;
            }

            Debug.Log(gameObject.name + "HasInterstitial start");
            if (MaxSdk.IsInterstitialReady(_adUnitIdInterstitial))
            {
                _interstitialOnLoadSuccess = null;
                _interstitialOnLoadFail = null;
                Debug.Log(gameObject.name + "HasInterstitial end was ready");
                onSuccess?.Invoke();
            }
            else
            {
                _interstitialOnLoadSuccess = onSuccess;
                _interstitialOnLoadFail = onFail;
                Debug.Log(gameObject.name + "HasInterstitial end loading");
                MaxSdk.LoadInterstitial(_adUnitIdInterstitial);
            }
        }

        private void ShowInterstitial(Action onSuccess = null, Action<string> onFail = null)
        {
            if (Application.isEditor)
            {
                onSuccess?.Invoke();
                return;
            }

            Debug.Log(gameObject.name + ".ShowInterstitial start");

            if (!MaxSdk.IsInterstitialReady(_adUnitIdInterstitial))
            {
                Debug.LogError(gameObject.name + ".ShowInterstitial end error: you must invoke HasInterstitial first!");
                onFail?.Invoke(gameObject.name + ".ShowInterstitial end error: you must invoke HasInterstitial first!");
                _interstitialOnShowSuccess = null;
                _interstitialOnShowFail = null;
            }
            else
            {
                _interstitialOnShowSuccess = onSuccess;
                _interstitialOnShowFail = onFail;
                MaxSdk.ShowInterstitial(_adUnitIdInterstitial);
                Debug.Log(gameObject.name + ".ShowInterstitial end showing");
            }
        }

        #region INTERSTITIAL_CALLBACKS

        private void OnAdLoadedEventInterstitial(string arg1, MaxSdkBase.AdInfo arg2)
        {
            if (Application.isEditor)
            {
                return;
            }

            Debug.Log(gameObject.name + ".OnAdLoadedEventInterstitial");

            _interstitialOnLoadSuccess?.Invoke();
            _interstitialOnLoadSuccess = null;
            _interstitialOnLoadFail = null;
        }

        private void OnAdLoadFailedEventInterstitial(string arg1, MaxSdkBase.ErrorInfo arg2)
        {
            if (Application.isEditor)
            {
                return;
            }

            Debug.Log(gameObject.name + ".OnAdLoadFailedEventInterstitial, code: " + arg2.Code + ", description: " +
                      arg2.AdLoadFailureInfo);

            _interstitialOnLoadFail?.Invoke(arg2.Code + " : " + arg2.AdLoadFailureInfo);
            _interstitialOnLoadSuccess = null;
            _interstitialOnLoadFail = null;
        }

        private void OnAdDisplayedEventInterstitial(string arg1, MaxSdkBase.AdInfo arg2)
        {
            if (Application.isEditor)
            {
                return;
            }

            Debug.Log(gameObject.name + ".OnAdDisplayedEventInterstitial");

            // _interstitialOnShowSuccess?.Invoke();
            // _interstitialOnShowSuccess = null;
            // _interstitialOnShowFail = null;
        }

        private void OnAdDisplayFailedEventInterstitial(string arg1, MaxSdkBase.ErrorInfo arg2, MaxSdkBase.AdInfo arg3)
        {
            if (Application.isEditor)
            {
                return;
            }

            Debug.Log(gameObject.name + ".OnAdDisplayFailedEventInterstitial, code: " + arg2.Code + ", description: " +
                      arg2.AdLoadFailureInfo);

            _interstitialOnShowFail?.Invoke(arg2.Code + " : " + arg2.AdLoadFailureInfo);
            _interstitialOnShowSuccess = null;
            _interstitialOnShowFail = null;
        }

        private void OnAdClickedEventInterstitial(string arg1, MaxSdkBase.AdInfo arg2)
        {
            if (Application.isEditor)
            {
                return;
            }

            Debug.Log(gameObject.name + ".OnAdClickedEventInterstitial");

            // _interstitialOnShowSuccess?.Invoke();
            // _interstitialOnShowSuccess = null;
            // _interstitialOnShowFail = null;
        }

        private void OnAdRevenuePaidEventInterstitial(string arg1, MaxSdkBase.AdInfo arg2)
        {
            if (Application.isEditor)
            {
                return;
            }

            Debug.Log(gameObject.name + ".OnAdRevenuePaidEventInterstitial adInfo:" + arg2.ToString());

            // _interstitialOnShowSuccess?.Invoke();
            // _interstitialOnShowSuccess = null;
            // _interstitialOnShowFail = null;
         
            //todo okan hammerlytics
            // Core.Hammer.Hammerlytics.ActionHammerlyticsOnAdRevenueEarned?.Invoke("MAX", arg2.AdFormat, arg2.AdUnitIdentifier, arg2.Placement, arg2.Revenue, arg2.NetworkName, arg2.NetworkPlacement, arg2.DspName, arg2.CreativeIdentifier);
            Core.Hammer.ActionAttributionAdRevenueEventMaxSdk?.Invoke(arg2.NetworkName, arg2.Revenue, "USD");
        }

        private void OnAdReviewCreativeIdGeneratedEventInterstitial(string arg1, string arg2, MaxSdkBase.AdInfo arg3)
        {
            if (Application.isEditor)
            {
                return;
            }

            Debug.Log(gameObject.name + ".OnAdReviewCreativeIdGeneratedEventInterstitial");
        }

        private void OnAdHiddenEventInterstitial(string arg1, MaxSdkBase.AdInfo arg2)
        {
            if (Application.isEditor)
            {
                return;
            }

            Debug.Log(gameObject.name + ".OnAdHiddenEventInterstitial");

            // _interstitialOnShowFail?.Invoke(
            //     "Manually failing as a failsafe, as we received ad hidden event before others, which is an unexpected behaviour; but can happen sometimes");
            // _interstitialOnShowSuccess = null;
            // _interstitialOnShowFail = null;
            
            _interstitialOnShowSuccess?.Invoke();
            _interstitialOnShowSuccess = null;
            _interstitialOnShowFail = null;
        }

        #endregion

        #endregion

        #region REWARDED

        private Action _rewardedOnLoadSuccess;
        private Action<string> _rewardedOnLoadFail;
        private Action _rewardedOnShowSuccess;
        private Action<string> _rewardedOnShowFail;

        public void HasRewarded(Action onSuccess = null, Action<string> onFail = null)
        {
            if (Application.isEditor)
            {
                onSuccess?.Invoke();
                return;
            }

            Debug.Log(gameObject.name + "HasRewarded start");
            if (MaxSdk.IsRewardedAdReady(_adUnitIdRewarded))
            {
                _rewardedOnLoadSuccess = null;
                _rewardedOnLoadFail = null;
                Debug.Log(gameObject.name + "HasRewarded end was ready");
                onSuccess?.Invoke();
            }
            else
            {
                _rewardedOnLoadSuccess = onSuccess;
                _rewardedOnLoadFail = onFail;
                Debug.Log(gameObject.name + "HasRewarded end loading");
                MaxSdk.LoadRewardedAd(_adUnitIdRewarded);
            }
        }

        public void ShowRewarded(Action onSuccess = null, Action<string> onFail = null)
        {
            if (Application.isEditor)
            {
                onSuccess?.Invoke();
                return;
            }

            Debug.Log(gameObject.name + ".ShowRewarded start");

            if (!MaxSdk.IsRewardedAdReady(_adUnitIdRewarded))
            {
                Debug.LogError(gameObject.name + ".ShowRewarded end error: you must invoke HasInterstitial first!");
                onFail?.Invoke(gameObject.name + ".ShowRewarded end error: you must invoke HasInterstitial first!");
                _rewardedOnShowSuccess = null;
                _rewardedOnShowFail = null;
            }
            else
            {
                _rewardedOnShowSuccess = onSuccess;
                _rewardedOnShowFail = onFail;
                MaxSdk.ShowRewardedAd(_adUnitIdRewarded);
                Debug.Log(gameObject.name + ".ShowRewarded end showing");
            }
        }

        #region REWARDED_CALLBACKS

        private void OnAdLoadedEventRewarded(string arg1, MaxSdkBase.AdInfo arg2)
        {
            if (Application.isEditor)
            {
                return;
            }

            Debug.Log(gameObject.name + ".OnAdLoadedEventRewarded");

            _rewardedOnLoadSuccess?.Invoke();
            _rewardedOnLoadSuccess = null;
            _rewardedOnLoadFail = null;
        }

        private void OnAdLoadFailedEventRewarded(string arg1, MaxSdkBase.ErrorInfo arg2)
        {
            if (Application.isEditor)
            {
                return;
            }

            Debug.Log(gameObject.name + ".OnAdLoadFailedEventRewarded, code: " + arg2.Code + ", description: " +
                      arg2.AdLoadFailureInfo);

            _rewardedOnLoadFail?.Invoke(arg2.Code + " : " + arg2.AdLoadFailureInfo);
            _rewardedOnLoadSuccess = null;
            _rewardedOnLoadFail = null;
        }

        private void OnAdDisplayedEventRewarded(string arg1, MaxSdkBase.AdInfo arg2)
        {
            if (Application.isEditor)
            {
                return;
            }

            Debug.Log(gameObject.name + ".OnAdDisplayedEventRewarded");
        }

        private void OnAdDisplayFailedEventRewarded(string arg1, MaxSdkBase.ErrorInfo arg2, MaxSdkBase.AdInfo arg3)
        {
            if (Application.isEditor)
            {
                return;
            }

            Debug.Log(gameObject.name + ".OnAdDisplayFailedEventRewarded, code :  " + arg2.Code + ", description : " +
                      arg2.AdLoadFailureInfo);

            _rewardedOnShowFail?.Invoke(arg2.Code + " : " + arg2.AdLoadFailureInfo);
            _rewardedOnShowSuccess = null;
            _rewardedOnShowFail = null;
        }

        private void OnAdClickedEventRewarded(string arg1, MaxSdkBase.AdInfo arg2)
        {
            if (Application.isEditor)
            {
                return;
            }

            Debug.Log(gameObject.name + ".OnAdClickedEventRewarded");

            // _rewardedOnShowSuccess?.Invoke();
            // _rewardedOnShowSuccess = null;
            // _rewardedOnShowFail = null;
        }

        private void OnAdRevenuePaidEventRewarded(string arg1, MaxSdkBase.AdInfo arg2)
        {
            if (Application.isEditor)
            {
                return;
            }

            Debug.Log(gameObject.name + ".OnAdRevenuePaidEventRewarded adInfo:" + arg2.ToString());

            // _rewardedOnShowSuccess?.Invoke();
            // _rewardedOnShowSuccess = null;
            // _rewardedOnShowFail = null;
         
            //todo okan hammerlytics
            // Core.Hammer.Hammerlytics.ActionHammerlyticsOnAdRevenueEarned?.Invoke("MAX", arg2.AdFormat, arg2.AdUnitIdentifier, arg2.Placement, arg2.Revenue, arg2.NetworkName, arg2.NetworkPlacement, arg2.DspName, arg2.CreativeIdentifier);
            Core.Hammer.ActionAttributionAdRevenueEventMaxSdk?.Invoke(arg2.NetworkName, arg2.Revenue, "USD");
        }

        private void OnAdReviewCreativeIdGeneratedEventRewarded(string arg1, string arg2, MaxSdkBase.AdInfo arg3)
        {
            if (Application.isEditor)
            {
                return;
            }

            Debug.Log(gameObject.name + ".OnAdReviewCreativeIdGeneratedEventRewarded");
        }

        private void OnAdReceivedRewardEventRewarded(string arg1, MaxSdkBase.Reward arg2, MaxSdkBase.AdInfo arg3)
        {
            if (Application.isEditor)
            {
                return;
            }

            Debug.Log(gameObject.name + ".OnAdReceivedRewardEventRewarded, amount = " + arg2.Amount + " name = " +
                      arg2.Label);

            _rewardedOnShowSuccess?.Invoke();
            _rewardedOnShowSuccess = null;
            _rewardedOnShowFail = null;
        }

        private void OnAdHiddenEventRewarded(string arg1, MaxSdkBase.AdInfo arg2)
        {
            if (Application.isEditor)
            {
                return;
            }

            Debug.Log(gameObject.name + ".OnAdHiddenEventRewarded");

            _rewardedOnShowFail?.Invoke(
                "Manually failing as a failsafe, as we received ad hidden event before others, which is an unexpected behaviour; but can happen sometimes");
            _rewardedOnShowSuccess = null;
            _rewardedOnShowFail = null;
        }

        #endregion

        #endregion
    }
}