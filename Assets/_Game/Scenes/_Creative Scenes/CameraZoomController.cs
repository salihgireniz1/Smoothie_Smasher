using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomController : MonoBehaviour
{
    public float startZoom;
    public float endZoom;
    public float waitDuration;
    public float zoomDuration;
    Camera cam;
    
    private void Awake()
    {
        cam = GetComponent<Camera>();
    }
    [Button]
    public void StartZooming()
    {
        StartCoroutine(ZoomRoutine());
    }
    IEnumerator ZoomRoutine()
    {
        cam.orthographicSize = startZoom;
        yield return new WaitForSeconds(waitDuration);
        cam.DOOrthoSize(endZoom, zoomDuration);
    }
}
