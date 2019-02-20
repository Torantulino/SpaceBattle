using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        for (int i = 0; i < 500; i++)
        {
            GameObject asteroid = GameObject.CreatePrimitive(PrimitiveType.Cube);
            asteroid.transform.position = Random.insideUnitSphere * 1000.0f;
            asteroid.transform.localScale = Vector3.one * Random.Range(5.0f, 40.0f);
            asteroid.transform.rotation = Random.rotation;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
