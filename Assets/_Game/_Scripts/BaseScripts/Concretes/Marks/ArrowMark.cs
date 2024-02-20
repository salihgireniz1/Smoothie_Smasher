using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ArrowMark : Mark
{
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void LateUpdate()
    {

        Vector3 relativePosition = transform.position - _camera.transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(relativePosition, Vector3.up);

        Vector3 euler = lookRotation.eulerAngles;
        euler.x += -90;
        euler.y = 0;
        lookRotation.eulerAngles = euler;
        transform.rotation = lookRotation;

    }

    public override void PlayAnimation()
    {
        Vector3 firstPosition = transform.localPosition;
        Vector3 lastPosition = firstPosition;
        lastPosition.y += 3;
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOLocalMoveY(lastPosition.y, 1f).SetEase(Ease.Linear))
            .Append(transform.DOLocalMoveY(firstPosition.y, 1f).SetEase(Ease.Linear)).SetLoops(-1);
    }

    public override void SetMark(float value, string text)
    {
        throw new System.NotImplementedException();
    }
}
