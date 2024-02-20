using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Rewarded : MonoBehaviour
{
    public abstract RewardedData RewardedData { get; set; }
    public abstract void ShowRewarded();
    public abstract void OpenRewarded();
    public abstract void CloseRewarded();
}
