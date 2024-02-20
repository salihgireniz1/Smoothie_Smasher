namespace Pax
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class InGamePanelController : MonoBehaviour, IPanelController
    {
        Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void ClosePanel(EventArgs args)
        {
            _animator.SetBool("isClosed", true);
        }

        public void OpenPanel(EventArgs args)
        {
            gameObject.SetActive(true);
            _animator.SetBool("isClosed", false);
        }
    }

}