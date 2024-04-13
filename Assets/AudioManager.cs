using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    private List<EventInstance> eventInstances;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        eventInstances = new List<EventInstance>();
    }

    public void PlayOneShot(EventReference audio, Vector2 pos)
    {
        RuntimeManager.PlayOneShot(audio, pos);
    }

    public EventInstance CreateInstance(EventReference audio)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(audio);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    void CleanUp()
    {
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
    }

    private void OnDestroy()
    {
        CleanUp();
    }
}
