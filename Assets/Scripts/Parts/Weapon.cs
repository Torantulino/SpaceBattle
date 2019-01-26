using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Part
{
    public Transform gunTransform;
    public GameObject bulletPrefab;

	// Use this for initialization
	new void Start () {
		base.Start();

	}
	
	// Update is called once per frame
	new void Update () {
		base.Update();

        //todo upgrade
        gunTransform.localEulerAngles = playerShip.Target;
	}

    /// <summary>
    /// If weapon is ready to shoot - it's aiming properly and there's no other parts on the way.
    /// </summary>
    public bool Ready()
    {
        return true;
    }
}
