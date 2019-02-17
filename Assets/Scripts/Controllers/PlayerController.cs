﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Class for all Player prefabs.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Ship))]
[RequireComponent(typeof(BuildController))]
public partial class PlayerController : NetworkBehaviour {

    private readonly List<ItemContainer> _items = new List<ItemContainer>();

    #region Properties

    /// <summary>
    /// Name of the player. Synchronized variable
    /// </summary>
    public string PlayerName
    {
        get { return _playerName; }
        set { CmdChangeName(value); }
    }

    /// <summary>
    /// Ship script of the Player.
    /// </summary>
    public Ship Ship { get; private set; }

    /// <summary>
    /// Items in the inventory. Local Player only.
    /// </summary>
    public ReadOnlyCollection<ItemContainer> Items
    {
        get { return _items.AsReadOnly(); }
    }

    private BuildController buildController;
    #endregion

    // Use this for initialization
    void Start()
    {
        // Update PlayerName, to reflect actual value for players that just joined
        OnPlayerNameChanged(PlayerName);
        //Get buildController
        buildController = GetComponent<BuildController>();

        // All other players
        if (isLocalPlayer)
            return;

        // Set the reference for Ship
        Ship = GetComponent<Ship>();
    }

    // Update is called once per physics tick
    void FixedUpdate ()
	{
        // Check if this code runs on the game object that represents my Player
	    if (!isLocalPlayer)
	        return;

        //Flight & Fight Mode
        if (!buildController.buildmode) {
	        //todo temporary code for testing - remove later
	        Ship.Thrust(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
	    
            //todo how to shoot - remove later
            if (Input.GetKeyDown(KeyCode.Space))
	            Ship.Shoot();

            //todo changing aiming direction - upgrade later
	        if (Input.GetKey(KeyCode.E))
	            Ship.Target = Ship.Target + new Vector3(0f, Time.fixedDeltaTime * 30f, 0f);
	        if (Input.GetKey(KeyCode.Q))
	            Ship.Target = Ship.Target + new Vector3(0f, -Time.fixedDeltaTime * 30f, 0f);

            //todo manually refresh all parts
	        if (Input.GetKeyUp(KeyCode.R))
	            Ship.RefreshParts();
        }
	        
        //Toggle build mode
	    if (Input.GetKeyDown(KeyCode.Tab))
            buildController.ToggleBuildmode();
    }

    /// <summary>
    /// Add items to the Local Player inventory.
    /// </summary>
    public void AddItem(int id, int quantity = 1)
    {
        if (!isLocalPlayer)
            return;

        CmdUpdateItem(new ItemData { ItemId = id, Quantity = quantity });
    }

    /// <summary>
    /// Remove items from the Local Player inventory.
    /// </summary>
    public void RemoveItem(int id, int quantity = 1)
    {
        if (!isLocalPlayer)
            return;

        CmdUpdateItem(new ItemData { ItemId = id, Quantity = -quantity });
    }

    /// <summary>
    /// Refreshes all items in the inventory.
    /// Usually not necessary to call that manually.
    /// </summary>
    public void RefreshItems()
    {
        if (!isLocalPlayer)
            return;

        CmdRefreshItems();
    }

    /// <summary>
    /// Clear all items from the inventory.
    /// </summary>
    public void ClearItems()
    {
        if (!isLocalPlayer)
            return;

        CmdClearItems();
    }

}
