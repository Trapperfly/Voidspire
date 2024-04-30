using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class LoadChunk : MonoBehaviour
{
    public Chunk chunk;
    public float debrisValue;
    public float factionValue;
    public float yieldValue;
    public bool eventValue;
    public bool shopValue;
    public float chunkDif;
    public int chunkLevel = 1;
    ChunkLoader loader;

    private void Start()
    {
        loader = ChunkLoader.Instance;
        debrisValue = Mathf.Pow(chunk.debrisValue, 3);
        factionValue = Mathf.Pow(chunk.factionValue, 3);
        yieldValue = chunk.yieldValue;
        eventValue = chunk.chunkEvent;
        shopValue = chunk.shop;
        chunkDif = chunk.chunkDif;
        chunkLevel = (10 * (GlobalRefs.Instance.currentSector - 1)) + 1 + Mathf.RoundToInt(Mathf.Pow(chunkDif, 2) * 9);
        StartCoroutine(LoadDebris(ChunkLoader.debrisMultiplier));
        StartCoroutine(LoadFaction());
    }

    IEnumerator LoadDebris(float amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (Random.value < Mathf.Pow(chunk.debrisValue, 3))
            {
                SpawnDebris.Instance.Spawn(transform.position, loader.chunkSize / 2, chunkLevel);
            }  
        }
        yield return null;
    }

    IEnumerator LoadFaction()
    {
        StartCoroutine(LoadVoidEnemies(ChunkLoader.voidMultiplier, chunkLevel));
        StartCoroutine(LoadChitinEnemies(ChunkLoader.chitinMultiplier, chunkLevel));
        //StartCoroutine(LoadChromeEnemies(ChunkLoader.chromeMultiplier, Mathf.RoundToInt(chunkDif)));
        StartCoroutine(LoadPirateEnemies(ChunkLoader.pirateMultiplier, chunkLevel));
        //StartCoroutine(LoadCiv(ChunkLoader.civMultiplier, Mathf.RoundToInt(chunkDif)));
        yield return null;
    }

    IEnumerator LoadVoidEnemies(float amount, int level)
    {
        for (int i = 0; i < amount; i++)
        {
            if (Random.value < Mathf.Pow(chunk.voidValue, 3))
                SpawnEnemies.Instance.SpawnVoidEnemies(transform.position, loader.chunkSize / 2, level);
        }
        yield return null;
    }
    IEnumerator LoadChitinEnemies(float amount, int level)
    {
        if (Random.value < Mathf.Pow(chunk.chitinValue, 5))
            SpawnEnemies.Instance.SpawnChitinEnemies(transform.position, loader.chunkSize / 2, level);
        yield return null;
    }
    IEnumerator LoadChromeEnemies(float amount, int level)
    {
        for (int i = 0; i < amount; i++)
        {
            if (Random.value < Mathf.Pow(chunk.chromeValue, 3))
                SpawnEnemies.Instance.SpawnChromeEnemies(transform.position, loader.chunkSize / 2, level);
        }
        yield return null;
    }
    IEnumerator LoadPirateEnemies(float amount, int level)
    {
        if (Random.value < Mathf.Pow(chunk.pirateValue, 3))
            SpawnEnemies.Instance.SpawnPirateEnemies(transform.position, loader.chunkSize / 2, level);
        yield return null;
    }
    IEnumerator LoadCiv(float amount, int level)
    {
        for (int i = 0; i < amount; i++)
        {
            if (Random.value < Mathf.Pow(chunk.civValue, 3))
                SpawnEnemies.Instance.SpawnCiv(transform.position, loader.chunkSize / 2, level);
        }
        yield return null;
    }
}
