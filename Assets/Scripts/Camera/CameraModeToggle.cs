using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraModeToggle : MonoBehaviour
{
	[SerializeField] GameObject FollowPoint;
	[SerializeField] CinemachineFreeLook cam;
	[SerializeField] float buildHeight = 5.08f;
	[SerializeField] float buildRadius = 5.08f;
	[SerializeField] float combatHeight = 1.08f;
	[SerializeField] float combatRadius = 1.08f;
	private bool buildMode = false;

	public float lastFrameCamX;
	public float lastFrameCamY;

	// Use lateupdate to try to act after any Cinemachine scripts to avoid jitter (not really working)
	void LateUpdate()
	{

		if (Input.GetKeyDown(KeyCode.Tab))
		{
			buildMode = !buildMode;
		}

		//if in build mode don't invert mouse and more focus point ot centre on ship
		if (buildMode)
		{
			FollowPoint.transform.localPosition = new Vector3(0, 0, 0);
			cam.m_YAxis.m_InvertAxis = false;

			//set centre radius
			cam.m_Orbits[1].m_Radius = buildRadius;

			//set top and bottom heights
			cam.m_Orbits[0].m_Height = buildHeight;
			cam.m_Orbits[2].m_Height = -buildHeight;

			//stop camera moving if C is held down
			if (Input.GetKey(KeyCode.C))
			{
				cam.m_XAxis.Value = lastFrameCamX;
				cam.m_YAxis.Value = lastFrameCamY;
			}

			//update the last known state of the camera
			lastFrameCamX = cam.m_XAxis.Value;
			lastFrameCamY = cam.m_YAxis.Value;

		}
		else
		{
			FollowPoint.transform.localPosition = new Vector3(0, 1.77f, 0);
			cam.m_YAxis.m_InvertAxis = true;
			cam.m_Orbits[1].m_Radius = 1.08f;

			//set centre radius
			cam.m_Orbits[1].m_Radius = combatRadius;

			//set top and bottom heights
			cam.m_Orbits[0].m_Height = combatHeight;
			cam.m_Orbits[2].m_Height = -combatHeight;
		}

	}
}