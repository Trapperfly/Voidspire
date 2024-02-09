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

    public const float viewDist = 20;
    public const float destroyDist = 100;
    public Transform viewer;
    public static Vector2 viewerPosition;
    public int chunkSize;
    public int chunksVisibleInViewDist;

    public Dictionary<Vector2, SpaceChunk> spaceChunkDictionary = new();
    List<SpaceChunk> chunksVisibleLastFrame = new();
    

    public static float debrisScale = 0.06f;
    public static float debrisMultiplier = 20;

    public static float factionScale = 0.03f;
    public static float factionMultiplier = 3;

    public static float shopScale = 0.6f;

    public static float yieldScale = 0.06f;
    public static float yieldMultiplier = 2;

    public static float eventScale = 0.5f;

    public static float shopThreshold = 0.90f;
    public static float eventThreshold = 0.90f;

    //static float xSeed;
    //static float ySeed;
    //static float debrisSeed;
    //static float factionSeed;
    //static float shopSeed;
    //static float yieldSeed;
    //static float eventSeed;

    public static List<SpaceChunk> toBeDestroyed = new();

    //Dictionary<Vector2Int, Chunk> chunks = new Dictionary<Vector2Int, Chunk>();
    //Chunk[,] grid;

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
        chunksVisibleInViewDist = Mathf.RoundToInt(viewDist / chunkSize);
        InvokeRepeating(nameof(UpdateHiddenChunks), 0,2);
        //GenerateChunks();
    }

    private void Update()
    {
        viewerPosition = new Vector2(viewer.position.x, viewer.position.y);
        GlobalRefs.playerPos = viewerPosition;
        UpdateVisibleChunks();
    }
    void UpdateVisibleChunks()
    {
        for (int i = 0; i < chunksVisibleLastFrame.Count; i++)
        {
            //chunksVisibleLastFrame[i].SetVisible(false);
        }
        chunksVisibleLastFrame.Clear();

        int currentChunkCoordX = Mathf.RoundToInt(viewerPosition.x/ chunkSize);
        int currentChunkCoordY = Mathf.RoundToInt(viewerPosition.y / chunkSize);

        for (int yOffset = -chunksVisibleInViewDist;  yOffset <= chunksVisibleInViewDist; yOffset++) {
            for (int xOffset = -chunksVisibleInViewDist; xOffset <= chunksVisibleInViewDist; xOffset++) {
                Vector2 viewedChunkCoord = new(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

                if (spaceChunkDictionary.ContainsKey(viewedChunkCoord))
                {
                    spaceChunkDictionary[viewedChunkCoord].UpdateSpaceChunk();
                    if (spaceChunkDictionary[viewedChunkCoord].IsVisible()) chunksVisibleLastFrame.Add(spaceChunkDictionary[viewedChunkCoord]);
                } else
                {
                    spaceChunkDictionary.Add(viewedChunkCoord, new SpaceChunk(viewedChunkCoord, chunkSize, chunkHolder, unlitMat, testingSprite));
                }
            }
        }
    }

    void UpdateHiddenChunks()
    {
        Debug.Log("Updating hidden chunks");
        foreach (KeyValuePair<Vector2, SpaceChunk> chunk in spaceChunkDictionary)
        {
            chunk.Value.CheckDistanceAndDestroy();
        }
        for (int i = 0; i < toBeDestroyed.Count; i++)
        {
            spaceChunkDictionary.Remove
                (new Vector2(toBeDestroyed[i].chunkGO.transform.position.x / chunkSize, toBeDestroyed[i].chunkGO.transform.position.y / chunkSize));
            foreach (GameObject go in toBeDestroyed[i].entities)
            {
                Destroy(go);
            }
            Destroy(toBeDestroyed[i].chunkGO);
        }
        toBeDestroyed.Clear();
    }

    public class SpaceChunk
    {
        public GameObject chunkGO;
        bool visibleLastFrame = false;
        Vector2 position;
        Bounds bounds;
        Chunk chunk;
        public List<GameObject> entities = new();

        public SpaceChunk(Vector2 coord, int size, Transform parent, Material mat, Sprite testingSprite)
        {
            position = coord * size;
            bounds = new Bounds(position, Vector2.one * size);
            Vector2 positionV2 = new(position.x, position.y);
            
            chunkGO = CreateNewChunk(positionV2, parent, size, mat, testingSprite);
            SetVisible(false);
        }

        public void UpdateSpaceChunk()
        {
            float viewerDstFromNearestEdge = bounds.SqrDistance(viewerPosition);
            bool visible = viewerDstFromNearestEdge <= viewDist * viewDist;
            if (visibleLastFrame == visible) { Debug.Log("It is the same"); }
            else
            {
                SetChildVisibility(visible);
                SetVisible(visible);
            }
            visibleLastFrame = visible;
        }

        public void CheckDistanceAndDestroy()
        {
            float viewerDstFromNearestEdge = bounds.SqrDistance(viewerPosition);
            bool toBeKept = viewerDstFromNearestEdge <= destroyDist * destroyDist;
            Debug.Log(toBeKept);
            if (toBeKept) { }
            else
            {
                //Debug.Log("Destroying " + chunkGO + " because it was too far away");
                toBeDestroyed.Add(this);
            }
        }

        public void SetVisible(bool visible)
        {
            //Debug.Log("Checking chunk visibility. It is " + visible + " and I am " + chunkGO);
            chunkGO.SetActive(visible);
        }

        void SetChildVisibility(bool visible)
        {
            if (visible == true)
            {
                //Debug.Log("Enabling " + entities.Count + " entities");
                for (int i = 0; i < entities.Count; i++)
                {
                    if (entities[i] == null) { return; }
                    entities[i].SetActive(true);
                }
            }
            else
            {
                //Debug.Log("Disabling " + entities.Count + " entities");
                for (int i = 0; i < entities.Count; i++)
                {
                    if (entities[i] == null) { return; }
                    entities[i].SetActive(false);
                }
            }
        }

        public bool IsVisible()
        {
            return chunkGO.activeSelf;
        }

        
        public GameObject CreateNewChunk(Vector2 coord, Transform parent, float size, Material mat, Sprite testingSprite)
        {
            //Debug.Log("Creating chunk " + x + y);
            GameObject newChunk = new()
            {
                name = "Chunk" + coord.x.ToString() + coord.y.ToString()
            };
            newChunk.transform.parent = parent;
            newChunk.transform.position = coord;

            chunk = SetChunkValues(coord / size);
            newChunk.AddComponent<LoadChunk>().chunk = chunk;

            GameObject spriteHolder = new() { name = "Sprite" };
            spriteHolder.transform.parent = newChunk.transform;
            spriteHolder.transform.localScale = new Vector3(size, size, 1);
            spriteHolder.transform.localPosition = Vector3.zero;
            spriteHolder.layer = 29;

            SpriteRenderer sprite = spriteHolder.AddComponent<SpriteRenderer>();
            sprite.material = mat;
            sprite.sortingOrder = -10;
            if (chunk.chunkEvent) { sprite.color = new Color(1, 0, 0, 1); }
            else if (chunk.shop) { sprite.color = new Color(0, 1, 0, 1); }
            else { sprite.color = new Color(chunk.factionValue, chunk.yieldValue, chunk.debrisValue, 1); }
            spriteHolder.GetComponent<SpriteRenderer>().sprite = testingSprite;
            return newChunk;
        }

        public Chunk SetChunkValues(Vector2 coord)
        {
            Chunk chunk = new();

            float debrisValue = Mathf.PerlinNoise(coord.x * debrisScale + GlobalRefs.xSeed + GlobalRefs.debrisSeed, coord.y * debrisScale + GlobalRefs.ySeed + GlobalRefs.debrisSeed);
            chunk.debrisValue = debrisValue;

            float factionValue = Mathf.PerlinNoise(coord.x * factionScale + GlobalRefs.xSeed + GlobalRefs.factionSeed, coord.y * factionScale + GlobalRefs.ySeed + GlobalRefs.factionSeed);
            chunk.factionValue = factionValue;

            float shopValue = Mathf.PerlinNoise(coord.x * shopScale + GlobalRefs.xSeed + GlobalRefs.shopSeed, coord.y * shopScale + GlobalRefs.ySeed + GlobalRefs.shopSeed);
            chunk.shop = shopValue > shopThreshold;

            float yieldValue = Mathf.PerlinNoise(coord.x * yieldScale + GlobalRefs.xSeed + GlobalRefs.yieldSeed, coord.y * yieldScale + GlobalRefs.ySeed + GlobalRefs.yieldSeed);
            chunk.yieldValue = yieldValue;

            float eventValue = Mathf.PerlinNoise(coord.x * eventScale + GlobalRefs.xSeed + GlobalRefs.eventSeed, coord.y * eventScale + GlobalRefs.ySeed + GlobalRefs.eventSeed);
            chunk.chunkEvent = eventValue > eventThreshold;

            return chunk;
        }
    }

    public GameObject CreateNewChunk(Vector2 coord)
    {
        //Debug.Log("Creating chunk " + x + y);
        GameObject newChunk = new()
        {
            name = "Chunk" + coord.x.ToString() + coord.y.ToString()
        };
        newChunk.transform.parent = chunkHolder;
        newChunk.transform.position = coord;
        //new Vector2(
        //    (x * chunkSize)
        //+ chunkHolder.position.x
        //- (viewDist * chunkSize),
        //    (y * chunkSize)
        //+ chunkHolder.position.y
        //- (viewDist * chunkSize));
        return newChunk;
    }

    public void GenerateChunks()
    {
        float xSeed = Random.Range(-10000, 10000);
        float ySeed = Random.Range(-10000, 10000);
        //float[,] debrisMap = new float[viewDist * 2 + 1, viewDist * 2 + 1];
        float debrisSeed = Random.Range(-10000, 10000);
        //float[,] factionMap = new float[viewDist * 2 + 1, viewDist * 2 + 1];
        float factionSeed = Random.Range(-10000, 10000);
        //float[,] shopMap = new float[viewDist * 2 + 1, viewDist * 2 + 1];
        float shopSeed = Random.Range(-10000, 10000);
        //float[,] yieldMap = new float[viewDist * 2 + 1, viewDist * 2 + 1];
        float yieldSeed = Random.Range(-10000, 10000);
        //float[,] eventMap = new float[viewDist * 2 + 1, viewDist * 2 + 1];
        float eventSeed = Random.Range(-10000, 10000);

        //grid = new Chunk[viewDist * 2 + 1, viewDist * 2 + 1];
        //Generate chunks to dictionary based on view distance
        for (int x = 0; x < viewDist * 2 + 1; x++)
        {
            for(int y = 0; y < viewDist * 2 + 1; y++)
            {
                Chunk chunk = new Chunk();

                float debrisValue = Mathf.PerlinNoise(x * debrisScale + xSeed + debrisSeed, y * debrisScale + ySeed + debrisSeed);
                //debrisMap[x, y] = debrisValue;
                chunk.debrisValue = debrisValue;

                float factionValue = Mathf.PerlinNoise(x * factionScale + xSeed + factionSeed, y * factionScale + ySeed + factionSeed);
                //factionMap[x, y] = factionValue;
                chunk.factionValue = factionValue;

                float shopValue = Mathf.PerlinNoise(x * shopScale + xSeed + shopSeed, y * shopScale + ySeed + shopSeed);
                //shopMap[x, y] = shopValue;
                chunk.shop = shopValue > shopThreshold;

                float yieldValue = Mathf.PerlinNoise(x * yieldScale + xSeed + yieldSeed, y * yieldScale + ySeed + yieldSeed);
                //yieldMap[x, y] = yieldValue;
                chunk.yieldValue = yieldValue;

                float eventValue = Mathf.PerlinNoise(x * eventScale + xSeed + eventSeed, y * eventScale + ySeed + eventSeed);
                //eventMap[x, y] = eventValue;
                chunk.chunkEvent = eventValue > eventThreshold;

                GameObject newChunk = CreateNewChunk(new Vector2(x, y));
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

                //grid[x, y] = chunk;
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

    float GenerateNoise(int coordX, int coordY)
    {
        float v = Mathf.PerlinNoise(coordX, coordY);
        return v;
    }
}
