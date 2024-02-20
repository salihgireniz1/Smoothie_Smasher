namespace Pax
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class LosePanelController : MonoBehaviour, IPanelController
    {
        [SerializeField] Button _button;

        private void Awake()
        {
            _button.onClick.AddListener(ClickButton);
        }

        void ClickButton()
        {
            MainManager.Instance.EventManager.InvokeEvent(EventTypes.LoadCurrentScene);
            gameObject.SetActive(false);
        }

        public void ClosePanel(EventArgs args)
        {
            gameObject.SetActive(false);
        }

        public void OpenPanel(EventArgs args)
        {
            gameObject.SetActive(true);
        }
    }
}
