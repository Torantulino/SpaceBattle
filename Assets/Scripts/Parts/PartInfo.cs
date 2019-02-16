using UnityEngine;

/// <summary>
/// ScriptableObject that holds information about a part.
/// </summary>
[CreateAssetMenu(fileName = "New Part", menuName = "Parts/PartInfo")]
public class PartInfo : ScriptableObject
{
    [Tooltip("Unique id of a part")]
    public int id;

    [Tooltip("Name of a Part")]
    public new string name = "New Part";

    [Tooltip("Part icon to show on UI")]
    public Sprite icon = null;

    [Tooltip("GameObject that is created when this part is attached to a ship")]
    public GameObject prefab = null;
}