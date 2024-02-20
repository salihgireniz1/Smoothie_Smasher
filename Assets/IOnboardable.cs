using UnityEngine;

public interface IOnboardable
{
    public bool IsOnboarded { get; }
    public string Explanation { get; }
    public Transform DefaultParent { get; }
    public RectTransform OnboardableTransform { get; }
    public RectTransform HandPositionFocus { get; }
    public Vector3 HandPositionOffset { get; }
    public Vector3 HandRotation { get; }
    public Vector3 ExplanationPos { get; }

    void Onboard(OnboardingPanel panel); 
    void UnOnboard(OnboardingPanel panel);
}
