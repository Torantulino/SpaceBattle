using System;
using UnityEngine.Networking;

/// <summary>
/// Base class for all objects that can be damaged/destroyed.
/// </summary>
public class Destructible : NetworkBehaviour
{
    [SyncVar(hook = "OnHpChanged")]
    private float _hp;

    #region Properties

    /// <summary>
    /// Hit points. Synchronized variable
    /// </summary>
    public float Hp
    {
        get { return _hp; }
        set { CmdChangeHp(value); }
    }

    #endregion

    /// <summary>
    /// Invoked when Hp value was changed.
    /// </summary>
    public event EventHandler<EventArgs<float>> HpChanged;

    // Use this for initialization
    public void Start()
    {
        // Update Hp, to reflect actual value for players that just joined
        OnHpChanged(Hp);
    }

    #region Networking

    /// <summary>
    /// Called when Hp value was synchronized.
    /// </summary>
    private void OnHpChanged(float newHp)
    {
        // Invoke HpChanged event
        if (HpChanged != null)
            HpChanged(this, new EventArgs<float>(newHp));
    }

    /// <summary>
    /// Request server to change Hp.
    /// </summary>
    /// <param name="newHp"></param>
    [Command]
    private void CmdChangeHp(float newHp)
    {
        _hp = newHp;
    }

    #endregion

}
