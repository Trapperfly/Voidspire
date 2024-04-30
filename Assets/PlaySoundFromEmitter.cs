using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class PlaySoundFromEmitter : MonoBehaviour
{
    public StudioEventEmitter emitter;
    private void Start()
    {
        emitter = GetComponent<StudioEventEmitter>();
    }
    void Update()
    {
        if (emitter.IsPlaying()) return;
        Destroy(emitter.gameObject);
    }
}
