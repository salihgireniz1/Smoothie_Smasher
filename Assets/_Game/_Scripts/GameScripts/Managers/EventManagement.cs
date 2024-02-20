using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManagement
{
    public static event Action OnActiveWinPanel;
    public static event Action OnActiveLosePanel;
    public static event Action OnActiveStartPanel;
    public static event Action<float> OnScoreChange;
    public static event Action<int,Vector3> OnGoldChange;
    public static event Action OnPlayerLevelChange;
    public static event Action<int> OnOrderProgressChange;
    public static event Action< int> OnProgressChange;
    public static event Action<FruitUpgradeObject, bool, int> OnFruitAmountChange;
    public static event Action<Vector3, int> OnCollectMoneyUI;
    public static event Action<Vector3, int> OnCollectPlayerLevelUI;
    public static event Action OnTransitionStart;


    public static void Invoke_OnActiveWinPanel()
    {
        OnActiveWinPanel?.Invoke();
    }

    public static void Invoke_OnActiveLosePanel()
    {
        OnActiveLosePanel?.Invoke();
    }
    public static void Invoke_OnChangeStartPanelCondition()
    {
        OnActiveStartPanel?.Invoke();
    }
    public static void Invoke_OnGoldChange(int goldAmount,Vector3 position)
    {
        OnGoldChange?.Invoke(goldAmount,position);
    }

    public static void Invoke_OnPlayerLevelChange()
    {
        OnPlayerLevelChange?.Invoke();
    }

    public static void Invoke_OnOrderProgressChange(int amount)
    {
        OnOrderProgressChange?.Invoke(amount);
    }

    public static void Invoke_OnProgressChange(int progressAmount)
    {
        OnProgressChange?.Invoke(progressAmount);
    }

    public static void Invoke_OnFruitAmountChange(FruitUpgradeObject fruitUpgradeObject, bool isIncreasing, int fruitAmount)
    {
        OnFruitAmountChange?.Invoke(fruitUpgradeObject, isIncreasing, fruitAmount);
    }
    public static void Invoke_OnCollectMoneyUI(Vector3 UIStartPosition, int amount)
    {
        OnCollectMoneyUI?.Invoke(UIStartPosition, amount);
    }

    public static void Invoke_OnCollectPlayerLevelUI(Vector3 UIStartPosition, int amount)
    {
        OnCollectPlayerLevelUI?.Invoke(UIStartPosition, amount);
    }

    public static void Invoke_OnTransitionStart()
    {
        OnTransitionStart?.Invoke();
    }
}
