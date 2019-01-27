using UnityEngine;

/// <summary>
/// Class for all Units that can move.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Ship : Unit {

    private Rigidbody body;

    // Use this for initialization
    new void Start()
    {
        base.Start();

        body = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Tries to use engines of the Ship.
    /// </summary>
    public void Thrust(Vector3 force)
    {
        body.AddForce(force * body.mass);
    }

}
