using System;
using Facebook.Unity;
using Udo.Hammer.Runtime.Core;
using UnityEngine;

namespace Udo.Hammer.Runtime._FacebookSDK
{
    public class HammerFacebookSDK : MonoBehaviour, IHammerThirdPartySdk
    {
        private bool _hasConsent;
        private Func<bool> _funcPrivacyGetCcpaFlag;
        
        public string DpsTemplateIdOrName()
        {
            return "ocv9HNX_g";
        }
        
        private void Awake()
        {
            Core.Hammer.ActionsSocialsInit.Add(Initialize);
        }
        
        public void Initialize(Func<string> funcPrivacyGetUserId, Func<string, bool> funcPrivacyGetConsentByDpsTemplateIdOrName, Func<bool> funcPrivacyGetCcpaFlag,
            Func<bool> funcPrivacyGetCoppaFlag, Func<bool> funcPrivacyGetAttFlag, Transform hammerTransform)
        {
            if (Application.isEditor)
            {
                return;
            }

            _funcPrivacyGetCcpaFlag = funcPrivacyGetCcpaFlag;
            
            _hasConsent = funcPrivacyGetConsentByDpsTemplateIdOrName(DpsTemplateIdOrName());
            
            //todo okan COPPA
            // if (funcPrivacyGetCoppaFlag())
            // {
            //     _hasConsent = false;
            // }
            
            if (!FB.IsInitialized) {
                // Initialize the Facebook SDK
                FB.Init(InitCallback, OnHideUnity);
            } else
            {
                HandleFacebookPrivacySettings();
                // Already initialized, signal an app activation App Event
                FB.ActivateApp();
            }
        }

        private void InitCallback ()
        {
            if (FB.IsInitialized) {
                HandleFacebookPrivacySettings();
                // Signal an app activation App Event
                FB.ActivateApp();
                // Continue with Facebook SDK
                // ...
            } else {
                Debug.Log("Failed to Initialize the Facebook SDK");
            }
        }

        private void HandleFacebookPrivacySettings()
        {
            if (_funcPrivacyGetCcpaFlag != null && _funcPrivacyGetCcpaFlag())
            {
                FB.Mobile.SetDataProcessingOptions(new string[] { "LDU" }, 1, 1000);
            }

            FB.Mobile.SetAutoLogAppEventsEnabled(_hasConsent);
#if UNITY_IOS
            FB.Mobile.SetAdvertiserTrackingEnabled(_hasConsent);
#endif
        }

        private void OnHideUnity (bool isGameShown)
        {
            if (!isGameShown) {
                // Pause the game - we will need to hide
                Time.timeScale = 0;
            } else {
                // Resume the game - we're getting focus again
                Time.timeScale = 1;
            }
        }
    }
}