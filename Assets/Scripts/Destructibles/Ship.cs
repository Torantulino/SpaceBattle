using UnityEngine;

/// <summary>
/// Class for all Units that can move.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Ship : Unit {

    

    // Use this for initialization
    new void Start()
    {
        base.Start();

    }

    /// <summary>
    /// Tries to use engines of the Ship.
    /// </summary>
    public void Thrust(Vector3 direction)
    {
        body.AddRelativeForce(power * direction.normalized, ForceMode.Force);
    }

    /// <summary>
    /// Rotates the Ship on the Z axis.
    /// Prevents Ship from rotating too fast.
    /// </summary>
    public void Roll(float degrees)
    {
        // Clamping degrees so Ship doesn't rotate too fast
        degrees = Mathf.Clamp(degrees, -180f, 180f);
        transform.Rotate(new Vector3(0f, 0f, degrees) * Time.fixedDeltaTime);
        //todo upgrade Roll
        //Quaternion rotation = transform.rotation * Quaternion.Euler(0f, 0f, degrees);
        //body.MoveRotation(rotation);
    }

}
