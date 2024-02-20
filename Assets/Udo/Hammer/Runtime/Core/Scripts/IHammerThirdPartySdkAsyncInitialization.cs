using System;
using System.Collections;
using UnityEngine;

namespace Udo.Hammer.Runtime.Core
{
    public interface IHammerThirdPartySdkAsyncInitialization : IHammerThirdPartySdk
    {
        public IEnumerator InitializeAsync(Func<string> funcPrivacyGetUserId,
            Func<string, bool> funcPrivacyGetConsentByDpsTemplateIdOrName,
            Func<bool> funcPrivacyGetCcpaFlag, Func<bool> funcPrivacyGetCoppaFlag, Func<bool> funcPrivacyGetAttFlag,
            Transform hammerTransform);
    }
}