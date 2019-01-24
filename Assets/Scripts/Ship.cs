using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ship : Unit {

    private Rigidbody body;

    new void Start()
    {
        base.Start();
        body = GetComponent<Rigidbody>();
    }

    public void Thrust(Vector3 force)
    {
        body.AddForce(force * body.mass);
    }

}
