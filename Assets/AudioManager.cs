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
    private EventInstance AmusicEvent;
    private EventInstance EmusicEvent;
    private EventInstance CmusicEvent;
    private EventInstance MmusicEvent;

    public float combatTime;
    public float explorationTime;
    public float magicTime;

    public float ambientVolume;
    public float combatVolume;
    public float explorationVolume;
    public float magicVolume;

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
        magicTime = 100;
        AmusicEvent = CreateInstance(FMODEvents.Instance.Amusic);
        AmusicEvent.setVolume(0);
        AmusicEvent.start();
        EmusicEvent = CreateInstance(FMODEvents.Instance.Cmusic);
        EmusicEvent.setVolume(0);
        EmusicEvent.start();
        CmusicEvent = CreateInstance(FMODEvents.Instance.Cmusic);
        CmusicEvent.setVolume(0);
        MmusicEvent = CreateInstance(FMODEvents.Instance.Mmusic);
        MmusicEvent.setVolume(0);
        MmusicEvent.start();
    }
    private void Update()
    {
        magicTime -= Time.deltaTime;
        combatTime -= Time.deltaTime;
        explorationTime -= Time.deltaTime;

        if (!isPlayingCombat && combatTime > 0) { CmusicEvent.start(); isPlayingCombat = true; }
        else if (isPlayingCombat && combatTime <= 0) CmusicEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        ambientVolume = Mathf.Lerp(ambientVolume,
            Mathf.Clamp(1 - (Mathf.Clamp(magicTime,0,1) + Mathf.Clamp(combatTime, 0, 1) + Mathf.Clamp(explorationTime, 0, 1)), 0, 1), 0.01f);
        AmusicEvent.setVolume(ambientVolume * SettingsManager.Instance.audioSettings[0] * SettingsManager.Instance.audioSettings[2]);
        combatVolume = Mathf.Lerp(combatVolume, Mathf.Clamp(combatTime, 0, 1), 0.01f);
        CmusicEvent.setVolume(combatVolume * SettingsManager.Instance.audioSettings[0] * SettingsManager.Instance.audioSettings[2]);
        explorationVolume = Mathf.Lerp(explorationVolume, Mathf.Clamp(Mathf.Clamp(explorationTime, 0, 1) - Mathf.Clamp(combatTime, 0, 1),0,1), 0.01f);
        EmusicEvent.setVolume(explorationVolume * SettingsManager.Instance.audioSettings[0] * SettingsManager.Instance.audioSettings[2]);
        magicVolume = Mathf.Lerp(magicVolume, 
            Mathf.Clamp(Mathf.Clamp(magicTime, 0, 1) - Mathf.Clamp(explorationTime, 0, 1) - Mathf.Clamp(combatTime, 0, 1),0,1), 0.01f);
        MmusicEvent.setVolume(magicVolume * SettingsManager.Instance.audioSettings[0] * SettingsManager.Instance.audioSettings[2]);

        //if (magicTime > 0) magicRuntime += Time.deltaTime; else magicRuntime = 0;

        //if (combatRuntime > 0) combatRuntime += Time.deltaTime; else combatRuntime = 0;

        //if (explorationRuntime > 0) explorationRuntime += Time.deltaTime; else explorationRuntime = 0;

        //if (combatTime > 0)
        //{
        //    if (isPlayingCombat) return;
        //    isPlayingCombat = true;
        //    isPlayingAmbient = false;
        //    isPlayingExploration = false;
        //    isPlayingMagic = false;
        //    SetMusicStyle(MusicStyle.COMBAT);
        //}
        //else if (explorationTime > 0)
        //{
        //    if (isPlayingExploration) return;
        //    isPlayingCombat = false;
        //    isPlayingAmbient = false;
        //    isPlayingExploration = true;
        //    isPlayingMagic = false;
        //    SetMusicStyle(MusicStyle.ADVENTURE);
        //}
        //else if (magicTime > 0)
        //{
        //    if (isPlayingMagic) return;
        //    isPlayingCombat = false;
        //    isPlayingAmbient = false;
        //    isPlayingExploration = false;
        //    isPlayingMagic = true;
        //    SetMusicStyle(MusicStyle.MAGIC);
        //}
        //else
        //{
        //    if (isPlayingAmbient) return;
        //    isPlayingCombat = false;
        //    isPlayingAmbient = true;
        //    isPlayingExploration = false;
        //    isPlayingMagic = false;
        //    SetMusicStyle(MusicStyle.AMBIENT);
        //}
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

    public void SetMusicStyle(MusicStyle style)
    {
        if (style == MusicStyle.AMBIENT) musicEvent.setTimelinePosition(Random.Range(0,120000));
        musicEvent.setParameterByName("musicfocus", (int)style);
        musicEvent.setVolume(0);
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
