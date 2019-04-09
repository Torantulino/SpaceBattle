using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainerUI : MonoBehaviour
{
    private ItemContainer _ItemContainer;

    public ItemContainer ItemContainer
    {
        get
        {
            if (_ItemContainer == null)
            {
                _ItemContainer = new ItemContainer();
            }
            return _ItemContainer;
        }
        set { _ItemContainer = value; }
    }

}
