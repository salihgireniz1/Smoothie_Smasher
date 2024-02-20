using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FruitUpgradeManager : MonoBehaviour
{
    InGamePanelController inGamePanelController;
    OrderManager orderManager;
    public GameObject FruitPrefab, FruitUpgradePanel, CheapestPanel, FruitUpgradeParent;
    public List<FruitUpgradeObject> FruitUpgradeObjectList = new();
    public List<bool> UpgradableFruitList = new();
    public FruitUpgradeOnboard onboardController;
    List<int> CurrentUpgradeCostList = new();
    List<FruitUpgradeObject> CurrentFruitObjectList = new();
    FruitUpgradeObject CheapestFruitUpgradeObject;
    public Image CheapestFruitImage;
    public TextMeshProUGUI CheapestFruitName, CheapestHarvestCount, CheapestUpgradeCost;
    public Button CheapestUpgradeButton;
    NumberFormatManager numberFormatManager = new();
    public List<ButtonPressed> ButtonPressedList = new();
    bool canClose = false;
    float firstYPos;

    private void Awake()
    {
        inGamePanelController = FindObjectOfType<InGamePanelController>();
        orderManager = FindObjectOfType<OrderManager>();
        firstYPos = FruitUpgradeParent.transform.localPosition.y;
    }

    private void Start()
    {
        CheapestPanel.SetActive(false);
        for (int i = 0; i < FruitUpgradeObjectList.Count; i++)
        {
            GameObject fruitPrefab = Instantiate(FruitPrefab, FruitUpgradeParent.transform);
            FruitUpgradeController _fruit = fruitPrefab.GetComponent<FruitUpgradeController>();

            _fruit.fruitUpgradeManager = this;
            _fruit.fruitUpgradeObject = FruitUpgradeObjectList[i];
            _fruit.childIndex = i;
            ButtonPressedList.Add(_fruit.buttonPressed);
            ButtonPressedList.Add(_fruit.buttonPressedRW);
        }
        CheckAllFruitUpgradeConditions();
        //CloseFruitUpgradePanel();
        CheckMinAndCheapestUpgrade();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && onboardController.IsOnboarded)
        {
            CloseOutFruitUpgradePanel();
        }
    }

    public void CloseOutFruitUpgradePanel()
    {
        canClose = false;
        for (int i = 0; i < ButtonPressedList.Count; i++)
        {
            if (ButtonPressedList[i].buttonPressed) canClose = true;
        }

        if (!canClose)
        {
            CloseFruitUpgradePanel();
        }
    }

    public void CheckMinAndCheapestUpgrade()
    {
        CurrentFruitObjectList.Clear();
        CurrentUpgradeCostList.Clear();
        int maxFruitIndex;
        if (LevelManager.Instance.PlayerLevel <= 20)
        {
            maxFruitIndex = LevelManager.Instance.PlayerLevel;
        }
        else
        {
            maxFruitIndex = 20;
        }
        for (int i = 0; i < maxFruitIndex; i++)
        {
            if (!FruitUpgradeObjectList[i].isMax)
            {
                CurrentFruitObjectList.Add(FruitUpgradeObjectList[i]);
            }
        }
        if (CurrentFruitObjectList.Count > 0)
        {
            for (int i = 0; i < CurrentFruitObjectList.Count; i++)
            {
                CurrentUpgradeCostList.Add(CurrentFruitObjectList[i].upgradeCost);
            }
            int minCost = CurrentUpgradeCostList.Min();
            int tempMinIndex = CurrentUpgradeCostList.IndexOf(minCost);
            int minIndex = FruitUpgradeObjectList.IndexOf(CurrentFruitObjectList[tempMinIndex]);
            inGamePanelController.minFruitUpgradeAmount = minCost;
            CheapestFruitUpgradeObject = FruitUpgradeObjectList[minIndex];
            if (LevelManager.Instance.CurrentGold >= minCost)
            {
                CheapestPanel.SetActive(true);
                CheapestFruitImage.sprite = orderManager.fruitImageList[minIndex].sprite;
                CheapestFruitName.text = CheapestFruitUpgradeObject.name;
                CheapestHarvestCount.text = CheapestFruitUpgradeObject.harvestFactor.ToString();
                CheapestUpgradeCost.text = numberFormatManager.FormatNumber(CheapestFruitUpgradeObject.upgradeCost);
                CheapestUpgradeButton.interactable = true;
                CheapestUpgradeButton.onClick.RemoveAllListeners();
                CheapestUpgradeButton.onClick.AddListener(() => FruitUpgradeParent.transform.GetChild(minIndex).GetComponent<FruitUpgradeController>().UpgradeFruitButton());
                CheapestUpgradeButton.onClick.AddListener(() => FruitUpgradeParent.transform.GetChild(minIndex).GetComponent<FruitUpgradeController>().CheapestUpgradePart2());
            }
            else
            {
                CheapestPanel.SetActive(false);
            }
        }
        else
        {
            CheapestPanel.SetActive(false);
        }
    }

    public void CheapestButtonClose()
    {
        CheapestUpgradeButton.interactable = false;
    }

    public void OpenFruitUpgradePanel()
    {
        if (LevelManager.Instance.PlayerLevel > 4)
        {
            float newYPos = firstYPos + Mathf.CeilToInt((float)(LevelManager.Instance.PlayerLevel - 4) / 2f) * 350f;
            FruitUpgradeParent.transform.localPosition = new Vector3(FruitUpgradeParent.transform.localPosition.x, newYPos, FruitUpgradeParent.transform.localPosition.z);
        }
        //Debug.Log(Mathf.CeilToInt((float)(LevelManager.Instance.PlayerLevel - 4) / 2f));
        FruitUpgradePanel.SetActive(true);

        CheckAllFruitUpgradeConditions();
        if (onboardController.IsOnboarded)
        {
            LevelManager.Instance.WatchInter();
        }
        OnboardingManager.Instance.OnboardObject(onboardController, true);
    }

    public void CloseFruitUpgradePanel()
    {
        FruitUpgradePanel.SetActive(false);
    }

    public void CheckAllFruitUpgradeConditions()
    {
        foreach (Transform item in FruitUpgradeParent.transform)
        {
            item.GetComponent<FruitUpgradeController>().CheckLockedConditions();
            item.GetComponent<FruitUpgradeController>().CheckUpgradableConditions();
        }
    }
}
