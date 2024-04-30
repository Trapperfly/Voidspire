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

    public GameObject emitterPrefab;
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
        DontDestroyOnLoad(gameObject);
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

    public void PlayEmitter(EventReference _event, Transform transform)
    {
        GameObject _emitterGO = Instantiate(emitterPrefab, transform);
        StudioEventEmitter _emitter = InitEmitter(_event, _emitterGO);
        _emitter.Play();
    }
    public StudioEventEmitter PlayEmitterWithReturn(EventReference _event, Transform transform)
    {
        GameObject _emitterGO = Instantiate(emitterPrefab, transform);
        StudioEventEmitter _emitter = InitEmitter(_event, _emitterGO);
        _emitter.Play();
        return _emitter;
    }
    public void PlayEmitter(EventReference _event, Transform transform, int param)
    {
        GameObject _emitterGO = Instantiate(emitterPrefab, transform);
        StudioEventEmitter _emitter = InitEmitter(_event, _emitterGO);
        _emitter.EventInstance.setParameterByName("EnemyAction", param);
        _emitter.Play();
    }
    public StudioEventEmitter PlayEmitterWithReturn(EventReference _event, Transform transform, int param)
    {
        GameObject _emitterGO = Instantiate(emitterPrefab, transform);
        StudioEventEmitter _emitter = InitEmitter(_event, _emitterGO);
        _emitter.EventInstance.setParameterByName("EnemyAction", param);
        _emitter.Play();
        return _emitter;
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
