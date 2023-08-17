using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum BulletType
{
    Bullet,     //Standard bullet, some punch, nothing special, free fire rate
    Laser,      //Innately fast, no punch, free fire rate
    Wave,       //Innately slow, slows down during flight, good punch, pierces, slower fire rate
    Rocket,     //Speeds up during flight, explosion knockback, cannot pierce or bounce, slow fire rate
    Needle,     //Small slow bullets that innately homes, damages target after a few seconds, very fast fire rate
    Railgun,    //Instant hit, leaves a visual trail, pierces, very slow fire rate
    Mine,       //Slows down to a stop during flight, long longevity, cannot pierce or bounce, very slow fire rate
    Hammer,     //Innate spread and multiple bullets, hard punch, very slow fire rate, pushes your ship
    Cluster,    //Slow shot, explodes into spread of bullets during flight, cannot bounce, slow fire rate
    Arrow,      //Fires large metal arrows that stick into targets to adjust their centre of mass, cannot pierce or bounce, slow fire rate
    Mirage,     //
    Grand,      //Large slower shot, very hard punch, very slow fire rate, pushes your ship, amount adds damage instead
    Void,       //No bullet, but creates a pulling singularity at mouse cursor
    Beam,       //Continous beam, bounces off walls
    Blade,      //Shoots blade drones, slight homing, innate bounce towards new targets
}

enum Prefix
{
    Gatling,    //Ramps up firerate AND spread when trigger is held
    Precise,    //Gets more precise when trigger is held, when max, pierce
    Homing,     //Shots home aggressively
    Swarm,      //Smaller and more shots that home slightly
    Piercing,   //Pierces infinite targets
    Bore,       //Pierces few targets but damage is multiplied per pierce, initial damage reduced
    TwoBurst,   //Shoots in bursts of two with little delay per burst
    SevenBurst, //Shoots in bursts of seven with long delay per burst
    Shotgun,    //Shoots many shots at once in an arc
    Bouncy,     //Shots bounce on objects and looses no speed on bounce
    BounceShot, //Shots bounce to other targets, but gets smaller and weaker on bounce
    Expanding,  //Shots get larger and more damaging during flight
    Fusion,     //Long charge-up with concentrated burst shot
    Flurry,     //Large spread with more projectiles
    Shooter,    //Fires as fast as you can pull the trigger
    Grim,       //Ramps up damage and speed the longer the trigger is held, release to fire
    Frenzy,     //Charge up and fire in rapid succession
    Dual,       //Each shot has a paralell shot
    Fiery,      //Last target hit is ignited
    Inferno,    //Every target is ignited, reduce damage, increased firerate
}
enum Modifier
{
    
}

enum Quality
{
    Scrap,
    Poor,
    Normal,
    Quality,
    Pristine,
}
public class GunController : MonoBehaviour
{
    [Header("Type of weapon")]
    [SerializeField] BulletType bulletType;
    [SerializeField] Prefix prefix;
    [SerializeField] Modifier modifier;
    [SerializeField] Quality quality;
    [Header("Stats")]
    [SerializeField] float damage;              //On bullet
    [SerializeField] float damageChange;        //On bullet
    [SerializeField] float bulletSize;          //On gun
    [SerializeField] float bulletSizeChange;    //On bullet
    [SerializeField] float fireRate;            //On gun
    [SerializeField] float fireRateChange;      //On gun
    [SerializeField] int amount;                //On gun
    [SerializeField] float spread;              //On gun
    [SerializeField] float spreadChange;        //On gun
    [SerializeField] float speed;               //On gun
    [SerializeField] float speedChange;         //On bullet
    [SerializeField] float longevity;           //On bullet
    [Header("Specials")]
    [SerializeField] bool homing;               //On bullet
    [SerializeField] float homingStrength;      //On bullet
    [SerializeField] int pierce;                //On bullet
    [SerializeField] int bounce;                //On bullet
    [SerializeField] bool bounceToTarget;
    [Header("Misc")]
    [SerializeField] float chargeUp;            //On gun
    [SerializeField] float charge;
    bool chargeAvailable = true;
    [SerializeField] int burst;                //On gun
    [SerializeField] float burstDelay;          //On gun
    [SerializeField] float punch;               //On bullet
    [SerializeField] float punchSelf;           //On gun
    [SerializeField] bool overheat;             //On gun
    [SerializeField] float overheatLimit;       //On gun
    [SerializeField] float overheatBuildup;     //On gun
    [SerializeField] bool inflictIgnition;
    [Header("Gun attributes")]
    [SerializeField] float angle;
    [SerializeField] float rotationSpeed;

    [Header("Prefabs")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject laserPrefab;
    [SerializeField] GameObject wavePrefab;
    int gunTimer = 0;
    [Header("Testing")]
    [SerializeField] AdjustToTarget target;
    [SerializeField] GunPointToMouse gunPoint;
    bool inBurst = false;
    [SerializeField] float weightScalar = 0.0001f;
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Mouse0) && gunTimer >= Mathf.Clamp(60 / fireRate, 1, 600))
        {
            if (chargeUp == 0 || charge > chargeUp * 60)
            {
                if (burst != 0 && !inBurst)
                {
                    StartCoroutine(Burst(burst, burstDelay));
                }
                else if (!inBurst)
                {
                    for (int i = amount; i > 0; i--)
                    {
                        StartCoroutine(Shoot(speed));
                        gunTimer = 0;
                        charge = 0;
                        chargeAvailable = false;
                    }
                }
            }
            else if (chargeUp != 0 && chargeAvailable == true)
            {
                charge++;
            }
        }
        if (!Input.GetKey(KeyCode.Mouse0))
        {
            if (charge >= 2)
            {
                charge -= 2;
            }
            chargeAvailable = true;
        }
        gunTimer++;
    }
    IEnumerator Burst(int times, float delay)
    {
        inBurst = true;
        for (int b = times; b > 0; b--)
        {
            for (int i = amount; i > 0; i--)
            {
                StartCoroutine(Shoot(speed));
                gunTimer = 0;
                charge = 0;
                chargeAvailable = false;
            }
            yield return new WaitForSeconds(delay);
        }
        yield return null;
        gunTimer = 0;
        inBurst = false;
    }
    public IEnumerator Shoot(float bulletSpeed)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Spread(transform.rotation));
        bullet.transform.localScale *= bulletSize;
        bullet.transform.parent = GameObject.FindGameObjectWithTag("BulletHolder").transform;
        bullet.GetComponent<Rigidbody2D>().AddForce(bulletSpeed * weightScalar * bullet.transform.up, ForceMode2D.Impulse);
        Bullet bulletSC = bullet.GetComponent<Bullet>();
        bulletSC._damage = damage;
        bulletSC._damageChange = damageChange;
        bulletSC._sizeChange = bulletSizeChange;
        bulletSC._speed = speed;
        bulletSC._bulletLongevity = longevity;
        bulletSC._pierce = pierce;
        bulletSC._bounce = bounce;
        bulletSC._bounceToTarget = bounceToTarget;
        bulletSC._homing = homing;
        bulletSC._homingStrength = homingStrength * 5 * speed;
        bulletSC._punch = punch;
        if (target.target != null)
            bulletSC.target = target.target;
        bulletSC._weightScalar = weightScalar;


        yield return null;
    }
    public void RandomizeGun()
    {

    }
    Quaternion Spread(Quaternion baseRotation)
    {
        Quaternion spreadValue = baseRotation * Quaternion.Euler(0, 0, Random.Range(-spread, spread));
        return spreadValue;
    }
}
