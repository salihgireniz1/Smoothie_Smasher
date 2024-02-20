using System;
using GameAnalyticsSDK;
using GameAnalyticsSDK.Events;
using Udo.Hammer.Runtime.Core;
using UnityEngine;

namespace Udo.Hammer.Runtime._GameAnalytics
{
    public class HammerGameAnalytics : MonoBehaviour, IHammerThirdPartySdk
    {
        [SerializeField] private GameObject gameObjectGameAnalytics;
        private GameAnalytics _componentGameAnalytics;
        private GA_SpecialEvents _componentGaSpecialEvents;
        private bool _hasConsent;
        
        public string DpsTemplateIdOrName()
        {
            return "bQTbuxnTb";
        }
        
        private void Awake()
        {
            Core.Hammer.ActionsAnalyticsInit.Add(Initialize);
            Core.Hammer.ActionsAnalyticsLevelStart.Add(LevelStart);
            Core.Hammer.ActionsAnalyticsLevelComplete.Add(LevelComplete);
            Core.Hammer.ActionsAnalyticsLevelFail.Add(LevelFail);
            Core.Hammer.ActionsAnalyticsCustomEvent.Add(CustomEvent);

            if (Application.isEditor)
            {
                if (GameAnalytics.SettingsGA == null)
                {
                    return;
                }
            
                GameAnalytics.SettingsGA.SubmitErrors = false;
                GameAnalytics.SettingsGA.SubmitFpsAverage = false;
                GameAnalytics.SettingsGA.SubmitFpsCritical = false;
            }
        }
        
        private void OnDestroy()
        {
            if (Application.isEditor)
            {
                if (GameAnalytics.SettingsGA == null)
                {
                    return;
                }
                GameAnalytics.SettingsGA.SubmitErrors = true;
                GameAnalytics.SettingsGA.SubmitFpsAverage = true;
                GameAnalytics.SettingsGA.SubmitFpsCritical = true;
            }
        }
        
        public void Initialize(Func<string> funcPrivacyGetUserId, Func<string, bool> funcPrivacyGetConsentByDpsTemplateIdOrName, Func<bool> funcPrivacyGetCcpaFlag,
            Func<bool> funcPrivacyGetCoppaFlag, Func<bool> funcPrivacyGetAttFlag, Transform hammerTransform)
        {
            if (Application.isEditor)
            {
                return;
            }
            
            Debug.Log(gameObject.name + ".Initialize start");
            
            _componentGameAnalytics = gameObjectGameAnalytics.GetComponent<GameAnalytics>();
            _componentGaSpecialEvents = gameObjectGameAnalytics.GetComponent<GA_SpecialEvents>();
            
            _hasConsent = funcPrivacyGetConsentByDpsTemplateIdOrName(DpsTemplateIdOrName());
            
            if (funcPrivacyGetCcpaFlag())
            {
                _hasConsent = false;
            }
            
            //todo okan COPPA
            // if (funcPrivacyGetCoppaFlag())
            // {
            //     _hasConsent = false;
            // }
            
            GameAnalytics.SetEnabledEventSubmission(_hasConsent);
            if (_hasConsent)
            {
                GameAnalytics.SetCustomId(funcPrivacyGetUserId());                
            }
            GameAnalytics.Initialize();
            
            //todo okan ilrd
            // GameAnalyticsILRD.SubscribeMaxImpressions();
            // GameAnalyticsILRD.SubscribeIronSourceImpressions();
            
            Debug.Log(gameObject.name + ".Initialize end");
        }

        public void LevelStart(int level)
        {
            if (Application.isEditor)
            {
                return;
            }
            
            Debug.Log(gameObject.name + ".LevelStart level: " + level);
            
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "" + level);
        }

        public void LevelComplete(int level)
        {
            if (Application.isEditor)
            {
                return;
            }
            
            Debug.Log(gameObject.name + ".LevelComplete level: " + level);
            
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "" + level);
        }

        public void LevelFail(int level)
        {
            if (Application.isEditor)
            {
                return;
            }
            
            Debug.Log(gameObject.name + ".LevelFail level: " + level);

            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "" + level);
        }

        public void CustomEvent(string key, float value = float.NaN)
        {
            if (Application.isEditor)
            {
                return;
            }

            Debug.Log(gameObject.name + ".CustomEvent key: " + key + " , value: " + value);
            
            if (float.IsNaN(value))
            {
                GameAnalytics.NewDesignEvent(key);
            }
            else
            {
                GameAnalytics.NewDesignEvent(key, value);
            }
        }
    }
}