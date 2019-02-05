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
        selectedPartID = 1;
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

                //Display 'ghost' block
                ghost = Instantiate(partManager.GetPartById(selectedPartID).prefab, transform);
                ghost.name = "ghost";
                //Make transparent - requires matrial rendering mode: Transparent. Doing this programatically is unfortunatly not currently simple.
                Color col = ghost.gameObject.GetComponent<Renderer>().material.color;
                col.a = 0.66f;
                ghost.gameObject.GetComponent<Renderer>().material.color = col;
                //Move ghost
                ghost.transform.position = availableNodes[currentNode].transform.position + availableNodes[currentNode].transform.localPosition;
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
                ghost.transform.position = availableNodes[currentNode].transform.position + availableNodes[currentNode].transform.localPosition;
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
                ghost.transform.position = availableNodes[currentNode].transform.position + availableNodes[currentNode].transform.localPosition;
            }

            //Build Part
            if (Input.GetKeyDown(KeyCode.Space))
            {
                BuildPart(selectedPartID, ghost.transform.localPosition);

                Destroy(ghost);

                partSelected = false;
            }

        }
    }

    private void BuildPart(int id, Vector3 pos)
    {
        PartData newPart = new PartData(id, pos);
        ship.AddPart(newPart);
    }

    private void GetCurrentParts()
    {
        currentParts.Clear();
        partPositions.Clear();
        //Occupy center (core)
        partPositions.Add(Vector3.zero, true);

        foreach (Part p in GetComponentsInChildren<Part>())
        {
            currentParts.Add(p);
            //round position to nearest int to ensure key is accurate
            Vector3 pos = new Vector3(Mathf.Round(p.transform.localPosition.x), Mathf.Round(p.transform.localPosition.y), Mathf.Round(p.transform.localPosition.z));
            partPositions.Add(pos, true);
        }
    }

    private void GetAvailableNodes()
    {
        availableNodes.Clear();

        //Ship Nodes
        foreach (Node n in GetComponentsInChildren<Node>())
        {
            Vector3 nodePos = n.transform.parent.parent.position + n.transform.localPosition * 2.0f;
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
        //todo: toggle cursor lock
    }

    //todo: wait for Unit AddPart then implement
    private void AddPart()
    {
    }
}
