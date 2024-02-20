namespace Pax
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    public class LevelTransitionController : MonoBehaviour
    {
        Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            MainManager.Instance.EventManager.Register(EventTypes.LevelLoaded, Open);
            MainManager.Instance.EventManager.Register(EventTypes.LoadSceneStart, Close);
        }

        void Open(EventArgs args)
        {
            _animator.SetBool("isClosed", false);
        }

        void Close(EventArgs args)
        {
            _animator.SetBool("isClosed", true);
        }
    }

}