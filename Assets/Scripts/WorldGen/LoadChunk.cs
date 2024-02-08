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
        StartCoroutine(LoadFaction(ChunkLoader.factionMultiplier));
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

    IEnumerator LoadFaction(float amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (Random.value < Mathf.Pow(chunk.factionValue, 3))
                SpawnEnemies.Instance.Spawn(transform.position, loader.chunkSize / 2);
        }
        yield return null;
    }
}
