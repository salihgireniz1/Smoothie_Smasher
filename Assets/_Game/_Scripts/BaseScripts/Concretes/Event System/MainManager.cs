using UnityEngine;
using PAG.Utility;
using PAG.Events;

/// <summary>
/// A class that manages the main game systems and events.
/// </summary>
public class MainManager : MonoSingleton<MainManager>
    {
        #region Properties
        /// <summary>
        /// Gets the event manager instance used by the game.
        /// </summary>
        public EventManager EventManager => _eventManager;
        #endregion

        #region Variables

        EventManager _eventManager;
        #endregion

        #region MonoBehaviour Callbacks
        private void Awake()
        {
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;

            Initialize();
        }
        #endregion

        #region Methods

        /// <summary>
        /// Initializes the loading tasks of every manager.
        /// </summary>
        private void Initialize()
        {
            // Initialize Event Manager.
            _eventManager = new();
            _eventManager.Initialize();
        }
        #endregion
    }

