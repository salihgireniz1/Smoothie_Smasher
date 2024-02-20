using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DirectionController : MonoBehaviour
{
    public PlayerController controller;

    Transform closestFruit;
    Vector3 direction;
    BurstHandler[] fruits = null;
    public GameObject pushRight;
    public GameObject pushLeft;
    private void OnEnable()
    {

        fruits = FindObjectsOfType<BurstHandler>();
        FindClosestFruit();

        if (controller != null)
        {
            controller.isOverrided = true;
        }
    }
    private void OnDisable()
    {
        if(controller != null)
        {
            controller.isOverrided = false;
            //arrow.gameObject.SetActive(false);
        }
    }
    public void SwitchDirectionUI()
    {
        if (pushRight.activeInHierarchy)
        {
            pushRight.SetActive(false);
            pushLeft.SetActive(true);
        }
        else
        {
            pushRight.SetActive(true);
            pushLeft.SetActive(false);
        }
        fruits = FindObjectsOfType<BurstHandler>();
        FindClosestFruit();
    }
    public void FindClosestFruit()
    {
        float d = Mathf.Infinity;
        foreach (var fruit in fruits)
        {
            float distance = Vector3.Distance(controller.transform.position, fruit.transform.position);
            if (distance < d && distance > 4f)
            {
                d = distance;
                closestFruit = fruit.transform;
            }
        }
    }
    private void Update()
    {
        if (controller == null) return;
        if (closestFruit == null) return;

        if (Vector3.Distance(controller.transform.position, closestFruit.transform.position) < 4f)
        {
            FindClosestFruit();
        }

        direction = closestFruit.position - controller.transform.position;
        direction.y = 0;
        controller.overridedDirection = direction.normalized;
    }
}
