using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FloatingPart : NetworkBehaviour
{
    [SyncVar(hook = "OnIdChanged")]
    private int _id;

    #region Properties

    public int ID
    {
        get { return _id; }
        set { _id = value; }
    }

    #endregion

    // Use this for initialization
    void Start () {
		OnIdChanged(ID);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnIdChanged(int id)
    {
        ID = id;
    }
}
