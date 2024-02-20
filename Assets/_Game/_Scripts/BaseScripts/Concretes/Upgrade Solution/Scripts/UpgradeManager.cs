using PAG.Currency;
using PAG.Editor;
using PAG.UpgradeSystem;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using PAG.Utility;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PAG.Managers
{
    public class UpgradeManager : MonoSingleton<UpgradeManager>
    {
        #region Variables
        public List<UpgradableObjectData> existingUpgrades = new();
        
        #region Accessable Dictionaries
        public Dictionary<UpgradeType, List<UpgradableObjectData>> typesSeparatedDict = new();
        public Dictionary<Currencies, List<UpgradableObjectData>> currenciesSeparatedDict = new();
        public Dictionary<ActiveUpgrades, UpgradableObjectData> enumDataDict = new();
        #endregion

        #region Hidden/Private Holders
        [HideInInspector]
        public List<string> existingUpgradeNames = new();
        private Dictionary<string, UpgradableObjectData> upgradesByEnumValue = new();
        #endregion

        #endregion

        #region MonoBehaviour Callbacks
        private void Awake()
        {
            Initialize();
        }
        #endregion

        #region Public Generators
        public void Initialize()
        {
            // Clear the dictionaries to start fresh
            typesSeparatedDict.Clear();
            currenciesSeparatedDict.Clear();
            upgradesByEnumValue.Clear();
            enumDataDict.Clear();

            // Loop through each existing upgrade
            for (int i = 0; i < existingUpgrades.Count; i++)
            {
                UpgradableObjectData upgrade = existingUpgrades[i];

                // Separate upgrades by UpgradeType
                if (!typesSeparatedDict.ContainsKey(upgrade.upgradeType))
                {
                    typesSeparatedDict.Add(upgrade.upgradeType, new List<UpgradableObjectData>());
                }
                typesSeparatedDict[upgrade.upgradeType].Add(upgrade);

                // Separate upgrades by Currencies
                if (!currenciesSeparatedDict.ContainsKey(upgrade.currencyType))
                {
                    currenciesSeparatedDict.Add(upgrade.currencyType, new List<UpgradableObjectData>());
                }
                currenciesSeparatedDict[upgrade.currencyType].Add(upgrade);

                // Remove any whitespace or blank characters from the display name
                string displayName = existingUpgradeNames[i];
                upgradesByEnumValue.Add(displayName, existingUpgrades[i]);
            }

            // Loop through all enum values and fill enumDataDict
            foreach (ActiveUpgrades upgradeEnum in Enum.GetValues(typeof(ActiveUpgrades)))
            {
                UpgradableObjectData data = DetectDataInfoFromEnum(upgradeEnum);
                if (data != null)
                {
                    enumDataDict.Add(upgradeEnum, data);
                }
            }
        }
        public UpgradableObjectData GetDataInfo(ActiveUpgrades upgradeEnum)
        {
            return enumDataDict[upgradeEnum];
        }

        #endregion

        #region Public Accessors
        public void UpgradeTheObject(ActiveUpgrades upgrade)
        {
            UpgradableObjectData data = enumDataDict[upgrade];
            CurrencyData currencyData = GetUpgradeCurrency(upgrade);
            float cost = data.GetCurrentPrice();
            if (currencyData.CanSpendCurrency(cost))
            {
                CurrencyManager.Instance.SpendCurrency(data.currencyType, cost);
                data.IncreaseUpgradeLevel();
            }
        }
        public int GetUpgradeLevel(ActiveUpgrades upgrade)
        {
            return enumDataDict[upgrade].GetCurrentLevel();
        }
        public float GetUpgradePrice(ActiveUpgrades upgrade)
        {
            return enumDataDict[upgrade].GetCurrentPrice();
        }
        public float GetUpgradePower(ActiveUpgrades upgrade)
        {
            return enumDataDict[upgrade].GetCurrentPower();
        }
        public float GetUpgradeNextPrice(ActiveUpgrades upgrade)
        {
            return enumDataDict[upgrade].GetNextPrice();
        }
        public float GetUpgradeNextPower(ActiveUpgrades upgrade)
        {
            return enumDataDict[upgrade].GetNextPower();
        }
        public bool CheckIfLevelUp(ActiveUpgrades upgrade)
        {
            float amount = GetUpgradePrice(upgrade);
            bool isNotMax = !CheckIfMaxed(upgrade);
            bool currencyIsEnough = CheckIfEnoughCurrencyToLevelUp(upgrade);

            return isNotMax && currencyIsEnough;
        }
        public bool CheckIfEnoughCurrencyToLevelUp(ActiveUpgrades upgrade)
        {
            float amount = GetUpgradePrice(upgrade);
            return GetUpgradeCurrency(upgrade).CanSpendCurrency(amount);
        }
        public bool CheckIfMaxed(ActiveUpgrades upgrade)
        {
            return enumDataDict[upgrade].IsMax();
        }
        public CurrencyData GetUpgradeCurrency(ActiveUpgrades upgrade)
        {
            Currencies currency = GetDataInfo(upgrade).currencyType;
            return CurrencyManager.Instance.GetCurrencyInfo(currency);
        }
        
        #endregion

        #region Private Methods
        [Button("Automatically Find Datas")]
        void DetectUpgrades()
        {
#if UNITY_EDITOR
            existingUpgrades = new();
            existingUpgrades = AssetDatabaseUtils.FindAssetsByType<UpgradableObjectData>();

            existingUpgradeNames = new();

            // Generate the elements array
            string[] elements = new string[existingUpgrades.Count];

            for (int i = 0; i < existingUpgrades.Count; i++)
            {
                // Remove any whitespace or blank characters from the display name
                string displayName = existingUpgrades[i].displayName.Replace(" ", string.Empty);
                elements[i] = displayName;
                existingUpgradeNames.Add(displayName);
            }
            // Generate enum class for these existing upgrades.
            EnumGenerator.Generate(EnumConsts.UpgradesEnumName, elements);
#endif
        }

        UpgradableObjectData DetectDataInfoFromEnum(ActiveUpgrades upgradeEnum)
        {
            Type type = Type.GetType(EnumConsts.UpgradesEnumName);
            string displayName = Enum.GetName(type, upgradeEnum);
            if (upgradesByEnumValue.ContainsKey(displayName))
            {
                return upgradesByEnumValue[displayName];
            }
            return null;
        }
    }
    #endregion
}