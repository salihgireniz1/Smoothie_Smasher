using DG.Tweening;
using PAG.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PAG.Utility;
public class UpgradePanelController : MonoSingleton<UpgradePanelController>
{
    [Header("Show/Hide Settings")]
    public float openDuration = 0.5f;
    public float closeDuration = 0.3f;

    [Header("Panel Spawn Settings")]
    public GameObject panelSamplePrefab;


    [Header("Types Part Settings")]
    [SerializeField]
    private GameObject typeButtonPrefab;
    
    [SerializeField]
    private Transform typesHolder;

    private ContentPanel activeContentPanel;

    List<ContentPanel> contents = new();
    List<ContentTypeButton> buttons = new();
    private void Start()
    {
        Initialize();
    }
    void Initialize()
    {
        // Get all types.
        foreach (var key in UpgradeManager.Instance.typesSeparatedDict)
        {
            // Create panel for each type.
            GameObject panelObj = Instantiate(panelSamplePrefab, this.transform);
            panelObj.SetActive(false);
            // Fill panels contents.
            ContentPanel contentPanel = panelObj.GetComponent<ContentPanel>();
            contentPanel.FillContent(key.Key);
            contents.Add(contentPanel);

            GameObject buttonObj = Instantiate(typeButtonPrefab, typesHolder);
            ContentTypeButton contentButton = buttonObj.GetComponent<ContentTypeButton>();
            contentButton.GenerateButton(contentPanel);
            buttons.Add(contentButton);
        }
    }

    public void SwitchContent(ContentPanel newContentPanel)
    {
        if (activeContentPanel) activeContentPanel.ClosePanel();
        activeContentPanel = newContentPanel;
        activeContentPanel.OpenPanel();
    }

    public void Show()
    {
        Vector3 targetScale = new Vector3(1f, 1f, 1f);
        transform.DOScale(targetScale, openDuration).SetEase(Ease.OutBack);
        SwitchContent(contents[0]);
    }
    public void Hide()
    {
        Vector3 targetScale = new Vector3(1f, 0f, 1f);
        transform.DOScale(targetScale, openDuration).SetEase(Ease.InBack);
    }
}