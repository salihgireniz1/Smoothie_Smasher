using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Splash : MonoBehaviour
{
    public Color32 defaultColor;
    public Color32 activeColor;
    public float defaultScale;
    public float activeScale;
    public float fadeTime;
    public float appearTime;
    public float remainTime;

    SpriteRenderer sr;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        sr.color = defaultColor;
        sr.transform.localScale = Vector3.one * defaultScale;
        if (sr == null)
        {
            sr = GetComponent<SpriteRenderer>();
        }
        sr.DOColor(activeColor, appearTime);
        sr.transform.DOScale(Vector3.one * activeScale, appearTime);
        StartCoroutine(FadeRoutine());
    }
    IEnumerator FadeRoutine()
    {
        yield return new WaitForSeconds(remainTime);
        sr.DOColor(defaultColor, fadeTime).OnComplete(()=> PoolManager.Instance.EnqueueToPool(gameObject));
        sr.transform.DOScale(Vector3.one * defaultScale, fadeTime * 2f);
    }
}
