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
    [Header("Normal Attacks")]
    public GameObject bulletPrefab;
    public GameObject VoidSpherePrefab;
    public GameObject MissilePrefab;
    public GameObject RailgunPrefab;
    public GameObject FlakPrefab;
    public GameObject RapidPrefab;
    public GameObject CannonPrefab;
    [Space]
    [Header("Special Attacks")]
    public GameObject ElectricBallPrefab;
    public GameObject HomingMissilesPrefab;
    public GameObject VoidNovaPrefab;
    public GameObject SelfDestructPrefab;

    [Space]
    [Header("Bullet controllers")]
    public Transform bh;
    public Transform hbh;
}
