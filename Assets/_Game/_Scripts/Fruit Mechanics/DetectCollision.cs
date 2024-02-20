using UnityEngine;

public class DetectCollision : MonoBehaviour
{
    public int wallLayer;
    public int handLayer;
    public int ballLayer;
    BurstHandler burstHandler;
    private void Awake()
    {
        burstHandler = GetComponentInParent<BurstHandler>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            burstHandler.AddToHandTouch(this);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            burstHandler.RemoveFromHandTouch(this);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == wallLayer)
        {
            burstHandler.wallTouchings.Add(this);
        }
        /*if (collision.gameObject.layer == handLayer || collision.gameObject.layer == ballLayer)
        {
            //burstHandler.handTouchings.Add(this);
            burstHandler.AddToHandTouch(this);
        }*/
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == wallLayer)
        {
            burstHandler.wallTouchings.Remove(this);
        }
        /*if (collision.gameObject.layer == handLayer || collision.gameObject.layer == ballLayer)
        {
            //burstHandler.handTouchings.Remove(this);
            burstHandler.RemoveFromHandTouch(this);
        }*/
    }
}
