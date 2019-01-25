using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Unit : Destructible
{
    public Transform aimTransform;

    [SerializeField]//todo should list weapons automatically
    protected List<Weapon> weapons;
    
    #region Properties

    /// <summary>
    /// The direction of aiming relative to Unit's transform.rotation.
    /// </summary>
    public Vector3 Target
    {
        get { return aimTransform.localEulerAngles; }
        set { aimTransform.localEulerAngles = value; }
    }

    #endregion

    // Use this for initialization
    public new void Start () {
		base.Start();

        // Making sure that there's an Aim GameObject
        if (!aimTransform)
        {
            if (!(aimTransform = transform.Find("Aim")))
            {
                aimTransform = Instantiate(new GameObject("Aim"), transform.position, transform.rotation, transform).transform;
            }
            GetComponent<NetworkTransformChild>().target = aimTransform;
        }

	}

    public void Shoot()
    {
        //todo should do checks on the client side before sending Command
        CmdShoot();
    }

    [Command]
    public void CmdShoot()
    {
        foreach (Weapon weapon in weapons)
        {
            // Is the weapon ready to shoot
            if (!weapon.Ready())
            continue;
            // Instantiate GameObject
            GameObject shot = Instantiate(weapon.bulletPrefab, weapon.gunTransform.position, weapon.gunTransform.rotation);
            // Spawn it - so it appears for all clients
            NetworkServer.Spawn(shot);
            // Destroy it after some time
            Destroy(shot, 5f);
        }       
    }

}
