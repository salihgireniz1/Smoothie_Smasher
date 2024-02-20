using UnityEngine;

public class RotateTowardsVelocity : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5f;

    private Rigidbody rb;


    private void Update()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
            return;
        }
        RotateObjectTowardsVelocity();
    }

    private void RotateObjectTowardsVelocity()
    {
        if (rb.velocity.magnitude > 0.5f)
        {
            // Get the current forward direction of the object.
            Vector3 currentForward = transform.forward;

            // Calculate the rotation direction from the current forward direction to the velocity direction.
            Quaternion targetRotation = Quaternion.FromToRotation(currentForward, rb.velocity.normalized) * transform.rotation;

            // Use Slerp to smoothly interpolate towards the target rotation.
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
