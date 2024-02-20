using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Udo.Hammer.Runtime.Core;

public class FruitUpgradeController : MonoBehaviour
{
    OrderManager orderManager;
    public FruitUpgradeManager fruitUpgradeManager;
    public GameObject LockedPanel, UnlockedPanel, RewardedPanel, NormalButton, ButtonParent, Camp2Parent;
    public Image FruitImage, LockedFruitImage, RWFillImage;
    public TextMeshProUGUI fruitName, fruitCount, harvestFactor, upgradeCostText, unlockedLevel;
    public int upgradeCost;
    public int childIndex;
    public FruitUpgradeObject fruitUpgradeObject;
    public Button UpgradeButton;
    public GameObject CheckArrow;
    public ButtonPressed buttonPressed, buttonPressedRW;
    //public bool isMax = false;

    NumberFormatManager numberFormatManager = new();

    private void Awake()
    {
        orderManager = FindObjectOfType<OrderManager>();
    }
    void Start()
    {
        FruitImage.sprite = orderManager.fruitImageList[(int)fruitUpgradeObject.fruitType].sprite;
        LockedFruitImage.sprite = orderManager.fruitImageList[(int)fruitUpgradeObject.fruitType].sprite;
        unlockedLevel.text = "Level" + ((int)fruitUpgradeObject.fruitType+1).ToString();
        fruitName.text = fruitUpgradeObject.fruitType.ToString();
        fruitCount.text = "x" + LevelManager.Instance.fruitAmountList[(int)fruitUpgradeObject.fruitType].ToString();
        harvestFactor.text = fruitUpgradeObject.harvestFactor.ToString();
        upgradeCost = fruitUpgradeObject.GetFirstUpgradeCost();
        upgradeCostText.text = numberFormatManager.FormatNumber(upgradeCost);
    }

    public void CheckLockedConditions()
    {
        if (!fruitUpgradeObject.isMax)
        {
            if (LevelManager.Instance.PlayerLevel < (int)fruitUpgradeObject.fruitType + 1)
            {
                LockedPanel.SetActive(true);
                UnlockedPanel.SetActive(false);
            }
            else
            {
                LockedPanel.SetActive(false);
                UnlockedPanel.SetActive(true);
                if (!fruitUpgradeObject.isUnlocked)
                {
                    fruitUpgradeObject.isUnlocked = true;
                }
            }
            RewardedPanel.SetActive(false);

            if (fruitUpgradeObject.upgradeLevel > LevelManager.Instance.UpgradeMultiplyFactorList.Count)
            {
                ButtonParent.SetActive(false);
                Camp2Parent.SetActive(true);
                fruitUpgradeObject.isMax = true;
            }
        }
    }

    public void CheckUpgradableConditions()
    {
        if (!fruitUpgradeObject.isMax)
        {
            if (LevelManager.Instance.CurrentGold < fruitUpgradeObject.upgradeCost)
            {
                if (Random.Range(0, 10) < 5 && fruitUpgradeObject.isUnlocked && LevelManager.Instance.PlayerLevel > 1 && LevelManager.Instance.IsInternetAvailable())
                {
                    //RewardedPanel.SetActive(true);
                    StartCoroutine(OpenRewardedFreeUpgrade(10));
                }
                else
                {
                    RewardedPanel.SetActive(false);
                    NormalButton.SetActive(true);
                }
                UpgradeButton.interactable = false;
                CheckArrow.SetActive(false);
                upgradeCostText.color = Color.red;
            }
            else
            {
                RewardedPanel.SetActive(false);
                NormalButton.SetActive(true);
                UpgradeButton.interactable = true;
                CheckArrow.SetActive(true);
                upgradeCostText.color = Color.white;
            }
            fruitCount.text = "x" + LevelManager.Instance.fruitAmountList[(int)fruitUpgradeObject.fruitType].ToString();
        }
    }
    public IOnboardable onboardHandler;
    IEnumerator DelayedUpgrade()
    {
        if (onboardHandler != null)
        {
            OnboardingManager.Instance.UnonboardObject(onboardHandler);
            yield return new WaitForEndOfFrame();
            onboardHandler = null;
        }
       
    }
    public void NormalUpgradePart2()
    {
        EventManagement.Invoke_OnGoldChange(-fruitUpgradeObject.upgradeCost, NormalButton.transform.position);
        fruitUpgradeObject.UpgradeFruit();
        harvestFactor.text = fruitUpgradeObject.harvestFactor.ToString();
        upgradeCostText.text = numberFormatManager.FormatNumber(fruitUpgradeObject.upgradeCost);
        fruitUpgradeManager.CheckAllFruitUpgradeConditions();
    }
    public void CheapestUpgradePart2()
    {
        EventManagement.Invoke_OnGoldChange(-fruitUpgradeObject.upgradeCost, fruitUpgradeManager.CheapestUpgradeButton.transform.position);
        fruitUpgradeObject.UpgradeFruit();
        harvestFactor.text = fruitUpgradeObject.harvestFactor.ToString();
        upgradeCostText.text = numberFormatManager.FormatNumber(fruitUpgradeObject.upgradeCost);
        fruitUpgradeManager.CheckAllFruitUpgradeConditions();
    }

    public void UpgradeFruitButton()
    {
        StartCoroutine(DelayedUpgrade());
        /*if (onboardHandler != null)
        {
            OnboardingManager.Instance.UnonboardObject(onboardHandler);
        }
        LevelManager.Instance.CurrentGold -= fruitUpgradeObject.upgradeCost;
        fruitUpgradeObject.UpgradeFruit();
        harvestFactor.text = fruitUpgradeObject.harvestFactor.ToString();
        upgradeCostText.text = numberFormatManager.FormatNumber(fruitUpgradeObject.upgradeCost);
        fruitUpgradeManager.FruitCostList[childIndex] = fruitUpgradeObject.upgradeCost;
        fruitUpgradeManager.CheckAllFruitUpgradeConditions();*/
    }

    IEnumerator OpenRewardedFreeUpgrade(float duration)
    {
        RewardedPanel.SetActive(true);
        NormalButton.SetActive(false);
        RWFillImage.transform.localScale = Vector3.one;
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            RWFillImage.transform.localScale = new Vector3(1 - (time / duration), 1, 1);
            yield return null;
        }

        RewardedPanel.SetActive(false);
        NormalButton.SetActive(true);
    }

    public void RewardedUpgrade()
    {
        LevelManager.Instance.ResetInterTimer();
        Hammer.Instance.MEDIATION_HasRewarded(
            () =>
            {
                Hammer.Instance.MEDIATION_ShowRewarded(
               () =>
               {
                   Hammer.Instance.ANALYTICS_CustomEvent("FruitUpgradeRewarded", LevelManager.Instance.PlayerLevel);
                   fruitUpgradeObject.UpgradeFruit();
                   harvestFactor.text = fruitUpgradeObject.harvestFactor.ToString();
                   upgradeCostText.text = numberFormatManager.FormatNumber(fruitUpgradeObject.upgradeCost);
                   //fruitUpgradeManager.FruitCostList[childIndex] = fruitUpgradeObject.upgradeCost;
                   fruitUpgradeManager.CheckAllFruitUpgradeConditions();
                   RewardedPanel.SetActive(false);
                   NormalButton.SetActive(true);
                   //Debug.Log("Rewarded gösterildi");
               },
               s =>
               {
                   //Debug.Log("Rewarded gösterilemedi");
                   RewardedPanel.SetActive(false);
                   NormalButton.SetActive(true);
               }
               );
            },
                s =>
                {
                    //Debug.Log("Rewarded yüklenemedi");
                    RewardedPanel.SetActive(false);
                    NormalButton.SetActive(true);
                }
                );
    }
}
