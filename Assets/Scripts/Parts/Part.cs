using UnityEngine;

/// <summary>
/// Base class for all parts.
/// </summary>
[SelectionBase]
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
