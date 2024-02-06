using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalRefs : MonoBehaviour 
{
    public static GlobalRefs Instance;

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

    public GameObject player;
    public static Vector2 playerPos;
    public int seed;
    public static float xSeed;
    public static float ySeed;
    public static float debrisSeed;
    public static float factionSeed;
    public static float shopSeed;
    public static float yieldSeed;
    public static float eventSeed;


    private void Start()
    {
        if (seed == 0) { seed = Random.Range(-10000, 10000); }
        Random.InitState(seed);
        //Set seeds 
        xSeed = Random.Range(-10000, 10000);
        ySeed = Random.Range(-10000, 10000);
        debrisSeed = Random.Range(-10000, 10000);
        factionSeed = Random.Range(-10000, 10000);
        shopSeed = Random.Range(-10000, 10000);
        yieldSeed = Random.Range(-10000, 10000);
        eventSeed = Random.Range(-10000, 10000);
        //GenerateChunks();
    }
}
