namespace Pax
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using DG.Tweening;
    using TMPro;
    using static UnityEngine.GraphicsBuffer;

    public class CurrencyTextController : MonoBehaviour
    {
        TextMeshProUGUI _currencyText;
        ParticleSystem _sparkleParticle;
        int _currency = 0;
        Vector3 _scale;

        private void Awake()
        {
            _currencyText = GetComponent<TextMeshProUGUI>();
            _sparkleParticle = transform.GetChild(0).GetComponent<ParticleSystem>();
            _currencyText.text = _currency.ToString();
            _scale = transform.localScale;

        }

        public void AddCurrency(int amount)
        {
            {
                Sequence seq = DOTween.Sequence();
                Vector3 upScale = _scale * 1.4f;
                seq.Append(transform.DOScale(upScale, 0.1f).SetEase(Ease.InSine).OnComplete(() =>
                {
                    _currency++;
                    _currencyText.text = _currency.ToString();
                    PlaySparkleParticle();

                }))
                        .Append(transform.DOScale(_scale, 0.1f).SetEase(Ease.OutSine));
            }

        }

        void PlaySparkleParticle()
        {
            if (!_sparkleParticle.isPlaying)
                _sparkleParticle.Play();
        }
    }
}
