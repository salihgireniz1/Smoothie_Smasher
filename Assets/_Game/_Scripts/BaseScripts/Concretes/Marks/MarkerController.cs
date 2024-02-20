using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using PAG.Utility;

public class MarkerController : MonoSingleton<MarkerController>
{
    [SerializeField] private List<Transform> _markedTransforms;
    [Header("Cirle Particle")]
    [SerializeField] private bool _isOpenCircleParticle;
    [SerializeField] private Mark _circleParticleMark;
    [SerializeField] private Vector3 _markParticleOffSet = Vector3.zero;
    [Header("Arrow Mark")]
    [SerializeField] private bool _isOpenArrowMark;
    [SerializeField] private Mark _arrowMarkPrefab;
    [SerializeField] private Vector3 _arrowOffSet = Vector3.zero;
    [Header("World Text")]
    [SerializeField] private bool _isOpenTextMark;
    [SerializeField] private TextMark _textMarkPrefab;
    [SerializeField] private Vector3 _textMarkOffSet = Vector3.zero;
    [SerializeField] private float _fontSize = 0.12f;
    [SerializeField] private string _text = " ";
    [Header("Info Canvas Text")]
    [SerializeField] private bool _isOpenInfoText = true;
    [SerializeField] private GameObject _infoText;
    [SerializeField] private float _waitTime =3f;
    [SerializeField] private float _destroyTime = 10f;
    

    private List<Mark> _arrowMarks = new List<Mark>();
    private List<Mark> _cirleParticlePrefabs = new List<Mark>();
    private List<TextMark> _textMarkPrefabs = new List<TextMark>();
    private List<GameObject> _instantiatedInfoTexts = new List<GameObject>();
    Canvas _canvas;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _canvas.worldCamera = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
    }

    private void Start()
    {
        if (_isOpenCircleParticle)
        {
            _cirleParticlePrefabs = SpawnMarkedPrefab(_circleParticleMark, _markParticleOffSet);
        }

        if (_isOpenArrowMark)
        {
            _arrowMarks = SpawnMarkedPrefab(_arrowMarkPrefab, _arrowOffSet);
            StartArrowAnimation();
        }

        if (_isOpenTextMark)
        {
            _textMarkPrefabs = SpawnTextMarkedPrefab(_textMarkPrefab, _textMarkOffSet);

            foreach (var text in _textMarkPrefabs)
            {
                text.SetMark(_fontSize, _text);
            }
        }
        if (_isOpenInfoText)
        {
            PlayInfoText();
            StartCoroutine(DestroyAfterSeconds(_destroyTime));
        }
    }

    private List<Mark> SpawnMarkedPrefab(Mark mark, Vector3 offSet)
    {
        List<Mark> marks = new List<Mark>();
        int count = _markedTransforms.Count;

        for (int i = 0; i < count; i++)
        {
            Mark markInstantiated = Instantiate(mark, _markedTransforms[i]);
            markInstantiated.transform.localPosition = Vector3.zero + offSet;
            marks.Add(markInstantiated);
        }

        return marks;
    }

    private List<TextMark> SpawnTextMarkedPrefab(TextMark textMark, Vector3 offSet)
    {
        List<TextMark> marks = new List<TextMark>();
        int count = _markedTransforms.Count;

        for (int i = 0; i < count; i++)
        {
            TextMark markInstantiated = Instantiate(textMark, _markedTransforms[i]);
            markInstantiated.transform.localPosition = Vector3.zero + offSet;
            markInstantiated.text = _markedTransforms[i].gameObject.name;
            marks.Add(markInstantiated);
        }

        return marks;
    }

    public void PlayInfoText()
    {
        MoveOtherComboGameObject();
        Sequence seq = DOTween.Sequence();
        GameObject infoText = SpawnInfoText();

        seq.Append(infoText.transform.DOLocalMoveX(0, 0.2f).SetEase(Ease.Linear).SetDelay(1.5f))
            .Append(infoText.transform.DOLocalMoveX(1080f, 0.4f).SetEase(Ease.Linear).SetDelay(_waitTime)).OnComplete(() => DestroyComboGameObject(infoText));
    }

    private GameObject SpawnInfoText()
    {
        GameObject infoText = Instantiate(_infoText, _infoText.transform.position, _infoText.transform.rotation, transform);

        Vector3 position = infoText.transform.position;
        position.x = -1080f;
        infoText.transform.position = position;
        infoText.gameObject.SetActive(true);
        _instantiatedInfoTexts.Add(infoText);

        return infoText;
    }

    private void MoveOtherComboGameObject()
    {
        int count = _instantiatedInfoTexts.Count;

        if (count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject infoText = _instantiatedInfoTexts[0];
                infoText.transform.DOLocalMoveX(1080f, 0.2f).SetEase(Ease.Linear).OnComplete(() => Destroy(infoText));
                _instantiatedInfoTexts.RemoveAt(0);
                count = _instantiatedInfoTexts.Count;
            }
        }
    }

    private void DestroyComboGameObject(GameObject comboObject)
    {
        if (_instantiatedInfoTexts.Contains(comboObject))
        {
            int index = _instantiatedInfoTexts.IndexOf(comboObject);
            _instantiatedInfoTexts.RemoveAt(index);
            Destroy(comboObject);
        }
    }

    private void StartArrowAnimation()
    {
        int count = _arrowMarks.Count;

        for (int i = 0; i < count; i++)
        {
            _arrowMarks[i].PlayAnimation();
        }
    }

    private IEnumerator DestroyAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        int count = _markedTransforms.Count;

        for (int i = 0; i < count; i++)
        {
            if (_arrowMarks[i].gameObject != null)
            {
                
                _arrowMarks[i].gameObject.SetActive(false);
            }

            if (_cirleParticlePrefabs[i].gameObject != null)
            {
                _cirleParticlePrefabs[i].gameObject.SetActive(false);
            }

            if (_textMarkPrefabs[i].gameObject != null)
            {
                _textMarkPrefabs[i].gameObject.SetActive(false);
            }
        }
    }
}