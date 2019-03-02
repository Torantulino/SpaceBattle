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

}
