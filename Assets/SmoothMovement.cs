using UnityEngine;

public class SmoothMovement : MonoBehaviour
{
    public Transform target;
    public float moveSpeed = 5f;
    public float destroyDistanceThreshold = 0.1f;

    private Vector3 initialPosition;
    private Vector3 intermediateDestination;
    private float startTime;
    private bool reachedIntermediateDestination;

    private void Start()
    {
        initialPosition = transform.position;
        SetRandomIntermediateDestination();
        startTime = Time.time;
    }

    private void Update()
    {
        if (target == null)
            return;

        Vector3 direction = target.position - transform.position;
        float distance = direction.magnitude;

        if (distance <= destroyDistanceThreshold)
        {
            //Destroy(gameObject);
            PoolManager.Instance.EnqueueToPool(gameObject);
            return;
        }

        // Calculate the time since the start of the movement
        float elapsedTime = Time.time - startTime;

        // Calculate the horizontal movement towards the current destination
        float step = moveSpeed * Time.deltaTime;
        Vector3 targetPosition;
        if (reachedIntermediateDestination)
        {
            targetPosition = Vector3.MoveTowards(transform.position, target.position, step);
        }
        else
        {
            targetPosition = Vector3.MoveTowards(transform.position, intermediateDestination, step);
            if (transform.position == intermediateDestination)
                reachedIntermediateDestination = true;
        }

        // Move the object to the new position
        transform.position = targetPosition;
    }

    private void SetRandomIntermediateDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 2f;
        randomDirection = new Vector3(randomDirection.x, Mathf.Abs(randomDirection.y), randomDirection.z);
        intermediateDestination = initialPosition + randomDirection;
        reachedIntermediateDestination = false;
    }
}
