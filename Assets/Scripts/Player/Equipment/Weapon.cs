using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory/Equipment/Weapon")]
public class Weapon : Equipment
{
    public Sprite gameSpaceSprite;
    public WeaponType weaponType;
    public bool isExplosive;
    public bool savedIsExplosive;
    public float explosiveMultiplier;
    public float splashDamage;
    public float splashRange;

    public int cluster;
    public int clusterAmount;
    public float clusterSpeed;

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
    public bool savedHoming;
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
{ //Projectiles can be homing, and most can pierce or bounce
    Bullet, //Normal projectile. Nothing special
    Beam, //Continous beam. Can pierce
    Railgun, //Hitscan beam. Can pierce
    Laser, //Fast small projectiles. Higher fire-rate, lower damage
    Wave, //Expanding projectiles. Lower fire rate
    Rocket, //Explosive rockets. Average stats
    Needle, //Homing projectiles. Fast fire-rate, low damage, slow speed
    Mine, //Highly explosive. Slow speed, but superior damage and radius
    Hammer, //Shotgun style. Slow fire-rate, but lots of projectiles
    Cluster, //Large shot that explodes into shrapnel. Good damage, but somewhat unreliable
    Arrow, //Large high damage physical projectile. Will have various effects later, like slow and hard knockback
    Grand, //Very high damage, very low fire rate. 
    Mirage,
    Void,
    Blade
}
