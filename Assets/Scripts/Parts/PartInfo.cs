using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Part", menuName = "Parts/PartInfo")]
public class PartInfo : ScriptableObject
{

    public int id;// unique id of a part
    public new string name = "New Part";// default name for ScriptableObject instance
    public Sprite icon = null;// Icon for this part
    public GameObject prefab = null;// GameObject that is created when this part is attached to a ship

}