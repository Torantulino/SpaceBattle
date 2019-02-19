using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraModeToggle : MonoBehaviour
{
    [SerializeField] GameObject FollowPoint;
    [SerializeField] CinemachineFreeLook buildCam;
    CinemachineVirtualCamera flightCam;
    [SerializeField] float buildHeight = 5.08f;
    [SerializeField] float buildRadius = 5.08f;
    [SerializeField] float combatHeight = 1.08f;
    [SerializeField] float combatRadius = 1.08f;
    private bool buildMode = false;

    public float lastFrameCamX;
    public float lastFrameCamY;

	private void Start()
	{
		buildCam = GetComponentInChildren<CinemachineFreeLook>();
        flightCam = transform.Find("CM 3rd-Person-FlightMode").GetComponent<CinemachineVirtualCamera>();
    }

	// Use lateupdate to try to act after any Cinemachine scripts to avoid jitter (not really working)
	void LateUpdate()
	{
		//if a player with a camera anchor exists:
		if (CameraController.CameraAnchor != null)
		{
			//if in build mode don't invert mouse and more focus point ot centre on ship
			if (buildMode)
			{
                //Switch Cameras
                flightCam.gameObject.SetActive(false);
                buildCam.gameObject.SetActive(true);

				CameraController.CameraAnchor.transform.localPosition = new Vector3(0, 0, 0);
				buildCam.m_YAxis.m_InvertAxis = false;

				//set centre radius
				buildCam.m_Orbits[1].m_Radius = buildRadius;

				//set top and bottom heights
				buildCam.m_Orbits[0].m_Height = buildHeight;
				buildCam.m_Orbits[2].m_Height = -buildHeight;

				//stop camera moving if Shift is held down
				if (Input.GetKey(KeyCode.LeftShift))
				{
					buildCam.m_XAxis.Value = lastFrameCamX;
					buildCam.m_YAxis.Value = lastFrameCamY;
					buildCam.m_XAxis.m_InputAxisValue = 0;
					buildCam.m_YAxis.m_InputAxisValue = 0;
				}

				//update the last known state of the camera
				lastFrameCamX = buildCam.m_XAxis.Value;
				lastFrameCamY = buildCam.m_YAxis.Value;

			}
            //Flight mode
			else
			{
                //Switch Cameras
                flightCam.gameObject.SetActive(true);
                buildCam.gameObject.SetActive(false);

                CameraController.CameraAnchor.transform.localPosition = new Vector3(0.0f, 0.0f, 10.0f);
				//cam.m_YAxis.m_InvertAxis = true;
				//cam.m_Orbits[1].m_Radius = 1.08f;

				////set centre radius
				//cam.m_Orbits[1].m_Radius = combatRadius;

				////set top and bottom heights
				//cam.m_Orbits[0].m_Height = combatHeight;
				//cam.m_Orbits[2].m_Height = -combatHeight;

				if (Input.GetKey(KeyCode.LeftShift))
				{
					Cursor.lockState = CursorLockMode.None;
					Cursor.visible = true;
				}
				else
				{
					Cursor.lockState = CursorLockMode.Locked;
					Cursor.visible = false;
				}
			}
		}
		else
		{
			Debug.LogWarning("Waiting for valid camera Player/CameraAnchor");
		}
	}

    public void UpdateBuildmode(bool b)
    {
        buildMode = b;  //Hard set rather than toggle to ensure sync
    }

}