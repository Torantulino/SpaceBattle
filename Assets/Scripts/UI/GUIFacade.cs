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

	/// <summary>
	/// Toggle build mode on/off
	///<para>Changes state of build mode UI to on or off then appropriately locks or unlocks the cursor.</para>
	/// </summary>
	public void ToggleBuildMode()
	{
		ToggleElement(BuildPanel);

		//when the Build panel is active unlock mouse, else lock the mouse to the centre
		switch (BuildPanel.isActiveAndEnabled)
		{
			case true:
				Cursor.lockState = CursorLockMode.None;
				break;
			case false:
				Cursor.lockState = CursorLockMode.Locked;
				break;
			default:
				Debug.LogError("Something's null and that's Bad.");
				break;
		}
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