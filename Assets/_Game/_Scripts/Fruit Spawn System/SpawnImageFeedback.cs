using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpawnImageFeedback : MonoBehaviour
{
    public FruitUpgradeObject fruitUpgradeObject;
    public Image FruitImage;
    public TextMeshProUGUI HarvestText;
    OrderManager orderManager;

    private void Awake()
    {
        orderManager = FindObjectOfType<OrderManager>();
    }
    void Start()
    {
        if (fruitUpgradeObject != null)
        {
            FruitImage.sprite = orderManager.fruitImageList[(int)fruitUpgradeObject.fruitType].sprite;
            HarvestText.text = "+" + fruitUpgradeObject.harvestFactor;
        }
    }
}
