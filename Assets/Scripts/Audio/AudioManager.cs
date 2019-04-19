﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    [FMODUnity.EventRef]
    public List<string> itemEventStrings;
    public List<FMOD.Studio.EventInstance> audioEvents;

    // Use this for initialization
    void Start () {

        for (int i = 0; i < itemEventStrings.Count; i++)

        {

            audioEvents.Add(FMODUnity.RuntimeManager.CreateInstance(itemEventStrings[i]));

        }

    }

    // Update is called once per frame
    void Update () {
		
	}
}
