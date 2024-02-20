using System;
using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Analytics;
using Udo.Hammer.Runtime.Core;
using UnityEngine;

namespace Udo.Hammer.Runtime._Firebase.Scripts
{
    public class HammerFirebase : MonoBehaviour, IHammerThirdPartySdkAsyncInitialization
    {
        private FirebaseApp _firebaseApp;
        private bool _initialized;
        private bool _hasConsent;

        public string DpsTemplateIdOrName()
        {
            return "uNl9XGnZC";
        }

        private void Awake()
        {
            Core.Hammer.ActionsAnalyticsInitAsync.Add(InitializeAsync);
            Core.Hammer.ActionsAnalyticsLevelStart.Add(LevelStart);
            Core.Hammer.ActionsAnalyticsLevelComplete.Add(LevelComplete);
            Core.Hammer.ActionsAnalyticsLevelFail.Add(LevelFail);
            Core.Hammer.ActionsAnalyticsCustomEvent.Add(CustomEvent);
            Core.Hammer.ActionsAnalyticsCustomEventDictionary.Add(CustomEvent);
        }

        public void Initialize(Func<string> funcPrivacyGetUserId,
            Func<string, bool> funcPrivacyGetConsentByDpsTemplateIdOrName, Func<bool> funcPrivacyGetCcpaFlag,
            Func<bool> funcPrivacyGetCoppaFlag, Func<bool> funcPrivacyGetAttFlag, Transform hammerTransform)
        {
            throw new NotImplementedException();
        }

        public IEnumerator InitializeAsync(Func<string> funcPrivacyGetUserId,
            Func<string, bool> funcPrivacyGetConsentByDpsTemplateIdOrName, Func<bool> funcPrivacyGetCcpaFlag,
            Func<bool> funcPrivacyGetCoppaFlag, Func<bool> funcPrivacyGetAttFlag, Transform hammerTransform)
        {
            _initialized = false;

            if (Application.isEditor)
            {
                yield break;
            }

            Debug.Log(gameObject.name + ".InitializeAsync start");

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

            var hasError = false;
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                FirebaseAnalytics.SetUserProperty(FirebaseAnalytics.UserPropertyAllowAdPersonalizationSignals,
                    _hasConsent ? "YES" : "NO");
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(_hasConsent);
                if (_hasConsent)
                {
                    FirebaseAnalytics.SetUserId(funcPrivacyGetUserId());
                }

                var dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available)
                {
                    _firebaseApp = FirebaseApp.DefaultInstance;
                    _initialized = true;
                    //todo okan ilrd
                }
                else
                {
                    Debug.Log(gameObject.name + ".InitializeAsync could not resolve all Firebase dependencies: " +
                              dependencyStatus);
                    _initialized = false;
                    hasError = true;
                }
            });

            while (true)
            {
                yield return new WaitForSeconds(0.1f);
                if (hasError)
                {
                    Debug.Log(gameObject.name + ".InitializeAsync end with error!");
                    yield break;
                }

                if (_initialized)
                {
                    Debug.Log(gameObject.name + ".InitializeAsync end");
                    yield break;
                }
            }
        }

        public void LevelStart(int level)
        {
            if (Application.isEditor)
            {
                return;
            }

            if (!_initialized)
            {
                return;
            }

            Debug.Log(gameObject.name + ".LevelStart level: " + level);

            FirebaseAnalytics.LogEvent("hammer_level_start", "level", "" + level);
        }

        public void LevelComplete(int level)
        {
            if (Application.isEditor)
            {
                return;
            }

            if (!_initialized)
            {
                return;
            }

            Debug.Log(gameObject.name + ".LevelComplete level: " + level);

            FirebaseAnalytics.LogEvent("hammer_level_end", "level", "" + level);
        }

        public void LevelFail(int level)
        {
            if (Application.isEditor)
            {
                return;
            }

            if (!_initialized)
            {
                return;
            }

            Debug.Log(gameObject.name + ".LevelFail level: " + level);

            FirebaseAnalytics.LogEvent("hammer_level_fail", "level", "" + level);
        }

        public void CustomEvent(string key, float value = float.NaN)
        {
            if (Application.isEditor)
            {
                return;
            }

            if (!_initialized)
            {
                return;
            }

            Debug.Log(gameObject.name + ".CustomEvent key: " + key + " , value: " + value);

            if (!string.IsNullOrEmpty(key))
            {
                while (key.Contains(" "))
                {
                    key = key.Replace(" ", "_");                    
                }
                while (key.Contains(":"))
                {
                    key = key.Replace(":", "_");                    
                }
                while (key.Contains("-"))
                {
                    key = key.Replace("-", "_");                    
                }
            }

            FirebaseAnalytics.LogEvent(key, "value", value);
        }

        public void CustomEvent(string key, Dictionary<string, object> parameters)
        {
            if (Application.isEditor)
            {
                return;
            }

            if (!_initialized)
            {
                return;
            }

            if (parameters.Keys.Count <= 0)
            {
                return;
            }
            
            if (!string.IsNullOrEmpty(key))
            {
                while (key.Contains(" "))
                {
                    key = key.Replace(" ", "_");                    
                }
                while (key.Contains(":"))
                {
                    key = key.Replace(":", "_");                    
                }
                while (key.Contains("-"))
                {
                    key = key.Replace("-", "_");                    
                }
            }

            var firebaseParams = new List<Parameter>();
            foreach (var parameter in parameters)
            {
                switch (parameter.Value)
                {
                    case string sValue:
                        firebaseParams.Add(new Parameter(parameter.Key, sValue));
                        break;
                    case int iValue:
                        firebaseParams.Add(new Parameter(parameter.Key, iValue));
                        break;
                    case float fValue:
                        firebaseParams.Add(new Parameter(parameter.Key, fValue));
                        break;
                    case long lValue:
                        firebaseParams.Add(new Parameter(parameter.Key, lValue));
                        break;
                    case double dValue:
                        firebaseParams.Add(new Parameter(parameter.Key, dValue));
                        break;
                    case bool bValue:
                        firebaseParams.Add(new Parameter(parameter.Key, bValue ? bool.TrueString : bool.FalseString));
                        break;
                }
            }

            Debug.Log(gameObject.name + ".CustomEvent key: " + key + " , dictionary: " + firebaseParams.ToArray());

            FirebaseAnalytics.LogEvent(key, firebaseParams.ToArray());
        }
    }
}