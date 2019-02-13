using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
	/// <summary>
	/// 2D array representing inventory locations [rows, collumns]
	/// </summary>
	public static Part[, ] locations = new Part[3, 5];

	[SerializeField] GameObject invPanel;
	Button[, ] displayCells2D = new Button[3, 5];

	// Use this for initialization
	void Start()
	{
		Button[] displayCells = invPanel.GetComponentsInChildren<Button>();

		//read the linear array of buttons on the UI into a 2d array like our inventory datastructure.
		int index = 0;
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				displayCells2D[i, j] = displayCells[index];
				index++;
			}
		}
	}

	// Update is called once per frame
	void Update()
	{

	}

}