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
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			gui.ToggleBuildMode();
		}

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}

		//debug and demo options
		if (Input.GetKeyDown("z"))
		{
			gui.SetHealthBar(0);
		}

		if (Input.GetKeyDown("x"))
		{
			gui.SetHealthBar(0.5f);
		}

		if (Input.GetKeyDown("c"))
		{
			gui.SetHealthBar(1.0f);
		}

		if (Input.GetKey("up"))
		{
			gui.IncrementHealthBar(1 * Time.deltaTime);
		}

		if (Input.GetKey("down"))
		{
			gui.IncrementHealthBar(-1 * Time.deltaTime);
		}
	}
}