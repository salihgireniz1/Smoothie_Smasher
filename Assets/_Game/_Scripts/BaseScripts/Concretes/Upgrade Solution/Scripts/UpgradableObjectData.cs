using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace PAG.UpgradeSystem
{
    [CreateAssetMenu(fileName = "Upgradable Object Data", menuName = "Scriptable Objects/Upgradable Object")]
    public class UpgradableObjectData : ScriptableObject
    {
        #region Upgrade Settings
        [TabGroup("Upgrade Settings")]
        public Currencies currencyType;

        [TabGroup("Upgrade Settings")]
        public UpgradeType upgradeType;

        [EnumToggleButtons, TabGroup("Upgrade Settings")]
        public PricingType pricingType;

        [ShowIf("IsMultiplyOrIncrement"), TabGroup("Upgrade Settings")]
        public float startPrice;

        [ShowIf("IsMultiplyOrIncrement"), TabGroup("Upgrade Settings")]
        public int maxUpgradeCount;

        [ShowIf("IsListed"), TabGroup("Upgrade Settings")]
        public List<float> prices;

        [ShowIf("IsIncrement"), TabGroup("Upgrade Settings")]
        public float priceIncreaseAmount;

        [ShowIf("IsMultiply"), TabGroup("Upgrade Settings")]
        public float priceCoefficient;

        [ShowIf("IsGraph"), TabGroup("Upgrade Settings")]
        public AnimationCurve priceCurve;
        #endregion

        #region Power Settings
        [TabGroup("Power Settings")]
        public bool isPercentage;

        [EnumToggleButtons, TabGroup("Power Settings")]
        public PricingType powerIncreaseType;

        [ShowIf("PowerTypeIsMultiplyOrIncrement"), TabGroup("Power Settings")]
        public float startPower;

        [ShowIf("PowerTypeIsListed"), TabGroup("Power Settings")]
        public List<float> powers;

        [ShowIf("PowerTypeIsIncrement"), TabGroup("Power Settings")]
        public float powerIncreaseAmount;

        [ShowIf("PowerTypeIsMultiply"), TabGroup("Power Settings")]
        public float powerCoefficient;

        [ShowIf("PowerTypeIsGraph"), TabGroup("Power Settings")]
        public AnimationCurve powerCurve;
        #endregion

        #region Visual Settings
        [TabGroup("Visual Settings")]
        public string displayName;

        [TabGroup("Visual Settings")]
        public Sprite upgradeSprite;

        [TabGroup("Visual Settings")]
        public string longExplanation;
        #endregion

        #region Public Methods
        public bool IsMax()
        {
            bool isMax = false;
            int currentLevel = GetCurrentLevel();
            switch (pricingType)
            {
                case PricingType.Listed:
                    int maxCount = prices.Count - 1;
                    if (currentLevel >= maxCount) isMax = true;
                    break;
                default:
                    if (currentLevel >= maxUpgradeCount) isMax = true;
                    break;
            }
            return isMax;
        }
        public int GetCurrentLevel()
        {
            return UpgradeRecordSystem.GetLevel(displayName);
        }
        public void IncreaseUpgradeLevel()
        {
            UpgradeRecordSystem.Upgrade(displayName);
        }
        public void IncreaseUpgradeLevel(int amount)
        {
            UpgradeRecordSystem.Upgrade(displayName, amount);
        }
        public float GetCurrentPrice()
        {
            int currentLevel = UpgradeRecordSystem.GetLevel(displayName);
            return GetPriceAtLevel(currentLevel);
        }
        public float GetPreviousPrice()
        {
            int currentLevel = UpgradeRecordSystem.GetLevel(displayName);
            return GetPriceAtLevel(currentLevel - 1);
        }
        public float GetNextPrice()
        {
            int currentLevel = UpgradeRecordSystem.GetLevel(displayName);
            return GetPriceAtLevel(currentLevel + 1);
        }
        public float GetPriceAtLevel(int currentLevel)
        {
            float currentPrice = 0;
            switch (pricingType)
            {
                case PricingType.Increment:
                    currentPrice = startPrice + (priceIncreaseAmount * currentLevel);
                    break;
                case PricingType.Multiply:
                    currentPrice = startPrice * Mathf.Pow(priceCoefficient, currentLevel);
                    break;
                case PricingType.Listed:
                    if(currentLevel > prices.Count-1)
                    {
                        currentLevel = prices.Count - 1;
                    }
                    currentPrice = prices[currentLevel];
                    break;
                case PricingType.Graph:
                    break;
                default:
                    break;
            }
            return currentPrice;
        }
        public float GetCurrentPower()
        {
            int currentLevel = UpgradeRecordSystem.GetLevel(displayName);
            return GetPowerAtLevel(currentLevel);
        }
        public float GetPreviousPower()
        {
            int currentLevel = UpgradeRecordSystem.GetLevel(displayName);
            return GetPowerAtLevel(currentLevel-1);
        }
        public float GetNextPower()
        {
            int currentLevel = UpgradeRecordSystem.GetLevel(displayName);
            return GetPowerAtLevel(currentLevel + 1);
        }
        public float GetPowerAtLevel(int currentLevel)
        {
            float currentPower = 0;
            switch (powerIncreaseType)
            {
                case PricingType.Increment:
                    currentPower = startPower + (powerIncreaseAmount * currentLevel);
                    //Debug.Log(currentPower);
                    break;
                case PricingType.Multiply:
                    currentPower = startPower * Mathf.Pow(powerCoefficient, currentLevel);
                    break;
                case PricingType.Listed:
                    if (currentLevel > powers.Count - 1)
                    {
                        currentLevel = powers.Count - 1;
                    }
                    currentPower = prices[currentLevel];
                    break;
                case PricingType.Graph:
                    break;
                default:
                    break;
            }
            return currentPower;
        }
        #endregion

        #region pricingType Check Methods

        private bool IsMultiplyOrIncrement()
        {
            return pricingType == PricingType.Multiply || pricingType == PricingType.Increment;
        }
        private bool IsNotListed()
        {
            return pricingType != PricingType.Listed;
        }

        private bool IsListed()
        {
            return pricingType == PricingType.Listed;
        }

        private bool IsMultiply()
        {
            return pricingType == PricingType.Multiply;
        }

        private bool IsIncrement()
        {
            return pricingType == PricingType.Increment;
        }

        private bool IsGraph()
        {
            return pricingType == PricingType.Graph;
        }
        #endregion

        #region powerIncreaseType Check Methods

        private bool PowerTypeIsMultiplyOrIncrement()
        {
            return 
                powerIncreaseType == PricingType.Multiply || 
                powerIncreaseType == PricingType.Increment;
        }
        private bool PowerTypeIsNotListed()
        {
            return powerIncreaseType != PricingType.Listed;
        }

        private bool PowerTypeIsListed()
        {
            return powerIncreaseType == PricingType.Listed;
        }

        private bool PowerTypeIsMultiply()
        {
            return powerIncreaseType == PricingType.Multiply;
        }

        private bool PowerTypeIsIncrement()
        {
            return powerIncreaseType == PricingType.Increment;
        }
        
        private bool PowerTypeIsGraph()
        {
            return powerIncreaseType == PricingType.Graph;
        }
        #endregion
    }
}