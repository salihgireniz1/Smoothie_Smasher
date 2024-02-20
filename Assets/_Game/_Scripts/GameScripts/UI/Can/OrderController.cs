using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using static OrderManager;

public class OrderController : MonoBehaviour
{
    OrderManager orderManager;
    FruitUpgradeManager fruitUpgradeManager;
    InGamePanelController inGamePanelController;

    public AudioSource source;
    public OrderType orderType1, orderType2, orderType3;
    public OrderClassType orderClassType;
    public OrderAmountType orderAmountType;
    public BottleType bottleType;
    public GameObject SingleParent, DoubleParent, BackgroundImageParent1, BackgroundImageParent2, DoubleBG1, DoubleBG2;
    public TextMeshProUGUI FruitText1, FruitText2, FruitText3, GoldText;
    public int fruitAmount1, fruitAmount2, fruitAmount3, goldAmount;
    public Button FruitButton;
    public GameObject CheckImage1, CheckImage2, CheckImage3, RegularBGFrame, GoldBGFrame;
    public Image fruitImage1, fruitImage2, fruitImage3, bottleImage, CostImage, FullImage, HalfImage;
    public Tween tweener1, tweener2, tweener3;
    NumberFormatManager numberFormatManager = new();

    private void Awake()
    {
        source  = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
        orderListOnboard = FindObjectOfType<OrderListOnboard>();
        fruitUpgradeManager = FindObjectOfType<FruitUpgradeManager>();
        inGamePanelController = FindObjectOfType<InGamePanelController>();
        orderManager = FindObjectOfType<OrderManager>();
        BackgroundImageParent1.transform.GetChild(0).gameObject.SetActive(false);
        BackgroundImageParent1.transform.GetChild(1).gameObject.SetActive(false);
        BackgroundImageParent1.transform.GetChild(2).gameObject.SetActive(false);
    }

    private void Start()
    {
        RandomDatas();
        if (orderAmountType == OrderAmountType.Single)
        {
            SingleParent.SetActive(true);
            DoubleParent.SetActive(false);
        }
        else
        {
            SingleParent.SetActive(false);
            DoubleParent.SetActive(true);
        }
        CheckOrderConditions();
        CheckFruitAmount();
        if (orderClassType == OrderClassType.Gold)
        {
            goldAmount *= 3;
            RegularBGFrame.SetActive(false);
            GoldBGFrame.SetActive(true);
            bottleType = BottleType.Gold;
            bottleImage.sprite = orderManager.BottleImageList[(int)bottleType].sprite;

            BackgroundImageParent2.transform.GetChild(2).gameObject.SetActive(true);

            if (orderAmountType == OrderAmountType.Single)
            {
                FullImage.sprite = orderManager.JuiceDatas[(int)bottleType].FullJuicesList[(int)orderType1];
            }
            else
            {
                FullImage.sprite = orderManager.JuiceDatas[(int)bottleType].FullJuicesList[(int)orderType2];
                HalfImage.gameObject.SetActive(true);
                HalfImage.sprite = orderManager.JuiceDatas[(int)bottleType].HalfJuicesList[(int)orderType3];
            }
        }
        GoldText.text = numberFormatManager.FormatNumber(goldAmount).ToString();

        EventManagement.Invoke_OnFruitAmountChange(fruitUpgradeManager.FruitUpgradeObjectList[(int)orderType1], false, 0);
        EventManagement.Invoke_OnFruitAmountChange(fruitUpgradeManager.FruitUpgradeObjectList[(int)orderType2], false, 0);
        EventManagement.Invoke_OnFruitAmountChange(fruitUpgradeManager.FruitUpgradeObjectList[(int)orderType3], false, 0);
    }
    void CheckOrderConditions()
    {
        bottleType = (BottleType)Random.Range(1, 4);
        bottleImage.sprite = orderManager.BottleImageList[(int)bottleType].sprite;
        if (orderAmountType == OrderAmountType.Single)
        {
            fruitImage1.sprite = orderManager.fruitImageList[(int)orderType1].sprite;
        }
        else
        {
            fruitImage2.sprite = orderManager.fruitImageList[(int)orderType2].sprite;
            fruitImage3.sprite = orderManager.fruitImageList[(int)orderType3].sprite;
        }
    }

    public void CheckFruitAmount()
    {
        if (orderAmountType == OrderAmountType.Single)
        {
            FullImage.sprite = orderManager.JuiceDatas[(int)bottleType].FullJuicesList[(int)orderType1];
            if (LevelManager.Instance.fruitAmountList[(int)orderType1] < fruitAmount1)
            {
                StartCoroutine(WaitCompletingSingleOrder());
            }
            else
            {
                canSingleSellOrder();
            }
        }
        else
        {
            FullImage.sprite = orderManager.JuiceDatas[(int)bottleType].FullJuicesList[(int)orderType2];
            HalfImage.gameObject.SetActive(true);
            HalfImage.sprite = orderManager.JuiceDatas[(int)bottleType].HalfJuicesList[(int)orderType3];

            if ((LevelManager.Instance.fruitAmountList[(int)orderType2] < fruitAmount2 || LevelManager.Instance.fruitAmountList[(int)orderType3] < fruitAmount3))
            {
                StartCoroutine(WaitCompletingDoubleOrder());
                StartCoroutine(WaitCompletingFirstOrder());
                StartCoroutine(WaitCompletingSecondOrder());
            }
            else
            {
                CanDoubleSellOrder();
            }
        }
    }

    IEnumerator WaitCompletingSingleOrder()
    {
        CanNotSingleSellOrder();
        yield return new WaitUntil(() => LevelManager.Instance.fruitAmountList[(int)orderType1] >= fruitAmount1);
        canSingleSellOrder();
    }

    IEnumerator WaitCompletingDoubleOrder()
    {
        CanNotDoubleSellOrder();
        yield return new WaitUntil(() => (LevelManager.Instance.fruitAmountList[(int)orderType2] >= fruitAmount2 && LevelManager.Instance.fruitAmountList[(int)orderType3] >= fruitAmount3));
        CanDoubleSellOrder();

    }

    IEnumerator WaitCompletingFirstOrder()
    {
        yield return new WaitUntil(() => LevelManager.Instance.fruitAmountList[(int)orderType2] >= fruitAmount2);
        DoubleBG1.SetActive(true);
        CheckImage2.SetActive(true);
    }

    IEnumerator WaitCompletingSecondOrder()
    {

        yield return new WaitUntil(() => LevelManager.Instance.fruitAmountList[(int)orderType3] >= fruitAmount3);
        DoubleBG2.SetActive(true);
        CheckImage3.SetActive(true);
    }

    void CanDoubleSellOrder()
    {
        BackgroundImageParent1.transform.GetChild((int)orderClassType).gameObject.SetActive(false);
        BackgroundImageParent1.transform.GetChild(2).gameObject.SetActive(true);

        FruitButton.interactable = true;
    }

    void CanNotDoubleSellOrder()
    {
        FruitButton.interactable = false;
        DoubleBG1.SetActive(false);
        CheckImage2.SetActive(false);
        DoubleBG2.SetActive(false);
        CheckImage3.SetActive(false);
        BackgroundImageParent1.transform.GetChild((int)orderClassType).gameObject.SetActive(true);
        BackgroundImageParent1.transform.GetChild(2).gameObject.SetActive(false);

        FruitText2.text = LevelManager.Instance.fruitAmountList[(int)orderType2].ToString() + "/" + fruitAmount2.ToString();
        FruitText3.text = LevelManager.Instance.fruitAmountList[(int)orderType3].ToString() + "/" + fruitAmount3.ToString();
    }

    public OrderListOnboard orderListOnboard;

    void canSingleSellOrder()
    {
        BackgroundImageParent1.transform.GetChild((int)orderClassType).gameObject.SetActive(false);
        BackgroundImageParent1.transform.GetChild(2).gameObject.SetActive(true);

        FruitButton.interactable = true;
        CheckImage1.SetActive(true);

        if (orderListOnboard != null)
        {
            OnboardingManager.Instance.OnboardObject(orderListOnboard);
        }
    }

    void CanNotSingleSellOrder()
    {
        FruitButton.interactable = false;
        CheckImage1.SetActive(false);
        BackgroundImageParent1.transform.GetChild((int)orderClassType).gameObject.SetActive(true);
        BackgroundImageParent1.transform.GetChild(2).gameObject.SetActive(false);

        FruitText1.text = LevelManager.Instance.fruitAmountList[(int)orderType1].ToString() + "/" + fruitAmount1.ToString();
    }


    void RandomDatas()
    {
        int index1, index2;
        if (LevelManager.Instance.PlayerLevel == 1)
        {
            index1 = 5;
            index2 = 10;
        }
        else
        {
            index1 = 5 + (Mathf.CeilToInt((float)LevelManager.Instance.PlayerLevel / 5) * 5);
            index2 = 15 + (Mathf.CeilToInt((float)LevelManager.Instance.PlayerLevel / 5) * 5);
        }

        fruitAmount1 = Random.RandomRange(index1, index2);
        fruitAmount2 = Random.RandomRange(index1, index2);
        fruitAmount3 = Random.RandomRange(index1, index2);
        if (orderClassType == OrderClassType.Gold)
        {
            fruitAmount1 *= 3;
            fruitAmount2 *= 3;
            fruitAmount3 *= 3;
        }

        if (orderAmountType == OrderAmountType.Single)
        {
            goldAmount = fruitAmount1 * ((int)orderType1 + 1);
        }
        else
        {
            goldAmount = (fruitAmount2 * ((int)orderType2 + 1)) + (fruitAmount3 * ((int)orderType3 + 1));
        }
    }

    private void OnEnable()
    {
        EventManagement.OnFruitAmountChange += ChangeTextAndFillImage;
    }

    private void OnDisable()
    {
        EventManagement.OnFruitAmountChange -= ChangeTextAndFillImage;
    }

    public void ChangeTextAndFillImage(FruitUpgradeObject fruitUpgradeObject, bool isIncreasing, int fruitAmount)
    {

        if (orderAmountType == OrderAmountType.Single)
        {
            if ((int)orderType1 == fruitUpgradeManager.FruitUpgradeObjectList.IndexOf(fruitUpgradeObject))
            {
                FruitText1.text = LevelManager.Instance.fruitAmountList[(int)orderType1].ToString() + "/" + fruitAmount1.ToString();
                float percentage = ((float)LevelManager.Instance.fruitAmountList[(int)orderType1]) / (float)fruitAmount1;
  
                tweener1.Kill();
                tweener1 = DOTween.To(x => FullImage.fillAmount = x, FullImage.fillAmount, percentage, 0.25f).SetEase(Ease.InCubic);
            }
        }
        else
        {
            if ((int)orderType2 == fruitUpgradeManager.FruitUpgradeObjectList.IndexOf(fruitUpgradeObject))
            {
                FruitText2.text = LevelManager.Instance.fruitAmountList[(int)orderType2].ToString() + "/" + fruitAmount2.ToString();
            }
            if ((int)orderType3 == fruitUpgradeManager.FruitUpgradeObjectList.IndexOf(fruitUpgradeObject))
            {
                FruitText3.text = LevelManager.Instance.fruitAmountList[(int)orderType3].ToString() + "/" + fruitAmount3.ToString();
            }

            int min1 = LevelManager.Instance.fruitAmountList[(int)orderType2];
            if (fruitAmount2 < LevelManager.Instance.fruitAmountList[(int)orderType2]) min1 = fruitAmount2;
            int min2 = LevelManager.Instance.fruitAmountList[(int)orderType3];
            if (fruitAmount3 < LevelManager.Instance.fruitAmountList[(int)orderType3]) min2 = fruitAmount3;

            float percentage = (float)(min1 + min2) / (float)(fruitAmount2 + fruitAmount3);

            tweener2.Kill();
            tweener2 = DOTween.To(x => FullImage.fillAmount = x, FullImage.fillAmount, percentage, 0.25f).SetEase(Ease.InCubic);

            tweener3.Kill();
            tweener3 = DOTween.To(x => HalfImage.fillAmount = x, FullImage.fillAmount, percentage, 0.25f).SetEase(Ease.InCubic);
        }
    }
    public void CloseOrder()
    {
        if (source)
        {
            //Debug.Log("PLAY");
            source.Play();
        }
        OnboardingManager.Instance.UnonboardObject(orderListOnboard);
        EventManagement.Invoke_OnCollectMoneyUI(transform.position, goldAmount);
        if (orderAmountType == OrderAmountType.Single)
        {
            EventManagement.Invoke_OnCollectPlayerLevelUI(transform.position, fruitAmount1);
            EventManagement.Invoke_OnFruitAmountChange(fruitUpgradeManager.FruitUpgradeObjectList[(int)orderType1], false, fruitAmount1);
            EventManagement.Invoke_OnProgressChange(fruitAmount1);
        }
        else
        {
            EventManagement.Invoke_OnCollectPlayerLevelUI(transform.position, fruitAmount2);
            EventManagement.Invoke_OnCollectPlayerLevelUI(transform.position, fruitAmount3);
            EventManagement.Invoke_OnFruitAmountChange(fruitUpgradeManager.FruitUpgradeObjectList[(int)orderType2], false, fruitAmount2);
            EventManagement.Invoke_OnFruitAmountChange(fruitUpgradeManager.FruitUpgradeObjectList[(int)orderType3], false, fruitAmount3);
            EventManagement.Invoke_OnProgressChange(fruitAmount2);
            EventManagement.Invoke_OnProgressChange(fruitAmount3);
        }
        if (orderClassType == OrderClassType.Gold)
        {
            orderManager.isThereGoldOrder = false;
        }
        EventManagement.Invoke_OnGoldChange(goldAmount,transform.position);
        EventManagement.Invoke_OnOrderProgressChange(1);
        //if (LevelManager.Instance.CurrentProgressAmount >= inGamePanelController.ProgressAmount)
        //{
        //    LevelManager.Instance.CurrentProgressAmount -= inGamePanelController.ProgressAmount;
        //    inGamePanelController.PlayerLevelUp();
        //}
        Destroy(gameObject);
        orderManager.CheckOrderCount();
    }
}
