using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiObjectCameraController : MonoBehaviour
{
    public List<Transform> objectsToFollow; // The MotherLand script that holds the list of Land objects.
    public float smoothTime = 0.3f; // The smooth time for the camera movement.
    public float minZoom = 40f; // The minimum FoV for the camera.
    public float maxZoom = 10f; // The maximum FoV for the camera.
    public float zoomLimiter = 50f; // The maximum distance between objects to consider for FoV calculation.
    public Vector3 offSet = new Vector3(0, 20, -10); // The following offset for the camera.
    public float fovChangeSpeed = 1.5f;

    private Vector3 velocity; // The velocity of the camera movement.
    private Camera cam; // The camera component.

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void FixedUpdate()
    {
        if (objectsToFollow == null) return;

        // Calculate the center point of all the followed objects
        Vector3 centerPoint = GetCenterPoint();

        // Smoothly move the camera towards the center point
        transform.position = Vector3.SmoothDamp(transform.position, centerPoint, ref velocity, smoothTime);

        // Calculate the new FoV based on the distance between the objects
        float newZoom = CalculateZoom();
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime * fovChangeSpeed);
    }

    // Calculates the center point of all the followed objects
    Vector3 GetCenterPoint()
    {
        // Calculate the center point of all the followed objects
        Vector3 centerPoint = Vector3.zero;
        if (objectsToFollow.Count == 1)
        {
            centerPoint = objectsToFollow[0].transform.position;
        }
        else
        {
            var bounds = new Bounds(objectsToFollow[0].transform.position, Vector3.zero);
            for (int i = 0; i < objectsToFollow.Count; i++)
            {
                bounds.Encapsulate(objectsToFollow[i].transform.position);
            }
            centerPoint = bounds.center;
        }

        // Add the offset to the center point
        centerPoint += offSet;

        return centerPoint;
    }

    // Calculates the new FoV based on the distance between the objects
    float CalculateZoom()
    {
        float newZoom = minZoom;
        float distance = GetGreatestDistance();
        if (distance > zoomLimiter)
        {
            newZoom = maxZoom;
        }
        else
        {
            newZoom = distance + minZoom;
        }
        return newZoom;
    }

    // Calculates the greatest distance between the followed objects
    float GetGreatestDistance()
    {
        var bounds = new Bounds(objectsToFollow[0].transform.position, Vector3.zero);
        for (int i = 0; i < objectsToFollow.Count; i++)
        {
            bounds.Encapsulate(objectsToFollow[i].transform.position);
        }
        return bounds.size.x;
    }
}
