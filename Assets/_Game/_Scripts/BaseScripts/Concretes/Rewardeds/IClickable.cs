using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IClickable
{
    public Vector3 RewardedButtonPosition { get; }
    public Rewarded Rewarded { get; }
    public void GetReward();

}
