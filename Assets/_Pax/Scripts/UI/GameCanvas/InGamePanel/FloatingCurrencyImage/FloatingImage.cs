namespace Pax
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using DG.Tweening;

    public class FloatingImage : MonoBehaviour
    {

        private Transform _targetTransform;
        private Sequence _sequence;
        private float _spreadTime = 0.7f;
        private float _goingUpTime = 0.8f;
        private float _spreadTolerance = 1f;
        private float _rotatingTolerance = 120.0f;

        void Awake()
        {
            Init();
        }


        public void MoveToTarget(Transform target, System.Action onComplete = null)
        {
            Init();
            _sequence = DOTween.Sequence();

            _sequence
                .Append(transform.DOMove(transform.position + new Vector3(Random.Range(-_spreadTolerance, _spreadTolerance), Random.Range(-_spreadTolerance, _spreadTolerance), 0f), _spreadTime)).SetEase(Ease.OutSine)
                .Join(transform.DORotate(Vector3.forward * Random.Range(-_rotatingTolerance, _rotatingTolerance), _spreadTime))
                .Append(transform.DOMove(target.position, _goingUpTime)).SetEase(Ease.OutSine)
                .Join(transform.DORotate(Vector3.zero, _goingUpTime))
                .OnComplete(() =>
                {
                    onComplete?.Invoke();
                    transform.SetParent(_targetTransform);
                    gameObject.SetActive(false);

                });
        }

        void Init()
        {
            if (!_targetTransform)
            {
                _targetTransform = transform.parent;
                DOTween.SetTweensCapacity(2000, 250);
            }
        }
    }
}
