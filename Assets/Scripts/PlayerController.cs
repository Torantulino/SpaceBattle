using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Ship))]
public partial class PlayerController : NetworkBehaviour {

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

    void Start()
    {
        PlayerNameChanged += (s, e) => inputField.text = e.Value;

        OnPlayerNameChanged(PlayerName);
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

}
