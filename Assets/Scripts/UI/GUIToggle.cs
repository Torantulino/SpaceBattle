using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIToggle : MonoBehaviour
{

	public void Flip()
	{
		gameObject.SetActive(!gameObject.activeSelf);
	}
}