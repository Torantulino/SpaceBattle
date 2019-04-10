using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

public class LevelManager : NetworkBehaviour
{

    private UnityEngine.Object[] asteroids;

    // Use this for initialization
    void Start()
    {
        if(!isServer)
            return;

        // Spawning asteroids only on server
        try
        {
            asteroids = Resources.LoadAll("asteroids", typeof(GameObject));
        }
        catch (Exception e)
        {
            Debug.Log("asteroid loading failed for the following reason: " + e);
        }

        //Generate map
        for (int i = 0; i < 100; i++)
        {
            int rand = UnityEngine.Random.Range(0, asteroids.Length - 1);

            GameObject asteroid = Instantiate((GameObject)asteroids[rand]);
            asteroid.transform.position = UnityEngine.Random.insideUnitSphere * 150.0f;
            //asteroid.transform.localScale = Vector3.one * UnityEngine.Random.Range(5.0f, 40.0f);
            asteroid.transform.rotation = UnityEngine.Random.rotation;
            NetworkServer.Spawn(asteroid);
        }
    }
}
