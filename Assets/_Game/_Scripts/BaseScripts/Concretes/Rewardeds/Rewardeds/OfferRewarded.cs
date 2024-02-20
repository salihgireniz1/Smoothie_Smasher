using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using PAG.UpgradeSystem;
using PAG.Managers;
using PAG.Utility;
//using Udo.Hammer.Runtime.Core;

public class OfferRewarded : Rewarded
{
    [SerializeField] Offer _firstOffer;
    [SerializeField] Offer _secondOffer;
    [SerializeField] private Button _lostButton;
    [SerializeField] private TextMeshProUGUI _timerText;

    private RewardedData _rewardedData;
    public override RewardedData RewardedData { get => _rewardedData; set => _rewardedData = value; }
    private PanelAnimation _panelAnimation;

    [SerializeField] private float _offerDuration = 15;
    private float _counter = 0;
    private bool _isOfferStart = false;
    private int _time = 1;

    UpgradableObjectData myData1;
    UpgradableObjectData myData2;

    private void Awake()
    {
        _panelAnimation = new PanelAnimation(transform);

    }

    private void Start()
    {
        _firstOffer.onRewardedAnimationEnd += CloseRewarded;
        _secondOffer.onRewardedAnimationEnd += CloseRewarded;
        _firstOffer.onRewardedClicked += StopTimer;
        _secondOffer.onRewardedClicked += StopTimer;
        _lostButton.onClick.AddListener(CloseRewarded);
    }

    private void Update()
    {
        if (_isOfferStart)
        {
            _counter += Time.deltaTime;

            if(_counter > _time)
            {
                _time++;
                UpdateTimerText();

                if(_time == 4)
                {
                    OpenLostButton();
                }

                if (_counter > _offerDuration)
                {
                    CloseRewarded();
                }
            }

        }
    }

    public override void CloseRewarded()
    {
        _panelAnimation.PlayCloseAnimation(()=>
        {
            ResetOffer();
            gameObject.SetActive(false);
        });
    }

    public override void OpenRewarded()
    {
        gameObject.SetActive(true);
        _panelAnimation.PlayOpenAnimation(0.1f);
    }

    public override void ShowRewarded()
    {
        ////_offerDuration = _rewardedData.rewardAmount;
        //int offerCount = Mathf.FloorToInt(_rewardedData.rewardAmount);
        //UpdateTimerText();
        //_isOfferStart = true;

        //// Get the list of UpgradableObjectData from the dictionary
        //List<UpgradableObjectData> objectDataList = UpgradeManager.Instance.enumDataDict.Values.ToList();

        //// Sort the list based on the price using OrderBy method
        //List<UpgradableObjectData> sortedObjectDataList = objectDataList.OrderBy(data => data.GetCurrentPrice()).ToList();

        //// Return the first two elements of the sorted list
        //List<UpgradableObjectData> firstTwoElements = sortedObjectDataList.Take(2).ToList();
        //List<UpgradableObjectData> randomTwoElement = new List<UpgradableObjectData>();

        //for (int i = 0; i < 2; i++)
        //{
        //    int randomIndex = Random.Range(0, sortedObjectDataList.Count);
        //    UpgradableObjectData randomData = sortedObjectDataList[randomIndex];
        //    sortedObjectDataList.Remove(randomData);
        //    randomTwoElement.Add(randomData);
        //}
        

        //myData1 = randomTwoElement[0];

        ////string currentToNext = $"{GetCurrentPowerColoredText(myData1)}>{GetNextPowerColoredText(myData1)}";
        ////string longExp = myData1.longExplanation;

        ////string coloredExplanationText = PAGText.GetColoredText(longExp, Color.yellow);
        ////string wholeScript = $"{currentToNext} {coloredExplanationText}";
        
        //string infoText = "+" + _rewardedData.rewardAmount.ToString() + " " + _rewardedData.rewardText;
        //string headerText1 = myData1.displayName;
        //_firstOffer.SetHeaderText(headerText1);

        //for (int i = 0; i < offerCount; i++)
        //{
        //    _firstOffer.InitializeOffer(firstTwoElements[0].upgradeSprite, infoText, FreeUpgrade1);
        //}


        //myData2 = randomTwoElement[1];

        //string headerText2 = myData2.displayName;
        //_secondOffer.SetHeaderText(headerText2);

        ////currentToNext = $"{GetCurrentPowerColoredText(myData2)}>{GetNextPowerColoredText(myData2)}";
        ////longExp = myData2.longExplanation;

        ////coloredExplanationText = PAGText.GetColoredText(longExp, Color.yellow);
        ////wholeScript = $"{currentToNext} {coloredExplanationText}";

        //for (int i = 0; i < offerCount; i++)
        //{
        //    _secondOffer.InitializeOffer(firstTwoElements[1].upgradeSprite, infoText, FreeUpgrade2);
        //}

        //string key = headerText1.ToString() + "and" + headerText2.ToString() + "Offered";
        ////Hammer.Instance.ANALYTICS_CustomEvent(key, LevelManager.Instance.SDKLevel);

    }

    private void FreeUpgrade1()
    {

        //ActiveUpgrades matchingKey = UpgradeManager.Instance.enumDataDict.FirstOrDefault(pair => pair.Value == myData1).Key;
        //Debug.Log("Upgrade for key = " + matchingKey.ToString());
    }
    private void FreeUpgrade2()
    {

        //ActiveUpgrades matchingKey = UpgradeManager.Instance.enumDataDict.FirstOrDefault(pair => pair.Value == myData2).Key;
        //UpgradeManager.Instance.UpgradeForFree(matchingKey);
        //Debug.Log("Upgrade for key = " + matchingKey.ToString());
    }
    string GetCurrentPowerColoredText(UpgradableObjectData myData)
    {
        float currentPower = myData.GetCurrentPower();
        string coloredCurrentPowerText = PAGText.GetColoredText(currentPower.ToString(), Color.yellow);
        if (myData.isPercentage)
        {
            coloredCurrentPowerText += "%";
        }
        return coloredCurrentPowerText;
    }
    string GetNextPowerColoredText(UpgradableObjectData myData)
    {
        if (myData.IsMax())
        {
            return "";
        }
        float nextPower = myData.GetNextPower();
        string coloredNextPowerText = PAGText.GetColoredText(nextPower.ToString(), Color.green);
        if (myData.isPercentage) coloredNextPowerText += "%";

        return coloredNextPowerText;
    }

    private void UpdateTimerText()
    {
        _timerText.text = "Time: " + (_offerDuration - _counter).ToString("F0") + "sec";
    }

    private void ResetOffer()
    {
        _isOfferStart = false;
        _counter = 0;
        _time = 1;
        CloseLostButton();
    }

    public void OpenLostButton()
    {
        _lostButton.gameObject.SetActive(true);
    }

    public void CloseLostButton()
    {
        _lostButton.gameObject.SetActive(false);
    }

    private void StopTimer()
    {
        _isOfferStart = false;
    }
}
