using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
	//list of Buttons (cells) selected for swapping places
	private List<Button> selectedCell = new List<Button>();
	private bool init = false;

	public Sprite DefaultImage;

	GameObject invPanel;

	/// <summary>
	/// 2D array representing inventory locations [rows, collumns]
	/// </summary>
	public Button[, ] displayCells2D = new Button[3, 5];

	// Use this for initialization
	void Start()
	{
		invPanel = GameObject.Find("InventoryPanel");
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

	/// <summary>
	/// set a cell to be selected for switching
	/// </summary>
	public void Pick(Button cell)
	{
		selectedCell.Add(cell);
	}

	//switch itemcontainers for cells in the grid
	private void Swap()
	{

		ItemContainer first = selectedCell[0].GetComponent<ItemContainer>();
		ItemContainer second = selectedCell[1].GetComponent<ItemContainer>();

		ItemContainer temp = first;
		first = second;
		second = temp;
	}

	/// <summary>
	/// set a cells item container params and pass to server
	/// </summary>
	public void AddLocal(Button b, int id, int quantity)
	{
		ItemContainer cont = b.GetComponent<ItemContainer>();
		cont.ItemID = id;
		cont.Quantity = quantity;
		AddServer(cont);
	}

	private void AddServer(ItemContainer item)
	{
		GameController.LocalPlayerController.AddItem(item.ItemID, item.Quantity);
	}

	public void Clicked()
	{
		print("Clicked");
	}

	// Update is called once per frame
	void Update()
	{
		//if there are 2 selected cells
		if (selectedCell.Count == 2)
		{
			Swap();
		}

		//display the right icon
		foreach (Button b in displayCells2D)
		{
			if (b.GetComponent<ItemContainer>().ItemID != int.MaxValue)
			{
				b.GetComponentsInChildren<Image>() [1].sprite = b.GetComponent<ItemContainer>().Icon;
			}
			else
			{
				b.GetComponentsInChildren<Image>() [1].sprite = DefaultImage;
			}

		}

		//if local player present and not already initialised
		if (GameController.LocalPlayer != null && init == false)
		{
			//For now start with 2 items on top row
			AddLocal(displayCells2D[0, 0], 0, 5);
			AddLocal(displayCells2D[0, 1], 1, 5);
			init = true;
		}

	}

}