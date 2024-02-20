using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagPanelController : MonoBehaviour
{
    public FruitUpgradeManager fruitUpgradeManager;
    OrderManager orderManager;
    public GameObject FruitAmountPrefab, BagParent;
    int maxFruit;

    void Start()
    {
        orderManager = FindObjectOfType<OrderManager>();
        if (LevelManager.Instance.PlayerLevel <= 20)
        {
            maxFruit = LevelManager.Instance.PlayerLevel;
        }
        else
        {
            maxFruit = 20;
        }
        for (int i = 0; i < maxFruit; i++)
        {
            GenerateNewFruitAmount(i);
        }
    }

    public void GenerateNewFruitAmount(int index)
    {
        GameObject fruit = Instantiate(FruitAmountPrefab, BagParent.transform);
        FruitAmount fruitAmount = fruit.GetComponent<FruitAmount>();
        fruitAmount.fruitUpgradeObject = fruitUpgradeManager.FruitUpgradeObjectList[index];
        fruitAmount.FruitImage.sprite = orderManager.fruitImageList[index].sprite;
    }


    public void OpenBagPanel()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void CloseBagPanel()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
