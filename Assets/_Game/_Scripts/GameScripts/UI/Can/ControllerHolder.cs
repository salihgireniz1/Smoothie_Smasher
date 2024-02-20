using UnityEngine;

public class ControllerHolder : MonoBehaviour
{
    public GroundSizeController GroundSizeController
    {
        get => groundSizeController;
    }
    public CharacterUpgradeManager CharacterUpgradeManager
    {
        get => characterUpgradeManager;
    }
    public InGamePanelController InGamePanelController
    {
        get => inGamePanelController;
    }
    public SpeedDataHandler SpeedDataHandler
    {
        get => speedDataHandler;
    }
    public HandScaleHandler HandScaleHandler
    {
        get => handScaleHandler;
    }

    private GroundSizeController groundSizeController;
    private CharacterUpgradeManager characterUpgradeManager;
    private InGamePanelController inGamePanelController;
    private SpeedDataHandler speedDataHandler;
    private HandScaleHandler handScaleHandler;

    private void Awake()
    {
        groundSizeController = FindObjectOfType<GroundSizeController>();
        characterUpgradeManager = FindObjectOfType<CharacterUpgradeManager>();
        inGamePanelController = FindObjectOfType<InGamePanelController>();
        speedDataHandler = FindObjectOfType<SpeedDataHandler>();
        handScaleHandler = FindObjectOfType<HandScaleHandler>();
    }
}
