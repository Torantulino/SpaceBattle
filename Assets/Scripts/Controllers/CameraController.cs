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

    public static GameObject CameraAnchor;

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

    private CinemachineFreeLook _cinemachineFreeLookCamera;
    private CinemachineVirtualCamera _flightCamera;

    // Use this for initialization
    void Start()
    {
        _cinemachineFreeLookCamera = GetComponentInChildren<CinemachineFreeLook>();
        _flightCamera = transform.Find("CM 3rd-Person-FlightMode").GetComponent<CinemachineVirtualCamera>();    //Obtaining ref by GetComponent wasn't working
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.LocalPlayer == null)
        {
            return;
        }

        // Setting Player to follow the camera anchor on the active player
        if (!_cinemachineFreeLookCamera.m_Follow)
        {
            CameraAnchor = GameController.LocalPlayer.transform.Find("CameraAnchor").gameObject;
            _cinemachineFreeLookCamera.m_Follow = CameraAnchor.transform;
            _cinemachineFreeLookCamera.m_LookAt = CameraAnchor.transform;

        }
        //Set flightcam Follow and Lookat
        if (!_flightCamera.m_Follow)
        {
            //Follow player
            _flightCamera.m_Follow = GameController.LocalPlayer.transform;
            //Lookat Anchor
            _flightCamera.m_LookAt = GameController.LocalPlayer.transform.Find("CameraAnchor").gameObject.transform;
        }
    }
}