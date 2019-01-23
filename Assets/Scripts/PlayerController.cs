using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : NetworkBehaviour {

    [SyncVar(hook = "OnPlayerNameChanged")]
    public string playerName;

    public float speed = 4f;

    public GameObject projectile;
    public InputField inputField;

    private Rigidbody body;

    public void Start()
    {
        OnPlayerNameChanged(playerName);
    }

    public override void OnStartLocalPlayer()
    {
        // Set the local player as this game object
        GameController.SetLocalPlayer(gameObject);
        // Set rigidbody reference
        body = GetComponent<Rigidbody>();
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
	    body.AddForce(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * body.mass * speed);

        //todo how to shoot - remove later
	    if (Input.GetKeyDown(KeyCode.Space))
	    {
	        CmdShoot();
	    }
    }
    
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

    // Changing name using input field
    public void UpdateName(string newName)
    {
        CmdChangeName(newName);
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
        inputField.text = newName;
    }
}
