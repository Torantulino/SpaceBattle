﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Class for all objects that can have Parts.
/// </summary>
public class Unit : Destructible
{
    [Tooltip("Empty GameObject used to represent aiming direction. Should also be put into NetworkTransformChild component.")]
    public Transform aimTransform;

    protected List<PartData> parts = new List<PartData>();
    protected List<Weapon> weapons = new List<Weapon>();

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

    /// <summary>
    /// Invoked when all or some parts changed.
    /// </summary>
    public event EventHandler<EventArgs> PartsChanged;

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

        // Add all parts that every player should have when they start.
        parts.Add(new PartData(0, new Vector3(0, 1, 0)));
        parts.Add(new PartData(0, new Vector3(1, 0, 0)));
        parts.Add(new PartData(0, new Vector3(-1, 0, 0)));
    }

    /// <summary>
    /// Tries to shoot from all weapons.
    /// </summary>
    public void Shoot()
    {
        CmdShoot();
    }

    /// <summary>
    /// Refreshes the parts List. Rebuilds parts when successful.
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
        transform.DestroyChildren("Aim");
        // Loop through parts and instantiate them
        // Note: Those parts are only created locally
        foreach (PartData part in parts)
        {
            Instantiate(PartManager.Instance.GetPartById(part.Id).prefab, transform.position + part.Position,
                Quaternion.Euler(transform.localEulerAngles + part.Rotation), gameObject.transform);
        }
        // Invoking event
        if (PartsChanged != null)
            PartsChanged(this, EventArgs.Empty);
    }

    /// <summary>
    /// Event handler for PartsChanged.
    /// </summary>
    private void OnPartsChanged(object sender, EventArgs e)
    {
        // Clear weapons List
        weapons.Clear();
        // Search through children for Weapon and assign them to the List.
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

    /// <summary>
    /// Request for refreshing parts List.
    /// </summary>
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

    /// <summary>
    /// Send parts to clients.
    /// </summary>
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

    /// <summary>
    /// Request server to shoot.
    /// </summary>
    [Command]
    private void CmdShoot()
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

    #endregion

}