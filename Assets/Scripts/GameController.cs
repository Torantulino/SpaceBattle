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

    public static void SetLocalPlayer(GameObject player)
    {
        LocalPlayer = player;
    }

}
