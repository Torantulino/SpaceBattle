using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIndicator : MonoBehaviour
{
	// Use this for initialization

	// Update is called once per frame
	void Update()
	{
		if (GameController.LocalPlayer)
		{
			gameObject.transform.localRotation = GameController.LocalPlayer.transform.rotation;
		}
	}
}