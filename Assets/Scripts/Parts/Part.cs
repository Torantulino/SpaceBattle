using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;

/// <summary>
/// Base class for all parts.
/// </summary>
[SelectionBase]
public class Part : MonoBehaviour
{
    protected Ship playerShip;
    protected int id = -1;

    #region Properties

    /// <summary>
    /// If this Part was checked. Needed for attachment recalculations.
    /// </summary>
    public bool Checked;

    /// <summary>
    /// If this Part is directly or indirectly connected to the Unit.
    /// </summary>
    public bool ConnectedToUnit;

    #endregion

    //todo to readonly collection probably
    public List<Node> Nodes;
    public bool isGhost;            //todo: properly protect value

    public void Awake ()
	{
	    playerShip = GetComponentInParent<Ship>();
        //Initialise Nodes
        foreach (Node n in transform.Find("Nodes").GetComponentsInChildren<Node>())
        {
            Nodes.Add(n);
        }
	}

    // Use this for initialization
    public void Start()
    {

    }

    // Update is called once per frame
    public void Update ()
	{

    }

    /// <summary>
    /// ID is set when Part gameObject is instantiated.
    /// </summary>
    /// <param name="id"></param>
    public void SetID(int id)
    {
        this.id = id;
    }

}
