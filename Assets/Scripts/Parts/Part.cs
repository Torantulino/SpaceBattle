using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Base class for all parts.
/// </summary>
[SelectionBase]
public class Part : MonoBehaviour
{
    protected Ship playerShip;
    public List<Node> Nodes;
    public bool isGhost;            //todo: properly protect value

    // Use this for initialization
    public void Start ()
	{
	    playerShip = GetComponentInParent<Ship>();
        //Initialise Nodes
        foreach (Node n in GetComponentsInChildren<Node>())
            Nodes.Add(n);
	}
	
	// Update is called once per frame
	public void Update () {
		
	}

}
