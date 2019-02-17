using Cinemachine;
using UnityEngine;

/// <summary>
/// Controls Main Camera.
/// </summary>
public class CameraController : MonoBehaviour
{

    #region Singleton
    private static CameraController instance;

    public static CameraController Instance { get { return instance; } }

    public static Transform CameraAnchor;

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

    private CinemachineVirtualCamera _cinemachineVirtualCamera;

    // Use this for initialization
    void Start()
    {
        _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameController.LocalPlayer)
        {
            return;
        }
        // Setting Player to follow the camera anchor on the active player
        if (!_cinemachineVirtualCamera.m_Follow)
        {
            CameraAnchor = GameController.LocalPlayer.GetComponentsInChildren<Transform>() [1];
            _cinemachineVirtualCamera.m_Follow = CameraAnchor;
            print(CameraAnchor.position.x);
        }

    }
}