using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps all PartInfo scriptable objects
/// </summary>
public class PartController : MonoBehaviour
{
    #region Singleton
    private static PartController instance;

    public static PartController Instance { get { return instance; } }

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

    [SerializeField]
    private List<PartInfo> registeredParts;

    public PartInfo GetPartById(int id)
    {
        //todo handle null
        return registeredParts.Find(part => part.id == id);
    }
}

