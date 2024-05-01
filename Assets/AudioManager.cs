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

    private EventInstance musicEvent;

    public float combatTime;
    public float explorationTime;
    public float magicTime;

    bool isPlayingAmbient;
    bool isPlayingCombat;
    bool isPlayingExploration;
    bool isPlayingMagic;

    public enum MusicStyle
    {
        MAGIC = 0,
        ADVENTURE = 1,
        COMBAT = 2,
        AMBIENT = 3
    }

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
    private void Start()
    {
        magicTime = 0;
        InitMusic(FMODEvents.Instance.music);
    }
    private void Update()
    {
        magicTime -= Time.deltaTime;
        combatTime -= Time.deltaTime;
        explorationTime -= Time.deltaTime;
        if (combatTime > 0)
        {
            if (isPlayingCombat) return;
            isPlayingCombat = true;
            isPlayingAmbient = false;
            isPlayingExploration = false;
            isPlayingMagic = false;
            SetMusicStyle(MusicStyle.COMBAT);
        }
        else if (explorationTime > 0)
        {
            if (isPlayingExploration) return;
            isPlayingCombat = false;
            isPlayingAmbient = false;
            isPlayingExploration = true;
            isPlayingMagic = false;
            SetMusicStyle(MusicStyle.ADVENTURE);
        }
        else if (magicTime > 0)
        {
            if (isPlayingMagic) return;
            isPlayingCombat = false;
            isPlayingAmbient = false;
            isPlayingExploration = false;
            isPlayingMagic = true;
            SetMusicStyle(MusicStyle.MAGIC);
        }
        else
        {
            if (isPlayingAmbient) return;
            isPlayingCombat = false;
            isPlayingAmbient = true;
            isPlayingExploration = false;
            isPlayingMagic = false;
            SetMusicStyle(MusicStyle.AMBIENT);
        }
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

    private void InitMusic(EventReference reference)
    {
        musicEvent = CreateInstance(reference);
        musicEvent.start();
    }

    public void SetMusicStyle(MusicStyle style)
    {
        if (style == MusicStyle.AMBIENT) musicEvent.setTimelinePosition(Random.Range(0,120000));
        musicEvent.setParameterByName("musicfocus", (int)style);
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
        _emitter.Params[0].Value = param; //.SetParameter("EnemyAction", param);
        _emitter.Play();
    }
    public StudioEventEmitter PlayEmitterWithReturn(EventReference _event, Transform transform, int param)
    {
        GameObject _emitterGO = Instantiate(emitterPrefab, transform);
        StudioEventEmitter _emitter = InitEmitter(_event, _emitterGO);
        _emitter.Params[0].Value = param;
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
