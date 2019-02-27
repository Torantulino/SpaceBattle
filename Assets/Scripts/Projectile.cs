using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Base class for all projectiles.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public abstract class Projectile : NetworkBehaviour
{
    [SerializeField]
    protected float damage;

    [SerializeField]
    protected GameObject destroyParticles;

    //todo temporary, time after this projectile will damage anything (including weapon that is firing it!)
    private float _time = .5f;

    // Use this for initialization
    protected void Start()
    {

    }

    // Update is called once per frame
    protected void Update()
    {
        if (_time > 0f)
            _time -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (_time > 0f)
            return;

        Part part;

        if ((part = collision.gameObject.GetComponentInParent<Part>()) != null)
        {
            // Handle damage only on server
            if (isServer)
            {
                part.Damage(damage);
                OnDamage(part);
            }

        }

        //todo instantiate particles
        Destroy(gameObject);
    }

    /// <summary>
    /// Called when collision with a Part occured, just after damage is applied.
    /// </summary>
    /// <param name="part">Collided Part</param>
    protected virtual void OnDamage(Part part) { }


}
