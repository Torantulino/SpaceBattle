using UnityEngine;

/// <summary>
/// Controls a projectile.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class ProjectileController : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * 10f, ForceMode.Impulse);
    }

}
