using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MoreMountains.Feedbacks;
using DG.Tweening;

public class InGamePanelController : MonoBehaviour
{
    public MMF_Player LightHaptic; //, FruitUpgradeCheckFeedback, CharacterUpgradeCheckFeedback;
    public TextMeshProUGUI GoldText, PlayerLevelText, OrderProgressText, SpawnerText;
    public Image LevelImage, orderProgresImage, spawnerImage;
    public int LevelCount;
    public int ProgressAmount = 25;
    public int OrderProgressAmount = 1000;
    public int currentorderProgressAmount = 0;
    public GameObject FruitUpgradeCheckImage, CharacterUpgradeCheckImage, FruitSpawnImagePrefab;
    public int minFruitUpgradeAmount, minCharacterUpgradeAmount;
    public GameObject FruitSplashImagePrefab, WorldCanvas;

    [SerializeField] FruitUpgradeManager fruitUpgradeManager;
    [SerializeField] CharacterUpgradeManager characterUpgradeManager;
    [SerializeField] GameObject FruitUpgradePanel, CharacterUpgradePanel;
    [SerializeField] private InGamePanelSettings _settings;

    [SerializeField] private LevelPanel _levelPanel;
    [SerializeField] private ScorePanel _scorePanel;
    [SerializeField] private RestartPanel _restartPanel;
    [SerializeField] private DebugPanel _debugPanel;
    NumberFormatManager numberFormatManager = new();
    BagPanelController bagPanelController;
    LevelUpPanel levelUpPanel;
    BoosterPanelManager boosterPanelManager;
    InGamePanelController inGamePanelController;
    Tween tweener1, tweener2;

    public GameObject DebuggerPanel;
    public GameObject SizePanel;

    private void OnEnable()
    {
        EventManagement.OnGoldChange += GoldChange;
        EventManagement.OnPlayerLevelChange += PlayerLevelTextChange;
        EventManagement.OnOrderProgressChange += OrderProgressTextChange;
        EventManagement.OnProgressChange += ProgressChangeText;
    }

    private void OnDisable()
    {
        EventManagement.OnGoldChange -= GoldChange;
        EventManagement.OnPlayerLevelChange -= PlayerLevelTextChange;
        EventManagement.OnOrderProgressChange -= OrderProgressTextChange;
        EventManagement.OnProgressChange -= ProgressChangeText;
    }

    private void Awake()
    {
        inGamePanelController = FindObjectOfType<InGamePanelController>();
        boosterPanelManager = FindObjectOfType<BoosterPanelManager>();
        levelUpPanel = FindObjectOfType<LevelUpPanel>();
        bagPanelController = FindObjectOfType<BagPanelController>();
    }

    private void Start()
    {
        //Debug.Log(LevelManager.Instance.LevelUpRewardList[^1]);
        GoldText.text = numberFormatManager.FormatNumber(LevelManager.Instance.CurrentGold);
        PlayerLevelText.text = LevelManager.Instance.PlayerLevel.ToString();
        OrderProgressText.text = currentorderProgressAmount.ToString() + "/" + OrderProgressAmount.ToString();
        LevelImage.transform.localScale = new Vector3(0, 1, 1);
        orderProgresImage.transform.localScale = new Vector3(0, 1, 1);
        spawnerImage.transform.localScale = new Vector3(0, 1, 1);
        if (LevelManager.Instance.PlayerLevel <= LevelManager.Instance.ProgressAmountList.Count)
        {
            ProgressAmount = LevelManager.Instance.ProgressAmountList[LevelManager.Instance.PlayerLevel - 1];
        }
        else
        {
            ProgressAmount = LevelManager.Instance.ProgressAmountList[^1];
        }
        EventManagement.Invoke_OnGoldChange(0, transform.position);
        EventManagement.Invoke_OnOrderProgressChange(0);
        EventManagement.Invoke_OnProgressChange(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            AddGold();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            //EventManagement.Invoke_OnPlayerLevelChange();
            PlayerLevelUp();
        }
        if (Input.touchCount > 4)
        {
            OpenDebugger();
        }
    }

    public void OpenDebugger()
    {
        DebuggerPanel.SetActive(true);
        SizePanel.SetActive(false);
    }
    public void CloseDebugger()
    {
        SizePanel.SetActive(false);

        DebuggerPanel.SetActive(false);
    }

    public void AddGold()
    {
        EventManagement.Invoke_OnGoldChange(10000, transform.position);
    }

    public void OpenSizeList()
    {
        SizePanel.SetActive(true);
        for (int i = 0; i < SizePanel.transform.childCount; i++)
        {
            SizePanel.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = fruitUpgradeManager.FruitUpgradeObjectList[i].name + " : " + fruitUpgradeManager.FruitUpgradeObjectList[i].fruitSize;
        }
    }

    public void GenerateSpawnImage()
    {
        Instantiate(FruitSpawnImagePrefab, spawnerImage.transform.parent.transform.position, Quaternion.identity, spawnerImage.transform.parent.transform);
    }


    public void GenerateSplashImage(FruitUpgradeObject fruitUpgradeObject, Vector3 splashPos)
    {
        splashPos = new Vector3(splashPos.x, 5, splashPos.z);
        GameObject image = Instantiate(FruitSplashImagePrefab, splashPos, Quaternion.identity);
        image.transform.eulerAngles = new Vector3(70, 0, 0);
        image.transform.GetChild(0).GetComponent<SpawnImageFeedback>().fruitUpgradeObject = fruitUpgradeObject;
    }
    public void PlayerLevelUp()
    {
        EventManagement.Invoke_OnPlayerLevelChange();
        if (LevelManager.Instance.PlayerLevel <= 20)
        {
            bagPanelController.GenerateNewFruitAmount(LevelManager.Instance.PlayerLevel - 1);
        }
        levelUpPanel.OpenLevelUpPanel();
        if (!characterUpgradeManager.isActiveButton)
        {
            characterUpgradeManager.ButtonCheck();
        }
        if (!boosterPanelManager.isActiveButton)
        {
            boosterPanelManager.CheckBoostButtonLocked();
            boosterPanelManager.CheckBoostAmount();
        }
    }

    public void ProgressChangeText(int progressAmount)
    {
        LevelManager.Instance.CurrentProgressAmount += progressAmount;

        if (LevelManager.Instance.PlayerLevel <= LevelManager.Instance.ProgressAmountList.Count)
        {
            ProgressAmount = LevelManager.Instance.ProgressAmountList[LevelManager.Instance.PlayerLevel - 1];
        }
        else
        {
            ProgressAmount = LevelManager.Instance.ProgressAmountList[^1];
        }

        if (LevelManager.Instance.CurrentProgressAmount >= ProgressAmount)
        {
            LevelManager.Instance.CurrentProgressAmount -= ProgressAmount;
            inGamePanelController.PlayerLevelUp();
        }

        tweener2.Kill();
        float percentage = Mathf.Clamp01((float)LevelManager.Instance.CurrentProgressAmount / (float)ProgressAmount);
        if ((float)LevelManager.Instance.CurrentProgressAmount / (float)ProgressAmount > 1) percentage = ((float)LevelManager.Instance.CurrentProgressAmount / (float)ProgressAmount) - 1f;
        tweener2 = LevelImage.transform.DOScaleX(percentage, 0.25f);

    }

    public void OrderProgressTextChange(int amount)
    {
        LevelManager.Instance.CurrentOrderProgressAmount += amount;
        OrderProgressText.text = LevelManager.Instance.CurrentOrderProgressAmount.ToString() + "/" + OrderProgressAmount.ToString();
        float percentage = Mathf.Clamp01((float)LevelManager.Instance.CurrentOrderProgressAmount / (float)OrderProgressAmount);
        tweener1.Kill();
        tweener1 = orderProgresImage.transform.DOScaleX(percentage, 0.25f);
    }

    public void PlayerLevelTextChange()
    {
        LevelManager.Instance.PlayerLevel++;
        PlayerLevelText.text = LevelManager.Instance.PlayerLevel.ToString();
    }

    public void GoldChange(int goldAmount, Vector3 position)
    {
        LevelManager.Instance.CurrentGold += goldAmount;
        GoldText.text = numberFormatManager.FormatNumber(LevelManager.Instance.CurrentGold);

        StartCoroutine(delayedCheckMinAndCheapestUpgrade());
        if (goldAmount < 0)
        {
            FloatingTextController.Instance.SpawnFloatingText(position, goldAmount, SpriteType.Gold, false);
        }
    }

    IEnumerator delayedCheckMinAndCheapestUpgrade()
    {
        yield return new WaitForSeconds(0.05f);
        fruitUpgradeManager.CheckMinAndCheapestUpgrade();

        if (LevelManager.Instance.CurrentGold >= minFruitUpgradeAmount)
        {
            FruitUpgradeCheckImage.SetActive(true);
        }
        else
        {
            FruitUpgradeCheckImage.SetActive(false);
        }

        if ((Mathf.CeilToInt(((float)(LevelManager.Instance.PlayerLevel + 1)) / 3)) * 3 >= LevelManager.Instance.CharacterUpgradeLevel + 4)
        {
            characterUpgradeManager.canUpgrade = true;
        }
        else
        {
            characterUpgradeManager.canUpgrade = false;
        }

        if (LevelManager.Instance.CurrentGold >= minCharacterUpgradeAmount && LevelManager.Instance.PlayerLevel >= 3 && characterUpgradeManager.canUpgrade)
        {
            CharacterUpgradeCheckImage.SetActive(true);
        }
        else
        {
            CharacterUpgradeCheckImage.SetActive(false);
        }
    }



    public void SpawnerTexting()
    {
        if (LevelManager.Instance.PlayerLevel <= 19)
        {
            LevelManager.Instance.SpawnerMaxCount = (LevelManager.Instance.PlayerLevel + 1) * 5;
        }
        //else if (LevelManager.Instance.PlayerLevel <= 25)
        //{
        //    LevelManager.Instance.SpawnerMaxCount = 110 + ((LevelManager.Instance.PlayerLevel - 10) * 5);
        //}
        else
        {
            LevelManager.Instance.SpawnerMaxCount = 100;
        }
        SpawnerText.text = LevelManager.Instance.SpawnerCurrentCount.ToString() + "/" + LevelManager.Instance.SpawnerMaxCount.ToString();
    }

    public void SpawnerFillingImage(float currentTime, float duration)
    {
        spawnerImage.transform.localScale = new Vector3(Mathf.Clamp01((float)currentTime / (float)duration), 1, 1);
    }

    public void OpenPanel()
    {
        transform.gameObject.SetActive(true);
    }

    public void ClosePanel()
    {
        transform.gameObject.SetActive(false);
    }
}

public class NumberFormatManager
{
    public string FormatNumber(float number)
    {
        if (number >= 1000000)
        {
            if (number % 1000000f < 1000)
            {
                return (number / 1000000f).ToString("0") + "m";
            }
            else if (number % 100000f < 1000)
            {
                return (number / 1000000f).ToString("0.0") + "m";
            }
            else
            {
                return (number / 1000000f).ToString("0.00") + "m";
            }
        }
        else if (number >= 1000)
        {
            if (number % 1000f < 1)
            {
                return (number / 1000f).ToString("0") + "k";
            }
            else if (number % 100f < 1)
            {
                return (number / 1000f).ToString("0.0") + "k";
            }
            else
            {
                return (number / 1000f).ToString("0.00") + "k";
            }
        }
        else
            return ((int)number).ToString();
    }
}
