using UnityEngine;

public class ItemContainer
{
    public int ItemID { get; set; }
    public int Quantity { get; set; }

    public Sprite Icon
    {
        get { return PartManager.Instance.GetPartById(ItemID).icon; }
    }
}