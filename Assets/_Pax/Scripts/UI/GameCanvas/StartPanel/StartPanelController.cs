namespace Pax
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class StartPanelController : MonoBehaviour, IPanelController
    {
        [SerializeField] Button _button;

        private void Awake()
        {
            _button.onClick.AddListener(ClickButton);
        }

        void ClickButton()
        {
            Debug.Log("Game is started");
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