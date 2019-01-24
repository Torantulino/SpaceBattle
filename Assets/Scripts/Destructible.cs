using System;
using UnityEngine;
using UnityEngine.Networking;

public class Destructible : NetworkBehaviour
{
    [SyncVar(hook = "OnHpChanged")]
    private float hp;

    public event EventHandler<EventArgs<float>> HpChanged;

    public void Start()
    {
        OnHpChanged(hp);
    }

    public float GetHp()
    {
        return hp;
    }

    private void OnHpChanged(float newHp)
    {
        if (HpChanged != null)
            HpChanged(this, new EventArgs<float>(newHp));
    }

}
