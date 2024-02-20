using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CharacterUpgradeManager : MonoBehaviour
{
    public enum CharacterUpgradeType
    {
        Speed,
        HandSize,
        PressPower
    }

    public GameObject CharacterUpgradePanel, CharacterUpgradePrefab, PrefabParent, BlueSeaPanel;
    public CharacterUpgradeOnboard onboardController;
    public bool isActiveButton = false;
    public Image CharacterImage, BGImage;
    public GameObject UnlockLevelText, LockImage;
    public List<ButtonPressed> ButtonPressedList = new();
    public bool canUpgrade = false;
    bool canClose = false;

    Color firstColor;
   public float firstYPos;

    private void Awake()
    {
        firstColor = BGImage.color;
        firstYPos = PrefabParent.transform.localPosition.y;
    }
    private void Start()
    {
        GenerateUpgradeprefabs();
        ButtonCheck();
        CloseCharacterUpgradePanel();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && onboardController.IsOnboarded)
        {
            CloseOutCharacterUpgradePanel();
        }
    }

    public void CloseOutCharacterUpgradePanel()
    {
        canClose = false;
        for (int i = 0; i < ButtonPressedList.Count; i++)
        {
            if (ButtonPressedList[i].buttonPressed) canClose = true;
        }

        if (!canClose)
        {
            CloseCharacterUpgradePanel();
        }
    }

    public void OpenPanel()
    {
        if (isActiveButton)
        {
            if (LevelManager.Instance.CharacterUpgradeLevel > 1)
            {
                float newYPos = (firstYPos - 100) - (450 * (LevelManager.Instance.CharacterUpgradeLevel - 1));
                PrefabParent.transform.localPosition = new Vector3(PrefabParent.transform.localPosition.x, newYPos, PrefabParent.transform.localPosition.z);
                //Debug.Log("first " + firstYPos);
                //Debug.Log(newYPos);
            }
            CharacterUpgradePanel.SetActive(true);
            CheckAllUpgrades();
            if (onboardController.IsOnboarded)
            {
                LevelManager.Instance.WatchInter();
            }
            OnboardingManager.Instance.OnboardObject(onboardController);
        }
    }

    public void ButtonCheck()
    {
        if (LevelManager.Instance.PlayerLevel < 3)
        {
            isActiveButton = false;
            CharacterImage.color = Color.gray;
            BGImage.color = Color.gray;
        }
        else
        {
            isActiveButton = true;
            LockImage.SetActive(false);
            UnlockLevelText.SetActive(false);
            CharacterImage.color = Color.white;
            BGImage.color = firstColor;
        }
    }

    public void CheckAllUpgrades()
    {
        foreach (Transform item in PrefabParent.transform)
        {
            item.GetComponent<CharacterUpgradeController>().CheckUpgradeable();
        }
    }

    public void MoveSeaPanel()
    {
        BlueSeaPanel.transform.DOLocalMoveY(BlueSeaPanel.transform.localPosition.y + 450, 0.5f);
    }

    public void CloseCharacterUpgradePanel()
    {
        CharacterUpgradePanel.SetActive(false);
    }

    public void GenerateUpgradeprefabs()
    {
        for (int i = 0; i < 51; i++)
        {
            GameObject characterUpgradePrefab = Instantiate(CharacterUpgradePrefab, PrefabParent.transform);
            CharacterUpgradeController characterUpgradeController = characterUpgradePrefab.GetComponent<CharacterUpgradeController>();
            if (i % 3 == 0)
            {
                characterUpgradeController.characterUpgradeType = CharacterUpgradeType.Speed;
            }
            else if (i % 3 == 1)
            {
                characterUpgradeController.characterUpgradeType = CharacterUpgradeType.HandSize;
            }
            else if (i % 3 == 2)
            {
                characterUpgradeController.characterUpgradeType = CharacterUpgradeType.PressPower;
            }
            ButtonPressedList.Add(characterUpgradeController.mainButtonPressed);
            ButtonPressedList.Add(characterUpgradeController.buttonPressed);
            ButtonPressedList.Add(characterUpgradeController.buttonPressedRW);
        }
    }
}
