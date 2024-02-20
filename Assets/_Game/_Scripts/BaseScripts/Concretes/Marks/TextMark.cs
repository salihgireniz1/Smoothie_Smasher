using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextMark : Mark
{
    public string text { get; set; }
    private Camera _camera;
    [SerializeField] private TextMeshProUGUI _text;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void LateUpdate()
    {

        Vector3 relativePosition = transform.position - _camera.transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(relativePosition, Vector3.up);

        Vector3 euler = lookRotation.eulerAngles;
        euler.y = 0;
        lookRotation.eulerAngles = euler;
        transform.rotation = lookRotation;

    }
    public override void PlayAnimation()
    {
        throw new System.NotImplementedException();
    }

    public override void SetMark(float value, string text)
    {
        _text.fontSize = value;
        _text.text = text;
    }
}
