using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class CursorRewardedController : MonoBehaviour
{
    public List<TextMeshProUGUI> MultiplyTextList = new();
    public Button NoThanksButton, ClaimButton;
    public GameObject Cursor;
    public TextMeshProUGUI ClaimText, RewardedGoldText;
    //UIManager uIManager;
    public int RewardedDNAAmount;
    public int _level;
    NumberFormatManager numberFormatManager = new();
    Vector3 firstRot;

    // Start is called before the first frame update

    private void Awake()
    {
        firstRot = Cursor.transform.localEulerAngles;
        //uIManager = FindObjectOfType<UIManager>();
    }
    private void OnEnable()
    {
        StartCoroutine(StartForOpening());
    }
    IEnumerator StartForOpening()
    {
        Cursor.transform.localEulerAngles = firstRot;
        NoThanksButton.gameObject.SetActive(false);
        MoveCursor();
        ClaimButton.interactable = true;
        yield return new WaitForSeconds(1f);
        NoThanksButton.gameObject.SetActive(true);
        NoThanksButton.interactable = true;
    }

    public void MoveCursor()
    {
        Cursor.transform.DORotate(new Vector3(0, 0, 0), 2f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutCubic);
        //_level = (LevelManager.Instance.Level - 2);
    }

    public void CheckCursor()
    {
        if (Cursor.transform.localEulerAngles.z > 45)
        {
            ChangeTextColor(0);
            ChangeClaimText(2);
        }
        else if (Cursor.transform.localEulerAngles.z > 40)
        {
            ChangeTextColor(1);
            ChangeClaimText(3);
        }
        else if (Cursor.transform.localEulerAngles.z > 30)
        {
            ChangeTextColor(2);
            ChangeClaimText(4);
        }
        else if (Cursor.transform.localEulerAngles.z > 20)
        {
            ChangeTextColor(3);
            ChangeClaimText(5);
        }
        else if (Cursor.transform.localEulerAngles.z > 10)
        {
            ChangeTextColor(4);
            ChangeClaimText(4);
        }
        else if (Cursor.transform.localEulerAngles.z > 5)
        {
            ChangeTextColor(5);
            ChangeClaimText(3);
        }
        else if (Cursor.transform.localEulerAngles.z > 0)
        {
            ChangeTextColor(6);
            ChangeClaimText(2);
        }

    }

    void ChangeTextColor(int index)
    {
        for (int i = 0; i < MultiplyTextList.Count; i++)
        {
            if (i == index)
            {
                MultiplyTextList[i].color = Color.magenta;
            }
            else
            {
                MultiplyTextList[i].color = Color.white;
            }
        }
    }
    void ChangeClaimText(int index)
    {
        ClaimText.text = "CLAIM x" + index.ToString();

        if (LevelManager.Instance.PlayerLevel <= LevelManager.Instance.LevelUpRewardList.Count+1)
        {
            RewardedDNAAmount = (index) * LevelManager.Instance.LevelUpRewardList[LevelManager.Instance.PlayerLevel - 2];
        }
        else
        {
            RewardedDNAAmount = (index) * LevelManager.Instance.LevelUpRewardList[^1];
        }

        RewardedGoldText.text = numberFormatManager.FormatNumber(RewardedDNAAmount);
    }

    public void StopRotating()
    {
        Cursor.transform.DOPause();
    }

    public void CloseButtons()
    {
        NoThanksButton.interactable = false;
        ClaimButton.interactable = false;
    }

    void Update()
    {
        CheckCursor();
    }
}
