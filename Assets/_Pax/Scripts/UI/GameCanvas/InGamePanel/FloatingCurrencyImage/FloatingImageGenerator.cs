namespace Pax
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using System;
    using TMPro;


    public class FloatingImageGenerator : MonoBehaviour
    {
        [HideInInspector] public Queue<GameObject> _floatingObjectsPool;
        [SerializeField] GameObject _floatingGameObject;
        [SerializeField] int _poolSize;
        [SerializeField] RectTransform _canvasRectTransform;

        int _floatingObjectCount = 0;
        Sprite _floatingSprite;
        Image _floatingImage;
        CurrencyTextController _currenyTextController;

        [SerializeField] Camera _uiCamera;

        void OnEnable()
        {
            MainManager.Instance.EventManager.Register(EventTypes.CurrencyEarned, GenerateFloatingImagesFromWorld);
        }

        void OnDisable()
        {
            MainManager.Instance.EventManager.Unregister(EventTypes.CurrencyEarned, GenerateFloatingImagesFromWorld);
        }

        void Start()
        {
            _uiCamera = GameObject.FindGameObjectWithTag("UICamera").transform.GetComponent<Camera>();
            _floatingObjectsPool = new Queue<GameObject>();
            _floatingImage = transform.GetChild(0).GetComponent<Image>();
            _currenyTextController = transform.GetChild(1).GetComponent<CurrencyTextController>();
            _floatingGameObject.GetComponent<Image>().sprite = _floatingImage.sprite;

            for (int i = 0; i < _poolSize; i++)
            {
                GameObject instantiatedFloatingGameObject = Instantiate(_floatingGameObject, transform);
                instantiatedFloatingGameObject.SetActive(false);
                _floatingObjectsPool.Enqueue(instantiatedFloatingGameObject);
            }
        }

        void GenerateFloatingImagesFromWorld(EventArgs args)
        {
            SpawnArgs spawnArgs = args as SpawnArgs;

            Vector3 spawnPositionWorldPosition = spawnArgs.SpawnPosition;
            int spawnCount = spawnArgs.SpawnCount;


            for (int i = 0; i < spawnCount; i++)
            {
                GameObject obj = _floatingObjectsPool.Dequeue();
                obj.transform.SetParent(_canvasRectTransform);

                RectTransform imageRectTransform = obj.GetComponent<RectTransform>();
                FloatingImage floatingImage = obj.GetComponent<FloatingImage>();

                Vector3 screenPosition = Camera.main.WorldToScreenPoint(spawnPositionWorldPosition); //screen-space overlay deki screen pozisyonu almak gerekiyor asagidaki fonksiyon onu screen space camera daki konuma ceviriyor
                Vector2 canvasPosition;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRectTransform, screenPosition, _uiCamera, out canvasPosition); // screen-space overlay deki fonksiyonu screen space camera ya ceviriyor
                imageRectTransform.anchoredPosition = canvasPosition;
                obj.SetActive(true);
                floatingImage.MoveToTarget(_floatingImage.transform, () => _currenyTextController.AddCurrency(1));
                _floatingObjectsPool.Enqueue(obj);
            }
        }

        void GenerateFloatingImagesFromCanvas(EventArgs args)
        {
            SpawnArgs spawnArgs = args as SpawnArgs;

            Vector3 spawnPositionRectPosition = spawnArgs.SpawnPosition;
            int spawnCount = spawnArgs.SpawnCount;

            for (int i = 0; i < _floatingObjectCount; i++)
            {
                GameObject obj = _floatingObjectsPool.Dequeue();
                obj.transform.SetParent(_canvasRectTransform);
                RectTransform imageRectTransform = obj.GetComponent<RectTransform>();
                imageRectTransform.position = spawnPositionRectPosition;
                obj.SetActive(true);
                _floatingObjectsPool.Enqueue(obj);
            }
        }
    }
}