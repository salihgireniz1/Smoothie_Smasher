using TMPro;
using UnityEngine;

public class OnboardingPanel : MonoBehaviour
{
    public TextMeshProUGUI explanationText;
    public RectTransform handTransform;

    public void ExpPosition(IOnboardable obj)
    {
        //explanationText.transform.parent.GetComponent<RectTransform>().anchoredPosition = obj.ExplanationPos;
        explanationText.rectTransform.anchoredPosition = obj.ExplanationPos;
    }

    public void HandPosition(RectTransform objToFocus, Vector3 offSet = default(Vector3), bool autoOffset = false)
    {
        Vector3 targetPos = objToFocus.position;
        if (autoOffset)
        {
            float xOffset = objToFocus.sizeDelta.x / 2f;
            float yOffset = objToFocus.sizeDelta.y / 2f;

            targetPos = new Vector3(targetPos.x + xOffset, targetPos.y - yOffset, 0f);
        }
        else
        {
            targetPos = objToFocus.position + offSet;
        }
        // Calculate the rotation to look at the target position
        Vector3 lookDirection = objToFocus.position - targetPos;
        //Debug.Log(lookDirection);
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        handTransform.position = targetPos;
        handTransform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }
}