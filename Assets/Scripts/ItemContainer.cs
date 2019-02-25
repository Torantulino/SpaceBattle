using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemContainer : MonoBehaviour
{
    [SerializeField] private int itemID = int.MaxValue;
    [SerializeField] private int quantity = int.MaxValue;
    public int ItemID
    {
        get { return itemID; }
        set { itemID = value; }
    }
    public int Quantity
    {
        get { return quantity; }
        set { quantity = value; }
    }

    public Sprite Icon
    {
        get { return PartManager.Instance.GetPartById(ItemID).Icon; }
    }
}