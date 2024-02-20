using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Fruit : MonoBehaviour
{
    public FruitUpgradeObject fruitUpgradeObject;
    InGamePanelController inGamePanelController;

    void Awake()
    {
        inGamePanelController = FindObjectOfType<InGamePanelController>();
    }

    public void IncreaseFruitAmount()
    {
        if (fruitUpgradeObject != null)
        {
            //LevelManager.Instance.AddFruitAmount(fruitUpgradeObject,true,0);
            EventManagement.Invoke_OnFruitAmountChange(fruitUpgradeObject, true, 0);
            inGamePanelController.LightHaptic.PlayFeedbacks();
            inGamePanelController.GenerateSplashImage(fruitUpgradeObject, transform.position);
        }
    }
}
