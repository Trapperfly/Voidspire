using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadChunk : MonoBehaviour
{
    public Chunk chunk;
    ChunkLoader loader;

    private void Start()
    {
        loader = ChunkLoader.Instance;
        StartCoroutine(LoadDebris(ChunkLoader.debrisMultiplier));
        StartCoroutine(LoadFaction(ChunkLoader.factionMultiplier));
    }

    IEnumerator LoadDebris(float amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (Random.value < chunk.debrisValue)
                SpawnDebris.Instance.Spawn(transform.position, loader.chunkSize / 2);
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }

    IEnumerator LoadFaction(float amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (Random.value < chunk.factionValue)
                SpawnEnemies.Instance.Spawn(transform.position, loader.chunkSize / 2);
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
}
