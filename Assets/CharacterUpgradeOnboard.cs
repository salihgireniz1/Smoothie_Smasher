using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUpgradeOnboard : MonoBehaviour, IOnboardable
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

    public Vector3 ExplanationPos => expPos;

    public CharacterButtonOnboard myButtonOnboard;

    [SerializeField]
    private string explanation;

    [SerializeField]
    private Vector3 expPos;

    private RectTransform onboardableTransform;

    private RectTransform handPositionFocus;

    private Vector3 handPositionOffset;

    private Vector3 handRotation;

    private bool isOnboarded;

    private Transform defaultParent;

    int defaultChildIndex = 2;
    private void Awake()
    {
        defaultParent = this.transform;
        defaultChildIndex = 3;
    }
    public void UnonboardMe()
    {
        if (!IsOnboarded)
            OnboardingManager.Instance.UnonboardObject(this);
    }
    public void UnOnboard(OnboardingPanel panel)
    {
        IsOnboarded = true;

        if (defaultParent == null)
        {
            defaultParent = transform.GetChild(0).GetComponent<RectTransform>();
        }
        // Make the object child of panel.
        this.onboardableTransform.SetParent(this.defaultParent);
        this.onboardableTransform.SetSiblingIndex(defaultChildIndex);

        // Activate the panel.
        if (panel.gameObject.activeInHierarchy)
        {
            panel.gameObject.SetActive(false);
        }

    }
    public CharacterButtonOnboard cbo;
    IEnumerator DelayedOnboard(OnboardingPanel panel)
    {
        yield return new WaitForSeconds(.25f);

        if (cbo!= null && !cbo.IsOnboarded) yield return null;

        defaultParent = transform.GetChild(0).GetComponent<RectTransform>();
        onboardableTransform = defaultParent.GetChild(defaultChildIndex).GetComponent<RectTransform>();
        CharacterUpgradeController controller = defaultParent.GetComponent<CharacterUpgradeController>();
        controller.onboardController = this;

        handPositionFocus = onboardableTransform;

        // Activate the panel.
        if (!panel.gameObject.activeInHierarchy)
        {
            panel.gameObject.SetActive(true);
        }
        // Assign panel text.
        panel.explanationText.text = this.Explanation;

        if (handPositionFocus != null)
        {
            panel.HandPosition(this.handPositionFocus, Vector3.zero, true);
        }
        panel.ExpPosition(this);
        // Make the object child of panel.
        this.OnboardableTransform.SetParent(panel.transform);
        this.OnboardableTransform.SetAsFirstSibling();
    }
    public void Onboard(OnboardingPanel panel)
    {
        if (IsOnboarded) return;
        if (!myButtonOnboard.IsOnboarded) return;

        StartCoroutine(DelayedOnboard(panel));
    }
}
