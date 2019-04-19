using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    [FMODUnity.EventRef]
    public List<string> itemEventStrings;
    public List<FMOD.Studio.EventInstance> audioEvents;

    // Use this for initialization
    void Start () {
        audioEvents = new List<FMOD.Studio.EventInstance> {
                { FMODUnity.RuntimeManager.CreateInstance(itemEventStrings[0]) },
                { FMODUnity.RuntimeManager.CreateInstance(itemEventStrings[1]) },
                { FMODUnity.RuntimeManager.CreateInstance(itemEventStrings[2]) },
        };
    }

    // Update is called once per frame
    void Update () {
		
	}
}
