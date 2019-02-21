using UnityEngine;

/// <summary>
/// Controls a projectile.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class ProjectileController : MonoBehaviour
{
    [Tooltip("Time after this projectile is destroyed.")]
    public float lifetime;

    // Use this for initialization
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * 10f, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;

        //todo not synchronized Destroy
        if (lifetime <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
