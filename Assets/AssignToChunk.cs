using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignToChunk : MonoBehaviour
{
    ChunkLoader loader;
    Vector2Int lastFramePos;
    Vector2Int pos;
    void Start()
    {
        lastFramePos = new(100, 100);
        loader = ChunkLoader.Instance;
        pos = new(Mathf.RoundToInt(transform.position.x / loader.chunkSize), Mathf.RoundToInt(transform.position.y / loader.chunkSize));
        ChangeParent();
    }

    // Update is called once per frame
    void Update()
    {
        pos = new(Mathf.RoundToInt(transform.position.x / loader.chunkSize), Mathf.RoundToInt(transform.position.y / loader.chunkSize));
        ChangeParent();
        //if (lastFramePos == pos)
        //{
        //    ChangeParent(pos);
        //}
        //else
        //{
        //    ChangeParent(pos);
        //}
        //lastFramePos = pos;
    }

    void ChangeParent()
    {
        if (!loader.spaceChunkDictionary.ContainsKey(pos) || !loader.spaceChunkDictionary[pos].chunkGO.activeSelf) gameObject.SetActive(false);
        else loader.spaceChunkDictionary[pos].entities.Add(gameObject);

        if (lastFramePos == new Vector2Int(100, 100) || !loader.spaceChunkDictionary.ContainsKey(lastFramePos)) { }
        else loader.spaceChunkDictionary[lastFramePos].entities.Remove(gameObject);
    }
    bool quitting;
    private void OnApplicationQuit()
    {
        quitting = true;
    }
    private void OnDestroy()
    {
        if (quitting) return;
        loader.spaceChunkDictionary[pos].entities.Remove(gameObject);
    }
}
