#if UNITY_IOS
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Unity.Usercentrics
{
    internal class UsercentricsIOS: IUsercentricsPlatform
    {
        [DllImport("__Internal")]
        private static extern void initCMP(string initArgsJson);
    
        [DllImport("__Internal")]
        private static extern void showFirstLayer();

        [DllImport("__Internal")]
        private static extern void showSecondLayer(bool showCloseButton);
    
        [DllImport("__Internal")]
        private static extern string getControllerId();

        [DllImport("__Internal")]
        private static extern void getTCFData();

        [DllImport("__Internal")]
        private static extern string getUSPData();

        [DllImport("__Internal")]
        private static extern void restoreUserSession(string controllerId);

        [DllImport("__Internal")]
        private static extern void reset();

        [DllImport("__Internal")]
        private static extern void subscribeOnConsentUpdated();

        [DllImport("__Internal")]
        private static extern void disposeOnConsentUpdatedSubscription();

        [DllImport("__Internal")]
        private static extern string getFirstLayerSettings();

        [DllImport("__Internal")]
        private static extern void acceptAll();

        [DllImport("__Internal")]
        private static extern void denyAll();

        [DllImport("__Internal")]
        private static extern void track(int eventType);

        [DllImport("__Internal")]
        private static extern void setCmpId(int cmpId);

        public void Initialize(string initArgsJson)
        {
            initCMP(initArgsJson);
        }

        public void ShowFirstLayer(bool isSystemBackButtonDisabled)
        {
            showFirstLayer();
        }

        public void ShowSecondLayer(bool showCloseButton)
        {
            showSecondLayer(showCloseButton);
        }

        public string GetControllerID()
        {
            return getControllerId();
        }

        public void GetTCFData()
        {
            getTCFData();
        }

        public string GetUSPData()
        {
            return getUSPData();
        }

        public void RestoreUserSession(string controllerId)
        {
            restoreUserSession(controllerId);
        }

        public void Reset()
        {
            reset();
        }

        public void SubscribeOnConsentUpdated()
        {
            subscribeOnConsentUpdated();
        }

        public void DisposeOnConsentUpdatedSubscription()
        {
            disposeOnConsentUpdatedSubscription();
        }

        public string GetFirstLayerSettings()
        {
            return getFirstLayerSettings();
        }

        public void AcceptAll()
        {
            acceptAll();
        }

        public void DenyAll()
        {
            denyAll();
        }

        public void Track(int eventType)
        {
            track(eventType);
        }

        public void SetCmpId(int cmpId)
        {
            setCmpId(cmpId);
        }
    }
}
#endif
