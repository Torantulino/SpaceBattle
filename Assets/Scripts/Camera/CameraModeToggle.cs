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
    private float bounds;

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
				buildCam.m_Orbits[1].m_Radius = buildRadius + bounds;

				//set top and bottom heights
				buildCam.m_Orbits[0].m_Height = buildHeight + bounds;
				buildCam.m_Orbits[2].m_Height = -buildHeight - bounds;

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

                //Move lookat anchor infront of ship
                CameraController.CameraAnchor.transform.localPosition = new Vector3(0.0f, 0.0f, 10.0f * bounds);

                //Release cursor
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

    //            //Release mouse - Testing only?
    //            if (Input.GetKey(KeyCode.LeftShift))
				//{
				//	Cursor.lockState = CursorLockMode.None;
				//	Cursor.visible = true;
				//}
				//else
				//{
				//	Cursor.lockState = CursorLockMode.Locked;
				//	Cursor.visible = false;
				//}
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

        //update bounds every switch
        CalculateBounds();
    }

    //Returns and sets the total bounds for the player and it's children. This includes the 'ghost' of unbuilt parts (as you want to see them) but not the visualiser.
    public float CalculateBounds()
    {
        float furthest = 0.0f;

        //Obtain the furthest out value
        foreach (Renderer childRend in GameController.LocalPlayer.GetComponentsInChildren<Renderer>()) {
            if (childRend.name != "Visualiser" && childRend.name != "Crosshair") { 
                Vector3 min = childRend.bounds.min - GameController.LocalPlayer.transform.position;
                Vector3 max = childRend.bounds.max - GameController.LocalPlayer.transform.position;

                if (min.magnitude > furthest)
                    furthest = min.magnitude;

                if (max.magnitude > furthest)
                    furthest = max.magnitude;
            }
        }
        bounds = furthest;
        flightCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.z = -1.5f * (7.0f + bounds);
        return furthest;
    }

}