using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private Vector3 finalRotation = Vector3.zero;
    private Vector3 targetPosition;

    [Button]
    public void MoveAndRotateTowards(Vector3 target)
    {
        targetPosition = target;
        targetPosition.y = transform.position.y;
    }

    private void Update()
    {
        if (targetPosition != Vector3.zero)
        {
            // Calculate the direction to the target
            Vector3 direction = (targetPosition - transform.position).normalized;

            // Calculate the rotation towards the target
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Smoothly rotate towards the target
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Move towards the target
            transform.position += direction * movementSpeed * Time.deltaTime;

            // Check if we are close to the target, then stop moving
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                targetPosition = Vector3.zero;
                transform.DORotate(finalRotation, .75f);
            }
        }
    }
}
