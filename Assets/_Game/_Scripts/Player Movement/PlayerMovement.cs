using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IHandleMovement
{
    #region Properties
    public float RewardSpeed { get; private set; } = 1f;
    public MovementInfo Info { get => info; }
    public SpeedDataHandler SpeedDataHandler
    {
        get => speedDataHandler;
        set => speedDataHandler = value;
    }
    public Rigidbody Body 
    { 
        get => body; 
        set => body = value; 
    }
    public float SpeedMultiplier { get => speedMultiplier; set => speedMultiplier = value; }
    #endregion

    #region Variables

    // You can reduce this parameter to smooth the movement over multiple time steps,
    // to help reduce the effect of sudden jerks.
    [SerializeField]
    private float catchUp;

    [SerializeField]
    private MovementInfo info;

    [SerializeField]
    private Animator playerCharacterAnimator;

    [SerializeField]
    private float speedMultiplier = 1f;

    [SerializeField]
    SpeedDataHandler speedDataHandler;

    private Rigidbody body;
    float angle;
    Vector3 axis;

    #endregion

    #region Methods
    public void GetSpeedReward(float ratio, float time)
    {
        StartCoroutine(RewardSpeedRoutine(ratio, time));
    }
    IEnumerator RewardSpeedRoutine(float ratio, float time)
    {
        RewardSpeed *= ratio;
        yield return new WaitForSeconds(time);
        RewardSpeed = 1f;
    }
    public void Move(Vector3 direction)
    {
        body.velocity = direction * speedDataHandler.CurrentSpeed * RewardSpeed * Time.fixedDeltaTime;
    }
    private void LateUpdate()
    {
        playerCharacterAnimator.speed = body.velocity.magnitude / 8f;
    }
    public void Rotate(Vector3 direction)
    {
        if (direction.magnitude >= 0.1f)
        {
            // Define change amount of rotation.
            Quaternion rotationChange = Quaternion.LookRotation(direction) * Quaternion.Inverse(body.rotation);

            // Convert to an angle-axis representation.
            rotationChange.ToAngleAxis(out angle, out axis);

            // Change the range of returned angle to fit it between 0 and 360 degrees.
            if (angle > 180f)
                angle -= 360f;

            angle *= Mathf.Deg2Rad;

            // Compute an angular velocity that will bring us to the target orientation
            // in a single time step.
            var angularVelocity = axis * angle / Time.fixedDeltaTime * info.rotationSpeed * SpeedDataHandler.CurrentRot;

            Vector3 torqueAmount = catchUp * angularVelocity - (body.angularVelocity / 2f);

            // Apply a torque to finish the job.
            if (torqueAmount.magnitude > 0.1f)
            {
                body.AddTorque(torqueAmount, ForceMode.VelocityChange);
            }
        }
    }

    public void ResetMovement()
    {
        playerCharacterAnimator.speed = 0f;
        if (body.velocity != Vector3.zero)
        {
            body.velocity = Vector3.zero;
            playerCharacterAnimator.speed = 0f;
        }
        if(body.angularVelocity != Vector3.zero)
        {
            body.angularVelocity = Vector3.zero;
        }
    }
    #endregion
}