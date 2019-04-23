using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [FMODUnity.EventRef]
    public List<string> itemEventStrings;
    public List<FMOD.Studio.EventInstance> audioEvents;

    int musicIndex = 7;

    // Use this for initialization
    void Start()
    {
        audioEvents = new List<FMOD.Studio.EventInstance>
        { { FMODUnity.RuntimeManager.CreateInstance(itemEventStrings[0]) }, //laser shot
            { FMODUnity.RuntimeManager.CreateInstance(itemEventStrings[1]) }, //thrusters
            { FMODUnity.RuntimeManager.CreateInstance(itemEventStrings[2]) }, //mining laser
            { FMODUnity.RuntimeManager.CreateInstance(itemEventStrings[3]) }, //UI blip
            { FMODUnity.RuntimeManager.CreateInstance(itemEventStrings[4]) }, //inventory pick up
            { FMODUnity.RuntimeManager.CreateInstance(itemEventStrings[5]) }, //inventory place
            { FMODUnity.RuntimeManager.CreateInstance(itemEventStrings[6]) }, //hit marker
            { FMODUnity.RuntimeManager.CreateInstance(itemEventStrings[7]) }, //shield alarm
            { FMODUnity.RuntimeManager.CreateInstance(itemEventStrings[8]) }, //song 1
            { FMODUnity.RuntimeManager.CreateInstance(itemEventStrings[9]) }, //song 2
            { FMODUnity.RuntimeManager.CreateInstance(itemEventStrings[10]) }, //song 3
            { FMODUnity.RuntimeManager.CreateInstance(itemEventStrings[11]) }, //song 4
            { FMODUnity.RuntimeManager.CreateInstance(itemEventStrings[12]) } //song 5
        };

    }

    // Update is called once per frame
    void Update()
    {

        FMOD.Studio.PLAYBACK_STATE playbackState;
        audioEvents[musicIndex].getPlaybackState(out playbackState);
        //if music instance has stopped
        if (playbackState == FMOD.Studio.PLAYBACK_STATE.STOPPED)
        {
            //increment index
            musicIndex++;
            if (musicIndex > 12 || musicIndex < 7)
                musicIndex = 7;

            //play next music instance
            audioEvents[musicIndex].start();
        }
    }
}