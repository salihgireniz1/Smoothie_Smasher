using System;
using UnityEngine;

namespace Udo.Hammer.Runtime.Core
{
    public interface IHammerThirdPartySdk
    {
        public string DpsTemplateIdOrName();

        public void Initialize(Func<string> funcPrivacyGetUserId, Func<string, bool> funcPrivacyGetConsentByDpsTemplateIdOrName,
            Func<bool> funcPrivacyGetCcpaFlag, Func<bool> funcPrivacyGetCoppaFlag, Func<bool> funcPrivacyGetAttFlag,
            Transform hammerTransform);
    }
}