using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateUILevelProgress : MonoBehaviour
{
    [HideInInspector] public Queue<GameObject> moneyInPool;
    [SerializeField] private GameObject _moneyPrefab;
    [SerializeField] private int _poolSize;
    [SerializeField] private ScorePanel _scorePanel;

    private void OnEnable()
    {
        EventManagement.OnCollectPlayerLevelUI += GenerateCoin;
    }
    private void OnDisable()
    {
        EventManagement.OnCollectPlayerLevelUI -= GenerateCoin;
    }

    private void Awake()
    {
        moneyInPool = new Queue<GameObject>();

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
            GameObject obj = moneyInPool.Dequeue();
            obj.SetActive(true);
            //obj.transform.position = Camera.main.WorldToScreenPoint(generatePoint);
            obj.transform.position = generatePoint;
            //obj.transform.GetComponent<RectTransform>().position = generatePoint;
            moneyInPool.Enqueue(obj);
        }
    }
}
