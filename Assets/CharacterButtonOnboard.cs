using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterButtonOnboard : MonoBehaviour, IOnboardable
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

    public Vector3 ExplanationPos => new Vector3(0, 500, 0);

    [SerializeField]
    private string explanation;

    private RectTransform onboardableTransform;

    private RectTransform handPositionFocus;

    [SerializeField]
    private Vector3 handPositionOffset;

    private Vector3 handRotation;

    private bool isOnboarded;

    private Transform defaultParent;

    int defaultChildIndex = 0;
    private void Awake()
    {
        onboardableTransform = GetComponent<RectTransform>();
        handPositionFocus = onboardableTransform;
        defaultParent = transform.parent;
        defaultChildIndex = 1;
    }
    private void Start()
    {
        LevelManager.Instance.cbo = this;
    }
    public void UnonboardMe()
    {
        if (IsOnboarded)
            OnboardingManager.Instance.UnonboardObject(this);
    }
    public void UnOnboard(OnboardingPanel panel)
    {
        // Make the object child of panel.
        this.onboardableTransform.SetParent(this.defaultParent);
        this.onboardableTransform.SetSiblingIndex(defaultChildIndex);

        // Activate the panel.
        /*if (panel.gameObject.activeInHierarchy)
        {
            panel.gameObject.SetActive(false);
        }*/

    }
    public void Onboard(OnboardingPanel panel)
    {
        if (IsOnboarded) return;

        IsOnboarded = true;

        // Activate the panel.
        if (!panel.gameObject.activeInHierarchy)
        {
            panel.gameObject.SetActive(true);
        }
        // Assign panel text.
        panel.explanationText.text = this.Explanation;

        if (handPositionFocus != null)
        {
            panel.HandPosition(this.handPositionFocus, handPositionOffset);
        }
        panel.ExpPosition(this);
        // Make the object child of panel.
        this.OnboardableTransform.SetParent(panel.transform);
        this.OnboardableTransform.SetAsFirstSibling();
    }
}
