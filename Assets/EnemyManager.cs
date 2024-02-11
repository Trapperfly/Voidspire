using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    #region Singleton
    public static EnemyManager Instance;

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

    public GameObject bulletPrefab;
    public GameObject VoidSpherePrefab;
    public GameObject MissilePrefab;
    public GameObject RailgunPrefab;


    public GameObject ElectricBallPrefab;
    public GameObject HomingMissilesPrefab;
    public GameObject VoidNovaPrefab;
    public GameObject SelfDestructPrefab;
}
