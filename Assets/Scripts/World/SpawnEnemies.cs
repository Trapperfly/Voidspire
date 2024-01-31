using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    //[SerializeField] float secondsTillNextSpawnAttempt = 5f;
    [SerializeField] int maxAmount;
    int amount = 0;
    [SerializeField] GameObject lurkerPrefab;
    [SerializeField] GameObject arbalestPrefab;
    [SerializeField] const int oneSpawnPercent = 10;
    [SerializeField] const int twoSpawnPercent = 5;
    [SerializeField] const int fiveSpawnPercent = 2;
    [SerializeField] const int tenSpawnPercent = 1;
    
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
    public void Spawn(Vector2 pos, float range)
    {
        amount = Random.Range(1, 101) switch
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
                );
            else
            {
                Instantiate
                (
                    arbalestPrefab,
                    spawnLocation + (Vector2)Random.insideUnitSphere,
                    Quaternion.Euler(0, 0, (Random.value * 360) - 180),
                    transform
                );
                i--;
            }
        }
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
