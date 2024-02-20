using PAG.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BurstManager : MonoBehaviour
{
    public static BurstManager Instance { get; private set; }
    public int BurstDurationLevel
    {
        get => ES3.Load(Consts.BURST_DURATION_LEVEL, 0);
        set
        {
            ES3.Save(Consts.BURST_DURATION_LEVEL, value);
            GetBurstDuration();
        }
    }

    private Queue<BurstHandler> burstQueue = new Queue<BurstHandler>();
    private bool isBursting = false;
    BurstHandler nextFruit;
    ComboSound cs;
    bool isFirstTime;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
        GetBurstDuration();
        cs = GetComponent<ComboSound>();
    }
    private void Start()
    {
        isFirstTime = SpawnController.Instance.FirstTime;
    }
    public float CurrentBurstDuration
    {
        get => currentBurstDuration;
        set => currentBurstDuration = value;
    }
    public BurstDurationData datas;

    [SerializeField]
    private float currentBurstDuration;

    [SerializeField] DragToMoveController dragToMoveController;
    public void IncreaseBurstLevel()
    {
        BurstDurationLevel += 1;
    }

    public float GetBurstDuration()
    {
        CurrentBurstDuration = datas.burstDurations[BurstDurationLevel];
        return CurrentBurstDuration;
    }
    public void AddToFruitQueue(BurstHandler burstHandler)
    {
        burstQueue.Enqueue(burstHandler);
        if (!isBursting)
        {
            isBursting = true;
            SpawnController.Instance.CanSpawn = false;
        }
    }
    public float burstDelay = 0.1f;
    private float lastBurstTime = .1f;
    private bool isDelaying = false;
    private void Update()
    {
        if (isDelaying)
        {
            // Check if the delay time has passed
            if (Time.time - lastBurstTime >= burstDelay)
            {
                isDelaying = false;

                // Continue with processing the burstQueue
                if (burstQueue.Count > 0)
                {
                    nextFruit = burstQueue.Dequeue();
                    if (nextFruit != null && nextFruit.gameObject != null)
                    {
                        SpawnController.Instance.CanSpawn = false;
                        nextFruit.Burst();
                        cs.PlayComboSound();
                    }
                }
                else
                {
                    // Finish bursting
                    isBursting = false;
                    cs.ResetPitch();
                    //Debug.Log("NO MORE BURST!");
                }
            }
        }
        else if (isBursting && burstQueue.Count > 0)
        {
            // Start the delay timer
            lastBurstTime = Time.time;
            isDelaying = true;
        }
        else if (isBursting)
        {
            // Finish bursting
            isBursting = false;
            if (!isFirstTime)
            {
                SpawnController.Instance.CanSpawn = true;
            }
            else
            {
                SpawnController.Instance.SpawnOnboardFruit();
                Debug.Log("Still onboarding");
            }
            cs.ResetPitch();
            //Debug.Log("NO MORE BURST!");
        }
    }
    public static event Action OnBurst;
    public void HandleUnonboard()
    {
        if (dragToMoveController != null)
        {
            dragToMoveController.UnonboardMe();
        }
    }
}