using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Class for all Player prefabs.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Ship))]
[RequireComponent(typeof(BuildController))]
public partial class PlayerController : NetworkBehaviour {

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

    private BuildController buildController;
    private CameraModeToggle cameraModeToggle;
    private GUIFacade guiFacade;
    #endregion

    private int _testCounter = 0;

    // Use this for initialization
    void Start()
    {
        // Update PlayerName, to reflect actual value for players that just joined
        OnPlayerNameChanged(PlayerName);
        //Get buildController
        buildController = GetComponent<BuildController>();
        //etc
        cameraModeToggle = FindObjectOfType<CameraModeToggle>();
        guiFacade = GameObject.Find("GUI_Interface").GetComponent<GUIFacade>();

        // All other players
        if (isLocalPlayer)
            return;

        // Set the reference for Ship
        Ship = GetComponent<Ship>();

        buildMode = true;
        UpdateBuildMode();
    }

    // Update is called once per physics tick
    void FixedUpdate ()
	{
        // Check if this code runs on the game object that represents my Player
	    if (!isLocalPlayer)
	        return;

        //Flight & Fight Mode
        if (!buildMode) {
	        //todo testing
	        Ship.Thrust(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));

            if (Input.GetKeyDown(KeyCode.Space))
	            Ship.Shoot();

	        if (Input.GetKey(KeyCode.E))
	            Ship.Target = Ship.Target + new Vector3(0f, Time.fixedDeltaTime * 30f, 0f);
	        if (Input.GetKey(KeyCode.Q))
	            Ship.Target = Ship.Target + new Vector3(0f, -Time.fixedDeltaTime * 30f, 0f);

            if (Input.GetKeyUp(KeyCode.R))
                Ship.RefreshParts();

            if (Input.GetKeyUp(KeyCode.T))
            {
                switch (_testCounter++)
                {
                    case 1:
                        Ship.AddPart(new PartData(1, new Vector3(0, 1, 0)));
                        break;
                    case 2:
                        Ship.AddPart(new PartData(1, new Vector3(0, 2, 0)));
                        break;
                    case 3:
                        Ship.AddPart(new PartData(1, new Vector3(0, 3, 0)));
                        break;
                    case 4:
                        Ship.AddPart(new PartData(1, new Vector3(0, 4, 0)));
                        break;
                    case 5:
                        Ship.RemovePart(new Vector3(0, 2, 0));
                        break;
                    default:
                        break;
                }

            }

        }
        //Toggle build mode
	    if (Input.GetKeyDown(KeyCode.Tab))
	    {
	        buildMode = !buildMode;
            UpdateBuildMode();
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
