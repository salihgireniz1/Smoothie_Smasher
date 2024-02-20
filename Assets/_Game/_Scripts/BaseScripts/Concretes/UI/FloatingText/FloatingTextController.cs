using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PAG.Utility;
public class FloatingTextController : MonoSingleton<FloatingTextController>
{
    [SerializeField] private GameObject _floatingTextPrefab;
    [SerializeField] private Sprite _goldSprite, _fruitSprite;

    private Transform _floatingTextCanvasTransform;
    private Camera _camera;
    NumberFormatManager numberFormatManager = new();

    private void Awake()
    {
        _camera = Camera.main;
        _floatingTextCanvasTransform = transform;
    }

    public void SpawnFloatingText(Vector3 spawnPosition, float value, SpriteType spriteType, bool onlySprite)
    {

        Vector3 worldPoint = spawnPosition;
        //Debug.Log(spawnPosition);
        float offSet = .1f;
        worldPoint = new Vector3(worldPoint.x, worldPoint.y + offSet, worldPoint.z);
        //Vector3 screenPoint = _camera.WorldToScreenPoint(worldPoint);
        GameObject moneyUI = Instantiate(_floatingTextPrefab, worldPoint, Quaternion.identity, _floatingTextCanvasTransform) as GameObject;
        Image image = moneyUI.GetComponentInChildren<Image>();

        if (spriteType == SpriteType.Gold)
        {
            image.sprite = _goldSprite;
        }
        else if (spriteType == SpriteType.Fruit)
        {
            image.sprite = _fruitSprite;
        }
        TextMeshProUGUI text = moneyUI.GetComponentInChildren<TextMeshProUGUI>();
        if (!onlySprite)
        {
            text.text = "-" + numberFormatManager.FormatNumber(-value);
                //value.ToString();
        }
        else
        {
            text.gameObject.SetActive(false);
        }
        Destroy(moneyUI, 3.2f);
    }
}

public enum SpriteType
{
    Gold,
    Fruit
}
