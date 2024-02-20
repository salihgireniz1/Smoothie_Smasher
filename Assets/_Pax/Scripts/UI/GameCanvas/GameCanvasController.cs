namespace Pax
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class GameCanvasController : MonoBehaviour
    {
        [SerializeField] StartPanelController _startPanelController;
        [SerializeField] InGamePanelController _inGamePanelController;
        [SerializeField] LosePanelController _losePanelController;
        [SerializeField] WinPanelController _winPanelController;

        private void Awake()
        {
            //MainManager.Instance.EventManager.Register(EventTypes.LevelLoaded, _startPanelController.OpenPanel);

            MainManager.Instance.EventManager.Register(EventTypes.LevelLoaded, _inGamePanelController.OpenPanel);
            MainManager.Instance.EventManager.Register(EventTypes.LevelSuccess, _inGamePanelController.ClosePanel);
            MainManager.Instance.EventManager.Register(EventTypes.LevelFail, _inGamePanelController.ClosePanel);

            MainManager.Instance.EventManager.Register(EventTypes.LevelSuccess, _winPanelController.OpenPanel);
            MainManager.Instance.EventManager.Register(EventTypes.LevelFail, _losePanelController.OpenPanel);
        }
    }
}
