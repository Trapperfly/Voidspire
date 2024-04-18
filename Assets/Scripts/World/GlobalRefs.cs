using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GlobalRefs : MonoBehaviour 
{
    public static GlobalRefs Instance;
    public Transform[] clearTheseOfChildrenWhenNewSector;
    public List<GameObject> clearThese = new();

    public GameObject sectorBossEvent;
    public Transform eventParent;

    public int wallet = 0;

    public Transform tutorialPanel;

    [SerializeField] TMPro.TMP_Text seedText;
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
    public bool playerIsInFtl;
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

    private void Start()
    {
        Instantiate(sectorBossEvent, eventParent);
    }
    private void SetSeeds()
    {
        if (seed == 0) { seed = Random.Range(-10000000, 10000000); Random.InitState(seed); seedText.text = seed.ToString(); }
        
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

    public void StartNewSector()
    {
        currentSector++;
        SetSeeds();
        for (int i = 0; i < clearTheseOfChildrenWhenNewSector.Length; i++)
        {
            for (int j = 0; j < clearTheseOfChildrenWhenNewSector[i].childCount; j++)
            {
                Destroy(clearTheseOfChildrenWhenNewSector[i].GetChild(j).gameObject);
            }
        }
        for (int i = 0; i < clearThese.Count; i++)
        {
            Destroy(clearThese[i]);
        }
        clearThese.Clear();
        ChunkLoader.Instance.Clear();
        Instantiate(sectorBossEvent, eventParent);
    }

    public void ShowTutorialInfo(Vector2 where, string what)
    {
        tutorialPanel.gameObject.SetActive(true);
        Transform t = tutorialPanel.GetChild(1);
        t.position = where;
        t.GetChild(1).GetComponent<TMP_Text>().text = what;
    }
}
