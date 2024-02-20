using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class BurstHandler : Fruit
{
    public string fruitName = "Fruit";

    public float splashOffset = 1f;
    public GameObject splashParticle;
    public GameObject splashImage;
    public Transform spawnPoint;
    public List<DetectCollision> wallTouchings = new List<DetectCollision>();
    public List<DetectCollision> handTouchings = new List<DetectCollision>();
    private float enterTime;
    float myBurstDuration = 0f;
    bool isBursted;
    DetectCollision[] childCollisionDetectors;
    private void OnEnable()
    {
        childCollisionDetectors = GetComponentsInChildren<DetectCollision>();
        wallTouchings = new List<DetectCollision>();
        handTouchings = new List<DetectCollision>();
        isBursted = false;
        enterTime = Time.time;
        myBurstDuration = BurstManager.Instance.CurrentBurstDuration;
    }
    private void Update()
    {
        if (isBursted) return;
        if (wallTouchings.Count > 0 && handTouchings.Count > 0)
        {
            if (Time.time - enterTime >= myBurstDuration)
            {
                ReadyToBurst();
                //Burst();
            }
        }
        else
        {
            enterTime = Time.time;
        }
    }
    public void ReadyToBurst()
    {
        isBursted = true;
        BurstManager.Instance.AddToFruitQueue(this); // Add the fruit to the bursting queue
    }
    public void AddToHandTouch(DetectCollision dc)
    {
        if (!handTouchings.Contains(dc))
        {
            handTouchings.Add(dc);
        }
    }
    public void RemoveFromHandTouch(DetectCollision dc)
    {
        if (handTouchings.Contains(dc))
        {
            handTouchings.Remove(dc);
        }
    }
    public void Burst()
    {
        PoolManager.Instance.DequeueFromPool(splashParticle.name, spawnPoint.position + new Vector3(0, splashOffset, 0), Quaternion.identity);
        PoolManager.Instance.DequeueFromPool(splashImage.name, new Vector3(spawnPoint.position.x, 0.01f, spawnPoint.position.z), splashImage.transform.rotation);
        
        SpawnController.Instance.RemoveFruit(this.gameObject);
        IncreaseFruitAmount();
        Destroy(gameObject);
        //gameObject.SetActive(false);

        //StartCoroutine(BurstR());
    }

    IEnumerator BurstR()
    {
        PoolManager.Instance.DequeueFromPool(splashParticle.name, spawnPoint.position + new Vector3(0, splashOffset, 0), Quaternion.identity);
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        SpawnController.Instance.RemoveFruit(this.gameObject);
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
                                
        IncreaseFruitAmount();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        Destroy(gameObject);
    }
}
