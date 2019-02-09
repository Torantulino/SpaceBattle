using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMove : MonoBehaviour
{
	public float speed;
	public float rotation;
	// Update is called once per frame
	void Update()
	{

		float mod = Mathf.Sin(Time.time);

		gameObject.transform.position += new Vector3(mod * speed, 0, 0) * Time.deltaTime;

		Vector3 rot = new Vector3(0, 0, mod * 90);
		gameObject.transform.rotation = Quaternion.Euler(rot);

	}
}