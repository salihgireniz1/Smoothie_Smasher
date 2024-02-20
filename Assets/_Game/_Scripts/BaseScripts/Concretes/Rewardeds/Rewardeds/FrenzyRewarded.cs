using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
//using Udo.Hammer.Runtime.Core;

public class FrenzyRewarded : Rewarded
{

    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _infoText;
    [SerializeField] private Image _adsIcon;
    [SerializeField] private Image _rays;
    [SerializeField] private Animator _buttonAnimator;
    private GameObject _speedLineParticleGameObject;
    private GameObject _zombieFire;


    private RewardedData _rewardedData;
    public override RewardedData RewardedData { get => _rewardedData; set => _rewardedData = value; }

    private Vector3 _initialPosition;
    private Vector3 _targetPosition;
    private RectTransform _panelRectTransform;
    private Vector3 _buttonInitialScale;
    private float _duration = 0;
    private bool _isOfferGained;

    Sequence _seq;
    //private CountdownTimer _countdownTimer;
    private CountDown _countDown;

    private void Awake()
    {
        _panelRectTransform = GetComponent<RectTransform>();

        _initialPosition = transform.position;
        _initialPosition.x = 0;
        _targetPosition = _initialPosition;
        _targetPosition.x = 1080f;
        _buttonInitialScale = _button.transform.localScale;

        _speedLineParticleGameObject = GameObject.FindGameObjectWithTag("SpeedLine");
        _zombieFire = GameObject.FindGameObjectWithTag("ZombieFire");
    }

    private void Update()
    {
        if (_isOfferGained)
        {
            //_countdownTimer.UpdateTimer();

            if (_duration > 0f)
            {
                _duration -= Time.deltaTime;
                _countDown.UpdateTimer(_duration);
            }
            else
            {
                _isOfferGained = false;
            }

        }

        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    CraetiveFrenzyReward();
        //}
    }

    public override void ShowRewarded()
    {
        _infoText.text = _rewardedData.rewardText;

        _seq.Append(transform.DOMove(_targetPosition, 10).SetEase(Ease.Linear).OnComplete(() =>
        {
            CloseRewarded();
            ResetRewarded();
        }));
 
    }

    private void ResetRewarded()
    {
        _button.interactable = true;
        Vector3 resetPosition = transform.position;
        resetPosition.x = 0;
        transform.position = resetPosition;
        _rays.enabled = true;
        _infoText.enabled = true;
    }

    public override void OpenRewarded()
    {
        gameObject.SetActive(true);
        OpenButton();
    }

    public override void CloseRewarded()
    {

        gameObject.SetActive(false);
    }

    public void OpenButton()
    {
        _button.interactable = true;
        _button.transform.localScale = Vector3.zero;
        _button.transform.DOScale(_buttonInitialScale, 0.2f).SetEase(Ease.Linear).OnComplete(()=> _buttonAnimator.SetBool("isFloating", true));
    }

    private void StopButton()
    {

        _button.interactable = false;
        _buttonAnimator.SetBool("isFloating", false);
        _seq.Kill();
    }

    private void PlayRewarded()
    {
        //StopButton();
        //_duration = _rewardedData.rewardAmount;

        //string rewardedName = _rewardedData.rewardedType.ToString();
        //string key = rewardedName + "Click";
        //Hammer.Instance.ANALYTICS_CustomEvent(key, LevelManager.Instance.SDKLevel);

        //if (!_rewardedData.isFree)
        //{
        //    RewardedController.Instance.OpenAdBreak();

        //    Hammer.Instance.MEDIATION_HasRewarded(() =>
        //    {
        //        Hammer.Instance.MEDIATION_ShowRewarded(() =>
        //        {
        //            StopButton();
        //            StartCoroutine(RewardCoroutine(_duration));
        //            string key = rewardedName + "Success";
        //            Hammer.Instance.ANALYTICS_CustomEvent(key, LevelManager.Instance.SDKLevel);
        //            RewardedController.Instance.CloseAdBreak();

        //        },
        //        s =>
        //        {
        //            //Debug.Log("Rewarded gösterilemedi");
        //            CloseRewarded();
        //            ResetRewarded();
        //            string key = rewardedName + "Fail";
        //            Hammer.Instance.ANALYTICS_CustomEvent(key, LevelManager.Instance.SDKLevel);
        //            RewardedController.Instance.CloseAdBreak();

        //        });
        //    },
        //    s =>
        //    {
        //        //Debug.Log("Rewarded yüklenemedi");
        //        CloseRewarded();
        //        ResetRewarded();
        //        string key = rewardedName + "hasRewardedFalse";
        //        Hammer.Instance.ANALYTICS_CustomEvent(key, LevelManager.Instance.SDKLevel);
        //        RewardedController.Instance.CloseAdBreak();
        //    }
        //    );
        //}
    }

    public void CraetiveFrenzyReward()
    {
        StartCoroutine(RewardCoroutine(60f));
    }

    private IEnumerator RewardCoroutine(float duration)
    {
        //if (_speedLineParticleGameObject != null) _speedLineParticleGameObject.transform.GetChild(0).gameObject.SetActive(true);
        //if (_zombieFire != null) _zombieFire.transform.GetChild(0).gameObject.SetActive(true); 
        //bool isTimeStart = false;
        //MoveRewarded(()=> isTimeStart = true);

        ////_countdownTimer = new CountdownTimer(_infoText, duration);
        //_countDown = new CountDown(_infoText);
        //_adsIcon.enabled = false;
        //_rays.enabled = false;

        //Zombie[] zombies = FindObjectsOfType<Zombie>();

        //foreach (var zombie in zombies)
        //{
        //    zombie.RewardedStatsChange(0.5f); //
        //}

        //yield return new WaitWhile(() => !isTimeStart);
        //_isOfferGained = true;

        //yield return new WaitForSeconds(duration);

        //Zombie[] zombies2 = FindObjectsOfType<Zombie>();

        //GameManager.Instance.isFrenzyMode = false;

        //foreach (var zombie in zombies2)
        //{
        //    zombie.ReSetRewardedStatsChange();
        //}

        ////Debug.Log("Frenzy Mode End.");
        //_isOfferGained = false;
        //if (_speedLineParticleGameObject != null) _speedLineParticleGameObject.transform.GetChild(0).gameObject.SetActive(false);
        //if (_zombieFire != null) _zombieFire.transform.GetChild(0).gameObject.SetActive(false);

        //CloseRewarded();
        //ResetRewarded();
        yield return null;
    }

    private void MoveRewarded(System.Action onComplete = null)
    {
        Vector3 upRightPosition = _targetPosition;
        upRightPosition.x -= 125f;
        upRightPosition.y = 1650;

        transform.DOPunchScale(Vector3.one, 0.3f, 4, 0.4f).SetDelay(0.4f).OnComplete(() =>
        {
            transform.DOMove(upRightPosition, 1f).SetEase(Ease.Linear).OnComplete(()=> onComplete?.Invoke());
        });
    }

    private void OnEnable()
    {
        _seq = DOTween.Sequence();
        _button.onClick.AddListener(PlayRewarded);   
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(PlayRewarded);

    }
}

public class CountdownTimer
{
    private float _totalTime = 120f; // Geri sayım süresi (saniye)
    private TextMeshProUGUI _countdownText; // Süreyi gösterecek metin nesnesi

    private float _currentTime; // Geçen süre

    public CountdownTimer(TextMeshProUGUI countdownText, float totalTime)
    {
        _countdownText = countdownText;
        _totalTime = totalTime;
        _currentTime = _totalTime;
    }


    public void UpdateTimer()
    {
        if (_currentTime > 0)
        {
            _currentTime -= Time.deltaTime; // Geçen süreyi azalt

            // Süreyi 00:00 formatında metin olarak göster
            string minutes = Mathf.Floor(_currentTime / 60).ToString("00");
            string seconds = Mathf.Floor(_currentTime % 60).ToString("00");
            _countdownText.text = minutes + ":" + seconds;
        }
        else
        {
            _countdownText.text = "00:00"; // Süre tamamlandıysa metni 00:00 olarak ayarla
        }
    }
}
