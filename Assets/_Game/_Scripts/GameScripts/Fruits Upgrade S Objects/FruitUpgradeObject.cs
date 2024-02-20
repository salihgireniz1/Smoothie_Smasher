using System.Collections;
using System.Collections.Generic;
using Udo.Hammer.Runtime.Core;
using UnityEngine;
using UnityEngine.UI;
using static OrderManager;

[CreateAssetMenu(fileName = "UpgradeObjects", menuName = "Upgrade/Fruit", order = 1)]
public class FruitUpgradeObject : ScriptableObject
{
    public OrderType fruitType;
    int firstUpgradeCost = 25;
    public List<float> upgradeMultiplyList = new();
    //public int upgradeCost;

    public int upgradeLevel
    {
        get => ES3.Load("upgradeLevel" + fruitType.ToString(), 0);
        set => ES3.Save("upgradeLevel" + fruitType.ToString(), value);
    }

    public int upgradeCost
    {
        get => ES3.Load("upgradeCost" + fruitType.ToString(), firstUpgradeCost);
        set => ES3.Save("upgradeCost" + fruitType.ToString(), value);
    }
    public int harvestFactor
    {
        get => ES3.Load("harvestFactor" + fruitType.ToString(), 1);
        set => ES3.Save("harvestFactor" + fruitType.ToString(), value);
    }
    public float fruitSize
    {
        get => ES3.Load("fruitSize" + fruitType.ToString(), 1f);
        set => ES3.Save("fruitSize" + fruitType.ToString(), value);
    }

    public bool isInit
    {
        get => ES3.Load("isInit" + fruitType.ToString(), false);
        set => ES3.Save("isInit" + fruitType.ToString(), value);
    }
    public bool isUnlocked
    {
        get => ES3.Load("isUnlocked" + fruitType.ToString(), false);
        set => ES3.Save("isUnlocked" + fruitType.ToString(), value);
    }

    public bool isMax
    {
        get => ES3.Load("isMax" + fruitType.ToString(), false);
        set => ES3.Save("isMax" + fruitType.ToString(), value);
    }

    public int GetFirstUpgradeCost()
    {
        if (!isInit)
        {
            upgradeCost = LevelManager.Instance.FirstUpgradeCost * ((int)fruitType + 1);
            isInit = true;
        }

        return upgradeCost;
    }

    public void UpgradeFruit()
    {
        Hammer.Instance.ANALYTICS_CustomEvent("UpgradedFruit", LevelManager.Instance.PlayerLevel);
        upgradeLevel++;
        harvestFactor++;
        fruitSize += 0.125f;
        if (upgradeLevel <= LevelManager.Instance.UpgradeMultiplyFactorList.Count)
        {
            upgradeCost = (int)((float)upgradeCost * LevelManager.Instance.UpgradeMultiplyFactorList[upgradeLevel - 1]);
        }
    }

    public void ResetDatas()
    {
        firstUpgradeCost = 20;
        upgradeCost = firstUpgradeCost;
        harvestFactor = 1;
        fruitSize = 1;
        upgradeLevel = 0;
        isInit = false;
        isUnlocked = false;
        upgradeMultiplyList.Clear();
        upgradeMultiplyList.Add(6);
        upgradeMultiplyList.Add(5);
        upgradeMultiplyList.Add(2.5f);
    }
}
