using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkLoader : MonoBehaviour
{
    public int seed;

    public Material unlitMat;
    public Sprite testingSprite;

    public Transform chunkHolder;

    //Chunk chunk;

    public int viewDist;

    public int chunkSize;

    public float debrisScale;
    public float debrisMultiplier;

    public float factionScale;
    public float factionMultiplier;

    public float shopScale;

    public float yieldScale;
    public float yieldMultiplier;

    public float eventScale;

    public float shopThreshold = 0.95f;
    public float eventThreshold = 0.95f;

    //Dictionary<Vector2Int, Chunk> chunks = new Dictionary<Vector2Int, Chunk>();
    Chunk[,] grid;

    #region Singleton
    public static ChunkLoader Instance;

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
    }
    #endregion

    private void Start()
    {
        GenerateChunks();
    }

    public void GenerateChunks()
    {
        float xSeed = Random.Range(-10000, 10000);
        float ySeed = Random.Range(-10000, 10000);
        float[,] debrisMap = new float[viewDist * 2 + 1, viewDist * 2 + 1];
        float debrisSeed = Random.Range(-10000, 10000);
        float[,] factionMap = new float[viewDist * 2 + 1, viewDist * 2 + 1];
        float factionSeed = Random.Range(-10000, 10000);
        float[,] shopMap = new float[viewDist * 2 + 1, viewDist * 2 + 1];
        float shopSeed = Random.Range(-10000, 10000);
        float[,] yieldMap = new float[viewDist * 2 + 1, viewDist * 2 + 1];
        float yieldSeed = Random.Range(-10000, 10000);
        float[,] eventMap = new float[viewDist * 2 + 1, viewDist * 2 + 1];
        float eventSeed = Random.Range(-10000, 10000);

        grid = new Chunk[viewDist * 2 + 1, viewDist * 2 + 1];
        //Generate chunks to dictionary based on view distance
        for (int x = 0; x < viewDist * 2 + 1; x++)
        {
            for(int y = 0; y < viewDist * 2 + 1; y++)
            {
                Chunk chunk = new Chunk();

                float debrisValue = Mathf.PerlinNoise(x * debrisScale + xSeed + debrisSeed, y * debrisScale + ySeed + debrisSeed);
                debrisMap[x, y] = debrisValue;
                chunk.debrisValue = debrisValue;

                float factionValue = Mathf.PerlinNoise(x * factionScale + xSeed + factionSeed, y * factionScale + ySeed + factionSeed);
                factionMap[x, y] = factionValue;
                chunk.factionValue = factionValue;

                float shopValue = Mathf.PerlinNoise(x * shopScale + xSeed + shopSeed, y * shopScale + ySeed + shopSeed);
                shopMap[x, y] = shopValue;
                chunk.shop = shopValue > shopThreshold;

                float yieldValue = Mathf.PerlinNoise(x * yieldScale + xSeed + yieldSeed, y * yieldScale + ySeed + yieldSeed);
                yieldMap[x, y] = yieldValue;
                chunk.yieldValue = yieldValue;

                float eventValue = Mathf.PerlinNoise(x * eventScale + xSeed + eventSeed, y * eventScale + ySeed + eventSeed);
                eventMap[x, y] = eventValue;
                chunk.chunkEvent = eventValue > eventThreshold;

                GameObject newChunk = CreateNewChunk(x,y);
                newChunk.AddComponent<LoadChunk>().chunk = chunk;

                GameObject spriteHolder = new(name = "Sprite");
                spriteHolder.transform.parent = newChunk.transform;
                spriteHolder.transform.localScale = new Vector3(chunkSize,chunkSize,1);
                spriteHolder.transform.localPosition = Vector3.zero;
                spriteHolder.layer = 29;

                SpriteRenderer sprite = spriteHolder.AddComponent<SpriteRenderer>();
                sprite.material = unlitMat;
                sprite.sortingOrder = -10;
                if ( chunk.chunkEvent ) { sprite.color = new Color(1, 0, 0, 1); }
                else if ( chunk.shop ) { sprite.color = new Color(0, 1, 0, 1); }
                else { sprite.color = new Color(factionValue, yieldValue, debrisValue, 1); }

                spriteHolder.GetComponent<SpriteRenderer>().sprite = testingSprite;
                //chunks.Add(new Vector2Int(i,j), newChunk.GetComponent<Chunk>());

                grid[x, y] = chunk;
            }
        }
        StartCoroutine(nameof(Log));
    }

    IEnumerator Log()
    {
        yield return new WaitForEndOfFrame();
        Debug.Log("Loaded " + chunkHolder.childCount + " chunks...");
        Debug.Log("Loaded " + SpawnDebris.Instance.transform.childCount + " debris...");
        Debug.Log("Loaded " + SpawnEnemies.Instance.transform.childCount + " enemies...");
        yield return null;
    }

    GameObject CreateNewChunk(int x, int y)
    {
        //Debug.Log("Creating chunk " + x + y);
        GameObject newChunk = new()
        {
            name = "Chunk" + x.ToString() + y.ToString()
        };
        newChunk.transform.parent = chunkHolder;
        newChunk.transform.position =
            new Vector2(
                (x * chunkSize)
            + chunkHolder.position.x
            - (viewDist * chunkSize),
                (y * chunkSize)
            + chunkHolder.position.y
            - (viewDist * chunkSize));
        return newChunk;
    }

    float GenerateNoise(int coordX, int coordY)
    {
        float v = Mathf.PerlinNoise(coordX, coordY);
        return v;
    }
}
