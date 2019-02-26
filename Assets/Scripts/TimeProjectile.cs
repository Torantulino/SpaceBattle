using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple projectile destroyed after some time.
/// </summary>
public class TimeProjectile : Projectile {

    [Tooltip("Time after which this projectile is destroyed.")]
    public float lifetime;

    // Use this for initialization
    new void Start()
    {
        base.Start();

        GetComponent<Rigidbody>().AddForce(transform.forward * 10f, ForceMode.Impulse);
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        lifetime -= Time.deltaTime;

        //todo not synchronized Destroy
        if (lifetime <= 0f)
        {
            Destroy(gameObject);
        }
    }

    protected override void OnDamage(Part part)
    {
        base.OnDamage(part);

    }
}
