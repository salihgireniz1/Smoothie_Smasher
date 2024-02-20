using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandScaleHandler : MonoBehaviour
{
    public float RewardScale { get; private set; } = 1f;
    public float CurrentScale => currentScale;
    public int ScaleLevel
    {
        get => ES3.Load(Consts.HAND_SCALE_LEVEL, 0);
        set
        {
            ES3.Save(Consts.HAND_SCALE_LEVEL, value);
            UpdateHandScales();
        }
    }

    public Transform[] hands;
    public List<float> handScales = new List<float>();
    public float scaleDuration = .5f;
    public Ease scaleEase = Ease.OutBack;
    
    private float currentScale;
    List<Rigidbody> handBodies = new();
    private void Start()
    {
        UpdateHandScales();
        foreach (var item in hands)
        {
            handBodies.Add(item.GetComponentInChildren<Rigidbody>());
        }
    }
    public void GetHandScaleReward(float ratio, float time)
    {
        StartCoroutine(RewardHandScaleRoutine(ratio, time));
    }
    IEnumerator RewardHandScaleRoutine(float ratio, float time)
    {
        RewardScale *= ratio;
        UpdateHandScales();

        yield return new WaitForSeconds(time);

        RewardScale = 1f;
        UpdateHandScales();
    }
    public void UpdateHandScales()
    {
        currentScale = GetCurrentScale();
        ScaleHands();
    }
    public void ScaleHands()
    {
        foreach (var hand in hands)
        {
            hand.DOScale(Vector3.one * currentScale * RewardScale, scaleDuration).SetEase(scaleEase);
        }
    }
    public void UnfreezeHands()
    {
        foreach (var item in handBodies)
        {
            item.isKinematic = false;
        }
    }
    public void FreezeHands()
    {
        foreach (var item in handBodies)
        {
            item.isKinematic = true;
        }
    }
    [Button("Scale Hands To:")]
    public void ScaleHands(float scaleTo)
    {
        /*Vector3 dir = transform.forward;
        transform.DOMove(transform.position - (dir * scaleTo * 3f), scaleDuration / 2f);

        foreach (var hand in hands)
        {
            hand.DOScale(Vector3.one * scaleTo, scaleDuration).SetEase(scaleEase);
        }*/
        ScaleLevel += 1;
    }
    public float GetCurrentScale()
    {
        int level = ScaleLevel;
        // Clamp level value.
        level = Mathf.Min(level, handScales.Count - 1);
        level = Mathf.Max(0, level);

        return handScales[level];
    }
    public void ChangeScaleLevel(int newLevel)
    {
        ScaleLevel = newLevel;
    }
    public void IncreaseScaleLevel()
    {
        ScaleLevel += 1;
    }
}
