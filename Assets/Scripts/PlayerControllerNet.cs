using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public partial class PlayerController : NetworkBehaviour
{
    [SyncVar(hook = "OnPlayerNameChanged")]
    private string _playerName;

    public event EventHandler<EventArgs<string>> PlayerNameChanged;

    public override void OnStartLocalPlayer()
    {
        // Set the local player as this game object
        GameController.SetLocalPlayer(gameObject);
        // Set the reference for Ship
        Ship = GetComponent<Ship>();
        // Change color
        GetComponent<MeshRenderer>().material.color = Color.red;
        // Enabling input field for changing name
        inputField.interactable = true;
    }

    // Method is called when variable name was changed
    private void OnPlayerNameChanged(string newName)
    {
        if (PlayerNameChanged != null)
            PlayerNameChanged(this, new EventArgs<string>(newName));
    }

    // Method for updating variable playerName - syncVar should be updated on the server
    [Command]
    public void CmdChangeName(string newName)
    {
        _playerName = newName;
    }

}
