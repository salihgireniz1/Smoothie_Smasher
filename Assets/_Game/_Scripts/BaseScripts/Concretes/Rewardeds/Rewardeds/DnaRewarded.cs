using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using PAG.Managers;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
//using Udo.Hammer.Runtime.Core;

public class DnaRewarded : Rewarded
{
    //[SerializeField] private float _amplitude;
    //[SerializeField] private float _time = 0;
    //[SerializeField] private int _rewardCount = 10;

    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _infoText;
    [SerializeField] private Animator _buttonAnimator;


    private RewardedData _rewardedData;
    public override RewardedData RewardedData { get => _rewardedData; set => _rewardedData = value; }

    private Vector3 _initialPosition;
    private Vector3 _targetPosition;
    private RectTransform _panelRectTransform;
    private Vector3 _buttonInitialScale;

  

    private void Awake()
    {
        _panelRectTransform = GetComponent<RectTransform>();

        _initialPosition = transform.position;
        _initialPosition.x = 0;
        _targetPosition = _initialPosition;
        _targetPosition.x = 1080f;
        _buttonInitialScale = _button.transform.localScale;
    }

    public override void ShowRewarded()
    {
        _infoText.text = "+" + _rewardedData.rewardAmount + " " + _rewardedData.rewardText;
        _buttonAnimator.SetBool("isFloating", true);
        transform.DOMove(_targetPosition, 10).SetEase(Ease.Linear).OnComplete(() =>
        {
            CloseRewarded();
            ResetRewarded();
        });
    }

    private void ResetRewarded()
    {
        Vector3 resetPosition = transform.position;
        resetPosition.x = 0;
        transform.position = resetPosition;
        _button.interactable = true;
    }

    public override void OpenRewarded()
    {
        gameObject.SetActive(true);
    }

    public override void CloseRewarded()
    {
        gameObject.SetActive(false);
    }

    public void OpenButton()
    {
        _button.interactable = true;
        _button.transform.localScale = Vector3.zero;
        _button.transform.DOScale(_buttonInitialScale, 0.2f).SetEase(Ease.Linear);
    }

    public void CloseButton()
    {
        _button.interactable = false;
        _button.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.Linear);
    }

    private void GetReward()
    {
        _button.interactable = false;
        //CurrencyManager.Instance.EarnCurrency(Currencies.DNA, _rewardedData.rewardAmount);
        //EventManagement.Invoke_OnCollectMoneyUI(Camera.main.ScreenToWorldPoint(transform.position), (int)_rewardedData.rewardAmount);
        CloseRewarded();
        ResetRewarded();
    }

    public void PlayRewarded()
    {
        //string rewardedName = _rewardedData.rewardedType.ToString() + _rewardedData.rewardAmount.ToString();
        //string key = rewardedName + "Click";
        //Hammer.Instance.ANALYTICS_CustomEvent(key, LevelManager.Instance.SDKLevel);

        //if (!_rewardedData.isFree)
        //{
        //    RewardedController.Instance.OpenAdBreak();

        //    Hammer.Instance.MEDIATION_HasRewarded(() =>
        //    {
        //        Hammer.Instance.MEDIATION_ShowRewarded(() =>
        //        {
        //            GetReward();
        //            //string key = rewardedName + "Success";
        //            Hammer.Instance.ANALYTICS_CustomEvent(key, LevelManager.Instance.SDKLevel);
        //            RewardedController.Instance.CloseAdBreak();

        //        },
        //        s =>
        //        {
        //            //Debug.Log("Rewarded gösterilemedi");
        //            CloseRewarded();
        //            ResetRewarded();
        //            string key = rewardedName + "Fail";
        //            Hammer.Instance.ANALYTICS_CustomEvent(key, LevelManager.Instance.SDKLevel);
        //            RewardedController.Instance.CloseAdBreak();

        //        });
        //    },
        //    s =>
        //    {
        //        //Debug.Log("Rewarded yüklenemedi");
        //        CloseRewarded();
        //        ResetRewarded();
        //        string key = rewardedName + "hasRewardedFalse";
        //        Hammer.Instance.ANALYTICS_CustomEvent(key, LevelManager.Instance.SDKLevel);
        //        RewardedController.Instance.CloseAdBreak();

        //    }
        //    );
        //}
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(PlayRewarded);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(PlayRewarded);
    }
}