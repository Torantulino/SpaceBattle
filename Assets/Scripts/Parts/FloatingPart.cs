using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FloatingPart : NetworkBehaviour
{
    [SyncVar(hook = "OnIdChanged")]
    private int _id;

    #region Properties

    public int ID
    {
        get { return _id; }
        set { _id = value; }
    }

    #endregion

    // Use this for initialization
    void Start () {
		OnIdChanged(ID);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Picking up this Part.
    /// </summary>
    public void PickUp(string playerName)
    {
        CmdPickUp(playerName);
    }

    /// <summary>
    /// Picking up FloatingPart on the server
    /// </summary>
    /// <param name="playerName"></param>
    //todo should use player ID instead
    [Command]
    private void CmdPickUp(string playerName)
    {
        // Only on server
        if (!isServer)
            return;

        //todo not optimal
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            // Search for the player that wants to pick up this Part
            if (playerController.PlayerName == playerName)
            {
                //todo check distance to player, etc.
                playerController.AddItem(_id);
                NetworkServer.Destroy(gameObject);
                return;
            }
        }
        Debug.LogWarning("Player " + playerName + " tried to pick up a FloatingPart but wasn't found.");
    }

    void OnIdChanged(int id)
    {
        ID = id;
    }
}
