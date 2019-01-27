using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps all PartInfo scriptable objects.
/// </summary>
public class PartManager : MonoBehaviour
{
    #region Singleton
    private static PartManager instance;

    public static PartManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    #endregion

    [Header("Add all PartInfo scriptable objects below.")]
    [SerializeField]
    private List<PartInfo> registeredParts;

    /// <summary>
    /// Returns PartInfo for specified part.
    /// </summary>
    /// <param name="id">Id of a Part.</param>
    /// <returns></returns>
    public PartInfo GetPartById(int id)
    {
        return registeredParts.Find(part => part.id == id);
    }
}

