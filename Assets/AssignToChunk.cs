using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignToChunk : MonoBehaviour
{
    ChunkLoader loader;
    Vector2Int lastFramePos;
    Vector2Int pos;
    bool currentlyCheckingForChunk;

    private void Awake()
    {
        quitting = true;
    }
    void Start()
    {
        quitting = false;
        lastFramePos = new(100, 100);
        loader = ChunkLoader.Instance;
        pos = new(Mathf.RoundToInt(transform.position.x / loader.chunkSize), Mathf.RoundToInt(transform.position.y / loader.chunkSize));
        ChangeParent();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        pos = new(Mathf.RoundToInt(transform.position.x / loader.chunkSize), Mathf.RoundToInt(transform.position.y / loader.chunkSize));
        ChangeParent();
        lastFramePos = pos;
    }

    void CheckForNewChunk()
    {
        Debug.Log("Checking for new chunk. It was " + (bool)loader.spaceChunkDictionary.ContainsKey(pos));
        if (loader.spaceChunkDictionary.ContainsKey(pos))
        {
            Debug.Log("Found new chunk");
            GetComponent<Rigidbody2D>().simulated = true;
            ChangeParent();
            CancelInvoke(nameof(CheckForNewChunk));
            currentlyCheckingForChunk = false;
        }
    }

    void CheckForDisabledChunk()
    {
        Debug.Log("Checking for new chunk. It was " + (bool)loader.spaceChunkDictionary.ContainsKey(pos));
        if (loader.spaceChunkDictionary[pos].chunkGO.activeSelf)
        {
            Debug.Log("Found new chunk");
            GetComponent<Rigidbody2D>().simulated = true;
            ChangeParent();
            CancelInvoke(nameof(CheckForNewChunk));
            currentlyCheckingForChunk = false;
        }
    }

    void ChangeParent()
    {

        if (!loader.spaceChunkDictionary.ContainsKey(pos))
        {
            GetComponent<Rigidbody2D>().simulated = false;
            if (!currentlyCheckingForChunk) { InvokeRepeating(nameof(CheckForNewChunk), 0, 0.5f); currentlyCheckingForChunk = true; }
        }

        else if (!loader.spaceChunkDictionary[pos].chunkGO.activeSelf)
        {
            Debug.Log("entering a disabled chunk"); 
            if(!loader.spaceChunkDictionary[pos].entities.Contains(gameObject))
                loader.spaceChunkDictionary[pos].entities.Add(gameObject);
            GetComponent<Rigidbody2D>().simulated = false;
            if (!currentlyCheckingForChunk) 
            { 
                InvokeRepeating(nameof(CheckForDisabledChunk), 0, 0.5f); 
                currentlyCheckingForChunk = true; 
            }
        }

        else if (!loader.spaceChunkDictionary[pos].entities.Contains(gameObject))
        {
            Debug.Log("I am " + gameObject + " and i set my new parent to " + pos);
            loader.spaceChunkDictionary[pos].entities.Add(gameObject);
        } 


        if (lastFramePos == new Vector2Int(100, 100) || !loader.spaceChunkDictionary.ContainsKey(lastFramePos)) { }
        else if (lastFramePos != pos && loader.spaceChunkDictionary[lastFramePos].entities.Contains(gameObject))
        {
            Debug.Log("I am " + gameObject + " and i cleared my old parent from " + pos);
            loader.spaceChunkDictionary[lastFramePos].entities.Remove(gameObject);
        }
            
    }
    bool quitting;
    private void OnApplicationQuit()
    {
        quitting = true;
    }
    private void OnDestroy()
    {
        if (quitting) return;
        if (!loader.spaceChunkDictionary.ContainsKey(pos)) { Debug.LogWarning("Chunk didnt exist"); return; }
        loader.spaceChunkDictionary[pos].entities.Remove(gameObject);
    }
}
