using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory/Equipment/Weapon")]
public class Weapon : Equipment
{
    public Weapon unalteredVersion;
    public WeaponType weaponType;
    public bool isExplosive;
    public float explosiveMultiplier;
    public float splashDamage;
    public float splashRange;
    public float damage;              //On bullet
    public float bulletSize;          //On gun
    public float fireRate;            //On gun
    public float fireRateChange;   //On gun
    public float fireRateChangeTimer; //On gun
    public int amount;                //On gun
    public float spread;              //On gun
    public float spreadChange;        //On gun
    public float spreadChangeTimer;
    public float speed;               //On gun
    public float longevity;           //On bullet
    [Header("Specials")]
    public bool homing;               //On bullet
    public float homingStrength;      //On bullet
    public int pierce;                //On bullet
    public int bounce;                //On bullet
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
    public Color effectColor;
}

public enum WeaponType
{
    Bullet,
    Beam,
    Railgun,
    Laser,
    Wave,
    Rocket,
    Needle,
    Mine,
    Hammer,
    Cluster,
    Arrow,
    Grand,
    Mirage,
    Void,
    Blade
}
