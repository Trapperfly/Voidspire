using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunStats : MonoBehaviour
{
    public bool active;

    [Header("Type of weapon")]
    public BulletType bulletType;
    [SerializeField] Prefix prefix;
    [SerializeField] Modifier modifier;
    [SerializeField] Quality quality;
    [Header("Stats")]
    public float damage;              //On bullet
    public float damageChange;        //On bullet
    public float bulletSize;          //On gun
    public float bulletSizeChange;    //On bullet
    public float fireRate;            //On gun
    public float fireRateChange;   //On gun
    public float fireRateChangeTimer; //On gun
    public int amount;                //On gun
    public float spread;              //On gun
    public AnimationCurve spreadCurve;
    public float spreadChange;        //On gun
    public float spreadChangeTimer;
    public float speed;               //On gun
    public AnimationCurve speedCurve;
    public float speedChange;         //On bullet
    public float longevity;           //On bullet
    [Header("Specials")]
    public bool homing;               //On bullet
    public float homingStrength;      //On bullet
    public int pierce;                //On bullet
    public int bounce;                //On bullet
    //[SerializeField] bool bounceToTarget;
    [Header("Misc")]
    public float chargeUp;            //On gun
    public int burst;                 //On gun
    public float burstDelay;          //On gun
    public float punch;               //On bullet
    //[SerializeField] float punchSelf;           //On gun
    //[SerializeField] bool overheat;             //On gun
    //[SerializeField] float overheatLimit;       //On gun
    //[SerializeField] float overheatBuildup;     //On gun
    //[SerializeField] bool inflictIgnition;
    [Header("Gun attributes")]
    public Vector2 rotationAngle;
    public float rotationSpeed;

    [Header("Prefabs")]
    public GameObject bulletPrefab;
    public GameObject beamPrefab;
    public GameObject beamPsPrefab;
    public GameObject railgunPrefab;
    public GameObject railgunPsPrefab;
    public GameObject railgunLinePsPrefab;
    public GameObject wavePrefab;

    [Header("Testing")]
    public float weightScalar = 0.0001f;
}
