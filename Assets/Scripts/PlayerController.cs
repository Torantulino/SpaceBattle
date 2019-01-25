using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Ship))]
public class PlayerController : NetworkBehaviour {

    [SyncVar(hook = "OnPlayerNameChanged")]
    private string _playerName;

    #region Properties

    /// <summary>
    /// Synchronized variable
    /// </summary>
    public string PlayerName
    {
        get { return _playerName; }
        set { CmdChangeName(value); }
    }

    public Ship Ship { get; private set; }

    #endregion

    public InputField inputField;

    public event EventHandler<EventArgs<string>> PlayerNameChanged;

    void Start()
    {
        PlayerNameChanged += (s, e) => inputField.text = e.Value;

        OnPlayerNameChanged(PlayerName);
    }

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

    // Update is called once per frame
    void FixedUpdate ()
	{
        // Check if this code runs on the game object that represents my Player
	    if (!isLocalPlayer)
	        return;
        
	    //todo temporary code for testing - remove later
	    Ship.Thrust(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
	    
        //todo how to shoot - remove later
        if (Input.GetKeyDown(KeyCode.Space))
	    {
	        Ship.Shoot();
	    }

        //todo changing aiming direction - upgrade later
	    if (Input.GetKey(KeyCode.E))
	        Ship.Target = Ship.Target + new Vector3(0f, Time.fixedDeltaTime * 30f, 0f);
	    if (Input.GetKey(KeyCode.Q))
	        Ship.Target = Ship.Target + new Vector3(0f, -Time.fixedDeltaTime * 30f, 0f);
	}

    #region Networking

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

    #endregion

}
