using UnityEngine;

/// <summary>
/// ScriptableObject that holds information about a part.
/// </summary>
[CreateAssetMenu(fileName = "New Part", menuName = "Parts/PartInfo")]
public class PartInfo : ScriptableObject
{
    [SerializeField]
    [Tooltip("Unique id of a part")]
    private int _id;

    [SerializeField]
    [Tooltip("Name of a Part")]
    private string _name = "New Part";

    [SerializeField]
    [Tooltip("Part icon to show on UI")]
    private Sprite _icon = null;

    [SerializeField]
    [Tooltip("GameObject that is created when this part is attached to a ship")]
    private GameObject _prefab = null;

    #region Properties

    /// <summary>
    /// Unique id of a part.
    /// </summary>
    public int Id
    {
        get { return _id; }
    }

    /// <summary>
    /// Name of a part.
    /// </summary>
    public string Name
    {
        get { return _name; }
    }

    /// <summary>
    /// Part icon to show on UI.
    /// </summary>
    public Sprite Icon
    {
        get { return _icon; }
    }

    /// <summary>
    /// GameObject that is created when this part is attached to a ship
    /// </summary>
    public GameObject Prefab
    {
        get { return _prefab; }
    }

    #endregion
}