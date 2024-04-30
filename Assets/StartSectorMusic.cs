using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class StartSectorMusic : MonoBehaviour
{
    public bool isMainMenu;
    EventInstance backgroundMusic;
    // Start is called before the first frame update
    void Start()
    {
        if (isMainMenu) backgroundMusic = AudioManager.Instance.CreateInstance(FMODEvents.Instance.mainMenuAmbient);
        else backgroundMusic = AudioManager.Instance.CreateInstance(FMODEvents.Instance.spaceAmbient);
        StartPlayback(backgroundMusic);
    }
    private void OnDestroy()
    {
        StopPlayback(backgroundMusic, STOP_MODE.ALLOWFADEOUT);
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
