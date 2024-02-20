using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Udo.Hammer.Runtime.Core;
using System;

public class BoosterPanelManager : MonoBehaviour
{
    public enum BoosterType
    {
        Speed,
        HandSize,
        FruitSize
    }
    public GameObject BoosterPanel, CheckImage;
    public TextMeshProUGUI BoostCounterText;
    public BoosterController SpeedButton, HandSizeButton, FruitSpawnButton;
    public int CountdownTime;
    public float BoostTime;
    public int BoostCounter;
    bool isPanelOpen = false;
    PlayerMovement playerMovement;
    HandScaleHandler handScaleHandler;
    SpawnController spawnController;
    public UIParticleController uIParticleController;
    public bool isActiveButton = false;
    public GameObject LockImage, UnlockLevelText;
    public Image BoostImage, BGImage;
    Color firstColor;
    bool canClose = false;
    public bool isUnlocked
    {
        get => ES3.Load("isUnlocked", false);
        set => ES3.Save("isUnlocked", value);
    }
    public PlayerController playerController;

    public List<ButtonPressed> ButtonPressedList = new();
    [Header("Booster Factors")]
    public float SpeedBoostFactor;
    public float HandSizeBoostFactor;
    public float FruitSpawnBoostFactor;

    public TextMeshProUGUI CountDownText1, CountDownText2, CountDownText3;

    public int TotalSeconds { get => ES3.Load("totalSeconds", CountdownTime); set => ES3.Save("totalSeconds", value); } // Toplam geri sayım süresi (1 hafta = 604800 saniye)
    DateTime _startTime;
    TimeSpan _offlineTime1;
    TimeSpan _offlineTime2;
    TimeSpan _offlineTime3;
    public int passedTime1Key
    {
        get => ES3.Load("passedTime1Key", 0);
        set => ES3.Save("passedTime1Key", value);
    }

    public int passedTime2Key
    {
        get => ES3.Load("passedTime2Key", 0);
        set => ES3.Save("passedTime2Key", value);
    }
    public int passedTime3Key
    {
        get => ES3.Load("passedTime3Key", 0);
        set => ES3.Save("passedTime3Key", value);
    }

    float passedTime1 = 0;
    float passedTime2 = 0;
    float passedTime3 = 0;

    string lastDateTimeKeyA = "TimeKeyA";
    //string lastDateTimeKeyB = "TimeKeyB";
    //string lastDateTimeKeyC = "TimeKeyC";

    private void Awake()
    {
        firstColor = BGImage.color;
        uIParticleController = FindObjectOfType<UIParticleController>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        playerController = FindObjectOfType<PlayerController>();
        handScaleHandler = FindObjectOfType<HandScaleHandler>();
        spawnController = FindObjectOfType<SpawnController>();
    }

    private void Start()
    {
        CheckBoostButtonLocked();
        CheckTimeConditions();
        CheckBoostAmount();
    }

    public void CheckTimeConditions()
    {
        passedTime1 = (float)passedTime1Key;
        passedTime2 = (float)passedTime2Key;
        passedTime3 = (float)passedTime3Key;
        _startTime = DateTime.Now;

        if (ES3.KeyExists(lastDateTimeKeyA))
        {
            string lastDateTimeString = ES3.Load<string>(lastDateTimeKeyA);
            DateTime lastDateTime = DateTime.Parse(lastDateTimeString);

            _offlineTime1 = _startTime - lastDateTime;
            _offlineTime2 = _startTime - lastDateTime;
            _offlineTime3 = _startTime - lastDateTime;
            UpdateCountdownText(SpeedButton, _offlineTime1);
            UpdateCountdownText(HandSizeButton, _offlineTime2);
            UpdateCountdownText(FruitSpawnButton, _offlineTime3);
        }
        else
        {
            UpdateCountdownText(SpeedButton, _offlineTime1);
            UpdateCountdownText(HandSizeButton, _offlineTime2);
            UpdateCountdownText(FruitSpawnButton, _offlineTime3);
        }

        if (SpeedButton.canActive) SpeedButton.CompleteCountDown();
        else SpeedButton.StartCountDown();
        if (HandSizeButton.canActive) HandSizeButton.CompleteCountDown();
        else HandSizeButton.StartCountDown();
        if (FruitSpawnButton.canActive) FruitSpawnButton.CompleteCountDown();
        else FruitSpawnButton.StartCountDown();

        //if (boosterController.boosterType == BoosterType.Speed)
        //{
        //    passedTime1 = 0;
        //}
        //else if (boosterController.boosterType == BoosterType.HandSize)
        //{
        //    passedTime2 = 0;
        //}

        //else if (boosterController.boosterType == BoosterType.FruitSize)
        //{
        //    passedTime3 = 0;
        //}
    }

    private void UpdateCountdownText(BoosterController boosterController, TimeSpan _offlineTime) //, DateTime _startTime)
    {
        //TimeSpan elapsedTime = DateTime.Now - _startTime;
        //int remainingSeconds = TotalSeconds - (int)elapsedTime.TotalSeconds;
        int remainingSeconds = TotalSeconds;
        if (boosterController.boosterType == BoosterType.Speed)
        {
            passedTime1 += Time.deltaTime;
            remainingSeconds -= (int)passedTime1;
        }
        else if (boosterController.boosterType == BoosterType.HandSize)
        {
            passedTime2 += Time.deltaTime;
            remainingSeconds -= (int)passedTime2;
        }

        else if (boosterController.boosterType == BoosterType.FruitSize)
        {
            passedTime3 += Time.deltaTime;
            remainingSeconds -= (int)passedTime3;
        }
        //remainingSeconds = TotalSeconds - (int)passedTime1;
        remainingSeconds -= (int)_offlineTime.TotalSeconds;
        if (remainingSeconds < 0)
        {
            boosterController.timeIsUP = true;
            //boosterController.canActive = true;
            boosterController.CompleteCountDown();
        }
        else
        {
            ConvertSecondsToTime((int)remainingSeconds, out int hours, out int minutes, out int seconds);
            boosterController.CountDownText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);
        }
    }

    void Update()
    {
        UpdateCountdownText(SpeedButton, _offlineTime1);
        UpdateCountdownText(HandSizeButton, _offlineTime2);
        UpdateCountdownText(FruitSpawnButton, _offlineTime3);
        if (Input.GetMouseButtonDown(0))
        {
            CloseBoosterPanel();
        }
    }

    private void OnApplicationFocus(bool pause)
    {
        if (!pause)
        {
            ES3.Save<string>(lastDateTimeKeyA, DateTime.Now.ToString());
            passedTime1Key = (int)passedTime1;
            passedTime2Key = (int)passedTime2;
            passedTime3Key = (int)passedTime3;
        }
    }

    public void CheckBoostButtonLocked()
    {
        if (LevelManager.Instance.PlayerLevel >= 4)
        {
            isActiveButton = true;
            LockImage.SetActive(false);
            UnlockLevelText.SetActive(false);
            BoostImage.color = Color.white;
            BGImage.color = firstColor;
            //SpeedButton.timeIsUP = true;
            //HandSizeButton.timeIsUP = true;
            //FruitSpawnButton.timeIsUP = true;
            if (!isUnlocked)
            {
                isUnlocked = true;
                //CheckTimeConditions();
            }
        }
        else
        {
            isActiveButton = false;
            BoostImage.color = Color.gray;
            BGImage.color = Color.gray;
            CheckImage.SetActive(false);
        }
    }

    public void CheckBoostAmount()
    {
        if (isActiveButton)
        {
            BoostCounter = 0;
            if (SpeedButton.canActive) BoostCounter++;
            if (HandSizeButton.canActive) BoostCounter++;
            if (FruitSpawnButton.canActive) BoostCounter++;

            if (BoostCounter > 0 && isActiveButton)
            {
                CheckImage.SetActive(true);
                BoostCounterText.text = BoostCounter.ToString();
            }
            else
            {
                CheckImage.SetActive(false);
            }
        }
    }

    public bool CheckActiveBooster()
    {
        if (SpeedButton.isActive) return true;
        if (HandSizeButton.isActive) return true;
        if (FruitSpawnButton.isActive) return true;
        return false;
    }

    public void CloseBoosterPanel()
    {
        canClose = false;
        for (int i = 0; i < ButtonPressedList.Count; i++)
        {
            if (ButtonPressedList[i].buttonPressed) canClose = true;
        }

        if (!canClose)
        {
            if (isPanelOpen)
            {
                isPanelOpen = false;
                BoosterPanel.SetActive(false);
            }
        }
    }

    public void OpenAndCloseBoosterPanel()
    {
        if (isActiveButton)
        {
            if (!isPanelOpen)
            {
                isPanelOpen = true;
                BoosterPanel.SetActive(true);
            }
            else
            {
                isPanelOpen = false;
                BoosterPanel.SetActive(false);
            }
        }
    }

    public void SpeedButtonPressed()
    {
        StartCoroutine(CountDownStart(SpeedButton));
        playerMovement.GetSpeedReward(SpeedBoostFactor, BoostTime);  // 1.25f
        passedTime1 = 0;
        passedTime1Key = (int)passedTime1;
    }

    public void GrabSizeButtonPressed()
    {
        StartCoroutine(CountDownStart(HandSizeButton));
        handScaleHandler.GetHandScaleReward(HandSizeBoostFactor, BoostTime); // 1.5f
        passedTime2 = 0;
        passedTime2Key = (int)passedTime2;
    }

    public void FruitSpawnButtonPressed()
    {
        StartCoroutine(CountDownStart(FruitSpawnButton));
        spawnController.GetSpawnSpeedReward(FruitSpawnBoostFactor, BoostTime);// 0.33f
        passedTime3 = 0;
        passedTime3Key = (int)passedTime3;
    }

    public void ShowRewardSpeed()
    {
        LevelManager.Instance.ResetInterTimer();
        passedTime1 = 0;
        passedTime1Key = (int)passedTime1;
        Hammer.Instance.MEDIATION_HasRewarded(
            () =>
            {
                Hammer.Instance.MEDIATION_ShowRewarded(
                     () =>
                     {
                         Debug.Log("Rewarded gösterildi");
                         StartCoroutine(CountDownStart(SpeedButton));
                         Hammer.Instance.ANALYTICS_CustomEvent("SpeedBoosterRewarded", LevelManager.Instance.PlayerLevel);
                         playerMovement.GetSpeedReward(2, BoostTime);
                         // ödülü ver 
                     },
               s =>
               {
                   Debug.Log("Rewarded gösterilemedi");
               }
               );
            },
    s =>
    {
        Debug.Log("Rewarded yüklenemedi");
    }
        );
    }

    public void ShowRewardHandSize()
    {
        LevelManager.Instance.ResetInterTimer();
        passedTime2 = 0;
        passedTime2Key = (int)passedTime2;
        Hammer.Instance.MEDIATION_HasRewarded(
            () =>
            {
                Hammer.Instance.MEDIATION_ShowRewarded(
                     () =>
                     {
                         Debug.Log("Rewarded gösterildi");
                         StartCoroutine(CountDownStart(HandSizeButton));
                         Hammer.Instance.ANALYTICS_CustomEvent("HandSizeBoosterRewarded", LevelManager.Instance.PlayerLevel);
                         handScaleHandler.GetHandScaleReward(1.25f, BoostTime);
                         spawnController.GetSpawnSpeedReward(2, BoostTime);
                         // ödülü ver 
                     },
               s =>
               {
                   Debug.Log("Rewarded gösterilemedi");
               }
               );
            },
    s =>
    {
        Debug.Log("Rewarded yüklenemedi");
    }
        );
    }

    public void ShowRewardFruitSpawn()
    {
        LevelManager.Instance.ResetInterTimer();
        passedTime3 = 0;
        passedTime3Key = (int)passedTime3;
        Hammer.Instance.MEDIATION_HasRewarded(
            () =>
            {
                Hammer.Instance.MEDIATION_ShowRewarded(
                     () =>
                     {
                         Debug.Log("Rewarded gösterildi");
                         StartCoroutine(CountDownStart(FruitSpawnButton));
                         Hammer.Instance.ANALYTICS_CustomEvent("FruitSpawnBoosterRewarded", LevelManager.Instance.PlayerLevel);
                         // ödülü ver 
                     },
               s =>
               {
                   Debug.Log("Rewarded gösterilemedi");
               }
               );
            },
    s =>
    {
        Debug.Log("Rewarded yüklenemedi");
    }
        );
    }

    IEnumerator CountDownStart(BoosterController boosterController)
    {
        Hammer.Instance.ANALYTICS_CustomEvent("Booster", LevelManager.Instance.PlayerLevel);
        boosterController.canActive = false;
        CheckBoostAmount();
        boosterController.isActive = true;
        boosterController.Button.interactable = false;
        boosterController.RWButton.gameObject.SetActive(false);
        boosterController.CountDownParent.SetActive(false);
        boosterController.FreeText.gameObject.SetActive(true);
        boosterController.FreeText.text = "Active";
        var tempColor1 = boosterController.BlackImage.color;
        tempColor1.a = 1f;
        boosterController.BlackImage.color = tempColor1;
        boosterController.BlackImage.fillAmount = 0;
        boosterController.BoostTimeParent.SetActive(true);
        uIParticleController.PlayBoosterPanelParticle();
        playerController.PlayBoosterParticle();
        if (boosterController.tempCDTime <= 0.5f) boosterController.tempCDTime = (float)CountdownTime;
        float time1 = BoostTime;
        boosterController.BoostParticle.Play();
        while (time1 >= 0)
        {
            time1 -= Time.deltaTime;
            boosterController.BlackImage.fillAmount = (BoostTime - time1) / BoostTime;
            boosterController.BoostTimeText.text = ((int)time1).ToString() + "s";
            yield return null;
        }
        boosterController.StartCountDown();
        //yield return new WaitUntil(() => boosterController.timeIsUP);
        //boosterController.CompleteCountDown();
    }

    void ConvertSecondsToTime(int seconds, out int hours, out int minutes, out int remainingSeconds)
    {
        hours = seconds / 3600;
        minutes = (seconds % 3600) / 60;
        remainingSeconds = seconds % 60;
    }
}
