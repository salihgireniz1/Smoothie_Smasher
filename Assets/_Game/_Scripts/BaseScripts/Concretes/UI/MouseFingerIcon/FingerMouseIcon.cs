using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FingerMouseIcon : MonoBehaviour
{

    [SerializeField] private Image _fingerImage;
    [SerializeField] private float _scaleRatio = 1.3f;
    private RectTransform _rectTransform;
    private Sequence _seq;
    private Vector3 _fingerScale;
    private Vector3 _mousePosition;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        _fingerScale = _fingerImage.transform.localScale;
    }
    private void Update()
    {
        _mousePosition = Input.mousePosition;
        MoveRectFinger(_mousePosition);

        if (Input.GetKeyDown(KeyCode.A))
        {
            StartClickFingerAnimation();
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            StopClickFingerAnimation();
        }
    }

    private void MoveRectFinger(Vector3 position)
    {
        if (_fingerImage == null) return;
        
        if(_rectTransform.position != position)
            _rectTransform.position = position; 
    }

    private void StartClickFingerAnimation()
    {
        _seq = DOTween.Sequence();
        Vector3 animateScale = _fingerScale * _scaleRatio;

        _seq.Append(_fingerImage.transform.DOScale(animateScale, 0.3f).SetEase(Ease.Linear))
            .Append(_fingerImage.transform.DOScale(_fingerScale, 0.3f).SetEase(Ease.Linear)).SetLoops(-1);
    }

    private void StopClickFingerAnimation()
    {
        if (_seq != null)
        {
            _seq.Kill();
            _fingerImage.transform.DOScale(_fingerScale, 0.2f).SetEase(Ease.Linear);

        }
    }


}
