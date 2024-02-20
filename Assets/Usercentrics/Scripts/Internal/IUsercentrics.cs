using System.Collections.Generic;

namespace Unity.Usercentrics
{
    internal interface IUsercentricsPlatform
    {
        void Initialize(string initArgsJson);
        void ShowFirstLayer(bool isSystemBackButtonDisabled);
        void ShowSecondLayer(bool showCloseButton);
        string GetControllerID();
        void GetTCFData();
        void RestoreUserSession(string controllerId);
        void Reset();
        void SubscribeOnConsentUpdated();
        void DisposeOnConsentUpdatedSubscription();
        string GetUSPData();
        string GetFirstLayerSettings();
        void AcceptAll();
        void DenyAll();
        void Track(int eventType);
        void SetCmpId(int cmpId);
    }
}
