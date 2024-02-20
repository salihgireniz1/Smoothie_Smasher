using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OrderManager : MonoBehaviour
{
    public enum OrderType
    {
        Strawberry = 0,
        Apple = 1,
        Watermelon = 2,
        Orange = 3,
        Pineapple = 4,
        Carrot = 5,
        Banana = 6,
        Lemon = 7,
        Dragonfruit = 8,
        Blueberry = 9,
        Blackberry = 10,
        Grape = 11,
        Coconut = 12,
        Plum = 13,
        Pear = 14,
        Pomegranate = 15,
        Peach = 16,
        Melon = 17,
        Mango = 18,
        Starfruit = 19,
    }
    public enum OrderClassType
    {
        Regular,
        Gold
    }
    public enum OrderAmountType
    {
        Single,
        Double
    }
    public enum BottleType
    {
        Gold,
        Thick,
        Mason,
        Plastic
    }

    public GameObject OrderPrefab;
    public List<int> posXList = new();
    public List<Image> fruitImageList = new();
    public List<Image> BottleImageList = new();
    public bool isThereGoldOrder = false;
    [HideInInspector] public int goldOrderIndex1, goldOrderIndex2, goldOrderIndex3;
    public List<BottleData> JuiceDatas = new();
    int firstOrderCosts;

    void Start()
    {
        goldOrderIndex1 = -1;
        goldOrderIndex2 = -1;
        goldOrderIndex3 = -1;

        for (int i = 0; i < 4; i++)
        {
            GenerateOrder(i);
        }
    }

    public void CheckOrderCount()
    {
        StartCoroutine(FixTheOrders());
    }

    public void GenerateOrder(int index)
    {
        GameObject orderPrefab = Instantiate(OrderPrefab, new Vector3(posXList[index], 0, 0), Quaternion.identity, transform);
        orderPrefab.transform.localPosition = new Vector3(posXList[index], -180, 0);
        OrderController orderController = orderPrefab.GetComponent<OrderController>();
        // Only single order up to 11 level
        if (LevelManager.Instance.PlayerLevel <= 5)
        {
            orderController.orderAmountType = OrderAmountType.Single;
        }
        else
        {
            orderController.orderAmountType = (OrderAmountType)Random.Range(0, 2);
        }

        int index1 = goldOrderIndex1;
        int index2 = goldOrderIndex2;
        int index3 = goldOrderIndex3;
        if (orderController.orderAmountType == OrderAmountType.Single)
        {
            orderController.orderType1 = (OrderType)GetNewIndex(index1, index2, index3, -1);
        }
        else
        {
            orderController.orderType2 = (OrderType)GetNewIndex(index1, index2, index3, -1);
            orderController.orderType3 = (OrderType)GetNewIndex(index1, index2, index3, (int)orderController.orderType2);
        }

        // Gold order chance is 33 percent
        if (Random.Range(0, 3) == 1 && !isThereGoldOrder && LevelManager.Instance.PlayerLevel > 3)
        {
            orderController.orderClassType = OrderClassType.Gold;
            isThereGoldOrder = true;
            if (orderController.orderAmountType == OrderAmountType.Single)
            {
                goldOrderIndex1 = (int)orderController.orderType1;
                goldOrderIndex2 = -1;
                goldOrderIndex3 = -1;
            }
            else
            {
                goldOrderIndex1 = -1;
                goldOrderIndex2 = (int)orderController.orderType2;
                goldOrderIndex3 = (int)orderController.orderType3;
            }
        }
        else
        {
            orderController.orderClassType = OrderClassType.Regular;
        }
    }

    int GetNewIndex(int index1, int index2, int index3, int index4)
    {
        int newIndex;
        if (LevelManager.Instance.PlayerLevel <= 20)
        {
            newIndex = Random.Range(0, LevelManager.Instance.PlayerLevel);
        }
        else
        {
            newIndex = Random.Range(0, 20);
        }
        while (newIndex == index1 || newIndex == index2 || newIndex == index3 || newIndex == index4)
        {
            newIndex = Random.Range(0, LevelManager.Instance.PlayerLevel);
        }
        return newIndex;
    }

    IEnumerator FixTheOrders()
    {
        if (transform.childCount < 5)
        {
            GenerateOrder(4);
        }
        yield return new WaitForSeconds(0.1f);
        foreach (Transform item in transform)
        {
            if (item != null)
            {
                item.GetComponent<OrderController>().CheckFruitAmount();

                RectTransform orderPos = item.GetComponent<RectTransform>();
                orderPos.DOAnchorPos3DX(posXList[item.transform.GetSiblingIndex()], 0.3f);
            }
        }
    }
}
[System.Serializable]
public class BottleData
{
    public List<Sprite> FullJuicesList = new();
    public List<Sprite> HalfJuicesList = new();

}
