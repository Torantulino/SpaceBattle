using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Unit : Destructible
{
    public Transform aimTransform;

    protected List<PartData> parts = new List<PartData>();
    protected List<Weapon> weapons = new List<Weapon>();

    public event EventHandler<EventArgs> PartsChanged;

    #region Properties

    /// <summary>
    /// The direction of aiming relative to Unit's local rotation. Weapons will try to match to this rotation.
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

        PartsChanged += OnPartsChanged;

        // Only on server
        if (!isServer)
            return;

        //todo test part
        parts.Add(new PartData(0, new Vector3(0, 1, 0)));
    }

    /// <summary>
    /// Tries to shoot from all weapons
    /// </summary>
    public void Shoot()
    {
        //todo should do checks on the client side before sending Command
        CmdShoot();
    }

    /// <summary>
    /// Just refreshes parts List
    /// </summary>
    public void RefreshParts()
    {
        // We can't just instantiate our parts, we need to load informations about them from the server first
        // Send command to server to refresh parts list
        CmdRefreshParts();
    }

    /// <summary>
    /// Rebuilds game objects from prefabs. Is called automatically when parts are refreshed.
    /// </summary>
    private void RebuildParts()
    {
        // Remove all children
        //todo remove Canvas if no longer needed
        transform.DestroyChildren("Aim", "Canvas");
        // Loop through parts and instantiate them
        // Note: Those parts are only created locally
        foreach (PartData part in parts)
        {
            Instantiate(PartController.Instance.GetPartById(part.id).prefab, transform.position + part.position,
                Quaternion.Euler(transform.localEulerAngles + part.rotation), gameObject.transform);
        }
        // Invoking event
        if (PartsChanged != null)
            PartsChanged(this, EventArgs.Empty);
    }

    /// <summary>
    /// Method searches through children for Weapon and assigns them to the List
    /// </summary>
    private void OnPartsChanged(object sender, EventArgs e)
    {
        foreach (Transform child in transform)
        {
            Weapon weapon = child.GetComponent<Weapon>();
            if(weapon)
            {
                weapons.Add(weapon);
            }
        }
    }

    #region Networking

    [Command]
    private void CmdRefreshParts()
    {
        // Converting PartData to string array
        string[] strings = new string[parts.Count];
        for (int i = 0; i < parts.Count; i++)
        {
            strings[i] = parts[i].ToString();
        }
        // Sending parts to client
        RpcSendParts(strings);
    }

    [ClientRpc]
    private void RpcSendParts(string[] strings)
    {
        // Clearing parts List
        parts.Clear();
        // Loading PartData from string array
        foreach (string s in strings)
        {
            parts.Add(new PartData(s));
        }
        // Rebuild parts
        RebuildParts();
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
            //todo projectile should handle that itself
            Destroy(shot, 5f);
        }       
    }

    #endregion

}
