using System.Collections;
using System.Collections.Generic;
using Udo.Hammer.Runtime.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    [SerializeField] private float _loadingWaitTime = 1f;
    [SerializeField] private int _firstLevelIndex = 1;
    [SerializeField] private int _randomMinLevelIndex = 2;
    private int _sceneCount;

    //public int Level
    //{
    //    get => ES3.Load("Level", _firstLevelIndex);
    //    set => ES3.Save("Level", value);
    //}
    //public int RandomLevel
    //{
    //    get => ES3.Load("RandomLevel", _randomMinLevelIndex);
    //    set => ES3.Save("RandomLevel", value);
    //}

    public int PlayerLevel
    {
        get => ES3.Load("PlayerLevel", 1);
        set => ES3.Save("PlayerLevel", value);
    }

    public int SpawnerMaxCount
    {
        get => ES3.Load("SpawnerMaxCount", 10);
        set => ES3.Save("SpawnerMaxCount", value);
    }

    public int SpawnerCurrentCount
    {
        get => ES3.Load("SpawnerCurrentCount", 0);
        set => ES3.Save("SpawnerCurrentCount", value);
    }

    public int CharacterUpgradeLevel
    {
        get => ES3.Load("CharacterUpgradeLevel", 0);
        set => ES3.Save("CharacterUpgradeLevel", value);
    }

    public int CurrentAppleAmount
    {
        get => ES3.Load("CurrentAppleAmount", 0);
        set => ES3.Save("CurrentAppleAmount", value);
    }

    public int CurrentStrawberryAmount
    {
        get => ES3.Load("CurrentStrawberryAmount", 0);
        set => ES3.Save("CurrentStrawberryAmount", value);
    }

    public int CurrentLemonAmount
    {
        get => ES3.Load("CurrentLemonAmount", 0);
        set => ES3.Save("CurrentLemonAmount", value);
    }
    public int CurrentBananaAmount
    {
        get => ES3.Load("CurrentBananaAmount", 0);
        set => ES3.Save("CurrentBananaAmount", value);
    }
    public int CurrentOrangeAmount
    {
        get => ES3.Load("CurrentOrangeAmount", 0);
        set => ES3.Save("CurrentOrangeAmount", value);
    }
    public int CurrentWatermelonAmount
    {
        get => ES3.Load("CurrentWatermelonAmount", 0);
        set => ES3.Save("CurrentWatermelonAmount", value);
    }
    public int CurrentCarrotAmount
    {
        get => ES3.Load("CurrentCarrotAmount", 0);
        set => ES3.Save("CurrentCarrotAmount", value);
    }
    public int CurrentDragonfruitAmount
    {
        get => ES3.Load("CurrentKiwiAmount", 0);
        set => ES3.Save("CurrentKiwiAmount", value);
    }
    public int CurrentBlueberryAmount
    {
        get => ES3.Load("CurrentBlueberryAmount", 0);
        set => ES3.Save("CurrentBlueberryAmount", value);
    }
    public int CurrentBlackberryAmount
    {
        get => ES3.Load("CurrentBlackberryAmount", 0);
        set => ES3.Save("CurrentBlackberryAmount", value);
    }
    public int CurrentGrapeAmount
    {
        get => ES3.Load("CurrentGrapeAmount", 0);
        set => ES3.Save("CurrentGrapeAmount", value);
    }
    public int CurrentCoconutAmount
    {
        get => ES3.Load("CurrentCoconutAmount", 0);
        set => ES3.Save("CurrentCoconutAmount", value);
    }
    public int CurrentPlumAmount
    {
        get => ES3.Load("CurrentPlumAmount", 0);
        set => ES3.Save("CurrentPlumAmount", value);
    }
    public int CurrentPineappleAmount
    {
        get => ES3.Load("CurrentPineappleAmount", 0);
        set => ES3.Save("CurrentPineappleAmount", value);
    }

    public int CurrentPearAmount
    {
        get => ES3.Load("CurrentPearAmount", 0);
        set => ES3.Save("CurrentPearAmount", value);
    }
    public int CurrentPomegranateAmount
    {
        get => ES3.Load("CurrentPomegranateAmount", 0);
        set => ES3.Save("CurrentPomegranateAmount", value);
    }
    public int CurrentPeachAmount
    {
        get => ES3.Load("CurrentPeachAmount", 0);
        set => ES3.Save("CurrentPeachAmount", value);
    }
    public int CurrentMelonAmount
    {
        get => ES3.Load("CurrentMelonAmount", 0);
        set => ES3.Save("CurrentMelonAmount", value);
    }
    public int CurrentMangoAmount
    {
        get => ES3.Load("CurrentMangoAmount", 0);
        set => ES3.Save("CurrentMangoAmount", value);
    }
    public int CurrentStarfruitAmount
    {
        get => ES3.Load("CurrentStarfruitAmount", 0);
        set => ES3.Save("CurrentStarfruitAmount", value);
    }
    public bool FirstStart
    {
        get => ES3.Load("FirstStart", false);
        set => ES3.Save("FirstStart", value);
    }

    public List<int> fruitAmountList = new();
    public CharacterButtonOnboard cbo;
    public UpgradeButtonOnboard ubo;
    GroundSizeController groundSizeController;
    public int CurrentGold
    {
        get
        {
            currentGold = ES3.Load("CurrentGold", 0);
            return currentGold;
        }
        set
        {
            currentGold = value;
            ES3.Save("CurrentGold", currentGold);

            if (currentGold >= FirstUpgradeCost)
            {
                // Fruit Upgrade onboard.
                if (ubo && !ubo.IsOnboarded)
                {
                    OnboardingManager.Instance.OnboardObject(ubo);
                }
            }
            if (currentGold >= CharacterUpgradeCostList[0] && PlayerLevel >= 3)//5
            {
                // Character upgrade onboard.
                if (cbo && !cbo.IsOnboarded && groundSizeController.ScaleLevel > 1)
                {
                    OnboardingManager.Instance.OnboardObject(cbo, true);
                }
            }
        }
    }
    int currentGold;
    public int CurrentProgressAmount
    {
        get => ES3.Load("CurrentProgressAmount", 0);
        set => ES3.Save("CurrentProgressAmount", value);
    }

    public int CurrentOrderProgressAmount
    {
        get => ES3.Load("CurrentOrderProgressAmount", 0);
        set => ES3.Save("CurrentOrderProgressAmount", value);
    }

    public float CurrentPlayerSpeed
    {
        get => ES3.Load("CurrentPlayerSpeed", 1);
        set => ES3.Save("CurrentPlayerSpeed", value);
    }

    public float CurrentGrabSize
    {
        get => ES3.Load("CurrentGrabSize", 1);
        set => ES3.Save("CurrentGrabSize", value);
    }

    public float CurrentPlayerSize
    {
        get => ES3.Load("CurrentPlayerSize", 1);
        set => ES3.Save("CurrentPlayerSize", value);
    }
    [Header("Design Parameters")]

    public List<int> ProgressAmountList = new();
    public List<int> CharacterUpgradeCostList = new();
    public int FirstUpgradeCost;
    public List<float> UpgradeMultiplyFactorList = new();
    public List<int> LevelUpRewardList = new();

    private float _interDuration = 90f;
    private float _timePassed = 0;
    private bool _watchInter = false;


    private void Awake()
    {
        Singelton();
        groundSizeController = FindObjectOfType<GroundSizeController>();
        //if (!FirstStart)
        //{
        //    FirstStart = true;
        //    ResetSOData.ResetSO();
        //}
        fruitAmountList.Add(CurrentStrawberryAmount);
        fruitAmountList.Add(CurrentAppleAmount);
        fruitAmountList.Add(CurrentWatermelonAmount);
        fruitAmountList.Add(CurrentOrangeAmount);
        fruitAmountList.Add(CurrentPineappleAmount);
        fruitAmountList.Add(CurrentCarrotAmount);
        fruitAmountList.Add(CurrentBananaAmount);
        fruitAmountList.Add(CurrentLemonAmount);
        fruitAmountList.Add(CurrentDragonfruitAmount);
        fruitAmountList.Add(CurrentBlueberryAmount);
        fruitAmountList.Add(CurrentBlackberryAmount);
        fruitAmountList.Add(CurrentGrapeAmount);
        fruitAmountList.Add(CurrentCoconutAmount);
        fruitAmountList.Add(CurrentPlumAmount);
        fruitAmountList.Add(CurrentPearAmount);
        fruitAmountList.Add(CurrentPomegranateAmount);
        fruitAmountList.Add(CurrentPeachAmount);
        fruitAmountList.Add(CurrentMelonAmount);
        fruitAmountList.Add(CurrentMangoAmount);
        fruitAmountList.Add(CurrentStarfruitAmount);
    }
    //ResetSOData resetSOData = new();
    //private void Start()
    //{
    //    if (!FirstStart)
    //    {
    //        FirstStart = true;
    //        ResetSOData.ResetSO();
    //    }
    //}

    private void OnEnable()
    {
        EventManagement.OnFruitAmountChange += AddFruitAmount;
    }

    private void OnDisable()
    {
        EventManagement.OnFruitAmountChange -= AddFruitAmount;
    }


    public void AddFruitAmount(FruitUpgradeObject fruitSO, bool isIncreasing, int fruitAmount)
    {
        if (fruitSO != null)
        {
            if (isIncreasing)
            {
                fruitAmountList[(int)fruitSO.fruitType] += (int)fruitSO.harvestFactor;
                SaveFruit((int)fruitSO.fruitType);
            }
            else
            {
                fruitAmountList[(int)fruitSO.fruitType] -= fruitAmount;
                SaveFruit((int)fruitSO.fruitType);
            }
        }
    }


    void SaveFruit(int index)
    {
        switch (index)
        {
            case 0:
                CurrentStrawberryAmount = fruitAmountList[index];
                break;
            case 1:
                CurrentAppleAmount = fruitAmountList[index];
                break;
            case 2:
                CurrentWatermelonAmount = fruitAmountList[index];
                break;
            case 3:
                CurrentOrangeAmount = fruitAmountList[index];
                break;
            case 4:
                CurrentPineappleAmount = fruitAmountList[index];
                break;
            case 5:
                CurrentCarrotAmount = fruitAmountList[index];
                break;
            case 6:
                CurrentBananaAmount = fruitAmountList[index];
                break;
            case 7:
                CurrentLemonAmount = fruitAmountList[index];
                break;
            case 8:
                CurrentDragonfruitAmount = fruitAmountList[index];
                break;
            case 9:
                CurrentBlueberryAmount = fruitAmountList[index];
                break;
            case 10:
                CurrentBlackberryAmount = fruitAmountList[index];
                break;
            case 11:
                CurrentGrapeAmount = fruitAmountList[index];
                break;
            case 12:
                CurrentCoconutAmount = fruitAmountList[index];
                break;
            case 13:
                CurrentPlumAmount = fruitAmountList[index];
                break;
            case 14:
                CurrentPearAmount = fruitAmountList[index];
                break;
            case 15:
                CurrentPomegranateAmount = fruitAmountList[index];
                break;
            case 16:
                CurrentPeachAmount = fruitAmountList[index];
                break;
            case 17:
                CurrentMelonAmount = fruitAmountList[index];
                break;
            case 18:
                CurrentMangoAmount = fruitAmountList[index];
                break;
            case 19:
                CurrentStarfruitAmount = fruitAmountList[index];
                break;
        }
    }

    private void Update()
    {
        if (!_watchInter)
        {
            _timePassed += Time.deltaTime;

            if (_timePassed > _interDuration)
            {
                _watchInter = true;
                _timePassed = 0;
            }
        }
    }

    public void ResetInterTimer()
    {
        _timePassed = 0;
    }



    public void WatchInter()
    {

        if (IsInternetAvailable())
        {
            if (_watchInter)
            {
                string adsName = "Inter";
                string key = adsName + "Start";
                Hammer.Instance.ANALYTICS_CustomEvent(key, LevelManager.Instance.PlayerLevel);
                Hammer.Instance.MEDIATION_HasInterstitial(
                () =>
                {
                    Hammer.Instance.MEDIATION_ShowInterstitial(() =>
                    {
                        //Debug.Log("Inter gösterildi");
                        //onInterEnd?.Invoke();
                        key = adsName + "Success";
                        Hammer.Instance.ANALYTICS_CustomEvent(key, LevelManager.Instance.PlayerLevel);
                        _watchInter = false;


                    },
                    s =>
                    {
                        //Debug.Log("Inter gösterilemedi");
                        //onInterEnd?.Invoke();
                        key = adsName + "Fail";
                        Hammer.Instance.ANALYTICS_CustomEvent(key, LevelManager.Instance.PlayerLevel);
                        _watchInter = false;
                    });
                },
                s =>
                {
                    //Debug.Log("Inter yüklenemedi");
                    //onInterEnd?.Invoke();
                    _watchInter = false;
                    key = adsName + "hasRewardedFalse";
                    Hammer.Instance.ANALYTICS_CustomEvent(key, LevelManager.Instance.PlayerLevel);
                }
                );
            }
            else
            {
                _watchInter = false;
                string key = "90SecondsNotExpired";
                Hammer.Instance.ANALYTICS_CustomEvent(key, LevelManager.Instance.PlayerLevel);
                //onInterEnd?.Invoke();

            }
        }
        else
        {
            //onInterEnd?.Invoke();
        }

    }

    public bool IsInternetAvailable()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            // Telefonun internet bağlantısı var.
            return true;
        }
        else
        {
            // Telefonun internet bağlantısı yok.
            return false;
        }
    }

    //private void Start()
    //{
    //    _sceneCount = SceneManager.sceneCountInBuildSettings;
    //    StartCoroutine(LoadLastLevel(_loadingWaitTime));
    //}


    private void Singelton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //public void LoadNextLevel()
    //{
    //    StartCoroutine(StartLoadNextLevel());
    //}

    //private IEnumerator StartLoadNextLevel()
    //{
    //    EventManagement.Invoke_OnTransitionStart();
    //    yield return new WaitForSeconds(1f);

    //    Level++;

    //    if (Level <= _sceneCount - 1)
    //    {

    //        SceneManager.LoadScene(Level);
    //    }
    //    else
    //    {
    //        RandomLevel = UnityEngine.Random.Range(_randomMinLevelIndex, _sceneCount);
    //        SceneManager.LoadScene(RandomLevel);
    //    }
    //}

    //public void LoadCurrentLevel()
    //{
    //    StartCoroutine(StartLoadCurrentLevel());
    //}

    //private IEnumerator StartLoadCurrentLevel()
    //{
    //    EventManagement.Invoke_OnTransitionStart();
    //    yield return new WaitForSeconds(1f);

    //    if (Level <= _sceneCount - 1)
    //    {
    //        SceneManager.LoadScene(Level);
    //    }
    //    else
    //    {
    //        SceneManager.LoadScene(RandomLevel);
    //    }
    //}

    //private IEnumerator LoadLastLevel(float waitTime)
    //{
    //    yield return new WaitForSeconds(waitTime);

    //    if (Level <= _sceneCount - 1)
    //    {
    //        SceneManager.LoadScene(Level);
    //    }
    //    else
    //    {
    //        SceneManager.LoadScene(RandomLevel);
    //    }
    //}

    //public void LoadLevelFromDebugPanel(int level)
    //{
    //    if (level < _sceneCount && level != 0)
    //    {
    //        Level = level;
    //        SceneManager.LoadScene(Level);
    //    }
    //}

    //public void RestartCurrentLevel()
    //{
    //    SceneManager.LoadScene(Level);
    //}
}



