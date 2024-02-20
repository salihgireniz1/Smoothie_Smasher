using UnityEngine;

namespace PAG.Currency
{
    [CreateAssetMenu(fileName = "New Currency", menuName = "Scriptable Objects/Currency")]
    public class CurrencyData : ScriptableObject
    {
        public string currencyName;
        public Sprite currencyImage;
        public string currencySymbol;
        public float startAmount;

        public float GetWealth()
        {
            return ES3.Load(currencyName + "_Wealth", startAmount);
        }
        public bool CanSpendCurrency(float spendAmount)
        {
            return (GetWealth() - spendAmount) >= 0f;
        }

        public void EarnCurrency(float earnAmount)
        {
            float wealth = GetWealth();
            wealth += earnAmount;
            ES3.Save(currencyName + "_Wealth", wealth);
        }

        public void SpendCurrency(float spendAmount)
        {
            if (CanSpendCurrency(spendAmount))
            {
                EarnCurrency(-spendAmount);
            }
        }

        public void BruteSave(float amount)
        {
            ES3.Save(currencyName + "_Wealth", amount);
        }
    }
}