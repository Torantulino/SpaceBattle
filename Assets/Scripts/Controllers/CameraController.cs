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

    private CinemachineFreeLook _cinemachineVirtualCamera;

    // Use this for initialization
    void Start()
    {
        _cinemachineVirtualCamera = GetComponent<CinemachineFreeLook>();
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
            CameraAnchor = transform.Find("CameraAnchor");
            _cinemachineVirtualCamera.m_Follow = CameraAnchor;
            _cinemachineVirtualCamera.m_LookAt = CameraAnchor;

        }

    }
}