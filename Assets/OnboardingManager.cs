using PAG.Utility;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnboardingManager : MonoSingleton<OnboardingManager>
{
    public OnboardingPanel onboardingPanelPrefab;
    public Transform boardingObjectCanvas;

    [SerializeField]
    private OnboardingPanel activeOnboardingPanel;

    private IOnboardable onboardingCurrently;

    private void Awake()
    {
        InitBlackScreen();
    }
    public void InitBlackScreen()
    {
        if (activeOnboardingPanel == null)
        {
            activeOnboardingPanel = Instantiate(onboardingPanelPrefab, boardingObjectCanvas);
        }
        activeOnboardingPanel.gameObject.SetActive(false);
    }

    [Button]
    public void UnonboardActiveObject()
    {
        if (onboardingCurrently != null)
        {
            //Debug.Log(onboardingCurrently.Explanation + " is unboarded!");
            UnonboardObject(onboardingCurrently);
            onboardingCurrently = null;
        }
    }

    public void UnonboardObject(IOnboardable objToUnonboard)
    {
        objToUnonboard.UnOnboard(activeOnboardingPanel);
        if(onboardingCurrently == objToUnonboard) { onboardingCurrently = null; }
    }

    [Button]
    public void OnboardObject(IOnboardable objToOnboard, bool removePrevious = false)
    {
        if (objToOnboard.IsOnboarded) return;

        if(removePrevious)
        {
            UnonboardActiveObject();
        }
        onboardingCurrently = objToOnboard;
        onboardingCurrently.Onboard(activeOnboardingPanel);
    }
}
