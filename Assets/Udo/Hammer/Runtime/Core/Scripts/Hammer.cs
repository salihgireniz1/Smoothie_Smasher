using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Udo.Hammer.Runtime.Core
{
    public class Hammer : HammerSingleton<Hammer>
    {
        #region EVENTS

        public UnityEvent onSdkInitializationStart = new();

        public UnityEvent onPrivacyInitializationStart = new();
        public UnityEvent onPrivacyGdprPopupStart = new();
        public UnityEvent onPrivacyGdprPopupEnd = new();

        // ReSharper disable once NotAccessedField.Global
        public UnityEvent onPrivacyAttPopupStart = new();

        // ReSharper disable once NotAccessedField.Global
        public UnityEvent onPrivacyAttPopupEnd = new();
        public UnityEvent onPrivacyInitializationEnd = new();

        public UnityEvent onAttributionInitializationStart = new();
        public UnityEvent onAttributionInitializationEnd = new();

        public UnityEvent onSocialsInitializationStart = new();
        public UnityEvent onSocialsInitializationEnd = new();

        public UnityEvent onMediationInitializationStart = new();
        public UnityEvent onMediationInitializationEnd = new();

        public UnityEvent onAnalyticsInitializationStart = new();
        public UnityEvent onAnalyticsInitializationEnd = new();

        public UnityEvent onSdkInitializationEnd = new();

        #endregion

        #region STATICS

        public static HammerConfigObject HammerConfigObject;

        #region STATICS_PRIVACY

        public static Func<Transform, IEnumerator> FuncPrivacyGdprPopup;

        // ReSharper disable once UnusedMember.Global
        public static Func<Transform, IEnumerator> FuncPrivacyAttPopup;
        public static Func<string, bool> FuncPrivacyGetConsentByDpsTemplateIdOrName;
        public static Func<string> FuncPrivacyGetUserId;
        public static Func<bool> FuncPrivacyGetCcpaFlag;
        public static Func<bool> FuncPrivacyGetCoppaFlag;
        public static Func<bool> FuncPrivacyGetAttFlag;

        #endregion

        #region STATICS_ATTRIBUTION

        public static Action<Func<string>, Func<string, bool>, Func<bool>, Func<bool>, Func<bool>, Transform>
            ActionAttributionInit;
        
        public static Action<string, double, string> ActionAttributionAdRevenueEventIronSource;
        public static Action<string, double, string> ActionAttributionAdRevenueEventMaxSdk;

        #endregion

        #region STATICS_SOCIALS

        public static readonly
            List<Action<Func<string>, Func<string, bool>, Func<bool>, Func<bool>, Func<bool>, Transform>>
            ActionsSocialsInit = new();

        #endregion

        #region STATICS_MEDIATION

        public static Action<Func<string>, Func<string, bool>, Func<bool>, Func<bool>, Func<bool>, Transform>
            ActionMediationInit;

        public static Func<Func<string>, Func<string, bool>, Func<bool>, Func<bool>, Func<bool>, Transform, IEnumerator>
            ActionMediationInitAsync;

        public static Action ActionMediationShowBanner;
        public static Action ActionMediationDestroyBanner;
        public static Action<Action, Action<string>> ActionMediationHasInterstitial;
        public static Action<Action, Action<string>> ActionMediationShowInterstitial;
        public static Action<Action, Action<string>> ActionMediationHasRewarded;
        public static Action<Action, Action<string>> ActionMediationShowRewarded;

        #endregion

        #region STATICS_ANALYTICS

        public static readonly
            List<Action<Func<string>, Func<string, bool>, Func<bool>, Func<bool>, Func<bool>, Transform>>
            ActionsAnalyticsInit = new();

        public static readonly
            List<Func<Func<string>, Func<string, bool>, Func<bool>, Func<bool>, Func<bool>, Transform, IEnumerator>>
            ActionsAnalyticsInitAsync = new();

        public static readonly List<Action<int>> ActionsAnalyticsLevelStart = new();
        public static readonly List<Action<int>> ActionsAnalyticsLevelComplete = new();
        public static readonly List<Action<int>> ActionsAnalyticsLevelFail = new();
        public static readonly List<Action<string, float>> ActionsAnalyticsCustomEvent = new();

        public static readonly List<Action<string, Dictionary<string, object>>> ActionsAnalyticsCustomEventDictionary =
            new();

        #endregion

        #region STATICS_HAMMERLYTICS

        //todo okan hammerlytics
        // public static class Hammerlytics
        // {
        //     public static Action<string, string, string, string, double, string, string, string, string>
        //         ActionHammerlyticsOnAdRevenueEarned;
        //
        //     public static
        //         Action<string, string, string, string, string, string, string, string, string, double, string, string>
        //         ActionHammerlyticsOnAttributionChanged;
        // }

        #endregion

        #endregion

        #region INIT

        protected override void AwakeImpl()
        {
            //Debug.Log(gameObject.name + ".AwakeImpl Initializing Hammer SDK");
        }

        private IEnumerator Start()
        {
            //Debug.Log(gameObject.name + ".Start start");
            onSdkInitializationStart?.Invoke();
            yield return HandlePrivacy();
            HandleAttribution();
            HandleSocials();
            yield return HandleMediation();
            yield return HandleAnalytics();
            onSdkInitializationEnd?.Invoke();
            //Debug.Log(gameObject.name + ".Start end");
        }

        private IEnumerator HandlePrivacy()
        {
            //Debug.Log(gameObject.name + ".HandlePrivacy start");
            onPrivacyInitializationStart?.Invoke();
            yield return HandlePrivacyGdprPopup();
#if UNITY_IOS
            yield return HandlePrivacyAttPopup();
#endif
            onPrivacyInitializationEnd?.Invoke();
            //Debug.Log(gameObject.name + ".HandlePrivacy end");
        }

        private IEnumerator HandlePrivacyGdprPopup()
        {
            //Debug.Log(gameObject.name + ".HandlePrivacyGdprPopup start");
            onPrivacyGdprPopupStart?.Invoke();
            if (!Application.isEditor) yield return FuncPrivacyGdprPopup(transform);

            onPrivacyGdprPopupEnd?.Invoke();
            //Debug.Log(gameObject.name + ".HandlePrivacyGdprPopup end");
        }

#if UNITY_IOS
        private IEnumerator HandlePrivacyAttPopup()
        {
            Debug.Log(gameObject.name + ".HandlePrivacyAttPopup start");
            onPrivacyAttPopupStart?.Invoke();
            if (!Application.isEditor)
            {
                yield return FuncPrivacyAttPopup(transform);
            }
            onPrivacyAttPopupEnd?.Invoke();
            Debug.Log(gameObject.name + ".HandlePrivacyAttPopup end");
        }
#endif

        private void HandleAttribution()
        {
            //Debug.Log(gameObject.name + ".HandleAttribution start");
            onAttributionInitializationStart?.Invoke();
            if (!Application.isEditor)
                ActionAttributionInit?.Invoke(FuncPrivacyGetUserId, FuncPrivacyGetConsentByDpsTemplateIdOrName,
                    FuncPrivacyGetCcpaFlag, FuncPrivacyGetCoppaFlag, FuncPrivacyGetAttFlag, transform);

            onAttributionInitializationEnd?.Invoke();
            //Debug.Log(gameObject.name + ".HandleAttribution end");
        }

        private void HandleSocials()
        {
            //Debug.Log(gameObject.name + ".HandleSocials start");
            onSocialsInitializationStart?.Invoke();
            if (!Application.isEditor)
                foreach (var action in ActionsSocialsInit)
                    action?.Invoke(FuncPrivacyGetUserId, FuncPrivacyGetConsentByDpsTemplateIdOrName, FuncPrivacyGetCcpaFlag,
                        FuncPrivacyGetCoppaFlag, FuncPrivacyGetAttFlag, transform);

            onSocialsInitializationEnd?.Invoke();
            //Debug.Log(gameObject.name + ".HandleSocials end");
        }

        private IEnumerator HandleMediation()
        {
            //Debug.Log(gameObject.name + ".HandleMediation start");
            onMediationInitializationStart?.Invoke();
            if (!Application.isEditor)
            {
                if (ActionMediationInit != null)
                    ActionMediationInit.Invoke(FuncPrivacyGetUserId, FuncPrivacyGetConsentByDpsTemplateIdOrName,
                        FuncPrivacyGetCcpaFlag,
                        FuncPrivacyGetCoppaFlag, FuncPrivacyGetAttFlag, transform);
                else if (ActionMediationInitAsync != null)
                    yield return ActionMediationInitAsync(FuncPrivacyGetUserId, FuncPrivacyGetConsentByDpsTemplateIdOrName,
                        FuncPrivacyGetCcpaFlag,
                        FuncPrivacyGetCoppaFlag, FuncPrivacyGetAttFlag, transform);
            }

            onMediationInitializationEnd?.Invoke();
            //Debug.Log(gameObject.name + ".HandleMediation end");
        }

        private IEnumerator HandleAnalytics()
        {
            //Debug.Log(gameObject.name + ".HandleAnalytics start");
            onAnalyticsInitializationStart?.Invoke();
            if (!Application.isEditor)
            {
                foreach (var action in ActionsAnalyticsInit)
                    action?.Invoke(FuncPrivacyGetUserId, FuncPrivacyGetConsentByDpsTemplateIdOrName, FuncPrivacyGetCcpaFlag,
                        FuncPrivacyGetCoppaFlag, FuncPrivacyGetAttFlag, transform);

                foreach (var func in ActionsAnalyticsInitAsync)
                    yield return func(FuncPrivacyGetUserId, FuncPrivacyGetConsentByDpsTemplateIdOrName, FuncPrivacyGetCcpaFlag,
                        FuncPrivacyGetCoppaFlag, FuncPrivacyGetAttFlag, transform);
            }

            onAnalyticsInitializationEnd?.Invoke();
            //Debug.Log(gameObject.name + ".HandleAnalytics end");
        }

        #endregion

        #region ANALYTICS

        #region ANALYTICS_PREDEFINED

        // ReSharper disable once UnusedMember.Global
        public void ANALYTICS_LevelStart(int level)
        {
            if (Application.isEditor) return;

            //Debug.Log(gameObject.name + ".ANALYTICS_LevelStart start level: " + level);
            foreach (var action in ActionsAnalyticsLevelStart) action?.Invoke(level);

            //Debug.Log(gameObject.name + ".ANALYTICS_LevelStart end");
        }

        // ReSharper disable once UnusedMember.Global
        public void ANALYTICS_LevelComplete(int level)
        {
            if (Application.isEditor) return;

            //Debug.Log(gameObject.name + ".ANALYTICS_LevelComplete start level: " + level);
            foreach (var action in ActionsAnalyticsLevelComplete) action?.Invoke(level);

            //Debug.Log(gameObject.name + ".ANALYTICS_LevelComplete end");
        }

        // ReSharper disable once UnusedMember.Global
        public void ANALYTICS_LevelFail(int level)
        {
            if (Application.isEditor) return;

            //Debug.Log(gameObject.name + ".ANALYTICS_LevelFail start level: " + level);
            foreach (var action in ActionsAnalyticsLevelFail) action?.Invoke(level);

            //Debug.Log(gameObject.name + ".ANALYTICS_LevelFail end");
        }

        #endregion

        #region ANALYTICS_CUSTOM

        // ReSharper disable once UnusedMember.Global
        public void ANALYTICS_CustomEvent(string key, float value = float.NaN)
        {
            if (Application.isEditor) return;

            //Debug.Log(gameObject.name + ".ANALYTICS_CustomEvent start key: " + key + " , value: " + value);
            foreach (var action in ActionsAnalyticsCustomEvent) action?.Invoke(key, value);

            //Debug.Log(gameObject.name + ".ANALYTICS_CustomEvent end");
        }

        /*
         * IMPORTANT, THIS FUNCTION IS NOT FULLY SUPPORTED YET FOR ALL ANALYTICS SDKS!
         */
        // ReSharper disable once UnusedMember.Global
        public void ANALYTICS_CustomEvent(string key, Dictionary<string, object> parameters)
        {
            if (Application.isEditor) return;

            //Debug.Log(gameObject.name + ".ANALYTICS_CustomEvent start key: " + key + " , parameters: " + parameters);
            foreach (var action in ActionsAnalyticsCustomEventDictionary) action?.Invoke(key, parameters);

            //Debug.Log(gameObject.name + ".ANALYTICS_CustomEvent end");
        }

        #endregion

        #endregion

        #region MEDIATION

        #region MEDIATION_BANNER

        // ReSharper disable once UnusedMember.Global
        public void MEDIATION_ShowBanner()
        {
            if (Application.isEditor) return;

            //Debug.Log(gameObject.name + ".MEDIATION_ShowBanner start");
            ActionMediationShowBanner?.Invoke();
            //Debug.Log(gameObject.name + ".MEDIATION_ShowBanner end");
        }

        // ReSharper disable once UnusedMember.Global
        public void MEDIATION_DestroyBanner()
        {
            if (Application.isEditor) return;

            //Debug.Log(gameObject.name + ".MEDIATION_DestroyBanner start");
            ActionMediationDestroyBanner?.Invoke();
            //Debug.Log(gameObject.name + ".MEDIATION_DestroyBanner end");
        }

        #endregion

        #region MEDIATION_INTERSTITIAL

        // ReSharper disable once UnusedMember.Global
        public void MEDIATION_HasInterstitial(Action onSuccess = null, Action<string> onFail = null)
        {
            if (Application.isEditor)
            {
                onSuccess?.Invoke();
                return;
            }

            //Debug.Log(gameObject.name + ".MEDIATION_HasInterstitial start");
            ActionMediationHasInterstitial?.Invoke(onSuccess, onFail);
            //Debug.Log(gameObject.name + ".MEDIATION_HasInterstitial end");
        }

        // ReSharper disable once UnusedMember.Global
        public void MEDIATION_ShowInterstitial(Action onSuccess = null, Action<string> onFail = null)
        {
            if (Application.isEditor)
            {
                onSuccess?.Invoke();
                return;
            }

            //Debug.Log(gameObject.name + ".MEDIATION_ShowInterstitial start");
            ActionMediationShowInterstitial?.Invoke(onSuccess, onFail);
            //Debug.Log(gameObject.name + ".MEDIATION_ShowInterstitial end");
        }

        #endregion

        #region MEDIATION_REWARDED

        // ReSharper disable once UnusedMember.Global
        public void MEDIATION_HasRewarded(Action onSuccess = null, Action<string> onFail = null)
        {
            if (Application.isEditor)
            {
                onSuccess?.Invoke();
                return;
            }

            //Debug.Log(gameObject.name + ".MEDIATION_HasRewarded start");
            ActionMediationHasRewarded?.Invoke(onSuccess, onFail);
            //Debug.Log(gameObject.name + ".MEDIATION_HasRewarded end");
        }

        // ReSharper disable once UnusedMember.Global
        public void MEDIATION_ShowRewarded(Action onSuccess = null, Action<string> onFail = null)
        {
            if (Application.isEditor)
            {
                onSuccess?.Invoke();
                return;
            }

            //Debug.Log(gameObject.name + ".MEDIATION_ShowRewarded start");
            ActionMediationShowRewarded?.Invoke(onSuccess, onFail);
            //Debug.Log(gameObject.name + ".MEDIATION_ShowRewarded end");
        }

        #endregion

        #endregion
    }
}