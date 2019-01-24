using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Ship))]
public class PlayerController : NetworkBehaviour {

    [SyncVar(hook = "OnPlayerNameChanged")]
    private string playerName;

    public Ship ship;

    public GameObject projectile;
    public InputField inputField;

    public event EventHandler<EventArgs<string>> PlayerNameChanged;

    void Start()
    {
        OnPlayerNameChanged(playerName);

        PlayerNameChanged += (s, e) => inputField.text = e.Value;
    }

    public override void OnStartLocalPlayer()
    {
        // Set the local player as this game object
        GameController.SetLocalPlayer(gameObject);
        // Set the reference for Ship
        ship = GetComponent<Ship>();
        // Change color
        GetComponent<MeshRenderer>().material.color = Color.red;
        // Enabling input field for changing name
        inputField.interactable = true;
    }

    // Update is called once per frame
    void FixedUpdate ()
	{
        // Check if this code runs on the game object that represents my Player
	    if (!isLocalPlayer)
	        return;
        
	    //todo temporary code for testing - remove later
	    ship.Thrust(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
	    
        //todo how to shoot - remove later
        if (Input.GetKeyDown(KeyCode.Space))
	    {
	        CmdShoot();
	    }
    }

    // Getting playerName
    public string GetPlayerName()
    {
        return playerName;
    }

    // Changing playerName
    public void SetPlayerName(string newName)
    {
        CmdChangeName(newName);
    }

    #region Networking

    [Command]
    public void CmdShoot()
    {
        // Instantiate GameObject
        GameObject shot = Instantiate(projectile, transform.position, transform.rotation);
        // Spawn it - so it appears for all clients
        NetworkServer.Spawn(shot);
        // Destroy it after some time
        Destroy(shot, 5f);
    }

    // Method for updating variable playerName - syncVar should be updated on the server
    [Command]
    public void CmdChangeName(string newName)
    {
        playerName = newName;
    }

    // Method is called when variable name was changed
    private void OnPlayerNameChanged(string newName)
    {
        if (PlayerNameChanged != null)
            PlayerNameChanged(this, new EventArgs<string>(newName));
    }

    #endregion

}
