using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MoreMountains.Feedbacks;
using DG.Tweening;
//using Udo.Hammer.Runtime.Core;

public class Offer : MonoBehaviour
{
    public event System.Action onRewardWon;
    public event System.Action onRewardedClicked;
    public event System.Action onRewardedAnimationEnd;

    [SerializeField] Button _button;
    [SerializeField] Image _upgrageImage;
    [SerializeField] TextMeshProUGUI _infoText;
    [SerializeField] TextMeshProUGUI _headerText;
    [SerializeField] MMF_Player _buttonFeedback;


    private string _header = "headerNon";

    private void OnEnable()
    {
        _buttonFeedback.Initialization();
    }

    private void OnDisable()
    {
        ResetOffer();
        _button.onClick.RemoveListener(PlayRewarded);
    }

    private void PlayRewarded()
    {
        //onRewardedClicked?.Invoke();
        //_button.interactable = false;
        //_buttonFeedback.StopFeedbacks();
        //_buttonFeedback.ResetFeedbacks();

        //string rewardedName = _header;
        //string key = rewardedName + "offerClick";
        //Hammer.Instance.ANALYTICS_CustomEvent(key, LevelManager.Instance.SDKLevel);
        //RewardedController.Instance.OpenAdBreak();

        //Hammer.Instance.MEDIATION_HasRewarded(() =>
        //{
        //    Hammer.Instance.MEDIATION_ShowRewarded(() =>
        //    {
        //        onRewardWon?.Invoke();
        //        string key = rewardedName + "offerSuccess";
        //        Hammer.Instance.ANALYTICS_CustomEvent(key, LevelManager.Instance.SDKLevel);
        //        AnimateRewarded();
        //        RewardedController.Instance.CloseAdBreak();

        //    },
        //    s =>
        //    {
        //        //Debug.Log("Rewarded gösterilemedi");
        //        string key = rewardedName + "offerFail";
        //        Hammer.Instance.ANALYTICS_CustomEvent(key, LevelManager.Instance.SDKLevel);
        //        RewardedController.Instance.CloseAdBreak();

        //    });
        //},
        //    s =>
        //    {
        //        //Debug.Log("Rewarded yüklenemedi");
        //        key = rewardedName + "offerHasRewardedFalse";
        //        Hammer.Instance.ANALYTICS_CustomEvent(key, LevelManager.Instance.SDKLevel);
        //        RewardedController.Instance.CloseAdBreak();

        //    });

    }

    public void InitializeOffer(Sprite upgradeSprite, string infoText, System.Action OnRewardWon)
    {
        _upgrageImage.sprite = upgradeSprite;
        _infoText.text = infoText;
        onRewardWon += OnRewardWon;
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(PlayRewarded);
        _buttonFeedback.Initialization();
        gameObject.SetActive(true);
        
    }

    public void ResetOffer()
    {
        _button.interactable = true;
        onRewardWon = null;

    }

    public void SetHeaderText(string text)
    {
        _header = text;
        _headerText.text = _header;
    }

    private void AnimateRewarded()
    {
        //Vector3 upgradeCanvasPosition = FindObjectOfType<UpgradeButton>().transform.position;
        Image upgradeImage = Instantiate(_upgrageImage,transform);
        upgradeImage.transform.localScale = _upgrageImage.transform.localScale * 2.5f;
        Vector3 initialScale = upgradeImage.transform.localScale ;
        Vector3 bigScale = initialScale * 1.3f;

        Sequence seq = DOTween.Sequence();
        seq.Append(upgradeImage.transform.DOScale(bigScale, 0.3f).SetEase(Ease.OutBounce))
            //.Append(upgradeImage.transform.DOMove(upgradeCanvasPosition, 0.5f).SetEase(Ease.Linear))
            .Join(upgradeImage.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.Linear).SetDelay(0.2f)).OnComplete(() =>
            {
                Destroy(upgradeImage);
                onRewardedAnimationEnd?.Invoke();
            });

    }
}