using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using PAG.Pool;
using Udo.Hammer.Runtime.Core;

public class UIManager : MonoBehaviour
{
    [SerializeField] private InGamePanelController _inGamePanelController;
    [SerializeField] private StartPanelController _startPanelController;
    [SerializeField] private WinPanelController _winPanelController;
    [SerializeField] private LosePanelController _losePanelController;
    [SerializeField] private UIParticleController _uiParticlController;
    [SerializeField] private Animator _levelTransitionAnimator;

    private void Start()
    {
        Hammer.Instance.MEDIATION_ShowBanner();
        Hammer.Instance.MEDIATION_HasInterstitial(
            () =>
            {
                //Debug.Log("zaten yüklenmiş Inter mevcut veya Inter şimdi yüklenebildi");
            },
                s =>
                {
                    //Debug.Log("Inter yüklenemedi");
                }
                );


        Hammer.Instance.MEDIATION_HasRewarded(
        () =>
        {
            //Debug.Log("zaten yüklenmiş Rewarded mevcut veya Rewarded şimdi yüklenebildi");
        },
        s =>
        {
            //Debug.Log("Rewarded yüklenemedi");
        }
        );



    }




    private void StartTransitionAnimation()
    {
        _levelTransitionAnimator.SetTrigger("Start");
    }


    private void OnEnable()
    {
        EventManagement.OnActiveLosePanel += _losePanelController.OpenPanel;
        EventManagement.OnActiveWinPanel += _winPanelController.OpenPanel;
        EventManagement.OnActiveStartPanel += _startPanelController.OpenPanel;
        _winPanelController.OnOpenWinPanel += _uiParticlController.PlayConfettiParticle;
        EventManagement.OnTransitionStart += StartTransitionAnimation;
    }

    private void OnDisable()
    {
        EventManagement.OnActiveLosePanel -= _losePanelController.OpenPanel;
        EventManagement.OnActiveWinPanel -= _winPanelController.OpenPanel;
        EventManagement.OnActiveStartPanel -= _startPanelController.OpenPanel;
        _winPanelController.OnOpenWinPanel -= _uiParticlController.PlayConfettiParticle;
        EventManagement.OnTransitionStart -= StartTransitionAnimation;

    }
}
