using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScorePanel : MonoBehaviour
{
    [SerializeField] private ScorePanelSettings _settings;
    public ScorePanelSettings Settings { get => _settings; set => _settings = value; }

    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Image _collectibleImage;


    //private void Awake()
    //{
    //    _collectibleImage.sprite = _settings.CollectibleSprite;
    //    _backgroundImage.sprite = _settings.BackgroundSprite;
    //}

    private void OnEnable()
    {
        EventManagement.OnScoreChange += RefreshScoreText;
    }

    private void OnDisable()
    {
        EventManagement.OnScoreChange -= RefreshScoreText;
    }


    private void RefreshScoreText(float newScore)
    {
        _scoreText.text = newScore.ToString();
    }

    public void SetPanelActive(bool value)
    {
        transform.gameObject.SetActive(value);
    }

}
