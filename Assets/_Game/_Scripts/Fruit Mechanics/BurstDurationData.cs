using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Burst Duration", fileName = "Burst Duration Data")]
public class BurstDurationData : ScriptableObject
{
    public List<float> burstDurations = new List<float>();
}