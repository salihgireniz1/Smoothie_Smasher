using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquizeOnboard : MonoBehaviour, IOnboardable
{
    public bool IsOnboarded
    {
        get
        {
            isOnboarded = ES3.Load(Explanation + gameObject.name, false);
            return isOnboarded;
        }
        private set
        {
            isOnboarded = value;
            ES3.Save(Explanation + gameObject.name, isOnboarded);
        }
    }

    public string Explanation => "Drag";

    public Transform DefaultParent => transform.parent;

    public RectTransform OnboardableTransform => GetComponent<RectTransform>();

    public Vector3 HandPositionOffset => Vector3.zero;

    public Vector3 HandRotation => Vector3.zero;

    public RectTransform HandPositionFocus => GetComponent<RectTransform>();

    public Vector3 ExplanationPos => Vector3.zero;

    [SerializeField]
    private bool isOnboarded;

    [SerializeField]
    private GameObject myPanel;

    [SerializeField]
    private DragToMoveController dragCont;

    private void Start()
    {
        if (IsOnboarded)
        {
            gameObject.SetActive(false);
        }
        else
        {
            OnboardingManager.Instance.OnboardObject(this);
        }
    }
    public void UnonboardMe()
    {
        if (!IsOnboarded)
            OnboardingManager.Instance.UnonboardObject(this);
    }
    public void Onboard(OnboardingPanel panel)
    {
        if (!IsOnboarded)
        {
            myPanel.SetActive(true);
        }
    }

    public void UnOnboard(OnboardingPanel panel)
    {
        if (!IsOnboarded)
        {
            IsOnboarded = true;
            myPanel.SetActive(false);
            OnboardingManager.Instance.OnboardObject(dragCont);
        }
    }
}
