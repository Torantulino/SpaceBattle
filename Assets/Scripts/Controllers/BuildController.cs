using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Class for handling player input in buildmode
/// </summary>

public class BuildController : MonoBehaviour {

    public bool buildmode;
    public int SelectedPartID { private get; set; }            //ID of part to spawn

    private int currentNode;
    private bool partSelected;

    private List<Part> currentParts = new List<Part>();
    private List<Node> availableNodes = new List<Node>();
    private Dictionary<Vector3, bool> partPositions = new Dictionary<Vector3, bool>();

    private PartManager partManager;
    private GameObject ghost;
    private CameraModeToggle cameraModeToggle;

    // Use this for initialization
    void Start () {
        //initialise
        currentNode = 0;
        buildmode = false;
        partSelected = false;

        //TESTING - Intial value to be set by inventory system
        SelectedPartID = 1;

        //Find part manager in scene
	    partManager = FindObjectOfType<PartManager>();

        //Get player controller
        cameraModeToggle = FindObjectOfType<CameraModeToggle>();

        //Get nodes and parts
        GetCurrentParts();
        GetAvailableNodes();
    }

    // Update is called once per frame
    void Update()
    {
        // Buildmode
        if (buildmode)
        {
            if (!partSelected)
            {
                //setflag
                partSelected = true;
                GetCurrentParts();
                GetAvailableNodes();
                currentNode = 0;

                //Display 'ghost' block
                ghost = Instantiate(partManager.GetPartById(SelectedPartID).Prefab, transform);
                ghost.name = "ghost";
                ghost.GetComponent<Part>().isGhost = true;
                //Make transparent - requires matrial rendering mode: Transparent. Doing this programatically is unfortunatly not currently simple.
                Color col = ghost.gameObject.GetComponent<Renderer>().material.color;
                col.a = 0.66f;
                ghost.gameObject.GetComponent<Renderer>().material.color = col;
                //Move ghost
                ghost.transform.position = availableNodes[currentNode].transform.position + availableNodes[currentNode].transform.rotation * availableNodes[currentNode].transform.localPosition;
            }

            //Node Cycling
            //cycle left
            if (Input.GetKeyDown(KeyCode.A))
            {
                //Cycle
                if (availableNodes.Count - 1 > currentNode)
                    currentNode++;
                else
                    currentNode = 0;
                //Move ghost
                ghost.transform.position = availableNodes[currentNode].transform.position + availableNodes[currentNode].transform.rotation * availableNodes[currentNode].transform.localPosition;
            }
            //cycle right
            if (Input.GetKeyDown(KeyCode.D))
            {
                //Cycle
                if (currentNode != 0)
                    currentNode--;
                else
                    currentNode = availableNodes.Count - 1;
                //Move ghost
                ghost.transform.position = availableNodes[currentNode].transform.position + availableNodes[currentNode].transform.rotation * availableNodes[currentNode].transform.localPosition;
            }

            //Build Part
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //TESTING while waiting for serverside fix
                //GameObject testObj = Instantiate(partManager.GetPartById(SelectedPartID).Prefab, transform);
                //testObj.transform.position = ghost.transform.position;
                //testObj.name = "test";

                //Build Part (Networked)
                PartData newPart = new PartData(SelectedPartID, ghost.transform.localPosition);
                GameController.LocalPlayerController.Ship.AddPart(newPart);

                //Destory ghost and reset flag
                Destroy(ghost);
                partSelected = false;

                //Update bounds for camera zoom
                cameraModeToggle.CalculateBounds();
            }
            //Remove Part
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                //if part is not core
                if (availableNodes[currentNode].transform.parent.parent.gameObject.GetComponent<Ship>() == null)
                {
                    //Remove part from collection
                    currentParts.Remove(availableNodes[currentNode].transform.parent.parent.gameObject.GetComponent<Part>());

                    //TESTING while waiting for serverside fix
                    //Destroy(availableNodes[currentNode].transform.parent.parent.gameObject);

                    //Remove Part (Networked)
                    GameController.LocalPlayerController.Ship.RemovePart(availableNodes[currentNode].transform.parent.parent.transform.position);

                    //Destory ghost and reset flag
                    Destroy(ghost);
                    partSelected = false;

                    //Update bounds for camera zoom
                    cameraModeToggle.CalculateBounds();

                }
            }

            //TESTING - RETURN NETWORKED PART COUNT
            if (Input.GetKeyDown(KeyCode.F1))
            {
                Debug.Log(GameController.LocalPlayerController.Ship.PartsData.Count);
            }

        }
    }

    //Updates the current list of (non ghost) parts
    private void GetCurrentParts()
    {
        currentParts.Clear();
        partPositions.Clear();
        //Occupy center (core)
        partPositions.Add(Vector3.zero, true);

        foreach (Part p in GetComponentsInChildren<Part>())
        {
            //Add all but ghost
            if (!p.isGhost)
            {
                currentParts.Add(p);
                //round position to nearest int to ensure key is accurate
                Vector3 pos = new Vector3(Mathf.Round(p.transform.localPosition.x),
                    Mathf.Round(p.transform.localPosition.y), Mathf.Round(p.transform.localPosition.z));
                partPositions.Add(pos, true);
            }
        }
    }

    //Updates the current list of nodes not occupied by a part
    private void GetAvailableNodes()
    {
        //clear old list
        availableNodes.Clear();

        //Ship Nodes
        foreach (Node n in GetComponentsInChildren<Node>())
        {
            Vector3 nodePos = n.transform.parent.parent.localPosition + n.transform.localPosition * 2.0f;
            nodePos = new Vector3(Mathf.Round(nodePos.x), Mathf.Round(nodePos.y), Mathf.Round(nodePos.z));
            //Check if space is already occupied
            if (partPositions.ContainsKey(nodePos) && partPositions.ContainsValue(true))
            {
                //Make invisible, leave active to allow later reference
                Color col = n.gameObject.GetComponent<Renderer>().material.color;
                col.a = 0.0f;
                n.gameObject.GetComponent<Renderer>().material.color = col;
            }
            else
            {
                //Add to list of available nodes
                availableNodes.Add(n);
            }
        }
    }

    public void UpdateBuildmode(bool b)
    {
        buildmode = b;  //Hard set rather than toggle to ensure sync
        if (!buildmode)
        {
            Destroy(ghost);
            partSelected = false;
        }
    }
}
