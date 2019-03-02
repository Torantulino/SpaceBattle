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

    [SerializeField]
    private float _hp;
    private float _maxHp;
    [SerializeField]
    private float _mass;

    #region Properties

    /// <summary>
    /// If this Part was checked. Needed for attachment recalculations.
    /// </summary>
    [HideInInspector]
    public bool Checked;

    /// <summary>
    /// All nodes of this Part.
    /// </summary>
    [HideInInspector]
    public List<Node> Nodes = new List<Node>();

    /// <summary>
    /// Hit Points of this Part.
    /// </summary>
    public float Hp
    {
        get { return _hp; }
    }

    /// <summary>
    /// Mass of this Part
    /// </summary>
    public float Mass
    {
        get { return _mass; }
    }

    /// <summary>
    /// ID of this Part.
    /// </summary>
    public int ID
    {
        get { return id; }
    }

    #endregion

    [HideInInspector]
    public bool isGhost;//todo: properly protect value
    
    public void Awake ()
	{
        // Set reference to Player Ship
	    playerShip = GetComponentInParent<Ship>();
        // Initialise Nodes
        foreach (Node n in transform.Find("Nodes").GetComponentsInChildren<Node>())
        {
            Nodes.Add(n);
        }
        // Set max hp
	    _maxHp = Hp;
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
    /// Damage this Part.
    /// </summary>
    public void Damage(float hp)
    {
        _hp -= hp;
        
        // Handling destruction
        if (_hp <= 0f)
        {
            playerShip.RemovePart(gameObject.transform.localPosition);
        }
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
