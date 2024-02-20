using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using static CharacterUpgradeManager;
using Udo.Hammer.Runtime.Core;
using DG.Tweening;

public class CharacterUpgradeController : MonoBehaviour
{
    InGamePanelController inGamePanelController;
    CharacterUpgradeManager characterUpgradeManager;
    SpeedDataHandler speedDataHandler;
    HandScaleHandler handScaleHandler;
    public CharacterUpgradeType characterUpgradeType;
    public List<Image> UpgradeImageList = new();
    public Image UpgradeImage, RWFillImage;
    public Button UpgradeButton;
    public int UpgradeCost;
    public GameObject PopUp, UpgradeButtonObject, BGFirst, BGSecond, StickFirst, StickSecond, LevelCountParent, BlueSeaParent, CheckImage, RewardedPanel, NormalButton, FactorGroup;
    public TextMeshProUGUI UpgradeName, UnlockLevel, UpgradeCostText, PlayerLevelCountText, OldFactorText, NewFactorText;
    public bool ButtonPressed = false;
    public ButtonPressed mainButtonPressed, buttonPressed, buttonPressedRW;
    NumberFormatManager numberFormatManager = new();
    GroundSizeController groundSizeController;
    int childIndex;
    ControllerHolder controllerHolder;

    private void Awake()
    {
        if (controllerHolder == null)
        {
            controllerHolder = FindObjectOfType<ControllerHolder>();

            characterUpgradeManager = controllerHolder.CharacterUpgradeManager;

            childIndex = transform.GetSiblingIndex();
        }
        
    }

    void Start()
    {
        UpgradeImage.sprite = UpgradeImageList[(int)characterUpgradeType].sprite;
        UpgradeName.text = characterUpgradeType.ToString();
        if (characterUpgradeType == CharacterUpgradeType.Speed)
        {
            //OldFactorText.text = ((int)(speedDataHandler.speeds[Mathf.FloorToInt(((float)childIndex) / 3)])).ToString();
            //NewFactorText.text = ((int)(speedDataHandler.speeds[Mathf.FloorToInt(((float)childIndex) / 3) + 1])).ToString();
            UpgradeCost = (int)((float)(LevelManager.Instance.CharacterUpgradeCostList[(int)((float)(childIndex) / 3)]) * 0.9f);
        }
        else if (characterUpgradeType == CharacterUpgradeType.HandSize)
        {
            //OldFactorText.text = ((float)(handScaleHandler.handScales[Mathf.FloorToInt(((float)childIndex) / 3)])).ToString();
            //NewFactorText.text = ((float)(handScaleHandler.handScales[Mathf.FloorToInt(((float)childIndex) / 3) + 1])).ToString();
            UpgradeCost = LevelManager.Instance.CharacterUpgradeCostList[(int)((float)(childIndex) / 3)];
        }
        else if (characterUpgradeType == CharacterUpgradeType.PressPower)
        {
            //OldFactorText.text = ((float)(BurstManager.Instance.datas.burstDurations[Mathf.FloorToInt(((float)childIndex) / 3)])).ToString();
            //NewFactorText.text = ((float)(BurstManager.Instance.datas.burstDurations[Mathf.FloorToInt(((float)childIndex) / 3) + 1])).ToString();
            UpgradeCost = (int)((float)(LevelManager.Instance.CharacterUpgradeCostList[(int)((float)(childIndex) / 3)]) * 1.1f);
        }

        UpgradeCostText.text = numberFormatManager.FormatNumber(UpgradeCost);
        ClosePopUpPanel();
        CheckUpgradeable();

        if (childIndex == transform.parent.transform.childCount - 1)
        {
            StickFirst.SetActive(false);
            StickSecond.SetActive(false);
        }

        if (((float)childIndex % 3 == 0) && childIndex != 0)
        {
            LevelCountParent.SetActive(true);
            PlayerLevelCountText.text = (((childIndex / 3) + 1) * 3).ToString();
        }
        else
        {
            LevelCountParent.SetActive(false);
        }

        if (transform.GetSiblingIndex() == 0)
        {
            BlueSeaParent.SetActive(true);
            BlueSeaParent.transform.localPosition += Vector3.up * 450 * LevelManager.Instance.CharacterUpgradeLevel;
            characterUpgradeManager.BlueSeaPanel = BlueSeaParent;
        }
        else
        {
            BlueSeaParent.SetActive(false);
        }
        if (childIndex < LevelManager.Instance.CharacterUpgradeLevel)
        {
            UpgradePanel();
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ClosePopUpPanel();
        }
    }
    public IOnboardable onboardController;
    public void NormalUpgradeCharacter()
    {
        UpgradeCharacter();
        EventManagement.Invoke_OnGoldChange(-UpgradeCost, UpgradeButton.transform.position);
    }

    public void RewardedUpgradeCharacter()
    {
        UpgradeCharacter();
    }

    void UpgradeCharacter()
    {
        Hammer.Instance.ANALYTICS_CustomEvent("UpgradedCharacter", LevelManager.Instance.PlayerLevel);
        if (onboardController != null)
        {
            OnboardingManager.Instance.UnonboardObject(onboardController);
        }
        LevelManager.Instance.CharacterUpgradeLevel++;
        controllerHolder.InGamePanelController.minCharacterUpgradeAmount = characterUpgradeManager.PrefabParent.transform.GetChild(LevelManager.Instance.CharacterUpgradeLevel).GetComponent<CharacterUpgradeController>().UpgradeCost;
        characterUpgradeManager.MoveSeaPanel();
        float newYPos = (characterUpgradeManager.firstYPos - 100) - (450 * (LevelManager.Instance.CharacterUpgradeLevel - 1));
        characterUpgradeManager.PrefabParent.transform.DOLocalMoveY(newYPos, 2f);
        CheckImage.SetActive(false);
        UpgradePanel();

        if (characterUpgradeType == CharacterUpgradeType.Speed)
        {
            controllerHolder.SpeedDataHandler.IncreaseSpeedLevel();
        }
        else if (characterUpgradeType == CharacterUpgradeType.HandSize)
        {
            controllerHolder.HandScaleHandler.IncreaseScaleLevel();
        }
        else if (characterUpgradeType == CharacterUpgradeType.PressPower)
        {
            BurstManager.Instance.IncreaseBurstLevel();
        }
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
        //RewardedPanel.SetActive(false);
        LevelManager.Instance.ResetInterTimer();

        Hammer.Instance.MEDIATION_HasRewarded(
            () =>
            {
                Hammer.Instance.MEDIATION_ShowRewarded(
               () =>
               {
                   Hammer.Instance.ANALYTICS_CustomEvent("CharacterUpgradeRewarded", LevelManager.Instance.PlayerLevel);
                   RewardedUpgradeCharacter();
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

    public void UpgradePanel()
    {
        BGFirst.SetActive(false);
        BGSecond.SetActive(true);
        StickFirst.SetActive(false);
        if (childIndex != transform.parent.transform.childCount - 1)
        {
            StickSecond.SetActive(true);
        }
        characterUpgradeManager.CheckAllUpgrades();
        ClosePopUpPanel();
    }

    public void CheckUpgradeable()
    {
        if (((float)childIndex / 3) >= Mathf.FloorToInt((float)LevelManager.Instance.PlayerLevel / 3))
        {
            RewardedPanel.SetActive(false);
            NormalButton.SetActive(true);
            UpgradeButtonObject.SetActive(false);
            UnlockLevel.gameObject.SetActive(true);
            FactorGroup.SetActive(false);
            UnlockLevel.text = "Unlock at Level " + ((Mathf.FloorToInt((float)childIndex / 3) + 1) * 3).ToString();
        }
        else
        {
            FactorGroup.SetActive(true);
            if ((childIndex) == LevelManager.Instance.CharacterUpgradeLevel)
            {
                UpgradeButtonObject.SetActive(true);
                CheckEnoughGold();
            }
            else
            {
                UpgradeButtonObject.SetActive(false);
            }
            UnlockLevel.gameObject.SetActive(false);
        }
    }

    public void CheckEnoughGold()
    {
        if (UpgradeCost < LevelManager.Instance.CurrentGold)
        {
            RewardedPanel.SetActive(false);
            UpgradeCostText.color = Color.white;
            UpgradeButton.interactable = true;
            CheckImage.SetActive(true);
        }
        else
        {
            UpgradeCostText.color = Color.red;
            UpgradeButton.interactable = false;
            CheckImage.SetActive(false);
            //if (Random.RandomRange(0, 10) < 1)
            if (LevelManager.Instance.PlayerLevel > 3 && LevelManager.Instance.IsInternetAvailable())
            {
                StopCoroutine(OpenRewardedFreeUpgrade(10));
                StartCoroutine(OpenRewardedFreeUpgrade(10));
            }
        }
    }

    public void OpenPopUpPanel()
    {
        PopUp.SetActive(true);
        CheckUpgradeable();
    }

    public void ClosePopUpPanel()
    {
        if (!buttonPressed.buttonPressed && !mainButtonPressed.buttonPressed && !buttonPressedRW.buttonPressed)
        {
            PopUp.SetActive(false);
        }
    }
}
