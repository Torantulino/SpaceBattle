using UnityEngine;

/// <summary>
/// Weapon Part.
/// </summary>
public class Weapon : Part
{
    [Tooltip("GameObject for weapon barrel.")]
    public Transform gunTransform;
    [Tooltip("Bullet prefab - must be registered in the Network manager.")]
    public GameObject bulletPrefab;

	// Use this for initialization
	new void Start () {
		base.Start();

        //todo warn if there's no gun transform or no bullet prefab
	}
	
	// Update is called once per frame
	new void Update () {
		base.Update();

        //todo upgrade
        gunTransform.localEulerAngles = playerShip.Target;
	}

    /// <summary>
    /// If weapon is ready to shoot - it's aiming properly and there are no other parts on the way.
    /// </summary>
    public bool Ready()
    {
        return true;
    }
}
