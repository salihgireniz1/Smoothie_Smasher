using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static BoosterPanelManager;

public class BoosterController : MonoBehaviour
{
    public BoosterType boosterType;
    public Button Button, RWButton;
    public Image BlackImage;
    public TextMeshProUGUI FreeText, CountDownText, BoostTimeText;
    public GameObject CountDownParent, BoostTimeParent;
    //public bool canActive = true;
    public bool isActive = false;
    //public bool timeIsUP = false;
    public float tempCDTime;
    public ParticleSystem BoostParticle;
    public BoosterPanelManager boosterPanelManager;

    public bool canActive
    {
        get => ES3.Load("canActive" + boosterType, true);
        set => ES3.Save("canActive" + boosterType, value);
    }

    public bool timeIsUP
    {
        get => ES3.Load("timeIsUP" + boosterType, true);
        set => ES3.Save("timeIsUP" + boosterType, value);
    }

    private void OnEnable()
    {
        if (isActive) BoostParticle.Play();
    }


    public void CompleteCountDown()
    {
        if (!isActive)
        {
            //Debug.Log(boosterType + "Complete");
            tempCDTime = boosterPanelManager.CountdownTime;
            CountDownParent.SetActive(false);
            var tempColor3 = BlackImage.color;
            tempColor3.a = 0f;
            BlackImage.color = tempColor3;
            Button.transform.localScale = Vector3.one;
            FreeText.gameObject.SetActive(true);
            RWButton.gameObject.SetActive(false);
            FreeText.text = "FREE";
            Button.interactable = true;
            canActive = true;
            boosterPanelManager.CheckBoostAmount();
        }
    }

    public void StartCountDown()
    {
        //Debug.Log(boosterType + "Start");
        Button.interactable = false;
        timeIsUP = false;
        BoostParticle.Stop();
        BoostTimeParent.SetActive(false);
        isActive = false;
        if (!boosterPanelManager.CheckActiveBooster())
        {
            boosterPanelManager.playerController.StopBoosterParticle();
            boosterPanelManager.uIParticleController.StopBoosterPanelParticle();
        }
        BlackImage.fillAmount = 1;
        FreeText.gameObject.SetActive(false);
        var tempColor2 = BlackImage.color;
        tempColor2.a = 0.75f;
        BlackImage.color = tempColor2;
        CountDownParent.SetActive(true);
        if (LevelManager.Instance.IsInternetAvailable())
        {
            RWButton.gameObject.SetActive(true);
        }
    }



}
