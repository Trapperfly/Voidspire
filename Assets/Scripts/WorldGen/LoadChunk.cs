using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadChunk : MonoBehaviour
{
    public Chunk chunk;
    public float debrisValue;
    public float factionValue;
    public float yieldValue;
    public bool eventValue;
    public bool shopValue;
    ChunkLoader loader;

    private void Start()
    {
        loader = ChunkLoader.Instance;
        debrisValue = Mathf.Pow(chunk.debrisValue, 3);
        factionValue = Mathf.Pow(chunk.factionValue, 3);
        yieldValue = chunk.yieldValue;
        eventValue = chunk.chunkEvent;
        shopValue = chunk.shop;
        StartCoroutine(LoadDebris(ChunkLoader.debrisMultiplier));
        StartCoroutine(LoadFaction());
    }

    IEnumerator LoadDebris(float amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (Random.value < Mathf.Pow(chunk.debrisValue, 3))
                SpawnDebris.Instance.Spawn(transform.position, loader.chunkSize / 2);
        }
        yield return null;
    }

    IEnumerator LoadFaction()
    {
        StartCoroutine(LoadVoidEnemies(ChunkLoader.voidMultiplier));
        StartCoroutine(LoadChitinEnemies(ChunkLoader.chitinMultiplier));
        //StartCoroutine(LoadChromeEnemies(ChunkLoader.chromeMultiplier));
        //StartCoroutine(LoadPirateEnemies(ChunkLoader.pirateMultiplier));
        //StartCoroutine(LoadCiv(ChunkLoader.civMultiplier));
        yield return null;
    }

    IEnumerator LoadVoidEnemies(float amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (Random.value < Mathf.Pow(chunk.voidValue, 3))
                SpawnEnemies.Instance.SpawnVoidEnemies(transform.position, loader.chunkSize / 2);
        }
        yield return null;
    }
    IEnumerator LoadChitinEnemies(float amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (Random.value < Mathf.Pow(chunk.chitinValue, 3))
                SpawnEnemies.Instance.SpawnChitinEnemies(transform.position, loader.chunkSize / 2);
        }
        yield return null;
    }
    IEnumerator LoadChromeEnemies(float amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (Random.value < Mathf.Pow(chunk.chromeValue, 3))
                SpawnEnemies.Instance.SpawnChromeEnemies(transform.position, loader.chunkSize / 2);
        }
        yield return null;
    }
    IEnumerator LoadPirateEnemies(float amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (Random.value < Mathf.Pow(chunk.pirateValue, 3))
                SpawnEnemies.Instance.SpawnPirateEnemies(transform.position, loader.chunkSize / 2);
        }
        yield return null;
    }
    IEnumerator LoadCiv(float amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (Random.value < Mathf.Pow(chunk.civValue, 3))
                SpawnEnemies.Instance.SpawnCiv(transform.position, loader.chunkSize / 2);
        }
        yield return null;
    }
}
