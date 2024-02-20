using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnboardWithPanel : MonoBehaviour, IOnboardable
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

    public string Explanation => explanation;

    public Transform DefaultParent => defaultParent;

    public RectTransform OnboardableTransform => onboardableTransform;

    public Vector3 HandPositionOffset => handPositionOffset;

    public Vector3 HandRotation => handRotation;

    public RectTransform HandPositionFocus => handPositionFocus;

    public Vector3 ExplanationPos => Vector3.zero;

    [SerializeField]
    private bool isOnboarded;

    [SerializeField]
    private string explanation;

    [SerializeField]
    private RectTransform onboardableTransform;

    [SerializeField]
    private RectTransform handPositionFocus;

    [SerializeField]
    private Vector3 handPositionOffset;

    [SerializeField]
    private Vector3 handRotation;

    [SerializeField]
    private Transform defaultParent;

    private void Awake()
    {
        onboardableTransform = GetComponent<RectTransform>();
    }
    public void UnOnboard(OnboardingPanel panel)
    {
        if (IsOnboarded) return;

        // Make the object child of panel.
        this.onboardableTransform.SetParent(this.DefaultParent);
        this.onboardableTransform.SetAsFirstSibling();

        // Activate the panel.
        if (panel.gameObject.activeInHierarchy)
        {
            panel.gameObject.SetActive(false);
        }

        IsOnboarded = true;
    }
    public void Onboard(OnboardingPanel panel)
    {
        if (IsOnboarded) return;

        // Activate the panel.
        if (!panel.gameObject.activeInHierarchy)
        {
            panel.gameObject.SetActive(true);
        }
        // Assign panel text.
        panel.explanationText.text = this.Explanation;

        if (handPositionFocus != null)
        {
            panel.HandPosition(this.handPositionFocus, handPositionOffset, false);
        }

        // Make the object child of panel.
        this.OnboardableTransform.SetParent(panel.transform);
        this.OnboardableTransform.SetAsFirstSibling();
    }
}
