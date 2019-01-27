using System;
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
