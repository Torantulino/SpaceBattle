using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Class for all Player prefabs.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Ship))]
[RequireComponent(typeof(BuildController))]
public partial class PlayerController : NetworkBehaviour
{

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

    public bool BuildMode
    {
        get { return buildMode; }
        set { buildMode = value; } //todo -----------------------
    }

    private bool buildMode;

    private LineRenderer lineRenderer;
    private BuildController buildController;
    private CameraModeToggle cameraModeToggle;
    private CameraController cameraController;
    private GUIFacade guiFacade;
    #endregion

    private int _testCounter = 0;
    private FloatingPart _testPart;
    private bool shooting = false;
    bool lasing = false;

    // Use this for initialization
    void Start()
    {
        // Update PlayerName, to reflect actual value for players that just joined
        OnPlayerNameChanged(PlayerName);
        //Get buildController
        buildController = GetComponent<BuildController>();
        //etc
        cameraModeToggle = FindObjectOfType<CameraModeToggle>();
        cameraController = FindObjectOfType<CameraController>();
        guiFacade = GameObject.Find("GUI_Interface").GetComponent<GUIFacade>();
        lineRenderer = GetComponent<LineRenderer>();

        //Stop Atmospheric Noise
        cameraController.ShakeScreen(0.0f, 1.0f, true);


        //Set max AngularV
        GetComponent<Rigidbody>().maxAngularVelocity = 1.0f;

        //Update buildmode accross the board
        UpdateBuildMode();

        //todo setting random name (needed for picking up parts)
        PlayerName = "Player" + Mathf.RoundToInt(Random.value * 1000000);

        // All other players
        if (isLocalPlayer)
            return;

        // Set the reference for Ship
        Ship = GetComponent<Ship>();
    }

    //Use update for frame dependent input (Key up/Down)
    private void Update()
    {
        //Toggle build mode
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            buildMode = !buildMode;
            UpdateBuildMode();
        }
        //- Combat -
        //Fire Primary Weapon
        if (Input.GetKeyDown(KeyCode.Mouse0))       //todo: add check to see if weapon is on the ship
        {
            shooting = true;
            StartCoroutine("Shooting");
            cameraController.ShakeScreen(3.0f, 1.0f, true);
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            shooting = false;
            cameraController.ShakeScreen(0.0f, 1.0f, true);
        }
        //Fire Mining Laser
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            lasing = true;
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            lasing = false;
        }
        //TODO: EXTRACT TO METHOD A LA Ship.Shoot()
        if (lasing)
        {
            //Fire
            lineRenderer.SetPosition(0, transform.position);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                if (hit.collider)
                {   
                    //Stop laser at hit surface.
                    lineRenderer.SetPosition(1, hit.point);
                    if(hit.transform.GetComponent<FloatingPart>())
                    {
                        GameController.LocalPlayerController.PickUpFloatingPart(hit.transform.GetComponent<FloatingPart>());
                    }
                }
            }
            else lineRenderer.SetPosition(1, (CameraController.CameraAnchor.transform.position - transform.position)*20.0f);
        }
        else
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position);
        }
    }

    //Shooting for fully automatic weapons
    private IEnumerator Shooting()
    {
        while (shooting)
        {
            Ship.Shoot();
            yield return new WaitForSeconds(0.1f);  //todo: this will acquiesce to weapon's cooldown(s), but could/should be optimised by obtaining a reference to it
        }
        yield return null;
    }

    // FixedUpdate is called once per physics tick
    void FixedUpdate()
    {
        // Check if this code runs on the game object that represents my Player
        if (!isLocalPlayer)
            return;

        //Flight & Fight Mode
        if (!buildMode)
        {
            //- Flight -
            //Steer
            Vector3 steering = new Vector3((Camera.main.ScreenToViewportPoint(Input.mousePosition).y - 0.5f) * -1.0f, Camera.main.ScreenToViewportPoint(Input.mousePosition).x - 0.5f, 0.0f);
            GetComponent<Rigidbody>().angularVelocity = (transform.localToWorldMatrix.rotation * steering) * 2.0f;
            // Bank (roll due to steering)
            Ship.Roll(steering.y * -90f);
            //Roll
            if (Input.GetKey(KeyCode.Q))
            {
                Ship.Roll(60f);
            }
            if (Input.GetKey(KeyCode.E))
            {
                Ship.Roll(-60f);
            }
            //Thrust
            if (Input.GetKey(KeyCode.LeftShift))
                Ship.Thrust(Vector3.forward);
            //Reverse Thrust
            bool reversing = false;
            if (Input.GetKey(KeyCode.LeftControl))
            {
                Ship.Thrust(Vector3.back);
                reversing = true;
            }
            //Point velocity along ship direction (if not trying to reverse or currently going backwards)
            if (transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity).z > 0 && !reversing)
                GetComponent<Rigidbody>().velocity = transform.forward * GetComponent<Rigidbody>().velocity.magnitude;

            //todo testing
            if (Input.GetKeyUp(KeyCode.R))
            {
                _testCounter++;
                switch (_testCounter)
                {
                    case 1:
                        if (!isServer)
                            break;
                        GameObject floatingPart = Instantiate(GameController.Instance.FloatingPartGameObject, new Vector3(0f, 0f, 15f), new Quaternion());
                        NetworkServer.Spawn(floatingPart);
                        // Set FloatingPart ID
                        floatingPart.GetComponent<FloatingPart>().ID = 1;
                        _testPart = floatingPart.GetComponent<FloatingPart>();
                        Debug.Log("Created FloatingPart");
                        break;
                    case 2:
                        Debug.Log("Picking up FloatingPart");
                        PickUpFloatingPart(_testPart);
                        break;
                    case 3:
                        _testCounter = 0;
                        break;
                    default:
                        _testCounter = 0;
                        break;
                }
            }
        }
    }

    private void UpdateBuildMode()
    {
        //Update BuildController
        buildController.UpdateBuildmode(buildMode);
        //Update CameraModeToggle
        cameraModeToggle.UpdateBuildmode(buildMode);
        //Update Gui facade
        guiFacade.UpdateBuildmode(buildMode);
    }

    /// <summary>
    /// Picks up a floating part.
    /// </summary>
    /// <param name="part"></param>
    public void PickUpFloatingPart(FloatingPart part)
    {
        part.PickUp(PlayerName);
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
