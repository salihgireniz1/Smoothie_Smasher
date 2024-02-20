using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetParticle : MonoBehaviour
{
    public RectTransform myRectTransform;
    public RectTransform targetRectTransform;
    public RectTransform cameraCanvas;
    public Camera particleCamera;

    private void Awake()
    {
        myRectTransform = GetComponent<RectTransform>();
      
    }

    private void Start()
    {

        //RectTransform overlayChildRectTransform = targetRectTransform.GetComponent<RectTransform>();
        //Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, overlayChildRectTransform.position);
        //Vector3 worldPoint;
        //RectTransform cameraCanvasRectTransform = cameraCanvas.GetComponent<RectTransform>();
        //if (RectTransformUtility.ScreenPointToWorldPointInRectangle(cameraCanvasRectTransform, screenPoint, particleCamera, out worldPoint))
        //{
        //    // Set the position of the child object in the Camera canvas to the world point.
        //    transform.position = worldPoint;
        //}
       
    }
}
