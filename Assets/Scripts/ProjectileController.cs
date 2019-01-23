﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileController : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.right * 10f, ForceMode.Impulse);
    }

}