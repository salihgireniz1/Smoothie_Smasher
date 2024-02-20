using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Usercentrics;
using UnityEngine;

// ReSharper disable IdentifierTypo
namespace Udo.Hammer.Runtime._Usercentrics
{
    public class HammerUsercentrics : MonoBehaviour
    {
        [SerializeField] private GameObject gameObjectUsercentrics;
        [SerializeField] private GameObject gameObjectAtt;
        private Usercentrics _componentUsercentrics;
        private AutoInitialize _componentAutoInitialize;
        private AppTrackingTransparency _componentAtt;

        private bool _initializedGdpr;
        private bool _initializedAtt;
        private bool _attStatus;
        private List<UsercentricsServiceConsent> _consents;

        private void Awake()
        {
            Core.Hammer.FuncPrivacyGdprPopup = InitGdpr;
#if UNITY_IOS
            Core.Hammer.FuncPrivacyAttPopup = InitAtt;            
#endif
            Core.Hammer.FuncPrivacyGetConsentByDpsTemplateIdOrName = GetConsentByTemplateIdOrName;
            Core.Hammer.FuncPrivacyGetUserId = GetUserId;
            Core.Hammer.FuncPrivacyGetCcpaFlag = GetCcpaFlag;
            Core.Hammer.FuncPrivacyGetCoppaFlag = GetCoppaFlag;
            Core.Hammer.FuncPrivacyGetAttFlag = GetAttFlag;
            
            _componentUsercentrics = gameObjectUsercentrics.GetComponent<Usercentrics>();
            _componentAutoInitialize = gameObjectUsercentrics.GetComponent<AutoInitialize>();
            _componentAtt = gameObjectAtt.GetComponent<AppTrackingTransparency>();
        }

        private IEnumerator InitGdpr(Transform hammerTransform)
        {
            if (Application.isEditor)
            {
                yield break;
            }

            Debug.Log(gameObject.name + ".InitGdpr start");
            
            Usercentrics.Instance.Initialize((usercentricsReadyStatus) =>
                {
                    Debug.Log(gameObject.name + ".InitGdpr Usercentrics.Instance.Initialize start");
                    if (usercentricsReadyStatus.shouldCollectConsent)
                    {
                        Usercentrics.Instance.ShowFirstLayer((usercentricsConsentUserResponse) =>
                            {
                                _consents = usercentricsConsentUserResponse.consents;
                                _initializedGdpr = true;
                                Debug.Log(gameObject.name + ".InitGdpr Usercentrics.Instance.Initialize end from UI");
                            });
                    }
                    else
                    {
                        _consents = usercentricsReadyStatus.consents;
                        _initializedGdpr = true;
                        Debug.Log(gameObject.name + ".InitGdpr Usercentrics.Instance.Initialize end from cache");
                    }
                },
                (errorMessage) =>
                {
                    //todo okan hammerlytics
                    throw new Exception(errorMessage);
                });

            while (true)
            {
                yield return new WaitForSeconds(0.1f);
                if (_initializedGdpr)
                {
                    Usercentrics.Instance.gameObject.transform.parent = hammerTransform;
                    Debug.Log(gameObject.name + ".InitGdpr end");

                    foreach (var consent in _consents)
                    {
                        Debug.Log("dataProcessor: " + consent.dataProcessor + " , templateId: " + consent.templateId +
                                  " , status: " + consent.status + " , version: " + consent.version +
                                  " , isEssential: " + consent.isEssential);
                    }
                    
                    yield break;
                }
            }
        }

#if UNITY_IOS
        private IEnumerator InitAtt(Transform hammerTransform)
        {
            if (Application.isEditor)
            {
                yield break;
            }

            Debug.Log(gameObject.name + ".InitAtt start");

            AppTrackingTransparency.Instance.PromptForAppTrackingTransparency(arg0 =>
            {
                _initializedAtt = true;
                _attStatus = arg0 == AuthorizationStatus.AUTHORIZED;
                Debug.Log(gameObject.name + ".InitAtt ATT prompt done");
            });

            while (true)
            {
                yield return new WaitForSeconds(0.1f);
                if (_initializedAtt)
                {
                    AppTrackingTransparency.Instance.gameObject.transform.parent = hammerTransform;
                    Debug.Log(gameObject.name + ".InitAtt end");
                    yield break;
                }
            }
        }
#endif
        
        private bool GetConsentByTemplateIdOrName(string templateIdOrName)
        {
            if (Application.isEditor)
            {
                return true;
            }

            var result = false;

            //todo okan hardcoded workaround until Usercentrics ticket for custom DPS is resolved
            if (templateIdOrName == null)
            {
                result = true;
                Debug.Log(gameObject.name + ".GetConsentByTemplateIdOrName templateIdOrName: " + templateIdOrName + " , result: " + result);
                return result;
            }

            foreach (var consent in _consents)
            {
                if (!consent.templateId.Equals(templateIdOrName) 
                    && !consent.dataProcessor.Equals(templateIdOrName)) continue;
                
                result = consent.status;
                break;
            }

            Debug.Log(gameObject.name + ".GetConsentByTemplateIdOrName templateIdOrName: " + templateIdOrName + " , result: " + result);
            return result;
        }

        private string GetUserId()
        {
            if (Application.isEditor)
            {
                return "-1";
            }

            var result = Usercentrics.Instance.GetControllerId();
            Debug.Log(gameObject.name + ".GetUserId result: " + result);
            return result;
        }

        private bool GetCcpaFlag()
        {
            if (Application.isEditor)
            {
                return false;
            }

            var result = Usercentrics.Instance.GetUSPData().optedOut;
            Debug.Log(gameObject.name + ".GetCcpaFlag result: " + result);
            return result;
        }

        private bool GetCoppaFlag()
        {
            if (Application.isEditor)
            {
                return false;
            }

            var result = false; //todo okan COPPA
            Debug.Log(gameObject.name + ".GetCoppaFlag result: " + result);
            return result;
        }

        private bool GetAttFlag()
        {
            if (Application.isEditor)
            {
                return true;
            }

            var result = _attStatus;
            Debug.Log(gameObject.name + ".GetAttFlag result: " + result);
            return result;
        }
    }
}