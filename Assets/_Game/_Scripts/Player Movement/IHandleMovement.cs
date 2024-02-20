using UnityEngine;
/// <summary>
/// Represents an object that can handle movement.
/// </summary>
public interface IHandleMovement
{
    /// <summary>
    /// Gets the current movement information.
    /// </summary>
    MovementInfo Info { get; }

    SpeedDataHandler SpeedDataHandler { get; set; }

    /// <summary>
    /// Gets and sets the current Rigidbody information.
    /// </summary>
    Rigidbody Body { get; set; }

    /// <summary>
    /// Gets and sets the speed multiplier amount.
    /// </summary>
    float SpeedMultiplier { get; set; }

    /// <summary>
    /// Makes the object move.
    /// </summary>
    void Move(Vector3 direction);

    /// <summary>
    /// Makes the object rotate.
    /// </summary>
    void Rotate(Vector3 direction);

    /// <summary>
    /// Makes the object freeze.
    /// </summary>
    void ResetMovement();
}