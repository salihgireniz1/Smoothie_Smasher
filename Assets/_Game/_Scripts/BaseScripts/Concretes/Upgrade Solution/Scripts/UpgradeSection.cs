using TMPro;
using PAG.Utility;
using UnityEngine;
using PAG.Managers;
using UnityEngine.UI;
using PAG.UpgradeSystem;
using Sirenix.OdinInspector;
using System.Collections.Generic;

public class UpgradeSection : MonoBehaviour
{
    #region Variables
    [Header("Icon Part Settings"), Space]
    [SerializeField]
    private Image icon;

    [SerializeField]
    private TextMeshProUGUI startMaxText;

    [Header("Explanation Part Settings"), Space]
    [SerializeField]
    private TextMeshProUGUI displayNameText;
    [SerializeField]
    private TextMeshProUGUI explanationText;

    [SerializeField]
    private Color32 currentPowerColor = Color.yellow;

    [SerializeField]
    private Color32 nextPowerColor = Color.blue;

    [SerializeField]
    private Color32 defaultExplanationColor = Color.white;

    [SerializeField]
    private Color32 maximumReachTextColor = Color.white;

    [Header("Button Part Settings"), Space]
    [SerializeField]
    private Button upgradeButton;

    [SerializeField]
    private TextMeshProUGUI buttonText;

    [SerializeField]
    private Image currencyImage;

    [SerializeField]
    private TextMeshProUGUI priceText;

    [SerializeField]
    private string buttonActiveText = "UPGRADE";
    
    [SerializeField]
    private string buttonMaxedText = "COMPLETED";

    [SerializeField]
    private string buttonNoMoneyText = "CAPATICY IS TOO LOW";

    [Header("Other Settings"), Space]
    [SerializeField]
    private ActiveUpgrades upgradeToGenerate;

    private UpgradableObjectData myData;
    #endregion

    #region MonoBehaviour Callbacks
    private void OnEnable()
    {
        CurrencyManager.OnCurrencyUpdate += Refresh;
        GenerateSection(upgradeToGenerate);
        upgradeButton.onClick.AddListener(Upgrade);
    }
    private void OnDisable()
    {
        CurrencyManager.OnCurrencyUpdate -= Refresh;
        upgradeButton.onClick.RemoveListener(Upgrade);
    }
    private void Start()
    {
        //GenerateSection(upgradeToGenerate);
    }
    #endregion

    #region Methods
    [Button("Generate!")]
    public void GenerateSection(ActiveUpgrades upgradeToGenerate)
    {
        this.upgradeToGenerate = upgradeToGenerate;
        myData = UpgradeManager.Instance.GetDataInfo(this.upgradeToGenerate);

        // Generate icon part -->
        GenerateIconPart();

        // Generate explanation part -->
        GenerateExplanationPart();

        // Generate button part -->
        GenerateButtonPart();
    }
    public void GenerateSection(UpgradableObjectData data)
    {
        myData = data;

        foreach (var kvp in UpgradeManager.Instance.enumDataDict)
        {
            if (kvp.Value == myData)
            {
                upgradeToGenerate = kvp.Key;
                Debug.Log($"Found Key: {upgradeToGenerate}.", this.gameObject);
                break;
            }
        }

        // Generate icon part -->
        GenerateIconPart();

        // Generate explanation part -->
        GenerateExplanationPart();

        // Generate button part -->
        GenerateButtonPart();
    }
    void Refresh(Currencies currency, float amount)
    {
        GenerateSection(upgradeToGenerate);
    }
    void GenerateIconPart()
    {
        icon.sprite = myData.upgradeSprite;

        string startMaxString = myData.GetCurrentLevel().ToString("F0") + "/" + myData.maxUpgradeCount.ToString("F0");
        startMaxText.text = startMaxString;
    }
    void GenerateExplanationPart()
    {
        displayNameText.text = myData.displayName;

        string currentToNext = $"{GetCurrentPowerColoredText()}>{GetNextPowerColoredText()}";
        string longExp = myData.longExplanation;
        string coloredExplanationText = PAGText.GetColoredText(longExp, defaultExplanationColor);
        string wholeScript = $"{currentToNext} {coloredExplanationText}";

        explanationText.text = wholeScript;
    }
    void GenerateButtonPart()
    {
        currencyImage.sprite = UpgradeManager.Instance.GetUpgradeCurrency(upgradeToGenerate).currencyImage;
        priceText.text = myData.GetCurrentPrice().ToString();
        buttonText.text = GetUpgradeButtonText();
        HandleButtonInteractibility();
    }
    string GetCurrentPowerColoredText()
    {
        float currentPower = myData.GetCurrentPower();
        string coloredCurrentPowerText = PAGText.GetColoredText(currentPower.ToString(), currentPowerColor);
        if (myData.isPercentage)
        {
            coloredCurrentPowerText += "%";
        }
        return coloredCurrentPowerText;
    }
    string GetNextPowerColoredText()
    {
        if (myData.IsMax())
        {
            return "";
        }
        float nextPower = myData.GetNextPower();
        string coloredNextPowerText = PAGText.GetColoredText(nextPower.ToString(), nextPowerColor);
        if (myData.isPercentage) coloredNextPowerText += "%";

        return coloredNextPowerText;
    }
    string GetUpgradeButtonText()
    {
        string currentConditionText = buttonActiveText;
        if (myData.IsMax())
        {
            currentConditionText = buttonMaxedText;
        }
        else if (!UpgradeManager.Instance.CheckIfEnoughCurrencyToLevelUp(upgradeToGenerate))
        {
            currentConditionText = UpgradeManager.Instance.GetUpgradeCurrency(upgradeToGenerate).currencyName + " " + buttonNoMoneyText;
        }
        return currentConditionText;
    }
    void HandleButtonInteractibility()
    {
        upgradeButton.interactable = false;

        if (UpgradeManager.Instance.CheckIfEnoughCurrencyToLevelUp(upgradeToGenerate))
        {
            upgradeButton.interactable = true;
        }
        if (myData.IsMax())
        {
            upgradeButton.interactable = false;
        }
    }
    void Upgrade()
    {
        Debug.Log($"Pressed {this.gameObject}");
        UpgradeManager.Instance.UpgradeTheObject(upgradeToGenerate);
        GenerateSection(upgradeToGenerate);
    }
    #endregion    
}