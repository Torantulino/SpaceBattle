using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : MonoBehaviour
{

    private UnityEngine.Object[] asteroids;

    // Use this for initialization
    void Start()
    {

        try
        {
            asteroids = Resources.LoadAll("asteroids", typeof(GameObject));
        }
        catch (Exception e)
        {
            Debug.Log("asteroid loading failed for the following reason: " + e);
        }

        //Generate map
        for (int i = 0; i < 500; i++)
        {
            int rand = UnityEngine.Random.Range(0, asteroids.Length - 1);

            GameObject asteroid = Instantiate((GameObject)asteroids[rand]);
            asteroid.transform.position = UnityEngine.Random.insideUnitSphere * 400.0f;
            //asteroid.transform.localScale = Vector3.one * UnityEngine.Random.Range(5.0f, 40.0f);
            asteroid.transform.rotation = UnityEngine.Random.rotation;
        }
    }
}
