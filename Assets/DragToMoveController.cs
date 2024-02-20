using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragToMoveController : MonoBehaviour, IOnboardable
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

    public RectTransform OnboardableTransform => onboardableTransform;

    public Vector3 HandPositionOffset => handPositionOffset;

    public Vector3 HandRotation => handRotation;

    public RectTransform HandPositionFocus => handPositionFocus;

    public Vector3 ExplanationPos => Vector3.zero;

    [SerializeField]
    private bool isOnboarded;

    [SerializeField]
    private RectTransform onboardableTransform;

    [SerializeField]
    private RectTransform handPositioner;

    [SerializeField]
    private Vector3 handPositionOffset;

    [SerializeField]
    private Vector3 handRotation;

    [SerializeField]
    private RectTransform handPositionFocus;

    [SerializeField]
    private Rigidbody playerBody;
    [SerializeField]
    private GameObject squizePanel;
    bool canCheckInput;
    private void Start()
    {
        if (IsOnboarded)
        {
            gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        if (!canCheckInput) return;

        if (Input.GetMouseButton(0))
        {
            if (OnboardableTransform.gameObject.activeInHierarchy)
            {
                squizePanel.SetActive(true);
                onboardableTransform.gameObject.SetActive(false);
            }

            // Create a ray from playerBody's position along its forward direction
            /*Ray ray = new Ray(playerBody.transform.position + Vector3.up, playerBody.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 5f, LayerMask.GetMask("Fruit_Base")))
            {
                canCheckInput = false;
                UnonboardMe();
                return;
            }

            // Draw the ray in the scene for visualization
            Debug.DrawRay(ray.origin, ray.direction * 5f, Color.red);*/
        }
        else
        {
            if (!OnboardableTransform.gameObject.activeInHierarchy)
            {
                squizePanel.SetActive(false);
                onboardableTransform.gameObject.SetActive(true);
            }
        }
    }
    public void UnonboardMe()
    {
        if (!IsOnboarded)
            OnboardingManager.Instance.UnonboardObject(this);
    }
    public void Onboard(OnboardingPanel panel)
    {
        if(!IsOnboarded)
        {
            canCheckInput = true; 
            if (!OnboardableTransform.gameObject.activeInHierarchy)
            {
                onboardableTransform.gameObject.SetActive(true);
            }
        }
    }

    public void UnOnboard(OnboardingPanel panel)
    {
        if (!IsOnboarded)
        {
            IsOnboarded = true;
            canCheckInput = false;
            squizePanel.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
