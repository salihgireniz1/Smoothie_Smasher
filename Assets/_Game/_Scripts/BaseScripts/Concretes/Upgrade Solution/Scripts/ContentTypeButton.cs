using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class ContentTypeButton : MonoBehaviour
{
    enum SelectType
    {
        colorTint, spriteSwitch
    }

    [Header("Selection Settings"), Space]
    [SerializeField]
    private Image backgroundImage;

    [SerializeField]
    private SelectType selectionType = SelectType.colorTint;

    [SerializeField, ShowIf("IfColorTint")]
    private Color32 normalColor = Color.white;

    [SerializeField, ShowIf("IfColorTint")]
    private Color32 selectedColor = Color.white;


    [SerializeField, HideIf("IfColorTint")]
    private Sprite normalBackground;

    [SerializeField, HideIf("IfColorTint")]
    private Sprite selectedBackground;

    [HideInInspector]
    public ContentPanel myPanel;
    private TextMeshProUGUI buttonText;
    public void GenerateButton(ContentPanel panelInfo)
    {
        myPanel = panelInfo;
        myPanel.myRespondedButton = this;

        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = panelInfo.displayName;
        gameObject.name = panelInfo.displayName + " Section Button";

        GetComponent<Button>().onClick.AddListener(ActivateMyPanel);
    }

    public void ActivateMyPanel()
    {
        UpgradePanelController.Instance.SwitchContent(myPanel);
    }
    public void ClickedVisualSettings()
    {
        switch (selectionType)
        {
            case SelectType.colorTint:
                backgroundImage.color = selectedColor;
                break;
            case SelectType.spriteSwitch:
                if (selectedBackground != null)
                    backgroundImage.sprite = selectedBackground;
                break;
            default:
                break;
        }
    }
    public void ResetVisualSettings()
    {
        switch (selectionType)
        {
            case SelectType.colorTint:
                    backgroundImage.color = normalColor;
                break;
            case SelectType.spriteSwitch:
                if(normalBackground != null)
                    backgroundImage.sprite = normalBackground;
                break;
            default:
                break;
        }
    }

    bool IfColorTint()
    {
        switch (selectionType)
        {
            case SelectType.colorTint:
                return true;
            default:
                return false;
        }
    }
}
