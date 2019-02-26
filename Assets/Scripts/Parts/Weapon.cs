using UnityEngine;

/// <summary>
/// Weapon Part.
/// </summary>
public class Weapon : Part
{
    [Tooltip("GameObject for weapon barrel.")]
    public Transform gunTransform;
    [Tooltip("Bullet prefab - must be registered in the Network manager.")]
    public GameObject bulletPrefab;//todo should be registered automatically in Unit
    [Tooltip("Minimum time between consecutive shots.")]
    public float cooldown = 1f;
    [Tooltip("Weapon won't change aiming direction.")]
    public bool fixedDirection;

    private float timeLeft;// For calculating cooldown

    // Use this for initialization
    new void Start () {
		base.Start();

        if(!gunTransform || !bulletPrefab)
            Debug.LogWarning("Weapon has no gunTransform and/or bulletPrefab attached.");

        timeLeft = cooldown;
    }
	
	// Update is called once per frame
	new void Update () {
		base.Update();

        // Aiming
        if(!fixedDirection)
            gunTransform.eulerAngles = playerShip.transform.eulerAngles + playerShip.Target;

        // Cooldown	    
	    if (timeLeft > 0f) timeLeft -= Time.deltaTime;
    }

    /// <summary>
    /// Tries to shoot. Should only be called from server.
    /// </summary>
    /// <returns>Instantiated projectile GameObject or null if shooting was unsuccessful.</returns>
    public GameObject Shoot()
    {
        if (!Ready())
            return null;

        timeLeft = cooldown;
        GameObject shot = Instantiate(bulletPrefab, gunTransform.position, gunTransform.rotation);
        //todo that isn't networked
        shot.GetComponent<Rigidbody>().velocity = playerShip.GetComponent<Rigidbody>().velocity;

        return shot;
    }

    /// <summary>
    /// If weapon is ready to shoot (proper aim, no parts on the way, cooled down).
    /// </summary>
    public bool Ready()
    {
        if (timeLeft <= 0f)
        {
            return true;
        }

        return false;
    }
}
