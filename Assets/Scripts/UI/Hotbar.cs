using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{

	///<summary>
	/// reference to a part in inventory corresponding to the index of the highlighted cell
	///</summary>
	public ItemContainer SelectedPart { get; private set; }

	private List<Image> highlights = new List<Image>();
	private List<Button> cells = new List<Button>();
	private int activeCell = 0;
	private Inventory inv;
    private BuildController buildController;

    // Use this for initialization
    void Start()
    {
        inv = GameObject.Find("InventoryManager").GetComponent<Inventory>();
		//populate cells
		cells = GetComponentsInChildren<Button>().ToList();
		//populate highlights with the child image from each cell
		foreach (Button b in cells)
		{
			//use getComponents plural and index of 1 to avoid finding the image on the parent instead.
			Image highlightBorder = b.GetComponentsInChildren<Image>() [1];
			highlights.Add(highlightBorder);
		}
		Debug.Log("cells : " + cells.Count);
		Debug.Log("highlights : " + highlights.Count);
	}

	// Update is called once per frame
	void Update()
	{
        if(buildController == null)
            buildController = FindObjectOfType<BuildController>();

        bool[] hotkeys = new bool[5];

		//all the inputs checked so I dont have lots of repeat code
		hotkeys[0] = Input.GetKeyDown(KeyCode.Alpha1);
		hotkeys[1] = Input.GetKeyDown(KeyCode.Alpha2);
		hotkeys[2] = Input.GetKeyDown(KeyCode.Alpha3);
		hotkeys[3] = Input.GetKeyDown(KeyCode.Alpha4);
		hotkeys[4] = Input.GetKeyDown(KeyCode.Alpha5);

		//if a kotkey is pressed set its correspondng cell as the active cell
		for (int i = 0; i < hotkeys.Length; i++)
		{
			if (hotkeys[i])
			{
				Debug.Log("hotbar index : " + i);
				activeCell = i;

				//set selected part to a reference to a part on the top row of the inventory
				SelectedPart = inv.displayCells2D[0, activeCell].GetComponent<ItemContainerUI>().ItemContainer;

                //set selected part in the build controller
			    buildController.SelectedPartID = SelectedPart.ItemID;
            }
		}

		for (int i = 0; i < cells.Count; i++)
		{
			ItemContainer ic = inv.displayCells2D[0, i].GetComponent<ItemContainerUI>().ItemContainer;
			if (ic.ItemID != int.MaxValue)
				cells[i].GetComponent<Image>().sprite = ic.Icon;
			else
				cells[i].GetComponent<Image>().sprite = inv.DefaultImage;
		}

		//turn on the highlight for the active cell and off for inactive cells
		for (int i = 0; i < cells.Count; i++)
		{
			if (i == activeCell)
			{
				highlights[i].gameObject.SetActive(true);
			}
			else
			{
				highlights[i].gameObject.SetActive(false);
			}

			cells[i].GetComponentInChildren<Text>().text = inv.displayCells2D[0, i].GetComponent<ItemContainerUI>().ItemContainer.Quantity.ToString();
		}

		//debug only
		if (Input.GetKeyDown(KeyCode.Alpha0))
		{
			inv.Increment(inv.displayCells2D[0, activeCell], 1);
		}
		if (Input.GetKeyDown(KeyCode.Alpha9))
		{
			inv.RemoveLocal(inv.displayCells2D[0, activeCell], 1);
		}

	}

	public void SetHotbarIndex(Button button)
	{
		//check if a valid button sent the message
		if (cells.Contains(button))
		{
			//set that cell to be highlighted
			activeCell = cells.IndexOf(button);
		}
	}
}