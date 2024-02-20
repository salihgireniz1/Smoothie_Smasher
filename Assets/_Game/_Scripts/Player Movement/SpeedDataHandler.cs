using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedDataHandler : MonoBehaviour
{
    public float CurrentSpeed => currentSpeed;
    public float CurrentRot => currentRot;
    public int SpeedLevel
    {
        get => ES3.Load(Consts.SPEED_LEVEL, 0);
        set
        {
            ES3.Save(Consts.SPEED_LEVEL, value);
            currentSpeed = GetCurrentSpeed();
            currentRot = GetCurrentRot();
        }
    }
    public List<float> speeds = new List<float>();
    public List<float> rots = new List<float>();
    private float currentSpeed;
    private float currentRot;

    private void Start()
    {
        currentSpeed = GetCurrentSpeed();
        currentRot = GetCurrentRot();
    }
    public float GetCurrentRot()
    {
        int level = SpeedLevel;
        // Clamp level value.
        level = Mathf.Min(level, rots.Count - 1);
        level = Mathf.Max(0, level);

        return rots[level];
    }
    public float GetCurrentSpeed()
    {
        int level = SpeedLevel;
        // Clamp level value.
        level = Mathf.Min(level, speeds.Count - 1);
        level = Mathf.Max(0, level);

        return speeds[level];
    }

    [Button("Change Speed To:")]
    public void ChangeSpeed(float targetSpeed)
    {
        currentSpeed = targetSpeed;
    }

    [Button("Change Rotation Speed To:")]
    public void ChangeRotSpeed(float targetSpeed)
    {
        currentRot = targetSpeed;
    }

    public void ChangeSpeedLevel(int newLevel)
    {
        SpeedLevel = newLevel;
    }
    public void IncreaseSpeedLevel()
    {
        SpeedLevel += 1;
    }
}