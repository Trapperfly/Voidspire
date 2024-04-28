using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class StartSectorMusic : MonoBehaviour
{
    EventInstance backgroundMusic;
    // Start is called before the first frame update
    void Start()
    {
        backgroundMusic = AudioManager.Instance.CreateInstance(FMODEvents.Instance.spaceAmbient);
        StartPlayback(backgroundMusic);
    }
    void StartPlayback(EventInstance audio)
    {
        PLAYBACK_STATE state;
        audio.getPlaybackState(out state);
        if (state.Equals(PLAYBACK_STATE.STOPPED))
        {
            audio.start();
        }
    }
    void StopPlayback(EventInstance audio, STOP_MODE stopMode)
    {
        audio.stop(stopMode);
    }
}
