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
    private GameObject ghost;
    private bool partSelected;

    // Use this for initialization
    void Start () {
        buildmode = false;
        partSelected = false;

        //TESTING
        selectedPartID = 1;
	    currentNode = 0;

        //Find part manager in scene
	    partManager = FindObjectOfType<PartManager>();

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
                partSelected = true;

                //Display 'ghost' block
                ghost = Instantiate(partManager.GetPartById(selectedPartID).prefab, transform);
                //Make transparent - requires matrial rendering mode: Transparent. Doing this programatically is unfortunatly not currently simple.
                Color col = ghost.gameObject.GetComponent<Renderer>().material.color;
                col.a = 0.66f;
                ghost.gameObject.GetComponent<Renderer>().material.color = col;
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

                ghost.transform.position = availableNodes[currentNode].transform.position + availableNodes[currentNode].transform.localPosition;
            }
            //cycle right
            if (Input.GetKeyDown(KeyCode.D))
            {

            }

        }
    }

    private void GetCurrentParts()
    {
        currentParts.Clear();
        foreach (Part p in GetComponentsInChildren<Part>())
            currentParts.Add(p);
    }

    private void GetAvailableNodes()
    {
        availableNodes.Clear();

        //Ship Nodes
        foreach (Node n in GetComponentsInChildren<Node>())
            availableNodes.Add(n);

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
