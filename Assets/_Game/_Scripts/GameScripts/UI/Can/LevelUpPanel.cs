using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Udo.Hammer.Runtime.Core;

public class LevelUpPanel : MonoBehaviour
{
    GroundSizeController groundSizeController;
    FruitUpgradeManager fruitUpgradeManager;
    UIParticleController uIParticleController;
    //InGamePanelController inGamePanelController;
    OrderManager orderManager;
    public GameObject FruitPart, CapacityPart, FieldPart, RequestPart, RewardedPart;
    public TextMeshProUGUI FruitName, OldCapacityText, NewCapacityText, RewardedAmount;
    public Image FruitImage;
    public bool nextButton = false;
    public CursorRewardedController cursorRewardedController;
    NumberFormatManager numberFormatManager = new();

    void Awake()
    {
        uIParticleController = FindObjectOfType<UIParticleController>();
        fruitUpgradeManager = FindObjectOfType<FruitUpgradeManager>();
        //inGamePanelController = FindObjectOfType<InGamePanelController>();
        orderManager = FindObjectOfType<OrderManager>();
        groundSizeController = FindObjectOfType<GroundSizeController>();
    }

    public void OpenLevelUpPanel()
    {
        Hammer.Instance.ANALYTICS_LevelComplete(LevelManager.Instance.PlayerLevel);
        nextButton = false;
        transform.GetChild(0).gameObject.SetActive(true);
        OldCapacityText.text = (LevelManager.Instance.SpawnerMaxCount).ToString();
        if (LevelManager.Instance.PlayerLevel <= 19)
        {
            NewCapacityText.text = ((LevelManager.Instance.PlayerLevel + 1) * 5).ToString();
        }
        //else if(LevelManager.Instance.PlayerLevel <= 25)
        //{
        //    NewCapacityText.text = (110 + ((LevelManager.Instance.PlayerLevel - 10) * 5)).ToString();
        //}
        else
        {
            CapacityPart.SetActive(false);
        }
        if (LevelManager.Instance.PlayerLevel <= 20)
        {
            FruitName.text = fruitUpgradeManager.FruitUpgradeObjectList[LevelManager.Instance.PlayerLevel - 1].name;
            FruitImage.sprite = orderManager.fruitImageList[LevelManager.Instance.PlayerLevel - 1].sprite;
            //NewCapacityText.text = (LevelManager.Instance.SpawnerMaxCount + 10).ToString();
            //RewardedAmount.text = "x" + LevelManager.Instance.LevelUpRewardList[LevelManager.Instance.PlayerLevel - 2].ToString();
        }
        else
        {
            //RewardedAmount.text = "x" + LevelManager.Instance.LevelUpRewardList[^1].ToString();
            //NewCapacityText.text = (LevelManager.Instance.SpawnerMaxCount + 10).ToString();
            FruitPart.SetActive(false);
            FieldPart.SetActive(false);
        }
        if (LevelManager.Instance.PlayerLevel <= LevelManager.Instance.LevelUpRewardList.Count + 1)
        {
            //RewardedDNAAmount = (index) * LevelManager.Instance.LevelUpRewardList[LevelManager.Instance.PlayerLevel - 2];
            RewardedAmount.text = "x" + numberFormatManager.FormatNumber(LevelManager.Instance.LevelUpRewardList[LevelManager.Instance.PlayerLevel - 2]);
        }
        else
        {
            //RewardedDNAAmount = (index) * LevelManager.Instance.LevelUpRewardList[^1];
            RewardedAmount.text = "x" + numberFormatManager.FormatNumber(LevelManager.Instance.LevelUpRewardList[^1]);

        }
        if (LevelManager.Instance.IsInternetAvailable())
        {
            RequestPart.SetActive(false);
            RewardedPart.SetActive(true);
        }
        else
        {
            RequestPart.SetActive(true);
            RewardedPart.SetActive(false);
        }

        uIParticleController.PlayConfettiParticle();
    }

    public void CloseLevelUpPanel()
    {
        if (!nextButton)
        {
            nextButton = true;
            cursorRewardedController.StopRotating();
            transform.GetChild(0).gameObject.SetActive(false);
            if (LevelManager.Instance.PlayerLevel <= 20)
            {
                groundSizeController.IncreaseScaleLevel();
                EventManagement.Invoke_OnGoldChange(LevelManager.Instance.LevelUpRewardList[LevelManager.Instance.PlayerLevel - 2], transform.position);
            }
            else
            {

                EventManagement.Invoke_OnGoldChange(LevelManager.Instance.LevelUpRewardList[^1], transform.position);
            }
            EventManagement.Invoke_OnCollectMoneyUI(transform.position, 50);
        }
    }


    public void CursorRewardedButtonClicked()
    {
        //Debug.Log("claim");
        LevelManager.Instance.ResetInterTimer();
        if (!nextButton)
        {
            nextButton = true;
            cursorRewardedController.StopRotating();
            //Hammer.Instance.ANALYTICS_LevelComplete(LevelManager.Instance.PlayerLevel);
            //EventManagement.Invoke_OnPlayerLevelChange();
            transform.GetChild(0).gameObject.SetActive(false);
            groundSizeController.IncreaseScaleLevel();
            //LevelManager.Instance.Level++;
            //LevelManager.Instance.SDKLevel++;


            Hammer.Instance.MEDIATION_HasRewarded(
            () =>
            {
                Hammer.Instance.MEDIATION_ShowRewarded(
                  () =>
                  {
                      //Debug.Log("Rewarded gösterildi");
                      cursorRewardedController.CloseButtons();
                      EventManagement.Invoke_OnGoldChange(cursorRewardedController.RewardedDNAAmount, transform.position);
                      EventManagement.Invoke_OnCollectMoneyUI(Camera.main.ScreenToWorldPoint(transform.position), cursorRewardedController.RewardedDNAAmount);

                      //ödülü ver
                  },
                  s =>
                  {
                      //Debug.Log("Rewarded gösterilemedi");
                      cursorRewardedController.CloseButtons();

                  }
                             );
            },
                     s =>
                     {
                         //Debug.Log("Rewarded yüklenemedi");
                         cursorRewardedController.CloseButtons();
                     }
            );

        }
    }
}
