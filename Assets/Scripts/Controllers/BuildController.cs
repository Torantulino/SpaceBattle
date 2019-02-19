using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Class for handling player input in buildmode
/// </summary>

public class BuildController : MonoBehaviour {

    public bool buildmode;
    private int selectedPartID;             //ID of part to spawn
    private List<Part> currentParts = new List<Part>();
    private List<Node> availableNodes = new List<Node>();
    private int currentNode;

    private PartManager partManager;
    private Ship ship;
    private GameObject ghost;
    private bool partSelected;

    private Dictionary<Vector3, bool> partPositions = new Dictionary<Vector3, bool>();

    // Use this for initialization
    void Start () {
        buildmode = false;
        partSelected = false;

        //TESTING
        selectedPartID = 0;
	    currentNode = 0;

        //Find part manager in scene
	    partManager = FindObjectOfType<PartManager>();
        ship = GetComponent<Ship>();

        GetCurrentParts();
        GetAvailableNodes();
    }
	
	// Update is called once per frame
	void Update () {


        // - Buildmode -
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
                ghost = Instantiate(partManager.GetPartById(selectedPartID).Prefab, transform);
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
                if (availableNodes.Count-1 > currentNode)
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
                //BuildPart(selectedPartID, ghost.transform.rotation * ghost.transform.localPosition);
                //BuildPart(selectedPartID, ghost.transform.position - transform.position);

                //TESTING while waiting for serverside fix
                GameObject testObj = Instantiate(partManager.GetPartById(selectedPartID).Prefab, transform);
                testObj.transform.position = ghost.transform.position;
                testObj.name = "test";

                Destroy(ghost);

                partSelected = false;

                //PartData newPart = new PartData(selectedPartID, ghost.transform.localPosition);
                ////ship.AddPart(newPart);
                //GameController.LocalPlayerController.Ship.AddPart(newPart);
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
                    Destroy(availableNodes[currentNode].transform.parent.parent.gameObject);
                    Destroy(ghost);

                    partSelected = false;
                }
            }

            //TESTING
            if (Input.GetKeyDown(KeyCode.F1))
            {
                Debug.Log(GameController.LocalPlayerController.Ship.PartsData.Count);
            }

        }
    }

    private void BuildPart(int id, Vector3 pos)
    {
        PartData newPart = new PartData(id, pos);
        //ship.AddPart(newPart);
        GameController.LocalPlayerController.Ship.AddPart(newPart);
    }

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

    private void GetAvailableNodes()
    {
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

        ////Part Nodes
        //foreach (Part p in currentParts)
        //{
        //    foreach (Node n in p.Nodes){
        //        if (n.gameObject.activeSelf)
        //            availableNodes.Add(n);
        //    }
        //}
    }

    public void ToggleBuildmode()
    {
        buildmode = !buildmode;
        if (!buildmode)
        {
            Destroy(ghost);
            partSelected = false;
        }
        //todo: toggle cursor lock
        //todo: entering buildmode should cause player to slow and stop before they can build. (no moving and building)
    }
}
