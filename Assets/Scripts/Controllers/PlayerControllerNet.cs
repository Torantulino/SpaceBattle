using System;
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

    // Use this for initialization on local player
    public override void OnStartLocalPlayer()
    {
        // Set the local player as this game object
        GameController.SetLocalPlayer(gameObject);
        // Set the reference for Ship
        Ship = GetComponent<Ship>();
        //// Adding Parts that every player receives when they start
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

}
