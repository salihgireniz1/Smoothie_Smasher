using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(IHandleMovement))]
public class PlayerController : MonoBehaviour
{
    public bool CanMove;
    public VariableJoystick Joystick;
    public Transform PlayerBody;
    public float raycastDistance = 2.0f; // Adjust the distance of the raycast
    public LayerMask wallLayer; // Assign the "Wall" layer to this in the Inspector

    IHandleMovement movementRespond;
    Vector3 direction = Vector3.zero;
    public ParticleSystem BoosterParticle;

    private void Awake()
    {
        movementRespond = GetComponent<IHandleMovement>();
        movementRespond.Body = GetComponent<Rigidbody>();
        movementRespond.Body.centerOfMass = PlayerBody.position;
        movementRespond.SpeedDataHandler = GetComponent<SpeedDataHandler>();
    }
    public void PlayBoosterParticle()
    {
        BoosterParticle.Play();
    }
    public void StopBoosterParticle()
    {
        BoosterParticle.Stop();
    }

    public void DisableMovementAbility()
    {
        CanMove = false;
        movementRespond.Body.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
    }

    public void EnableMovementAbility()
    {
        CanMove = true;
        movementRespond.Body.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
    }

    private void FixedUpdate()
    {
        if (CanMove)
        {
            direction = GetDirection();
            movementRespond.Move(direction);
            //RotatePlayer();
            movementRespond.Rotate(direction);
        }
        else
        {
            movementRespond.ResetMovement();
        }
    }
    public bool isOverrided;
    public Vector3 overridedDirection;
    Vector3 GetDirection()
    {
        if (isOverrided)
        {
            return overridedDirection;
        }
        return new Vector3(Joystick.Horizontal, 0f, Joystick.Vertical);
    }
    public bool isCollidingWall;
    Vector3 stickedDir = Vector3.zero;
    void RotatePlayer()
    {
        // Create a ray from the player's position in the forward direction
        Ray ray = new Ray(transform.position, transform.forward);

        // Check if the ray hits something on the "Wall" layer
        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, wallLayer))
        {
            if(stickedDir == Vector3.zero || Mathf.Abs(stickedDir.x) == Mathf.Abs(stickedDir.z))
            {
                isCollidingWall = true;
                stickedDir = direction;
                stickedDir = new Vector3(Mathf.RoundToInt(stickedDir.x), 0f, Mathf.RoundToInt(stickedDir.z));
            }
            movementRespond.Rotate(stickedDir);
        }
        else
        {
            isCollidingWall = false;
            stickedDir = Vector3.zero;
            movementRespond.Rotate(direction);
        }
    }
}
