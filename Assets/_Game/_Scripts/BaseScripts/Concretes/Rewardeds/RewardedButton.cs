using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class RewardedButton : MonoBehaviour
{

    public event System.Action onRewardedWon;
    [SerializeField] private TextMeshProUGUI _rewardText;
    Button _button;
    Camera _camera;
    Vector3 _lastRewardedWorldPosition;
    private PanelAnimation _panelAnimation;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _camera = Camera.main;
        _panelAnimation = new PanelAnimation(transform);
    }

    private void Start()
    {
        _button.onClick.AddListener(PlayRewarded);
    }

    private void LateUpdate()
    {
        if (gameObject.activeInHierarchy)
        {
            transform.position = _camera.WorldToScreenPoint(_lastRewardedWorldPosition);
        }
    }

    private void PlayRewarded()
    {
        Won();
    }

    private void Won()
    {
        transform.DOShakeScale(0.5f, 0.1f, 10, 90, true, ShakeRandomnessMode.Harmonic).OnComplete(() => CloseButton());
        _button.interactable = false;
        onRewardedWon?.Invoke();
        Debug.Log("Play Rewarded");
    }
    public void OpenButtonAtPosition(IClickable clickable)
    {
        if(_lastRewardedWorldPosition != clickable.RewardedButtonPosition || !gameObject.activeInHierarchy)
        {
            OpenButton();
            _lastRewardedWorldPosition = clickable.RewardedButtonPosition;
            _rewardText.text = clickable.Rewarded.RewardedData.rewardText;
            Vector3 screenPosition = _camera.WorldToScreenPoint(_lastRewardedWorldPosition);
            transform.position = screenPosition;
            _panelAnimation.PlayOpenAnimation();
            
            onRewardedWon = null;
            onRewardedWon += clickable.GetReward;

            Debug.Log("Button Init");
            
        }
    }

    private void OpenButton()
    {
        gameObject.SetActive(true);
    }

    public void CloseButton()
    {
        if (gameObject.activeInHierarchy)
        {
            _panelAnimation.PlayCloseAnimation(() =>
            {
                gameObject.SetActive(false);
                _button.interactable = true;
            });
        }

    }


    private void OnDisable()
    {
        onRewardedWon = null;
    }
}

public class PanelAnimation
{
    private Transform _panelTransform;
    private Vector3 _initialScale;

    public PanelAnimation(Transform panelTransform)
    {
        _panelTransform = panelTransform;
        _initialScale = _panelTransform.localScale;

    }

    public void PlayOpenAnimation(float punchOffset = 0.3f)
    {
        DOTween.Kill(_panelTransform);
        _panelTransform.gameObject.SetActive(true);
        _panelTransform.localScale = Vector3.zero;
        _panelTransform.DOScale(_initialScale, 0.1f).SetEase(Ease.Linear).OnComplete(() => _panelTransform.DOPunchScale(_initialScale * punchOffset, 0.1f, 5, 0.1f));
    }

    public void PlayCloseAnimation(System.Action onComplete = null)
    {
        _panelTransform.DOScale(Vector3.zero, 0.1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            onComplete?.Invoke();
            _panelTransform.localScale = _initialScale;
        });
    }
}

