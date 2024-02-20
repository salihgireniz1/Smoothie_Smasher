using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using PAG.Utility;

public class ComboController : MonoSingleton<ComboController>
{
    [SerializeField] private TextMeshProUGUI _comboText;
    [SerializeField] private Image _comboImage;
    [SerializeField] private List<ComboTextData> _comboTextDatas = new List<ComboTextData>();
    [SerializeField] private ParticleSystem _mergeFireworkParticle;
    private List<GameObject> _instantiatedComboObjects = new List<GameObject>();
    private int _comboCounter = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ShowComboImage(4);
        }
    }

    private ComboTextData GetComboTextData(int comboCount)
    {
        _comboCounter++;
        int combo = comboCount + _comboCounter;
        ComboTextData data = new ComboTextData();

        if(combo == 3)
        {
           data.comboMessage = _comboTextDatas[0].comboMessage;
            _comboTextDatas[0].color.a = 1;
            data.color = _comboTextDatas[0].color;
            data.comboSprite = _comboTextDatas[0].comboSprite;
        }
        else if(combo == 4)
        {
            data.comboMessage = _comboTextDatas[1].comboMessage;
            _comboTextDatas[1].color.a = 1;
            data.color = _comboTextDatas[1].color;
            data.comboSprite = _comboTextDatas[1].comboSprite;


        }
        else if(combo > 4)
        {
            data.comboMessage = _comboTextDatas[2].comboMessage;
            _comboTextDatas[2].color.a = 1;
            data.color = _comboTextDatas[2].color;
            data.comboSprite = _comboTextDatas[2].comboSprite;

        }

        return data;
    }

    public void ShowText(int comboCount) // Hatali
    {
        if (comboCount < 2) return;
        MoveOtherComboGameObject();
        Sequence seq = DOTween.Sequence();
        ComboTextData data = GetComboTextData(comboCount);
        TextMeshProUGUI comboText = SpawnText(data);

        seq.Append(comboText.transform.DOLocalMoveX(0, 0.2f).SetEase(Ease.Linear))
            .Append(comboText.transform.DOLocalMoveX(1080f, 0.4f).SetEase(Ease.Linear).SetDelay(3f)).OnComplete(() => DestroyComboGameObject(comboText.gameObject));
    }

    private TextMeshProUGUI SpawnText(ComboTextData data)
    {
        TextMeshProUGUI comboText = Instantiate(_comboText, _comboText.transform.position, _comboText.transform.rotation, transform);

        Vector3 position = comboText.transform.position;
        position.x = -1080f;
        comboText.transform.position = position;

        comboText.text = data.comboMessage;
        comboText.color = data.color;
        comboText.gameObject.SetActive(true);
        _instantiatedComboObjects.Add(comboText.gameObject);

        return comboText;
    }

    private void MoveOtherComboGameObject()
    {
        int count = _instantiatedComboObjects.Count;

        if (count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject comboObj = _instantiatedComboObjects[0];
                comboObj.transform.DOKill();
                comboObj.transform.DOLocalMoveX(1080f, 0.2f).SetEase(Ease.Linear).OnComplete(() => Destroy(comboObj.gameObject));
                _instantiatedComboObjects.RemoveAt(0);
                count = _instantiatedComboObjects.Count;
            }
        }
    }

    private  void DestroyComboGameObject(GameObject comboObject)
    {
        if (_instantiatedComboObjects.Contains(comboObject))
        {
            int index = _instantiatedComboObjects.IndexOf(comboObject);
            _instantiatedComboObjects.RemoveAt(index);
            _comboCounter = 0;
            Destroy(comboObject);
        }
    }

    public void ShowVictoryText()
    {
        TextMeshProUGUI victoryText = Instantiate(_comboText, _comboText.transform.position, _comboText.transform.rotation, transform);
        Vector3 position = victoryText.transform.position;
        position.x = -1080f;
        victoryText.transform.position = position;
        victoryText.text = "Victory!";
        victoryText.gameObject.SetActive(true);
        Sequence seq = DOTween.Sequence();

        seq.Append(victoryText.transform.DOLocalMoveX(0, 0.2f).SetEase(Ease.Linear))
            .Append(victoryText.transform.DOLocalMoveX(1080f, 0.4f).SetEase(Ease.Linear).SetDelay(3f)).OnComplete(() => Destroy(victoryText.gameObject));
    }

    public void SpawnMergeFirework(Vector3 position)
    {
        Vector3 gridPosition = position;
        ParticleSystem firework = Instantiate(_mergeFireworkParticle, gridPosition, Quaternion.identity);
        firework.Play();
    }

    public void ShowComboImage(int comboCount)
    {
        if (comboCount < 2) return;
        MoveOtherComboGameObject();

        Sequence seq = DOTween.Sequence();
        ComboTextData data = GetComboTextData(comboCount);
        Image comboImage = SpawnImage(data);

        seq.Append(comboImage.transform.DOLocalMoveX(0, 0.2f).SetEase(Ease.Linear))
            .Append(comboImage.transform.DOLocalMoveX(1080f, 0.4f).SetEase(Ease.Linear).SetDelay(3f)).
            OnComplete(() => DestroyComboGameObject(comboImage.gameObject));

    }

    private Image SpawnImage(ComboTextData data)
    {
        Image comboImage = Instantiate(_comboImage, _comboImage.transform.localPosition, _comboImage.transform.localRotation, transform);
        Vector3 position = comboImage.transform.localPosition;
        position.x = -1080f;
        position.y = 0;
        comboImage.transform.localPosition = position;
        comboImage.sprite = data.comboSprite;
        comboImage.gameObject.SetActive(true);
        _instantiatedComboObjects.Add(comboImage.gameObject);
        return comboImage;
    }
}



[System.Serializable]
public class ComboTextData
{
    public string comboMessage;
    public Color color;
    public Sprite comboSprite;
}