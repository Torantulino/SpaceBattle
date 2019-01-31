using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIFacade : MonoBehaviour
{

	//This class is used as a hub for calls coming from or to GUI processes/objects

	//build mode panel
	public static GUIToggle BuildPanel;

	void Start()
	{
		BuildPanel = GameObject.Find("BuildPanel").GetComponent<GUIToggle>();
	}

	public void ToggleBuildMode()
	{
		ToggleElement(BuildPanel);
	}

	/// <summary>
	/// Calls the toggle script on an object which has been defined on the object calling this function in the Unity Editor. 
	///<para>Allows Toggle method to change in future.</para>
	/// </summary>
	/// <param name="toggle">Takes only objects with GUIToggle scripts attached.</param>
	public void ToggleElement(GUIToggle toggle)
	{
		toggle.Flip();
	}
}