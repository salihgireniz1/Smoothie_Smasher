using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateUIMoney : MonoBehaviour
{
    [HideInInspector] public Queue<GameObject> moneyInPool;
    [SerializeField] private GameObject _moneyPrefab;
    [SerializeField] private int _poolSize;
    [SerializeField] private ScorePanel _scorePanel;
    [SerializeField] private Camera _camera;

    private int multipleCoinSize = 6;
    private int moneyAmount;

    private void OnEnable()
    {
        EventManagement.OnCollectMoneyUI += GenerateCoin;
    }
    private void OnDisable()
    {
        EventManagement.OnCollectMoneyUI -= GenerateCoin;
    }

    private void Awake()
    {
        moneyInPool = new Queue<GameObject>();
        //_moneyPrefab.GetComponent<Image>().sprite = _scorePanel.Settings.CollectibleSprite;

        for (int i = 0; i < _poolSize; i++)
        {
            GameObject willCreateCoin = Instantiate(_moneyPrefab, transform);
            willCreateCoin.SetActive(false);
            moneyInPool.Enqueue(willCreateCoin);
        }
    }

    public void GenerateCoin(Vector3 generatePoint, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            //Debug.Log(generatePoint);
            GameObject obj = moneyInPool.Dequeue();
            obj.SetActive(true);
            //obj.transform.position = _camera.WorldToScreenPoint(generatePoint);
            obj.transform.position = generatePoint;
            //obj.transform.GetComponent<RectTransform>().position = generatePoint;
            moneyInPool.Enqueue(obj);
        }
    }
}
