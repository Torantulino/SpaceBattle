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

    // Use this for initialization
    new void Start () {
		base.Start();

        if(!gunTransform || !bulletPrefab)
            Debug.LogWarning("Weapon has no gunTransform and/or bulletPrefab attached.");
	}
	
	// Update is called once per frame
	new void Update () {
		base.Update();

        gunTransform.eulerAngles = playerShip.transform.eulerAngles + playerShip.Target;
	}

    /// <summary>
    /// If weapon is ready to shoot - it's aiming properly and there are no other parts on the way.
    /// </summary>
    public bool Ready()
    {
        return true;
    }
}
