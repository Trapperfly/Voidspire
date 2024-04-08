using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class SpawnEnemies : MonoBehaviour
{
    //[SerializeField] float secondsTillNextSpawnAttempt = 5f;
    [SerializeField] int maxAmount;
    public int funds = 0;
    [Header("Void")]
    [SerializeField] GameObject lurkerPrefab;
    [SerializeField] GameObject arbalestPrefab;
    [SerializeField] const int oneSpawnPercent = 10;
    [SerializeField] const int twoSpawnPercent = 5;
    [SerializeField] const int fiveSpawnPercent = 2;
    [SerializeField] const int tenSpawnPercent = 1;

    [Header("Chitin")]
    public Enemy[] chitinShips;

    [System.Serializable]
    public class Enemy
    {
        public string name;
        public int cost;
        public GameObject prefab;
    }

    #region Singleton
    public static SpawnEnemies Instance;

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
    public void SpawnVoidEnemies(Vector2 pos, float range, int level)
    {
        int amount = Random.Range(1, 101) switch
        {
            > (100 - tenSpawnPercent) => 10,
            > (100 - fiveSpawnPercent) => 5,
            > (100 - twoSpawnPercent) => 2,
            > (100 - oneSpawnPercent) => 1,
            _ => 0
        };
        Vector2 spawnLocation = pos + new Vector2(Random.Range(-range, range), Random.Range(-range, range));
        for (int i = amount; i > 0; i--)
        {
            if (Random.Range(0, 2) == 0)
                Instantiate
                (
                    lurkerPrefab,
                    spawnLocation + (Vector2)Random.insideUnitSphere,
                    Quaternion.Euler(0, 0, (Random.value * 360) - 180),
                    transform
                ).GetComponent<ShipAI>().level = level;
            else
            {
                Instantiate
                (
                    arbalestPrefab,
                    spawnLocation + (Vector2)Random.insideUnitSphere,
                    Quaternion.Euler(0, 0, (Random.value * 360) - 180),
                    transform
                ).GetComponent<ShipAI>().level = level;
                i--;
            }
        }
    } // Spawns random amount

    public void SpawnChitinEnemies(Vector2 pos, float range, int level)
    {
        int amount = Random.Range(0, funds);
        Vector2 spawnLocation = pos + new Vector2(Random.Range(-range, range), Random.Range(-range, range));

        while (amount > 0)
        {
            int selected = Random.Range(0, chitinShips.Length);
            if (chitinShips[selected].cost <= amount)
            {
                SpawnEnemy(spawnLocation, level, chitinShips[selected].prefab);
                amount -= chitinShips[selected].cost;
            }
        }
        
        
    } // Spawns preset squadron sizes

    public void SpawnChromeEnemies(Vector2 pos, float range, int level)
    {
        int amount = Random.Range(1, 101) switch
        {
            > (100 - tenSpawnPercent) => 10,
            > (100 - fiveSpawnPercent) => 5,
            > (100 - twoSpawnPercent) => 2,
            > (100 - oneSpawnPercent) => 1,
            _ => 0
        };
        Vector2 spawnLocation = pos + new Vector2(Random.Range(-range, range), Random.Range(-range, range));
        for (int i = amount; i > 0; i--)
        {
            if (Random.Range(0, 2) == 0)
                Instantiate
                (
                    lurkerPrefab,
                    spawnLocation + (Vector2)Random.insideUnitSphere,
                    Quaternion.Euler(0, 0, (Random.value * 360) - 180),
                    transform
                ).GetComponent<ShipAI>().level = level;
            else
            {
                Instantiate
                (
                    arbalestPrefab,
                    spawnLocation + (Vector2)Random.insideUnitSphere,
                    Quaternion.Euler(0, 0, (Random.value * 360) - 180),
                    transform
                ).GetComponent<ShipAI>().level = level;
                i--;
            }
        }
    } // Spawns random amount with predetermined leader

    public void SpawnPirateEnemies(Vector2 pos, float range, int level)
    {
        int amount = Random.Range(1, 101) switch
        {
            > (100 - tenSpawnPercent) => 10,
            > (100 - fiveSpawnPercent) => 5,
            > (100 - twoSpawnPercent) => 2,
            > (100 - oneSpawnPercent) => 1,
            _ => 0
        };
        Vector2 spawnLocation = pos + new Vector2(Random.Range(-range, range), Random.Range(-range, range));
        for (int i = amount; i > 0; i--)
        {
            if (Random.Range(0, 2) == 0)
                Instantiate
                (
                    lurkerPrefab,
                    spawnLocation + (Vector2)Random.insideUnitSphere,
                    Quaternion.Euler(0, 0, (Random.value * 360) - 180),
                    transform
                ).GetComponent<ShipAI>().level = level;
            else
            {
                Instantiate
                (
                    arbalestPrefab,
                    spawnLocation + (Vector2)Random.insideUnitSphere,
                    Quaternion.Euler(0, 0, (Random.value * 360) - 180),
                    transform
                ).GetComponent<ShipAI>().level = level;
                i--;
            }
        }
    } // Spawns one

    public void SpawnCiv(Vector2 pos, float range, int level)
    {
        int amount = Random.Range(1, 101) switch
        {
            > (100 - tenSpawnPercent) => 10,
            > (100 - fiveSpawnPercent) => 5,
            > (100 - twoSpawnPercent) => 2,
            > (100 - oneSpawnPercent) => 1,
            _ => 0
        };
        Vector2 spawnLocation = pos + new Vector2(Random.Range(-range, range), Random.Range(-range, range));
        for (int i = amount; i > 0; i--)
        {
            if (Random.Range(0, 2) == 0)
                Instantiate
                (
                    lurkerPrefab,
                    spawnLocation + (Vector2)Random.insideUnitSphere,
                    Quaternion.Euler(0, 0, (Random.value * 360) - 180),
                    transform
                ).GetComponent<ShipAI>().level = level;
            else
            {
                Instantiate
                (
                    arbalestPrefab,
                    spawnLocation + (Vector2)Random.insideUnitSphere,
                    Quaternion.Euler(0, 0, (Random.value * 360) - 180),
                    transform
                ).GetComponent<ShipAI>().level = level;
                i--;
            }
        }
    } // Spawns a few, with military escorts

    public void SpawnEnemy(Vector2 spawnLocation, int level, GameObject enemyPrefab)
    {
        Instantiate
        (
            enemyPrefab,
            spawnLocation + ((Vector2)Random.insideUnitSphere * (Mathf.Sqrt(funds) / 2)),
            Quaternion.Euler(0, 0, (Random.value * 360) - 180),
            transform
        ).GetComponent<ShipAI>().level = level;
    }

    //Vector3 RandomOutsideView()
    //{
    //    int whichSide = Random.Range(1, 5);
    //    float _x = 0;
    //    float _y = 0;
    //    switch (whichSide)
    //    {
    //        case 1: //Up
    //            _x = Random.Range(-0.4f, 1.4f);
    //            _y = Random.Range(1.2f, 1.4f);
    //            break;
    //        case 2: //Right
    //            _x = Random.Range(1.2f, 1.4f);
    //            _y = Random.Range(-0.4f, 1.4f);
    //            break;
    //        case 3: //Down
    //            _x = Random.Range(-0.4f, 1.4f);
    //            _y = Random.Range(-0.2f, -0.4f);
    //            break;
    //        case 4: //Left
    //            _x = Random.Range(-0.2f, -0.4f);
    //            _y = Random.Range(-0.4f, 1.4f);
    //            break;
    //        default:
    //            Debug.Log("Something went wrong");
    //            break;
    //    }
    //    return new Vector3(_x, _y, 10);
    //}
}
