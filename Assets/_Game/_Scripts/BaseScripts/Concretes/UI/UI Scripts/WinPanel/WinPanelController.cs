using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class WinPanelController : MonoBehaviour
{
    public event System.Action OnOpenWinPanel;
    [SerializeField] private Button _nextButton;
    [SerializeField] private Animator _winAnimator;
    [SerializeField] private TextMeshProUGUI _winText;
    [SerializeField] private TextMeshProUGUI _scoreText;
    private ButtonAnimation _nextButtonAnimation;

    private void Awake()
    {
        //_backgroundImage.sprite = _settings.BackgroundSprite;
        //_winPanelImage.sprite = _settings.WinSprite;
        //_nextButtonImage.sprite = _settings.ButtonSprite;
        _nextButtonAnimation = new ButtonAnimation(_nextButton);
    }

    private void OnEnable()
    {
        StartCoroutine(PlayWinPanel());
    }

    private void OnDisable()
    {
        _nextButtonAnimation.Reset();
    }

    private void Start()
    {
        _nextButton.onClick.AddListener(NextButtonClicked);
    }

    private void NextButtonClicked()
    {
        //LevelManager.Instance.LoadNextLevel();
        ClosePanel();
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);
    }

    private void ClosePanel()
    {
        gameObject.SetActive(false);
    }



    private IEnumerator PlayWinPanel()
    {
        yield return new WaitForSeconds(0.5f);
        OnOpenWinPanel?.Invoke();
        yield return new WaitForSeconds(1f);
        _winAnimator.SetTrigger("Start");
        yield return new WaitForSeconds(1.1f);
        _nextButtonAnimation.ShowButtonWithGrowingAnimation(0.5f, DG.Tweening.Ease.Linear);

    }

}
