using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoneyUIFeedback : MonoBehaviour
{
    private Transform MoneyPanel;
    private Sequence MoneySequence;
    private bool isStarted = false;
    private float spreadTime = 0.7f;
    private float goingUpTime = 0.8f;
    //private float spreadTolerance = 250.0f;
    private float spreadTolerance = 1.25f;
    //private float rotatingTolerance = 120.0f;
    private float rotatingTolerance = 120.0f;

    private void Awake()
    {
        MoneyPanel = transform.parent;
        DOTween.SetTweensCapacity(4000, 1000);
    }
    void OnEnable()
    {
        isStarted = true;
    }

    private void Update()
    {
        if (isStarted)
        {
            isStarted = false;
            transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
            MoveCoinToUIPanel();
        }
    }
    void MoveCoinToUIPanel()
    {
        MoneySequence = DOTween.Sequence();

        MoneySequence
            .Append(transform.DOMove(transform.position + new Vector3(Random.Range(-spreadTolerance, spreadTolerance), Random.Range(-spreadTolerance, spreadTolerance), 0f), spreadTime)).SetEase(Ease.OutSine)
            //.Append(transform.DOMove(transform.position + new Vector3(Random.Range(-spreadTolerance, spreadTolerance), Random.Range(-spreadTolerance, spreadTolerance), 0f), spreadTime)).SetEase(Ease.OutSine)
            .Join(transform.DORotate(Vector3.forward * Random.Range(-rotatingTolerance, rotatingTolerance), spreadTime))
            .Append(transform.DOMove(MoneyPanel.position, goingUpTime)).SetEase(Ease.OutSine)
            .Join(transform.DORotate(Vector3.zero, goingUpTime))
            .OnComplete(() => gameObject.SetActive(false));
    }

}