using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardListener : MonoBehaviour
{

	GUIFacade gui;

	void Start()
	{
		gui = GameObject.Find("GUI_Interface").GetComponent<GUIFacade>();
	}
	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown("tab"))
		{
			gui.ToggleBuildMode();
		}
	}
}