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
        SetSeeds();
    }

    public GameObject player;
    public bool playerIsDead;
    public static Vector2 playerPos;
    public int seed;
    public int currentSector;
    public static float xSeed;
    public static float ySeed;
    public static float debrisSeed;
    public static float voidSeed;
    public static float chitinSeed;
    public static float chromeSeed;
    public static float pirateSeed;
    public static float civSeed;
    public static float shopSeed;
    public static float yieldSeed;
    public static float eventSeed;
    public static float difSeed;


    private void SetSeeds()
    {
        if (seed == 0) { seed = Random.Range(-10000, 10000); }
        Random.InitState(seed);
        //Set seeds 
        xSeed = Random.Range(-10000, 10000);
        ySeed = Random.Range(-10000, 10000);
        debrisSeed = Random.Range(-10000, 10000);
        voidSeed = Random.Range(-10000, 10000);
        chitinSeed = Random.Range(-10000, 10000);
        chromeSeed = Random.Range(-10000, 10000);
        pirateSeed = Random.Range(-10000, 10000);
        civSeed = Random.Range(-10000, 10000);
        shopSeed = Random.Range(-10000, 10000);
        yieldSeed = Random.Range(-10000, 10000);
        eventSeed = Random.Range(-10000, 10000);
        difSeed = Random.Range(-10000, 10000);
        //GenerateChunks();
    }
}
