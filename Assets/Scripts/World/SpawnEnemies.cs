using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    [SerializeField] float secondsTillNextSpawnAttempt = 5f;
    [SerializeField] int maxAmount;
    int amount = 0;
    [SerializeField] GameObject lurkerPrefab;
    [SerializeField] GameObject arbalestPrefab;
    [SerializeField] const int oneSpawnPercent = 20;
    [SerializeField] const int twoSpawnPercent = 10;
    [SerializeField] const int fiveSpawnPercent = 5;
    [SerializeField] const int tenSpawnPercent = 1;
    private void Awake()
    {
        StartCoroutine(nameof(Spawn));
    }
    IEnumerator Spawn()
    {
        while (true)
        {
            if (maxAmount == -1 || transform.childCount <= maxAmount)
            {
                amount = Random.Range(1, 101) switch
                {
                    > (100 - tenSpawnPercent) => 10,
                    > (100 - fiveSpawnPercent) => 5,
                    > (100 - twoSpawnPercent) => 2,
                    > (100 - oneSpawnPercent) => 1,
                    _ => 0,
                };
                Vector3 spawnLocation = Camera.main.ViewportToWorldPoint(RandomOutsideView());
                for (int i = amount; i > 0; i--)
                {
                    if (Random.Range(0,2) == 0)
                        Instantiate
                        (
                            lurkerPrefab,
                            spawnLocation + Random.insideUnitSphere,
                            Quaternion.Euler(0, 0, (Random.value * 360) - 180),
                            transform
                        );
                    else
                    {
                        Instantiate
                        (
                            arbalestPrefab,
                            spawnLocation + Random.insideUnitSphere,
                            Quaternion.Euler(0, 0, (Random.value * 360) - 180),
                            transform
                        );
                        i--;
                    }
                }
            }
            yield return new WaitForSeconds(secondsTillNextSpawnAttempt);
        }
    }

    Vector3 RandomOutsideView()
    {
        int whichSide = Random.Range(1, 5);
        float _x = 0;
        float _y = 0;
        switch (whichSide)
        {
            case 1: //Up
                _x = Random.Range(-0.4f, 1.4f);
                _y = Random.Range(1.2f, 1.4f);
                break;
            case 2: //Right
                _x = Random.Range(1.2f, 1.4f);
                _y = Random.Range(-0.4f, 1.4f);
                break;
            case 3: //Down
                _x = Random.Range(-0.4f, 1.4f);
                _y = Random.Range(-0.2f, -0.4f);
                break;
            case 4: //Left
                _x = Random.Range(-0.2f, -0.4f);
                _y = Random.Range(-0.4f, 1.4f);
                break;
            default:
                Debug.Log("Something went wrong");
                break;
        }
        return new Vector3(_x, _y, 10);
    }
}
