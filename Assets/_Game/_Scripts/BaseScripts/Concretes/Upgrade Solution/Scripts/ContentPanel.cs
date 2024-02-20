using PAG.Managers;
using PAG.UpgradeSystem;
using System.Collections.Generic;
using UnityEngine;

public class ContentPanel : MonoBehaviour
{
    public Transform contentHolder;
    public GameObject upgradeSectionPrefab;
    public string displayName;

    [HideInInspector]
    public ContentTypeButton myRespondedButton;
    public void FillContent(UpgradeType type)
    {
        List<UpgradableObjectData> contentList = UpgradeManager.Instance.typesSeparatedDict[type];
        displayName = type.ToString();
        gameObject.name = type.ToString() + " Panel";
        for (int i = 0; i < contentList.Count; i++)
        {
            GameObject sectionObj = Instantiate(upgradeSectionPrefab, contentHolder);
            UpgradeSection section = sectionObj.GetComponent<UpgradeSection>();
            section.GenerateSection(contentList[i]);
        }
    }
    public void ClosePanel()
    {
        this.gameObject.SetActive(false);
        if (myRespondedButton) myRespondedButton.ResetVisualSettings();

    }
    public void OpenPanel()
    {
        this.gameObject.SetActive(true);
        if (myRespondedButton) myRespondedButton.ClickedVisualSettings();
    }
}