using System;
using UnityEngine;
using UnityEngine.Networking;

public class Destructible : NetworkBehaviour
{
    [SyncVar(hook = "OnHpChanged")]
    private float _hp;

    #region Propeties

    public float Hp
    {
        get { return _hp; }
        private set { CmdChangeHp(value); }
    }

    #endregion

    public event EventHandler<EventArgs<float>> HpChanged;

    public void Start()
    {
        OnHpChanged(Hp);
    }

    private void OnHpChanged(float newHp)
    {
        if (HpChanged != null)
            HpChanged(this, new EventArgs<float>(newHp));
    }

    [Command]
    private void CmdChangeHp(float newHp)
    {
        _hp = newHp;
    }

}
