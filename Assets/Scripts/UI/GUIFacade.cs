using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIFacade : MonoBehaviour
{

	//This class is used as a hub for calls coming from or to GUI processes/objects

	//build mode panel
	[SerializeField] private GUIToggle BuildPanel;
	[SerializeField] private GUIToggle CombatPanel;
	[SerializeField] private DisplayHP Health;

	private void Start()
	{
		CursorMode();
	}

	/// <summary>
	/// Toggle between build/combat mode
	///<para>Changes between build/combat mode UI then appropriately locks or unlocks the cursor.</para>
	/// </summary>
	public void ToggleBuildMode()
	{
		ToggleElement(BuildPanel);
		ToggleElement(CombatPanel);
		CursorMode();
	}

	private void CursorMode()
	{
		//when the Build panel is active unlock mouse, else lock the mouse to the centre

		if (BuildPanel.isActiveAndEnabled) //this line give a null ref exception exactly once but still works for some reason.
		{
			Cursor.lockState = CursorLockMode.None;
		}
		else
		{
			Cursor.lockState = CursorLockMode.Locked;
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

	/// <summary>
	/// Sets level of HP bar to given value. 
	///<para>Values must range between 0 and 1.</para>
	/// </summary>
	public void SetHealthBar(float value)
	{
		if (value > 1 || value < 0)
		{
			Debug.LogError("Input out of bounds. Value must be > 0 && < 1.");
		}

		Health.SetHP(value);

	}

	/// <summary>
	/// Adds a value to current display level of health bar. 
	///<para>May be positive or negative. Absolute value should be no more than 1.</para>
	/// </summary>
	public void IncrementHealthBar(float value)
	{
		if (Mathf.Abs(value) > 1)
		{
			Debug.LogError("Input out of bounds. Value must be > -1 && < 1.");
		}

		Health.IncrementHP(value);
	}
}