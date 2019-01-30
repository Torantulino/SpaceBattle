using Cinemachine;
using UnityEngine;

/// <summary>
/// Controls Main Camera.
/// </summary>
public class CameraController : MonoBehaviour {

    #region Singleton
    private static CameraController instance;

    public static CameraController Instance { get { return instance; } }

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
            return;

        // Setting Player to follow
        if (!_cinemachineVirtualCamera.m_Follow)
            _cinemachineVirtualCamera.m_Follow = GameController.LocalPlayer.transform;
    }
}
