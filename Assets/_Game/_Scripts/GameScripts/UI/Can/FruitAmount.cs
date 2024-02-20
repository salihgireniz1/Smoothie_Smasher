using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FruitAmount : MonoBehaviour
{
    public Image FruitImage;
    public TextMeshProUGUI FruitAmountText;
    public FruitUpgradeObject fruitUpgradeObject;

    void Update()
    {
        if (fruitUpgradeObject != null)
        {
            FruitAmountText.text = LevelManager.Instance.fruitAmountList[(int)fruitUpgradeObject.fruitType].ToString();
        }
    }
}
