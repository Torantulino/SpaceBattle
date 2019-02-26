using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public partial class PlayerController : NetworkBehaviour
{
    [SyncVar(hook = "OnPlayerNameChanged")]
    private string _playerName;

    /// <summary>
    /// Invoked when player name was changed.
    /// </summary>
    public event EventHandler<EventArgs<string>> PlayerNameChanged;

    /// <summary>
    /// Invoked when player items changed.
    /// </summary>
    public event EventHandler<EventArgs> ItemsChanged; 

    // Use this for initialization on local player
    public override void OnStartLocalPlayer()
    {
        // Set the local player as this game object
        GameController.SetLocalPlayer(gameObject);
        // Set the reference for Ship
        Ship = GetComponent<Ship>();
        // Adding Parts that every player receives when they start
        //Ship.AddPart(new PartData(0, new Vector3(0, 1, 0)));
        //Ship.AddPart(new PartData(0, new Vector3(0, -1, 0), new Vector3(0f, 0f, 180f)));
    }

    /// <summary>
    /// Called when player name was synchronized.
    /// </summary>
    private void OnPlayerNameChanged(string newName)
    {
        // Invoke event
        if (PlayerNameChanged != null)
            PlayerNameChanged(this, new EventArgs<string>(newName));
    }

    /// <summary>
    /// Request playerName change.
    /// </summary>
    [Command]
    private void CmdChangeName(string newName)
    {
        _playerName = newName;
    }

    /// <summary>
    /// Request to add or remove items.
    /// </summary>
    [Command]
    private void CmdUpdateItem(ItemData itemData)
    {
        // Searching for an item
        ItemContainer foundItem = Items.SingleOrDefault(i => i.ItemID == itemData.ItemId);

        // If there is an item with this id
        if (foundItem != null)
        {
            // Updating quantity
            itemData.OldQuantity = foundItem.Quantity;
            foundItem.Quantity += itemData.Quantity;
            // Removing an item if quantity is below 1
            if (foundItem.Quantity < 1)
                _items.Remove(foundItem);
        }
        else
        {
            itemData.OldQuantity = 0;
            if(itemData.Quantity > 0)
                _items.Add(new ItemContainer {ItemID = itemData.ItemId, Quantity = itemData.Quantity});
        }
        // Sending items to the client
        TargetUpdateItem(connectionToClient, itemData);
    }

    /// <summary>
    /// Refreshes all items.
    /// </summary>
    [Command]
    private void CmdRefreshItems()
    {
        // Making a temporary copy of Items List
        List<ItemContainer> items = new List<ItemContainer>(_items);
        // Removing all items
        _items.Clear();
        TargetClearItems();
        // Adding all items again
        _items.AddRange(items);
        foreach (ItemContainer itemContainer in items)
        {
            TargetUpdateItem(connectionToClient, 
                new ItemData {ItemId = itemContainer.ItemID, OldQuantity = 0, Quantity = itemContainer.Quantity});   
        }
    }

    /// <summary>
    /// Request to clear all items.
    /// </summary>
    [Command]
    private void CmdClearItems()
    {
        _items.Clear();
        TargetClearItems();
    }

    /// <summary>
    /// Updating an client items.
    /// </summary>
    [TargetRpc]
    private void TargetUpdateItem(NetworkConnection target, ItemData itemData)
    {
        // Searching for an item
        ItemContainer foundItem = Items.SingleOrDefault(i => i.ItemID == itemData.ItemId);

        // If there is an item with this id
        if (foundItem != null)
        {
            // Updating quantity
            foundItem.Quantity = itemData.OldQuantity + itemData.Quantity;
            // Removing an item if quantity is below 1
            if (foundItem.Quantity < 1)
                _items.Remove(foundItem);
        }
        else
        {
            if (itemData.OldQuantity + itemData.Quantity > 0)
                _items.Add(new ItemContainer { ItemID = itemData.ItemId, Quantity = itemData.OldQuantity + itemData.Quantity });
        }

        // Invoke event
        if (ItemsChanged != null)
            ItemsChanged(this, EventArgs.Empty);
    }

    /// <summary>
    /// Clearing client items.
    /// </summary>
    private void TargetClearItems()
    {
        if (Items.Count <= 0) return;

        _items.Clear();
        
        // Invoke event
        if (ItemsChanged != null)
            ItemsChanged(this, EventArgs.Empty);
    }

    /// <summary>
    /// Represents single item container. Used to transfer data between server and clients.
    /// </summary>
    private struct ItemData
    {
        public int ItemId;
        public int OldQuantity;
        public int Quantity;
    }
}
