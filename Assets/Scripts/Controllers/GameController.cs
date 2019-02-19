using UnityEngine;

/// <summary>
/// Controls the game and players.
/// </summary>
public class GameController : MonoBehaviour
{

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

    #region Properties

    /// <summary>
    /// Local player GameObject
    /// </summary>
    public static GameObject LocalPlayer { get; private set; }

    /// <summary>
    /// Local player PlayerController script
    /// </summary>
    public static PlayerController LocalPlayerController { get; private set; }

    #endregion

    /// <summary>
    /// Sets reference to local player GameObject and its PlayerController script.
    /// </summary>
    /// <param name="player"></param>
    public static void SetLocalPlayer(GameObject player)
    {
        LocalPlayer = player;
        LocalPlayerController = player.GetComponent<PlayerController>();
    }

}