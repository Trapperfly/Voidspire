using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    private List<EventInstance> eventInstances;
    private List<StudioEventEmitter> eventEmitters;
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
        eventEmitters = new List<StudioEventEmitter>();
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

    public StudioEventEmitter InitEmitter(EventReference audio, GameObject emitterGO)
    {
        StudioEventEmitter emitter = emitterGO.GetComponent<StudioEventEmitter>();
        emitter.EventReference = audio;
        eventEmitters.Add(emitter);
        return emitter;
    }

    void CleanUp()
    {
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
        foreach (StudioEventEmitter emitter in eventEmitters)
        {
            emitter.Stop();
        }
    }

    private void OnDestroy()
    {
        CleanUp();
    }
}
