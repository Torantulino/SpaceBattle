using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Class for handling player input in buildmode
/// </summary>

public class BuildController : MonoBehaviour {

    public bool buildmode;
    private int selectedPartID;
    private List<Part> currentParts;
    private List<Node> availableNodes;

	// Use this for initialization
	void Start () {
        buildmode = false;
        //todo: TESTING
	    selectedPartID = 1;

	}
	
	// Update is called once per frame
	void Update () {


        // - Buildmode -
        if (buildmode)
        {
            //Node Cycling
            //cycle left
            if (Input.GetKeyDown(KeyCode.A))
            {

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
        foreach (Part p in currentParts)
        {
            foreach (Node n in p.Nodes){
                if (n.gameObject.activeSelf)
                    availableNodes.Add(n);
            }
        }
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
