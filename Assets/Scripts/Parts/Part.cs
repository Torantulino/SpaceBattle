using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{
    protected Ship playerShip;

	// Use this for initialization
	public void Start ()
	{
	    playerShip = GetComponentInParent<Ship>();
	}
	
	// Update is called once per frame
	public void Update () {
		
	}

}
