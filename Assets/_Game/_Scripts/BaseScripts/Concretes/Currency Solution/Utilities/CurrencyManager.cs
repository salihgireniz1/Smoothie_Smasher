using PAG.Currency;
using PAG.Editor;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PAG.Utility;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PAG.Managers
{
    public class CurrencyManager : MonoSingleton<CurrencyManager>
    {
        public static event Action<Currencies, float> OnCurrencySpent;
        public static event Action<Currencies, float> OnCurrencyUpdate;
        
        public List<CurrencyData> activeCurrencies = new();
        public Dictionary<Currencies, CurrencyData> currencyEnumDataDict = new();
        [HideInInspector]
        public List<string> activeCurrencyNames = new();

        Dictionary<string, CurrencyData> currencyByEnumValue = new();

        private void Awake()
        {
            Initialize();
        }
        public void Initialize()
        {
            currencyByEnumValue = new();

            for (int i = 0; i < activeCurrencies.Count; i++)
            {
                // Remove any whitespace or blank characters from the display name
                string currencyName = activeCurrencyNames[i];

                currencyByEnumValue.Add(currencyName, activeCurrencies[i]);
            }

            // Loop through all enum values and fill enumDataDict
            foreach (Currencies currencyEnum in Enum.GetValues(typeof(Currencies)))
            {
                CurrencyData data = FindCurrencyInfo(currencyEnum);
                if (data != null)
                {
                    currencyEnumDataDict.Add(currencyEnum, data);
                }
            }
        }

        [Button("Automatically Find Datas")]
        public void DetectCurrencies()
        {
#if UNITY_EDITOR
            activeCurrencies = new List<CurrencyData>();
            activeCurrencies = AssetDatabaseUtils.FindAssetsByType<CurrencyData>();
            activeCurrencyNames = new();

            // Generate the elements array for the EnumGenerator.Generate() method
            string[] elements = new string[activeCurrencies.Count];
            for (int i = 0; i < activeCurrencies.Count; i++)
            {
                // Remove any whitespace or blank characters from the display name
                string currencyName = activeCurrencies[i].currencyName.Replace(" ", string.Empty);
                elements[i] = currencyName;
                activeCurrencyNames.Add(currencyName);
            }

            EnumGenerator.Generate(EnumConsts.CurrenciesEnumName, elements);
#endif
        }
        CurrencyData FindCurrencyInfo(Currencies currencyEnum)
        {
            Type type = Type.GetType(EnumConsts.CurrenciesEnumName);
            string displayName = Enum.GetName(type, currencyEnum);
            if (currencyByEnumValue.ContainsKey(displayName))
            {
                return currencyByEnumValue[displayName];
            }
            return null;
        }
        public CurrencyData GetCurrencyInfo(Currencies currencyEnum)
        {
            return currencyEnumDataDict[currencyEnum];
        }
        public void SpendCurrency(Currencies currencyEnum, float amount)
        {
            CurrencyData data = FindCurrencyInfo(currencyEnum);
            if (data.CanSpendCurrency(amount))
            {
                data.SpendCurrency(amount);
                OnCurrencySpent?.Invoke(currencyEnum, amount); 
                OnCurrencyUpdate?.Invoke(currencyEnum, data.GetWealth());

                Debug.Log($"{amount} {currencyEnum} spent!");
            }
        }
        public void EarnCurrency(Currencies currencyEnum, float amount)
        {
            SpendCurrency(currencyEnum, -amount);
        }

        public float GetWealth(Currencies currency)
        {
            CurrencyData data = FindCurrencyInfo(currency);
            return data.GetWealth();
        }
    }
}
