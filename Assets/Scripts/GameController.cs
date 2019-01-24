using System;
using UnityEngine;

public class GameController : MonoBehaviour {

    #region Singleton
    private static GameController instance;

    public static GameController Instance { get { return instance; } }

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

    public static GameObject LocalPlayer { get; private set; }
    public static PlayerController LocalPlayerController { get; private set; }

    public static void SetLocalPlayer(GameObject player)
    {
        LocalPlayer = player;
        LocalPlayerController = player.GetComponent<PlayerController>();
    }


}

public class EventArgs<T> : EventArgs
{
    public EventArgs(T value)
    {
        Value = value;
    }

    public T Value { get; private set; }
}