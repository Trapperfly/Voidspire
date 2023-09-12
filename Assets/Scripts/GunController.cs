using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
using Unity.Entities;
using Unity.Burst;

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
    float fireRateA;
    [SerializeField] float fireRateChange;   //On gun
    [SerializeField] float fireRateChangeTimer; //On gun
    float fireRateScalar = 0;
    [SerializeField] int amount;                //On gun
    int extraShot = 0;
    [SerializeField] float spread;              //On gun
    float spreadA;
    [SerializeField] AnimationCurve spreadCurve;
    [SerializeField] float spreadChange;        //On gun
    [SerializeField] float spreadChangeTimer;
    float spreadScalar;
    [SerializeField] float speed;               //On gun
    [SerializeField] AnimationCurve speedCurve;
    [SerializeField] float speedChange;         //On bullet
    [SerializeField] float longevity;           //On bullet
    [Header("Specials")]
    [SerializeField] bool homing;               //On bullet
    [SerializeField] float homingStrength;      //On bullet
    [SerializeField] int pierce;                //On bullet
    [SerializeField] int bounce;                //On bullet
    //[SerializeField] bool bounceToTarget;
    [Header("Misc")]
    [SerializeField] float chargeUp;            //On gun
    [SerializeField] float charge;
    bool chargeAvailable = true;
    [SerializeField] int burst;                 //On gun
    [SerializeField] float burstDelay;          //On gun
    [SerializeField] float punch;               //On bullet
    //[SerializeField] float punchSelf;           //On gun
    //[SerializeField] bool overheat;             //On gun
    //[SerializeField] float overheatLimit;       //On gun
    //[SerializeField] float overheatBuildup;     //On gun
    //[SerializeField] bool inflictIgnition;
    [Header("Gun attributes")]
    public float angle;
    public float rotationSpeed;

    [Header("Prefabs")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject laserPrefab;
    [SerializeField] GameObject wavePrefab;
    int gunTimer = 0;

    [Header("Testing")]
    [SerializeField] AdjustToTarget target;
    bool inBurst = false;
    [SerializeField] float weightScalar = 0.0001f;
    [SerializeField] TMP_Text text;
    [SerializeField] Transform bulletHolder;
    [SerializeField] TMP_InputField input;
    public int currentSeed = 0;
    const string characters = "abcdefghijklmnopqrstuvwxyz0123456789";
    [SerializeField] int minSeedChars;
    [SerializeField] int maxSeedChars;
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] Transform textHolder;
    int special = 100;
    int gunBonus = 100;
    int bulletBonus = 100;
    public static Transform[] BulletTransforms;
    #region RandomizerValues
    [Header("RandomizerValues")]
    [SerializeField] Vector2 R_damage;              //On bullet
    [SerializeField] AnimationCurve R_damageChance;
    [SerializeField] Vector2 R_damageChange;        //On bullet
    [SerializeField] Vector2 R_bulletSize;          //On gun
    [SerializeField] Vector2 R_bulletSizeChange;    //On bullet
    [SerializeField] Vector2 R_fireRate;
    [SerializeField] AnimationCurve R_fireRateChance;        //On gun
    [SerializeField] Vector2 R_fireRateChange;   //On gun
    [SerializeField] Vector2 R_fireRateChangeTimer; //On gun
    //[SerializeField] Vector2 R_amount;                //On gun
    [SerializeField] AnimationCurve R_Amount;
    [SerializeField] Vector2 R_spread;              //On gun
    [SerializeField] Vector2 R_spreadChange;        //On gun
    [SerializeField] Vector2 R_spreadChangeTimer;
    [SerializeField] Vector2 R_speed;               //On gun
    [SerializeField] Vector2 R_speedChange;         //On bullet
    [SerializeField] Vector2 R_longevity;           //On bullet
    [SerializeField] Vector2 R_homingStrength;      //On bullet
    [SerializeField] Vector2 R_pierce;                //On bullet
    [SerializeField] Vector2 R_bounce;                //On bullet
    //[SerializeField] bool bounceToTarget;
    [SerializeField] Vector2 R_chargeUp;            //On gun
    [SerializeField] Vector2 R_burst;                 //On gun
    [SerializeField] Vector2 R_burstDelay;          //On gun
    [SerializeField] Vector2 R_punch;               //On bullet
    //[SerializeField] float punchSelf;           //On gun
    //[SerializeField] bool overheat;             //On gun
    //[SerializeField] float overheatLimit;       //On gun
    //[SerializeField] float overheatBuildup;     //On gun
    //[SerializeField] bool inflictIgnition;
    [Header("Gun attributes")]
    public Vector2 R_angle;
    public Vector2 R_rotationSpeed;
    #endregion

    private void Awake()
    {
        fireRateA = fireRate;
        spreadA = spread;
        SetValues();
    }
    private void Update()
    {
        UpdateText();
    }
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Mouse0) && gunTimer >= Mathf.Clamp(60 / fireRateA, 1, 600))
        {
            if (fireRateChange != 0 && fireRateChangeTimer != 0)
            {
                fireRateScalar += (1 / fireRateA) / fireRateChangeTimer;
            }
            if (spreadChange != 0 && spreadChangeTimer != 0)
            {
                spreadScalar += (1 / fireRateA) / spreadChangeTimer;
            }
            FindExtraShotChance(fireRateA);
            if (chargeUp == 0 || charge > chargeUp * 60)
            {
                if (burst != 0 && !inBurst)
                {
                    StartCoroutine(Burst(burst, burstDelay));
                }
                else if (!inBurst)
                {
                    for (int i = amount + extraShot; i > 0; i--)
                    {
                        StartCoroutine(Shoot());
                        charge = 0;
                        chargeAvailable = false;
                    }
                    extraShot = 0;
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
            if (fireRateScalar > 0)
            {
                fireRateScalar -= (1f / (60f)) / fireRateChangeTimer * 2;
            }
            if (spreadScalar > 0)
            {
                spreadScalar -= (1f / (60f)) / spreadChangeTimer * 2;
            }
        }
        fireRateA = Mathf.Lerp(fireRate, fireRate + fireRateChange, fireRateScalar);
        if (fireRateScalar < 0 || fireRateScalar > 1)
            fireRateScalar = Mathf.Clamp(fireRateScalar, 0, 1);
        if (fireRateA != fireRate && fireRateScalar == 0)
            fireRateA = fireRate;

        spreadA = Mathf.Lerp(spread, spread + spreadChange, spreadScalar);
        if (spreadScalar < 0 || spreadScalar > 1)
            spreadScalar = Mathf.Clamp(spreadScalar, 0, 1);
        if (spreadA != spread && spreadScalar == 0)
            spreadA = spread;
        gunTimer++;
    }
    IEnumerator Burst(int times, float delay)
    {
        inBurst = true;
        for (int b = times; b > 0; b--)
        {
            for (int i = amount + extraShot; i > 0; i--)
            {
                StartCoroutine(Shoot());
                charge = 0;
                chargeAvailable = false;
            }
            yield return new WaitForSeconds(delay);
        }
        yield return null;
        inBurst = false;
        extraShot = 0;
    }
    public IEnumerator Shoot()
    {
        GameObject b = 
            Instantiate(
                bulletPrefab, bulletSpawnPoint.position,
            Spread(transform.rotation),
            GameObject.FindGameObjectWithTag("BulletHolder").transform
            );
        b.transform.localScale *= bulletSize;
        b.GetComponent<Rigidbody2D>().AddForce(Speed(speed) * weightScalar * b.transform.up, ForceMode2D.Impulse);
        Bullet bc = b.GetComponent<Bullet>();
        bc._damage = damage;
        bc._damageChange = damageChange;
        bc._sizeChange = bulletSizeChange;
        bc._speed = speed;
        bc._speedChange = speedChange;
        bc._bulletLongevity = longevity;
        bc._pierce = pierce;
        bc._bounce = bounce;
        //bulletSC._bounceToTarget = bounceToTarget;
        bc._homing = homing;
        bc._homingStrength = homingStrength * 5 * speed;
        bc._punch = punch;
        bc._weightScalar = weightScalar;
        if (target.target != null)
            bc.target = target.target;
        gunTimer = 0;
        yield return null;
    }

    float Speed(float baseSpeed)
    {
        float spreadSpeed = baseSpeed;
        if (amount > 1)
        {
            if (Random.value >= .5f)
                spreadSpeed *= 1 + CurveWeightedRandom(speedCurve); 
            else
                spreadSpeed *= 1 - CurveWeightedRandom(speedCurve);
        }
        return spreadSpeed;
    }
    Quaternion Spread(Quaternion baseRotation)
    {
        spreadA *= CurveWeightedRandom(spreadCurve);
        Quaternion spreadValue = baseRotation * Quaternion.Euler(0, 0, Random.Range(-spreadA, spreadA));
        return spreadValue;
    }

    void UpdateText()
    {
        text.text = bulletHolder.childCount.ToString();
    }

    void FindExtraShotChance(float _fr)
    {
        float chance = (_fr - 60) / 60;
        float chanceModulus = chance % 1;
        extraShot = Mathf.Clamp(Mathf.FloorToInt(chance), 0, 100);
        if (Random.value <= chanceModulus)
        {
            extraShot += 1;
        }
    }
    public void RandomizeGun()
    {
        if (input.text == string.Empty)
        {
            currentSeed = System.Environment.TickCount.GetHashCode();
            Random.InitState(currentSeed);

            StringBuilder seedBuilder = new();
            int charAmount = Random.Range(minSeedChars, maxSeedChars);
            for (int i = 0; i < charAmount; i++)
            {
                seedBuilder.Append(characters[Random.Range(0, characters.Length)]);
            }
            currentSeed = seedBuilder.ToString().GetHashCode();
            Random.InitState(currentSeed);
        }
        else
        {
            currentSeed = input.text.GetHashCode();
            Random.InitState(currentSeed);
        }

        if (Random.Range(0, 10) < 5)
        {
            damage = Mathf.Lerp(R_damage.x, R_damage.y, CurveWeightedRandom(R_damageChance) * 1.5f);
            fireRate = Mathf.Lerp(R_fireRate.x, R_fireRate.y, CurveWeightedRandom(R_fireRateChance) * 0.75f);
        }
        else
        {
            damage = Mathf.Lerp(R_damage.x, R_damage.y, CurveWeightedRandom(R_damageChance) * 0.75f);
            fireRate = Mathf.Lerp(R_fireRate.x, R_fireRate.y, CurveWeightedRandom(R_fireRateChance) * 1.5f);
        }

        gunBonus = Random.Range(0, 4);
        switch (gunBonus)
        {
            case (0):
                fireRateChange = Random.Range(R_fireRateChange.x, R_fireRateChange.y);
                fireRateChangeTimer = Random.Range(R_fireRateChangeTimer.x, R_fireRateChangeTimer.y);
                spreadChange = 0;
                spreadChangeTimer = 0;
                break;
            case (1):
                switch (Random.Range(0, 2))
                {
                    case 0:
                        spreadChange = Random.Range(R_spreadChange.y / 2, R_spreadChange.y);
                        break;
                    case 1:
                        spreadChange = Random.Range(R_spreadChange.x, R_spreadChange.x / 2);
                        break;
                    default:
                        break;
                }
                spreadChangeTimer = Random.Range(R_spreadChangeTimer.x, R_spreadChangeTimer.y);
                fireRateChange = 0;
                fireRateChangeTimer = 0;
                break;
            case (2):
                fireRateChange = 0;
                fireRateChangeTimer = 0;
                spreadChange = 0;
                spreadChangeTimer = 0;
                break;
            case (3):
                fireRateChange = 0;
                fireRateChangeTimer = 0;
                spreadChange = 0;
                spreadChangeTimer = 0;
                break;
            default:
                break;
        }

        switch (Random.Range(0, 5))
        {
            case (0):
                burst = Mathf.FloorToInt(Random.Range(R_burst.x, R_burst.x + (R_burst.y - R_burst.x) / 2));
                burstDelay = Random.Range(R_burstDelay.x, R_burstDelay.x + (R_burstDelay.y - R_burstDelay.x) / 2);
                fireRate /= 2 * (burst / 2);
                fireRateChange /= 2;
                break;
            case (1):
                burst = Mathf.FloorToInt(Random.Range(R_burst.x + (R_burst.y - R_burst.x) / 2, R_burst.y));
                burstDelay = Random.Range(R_burstDelay.x + (R_burstDelay.y - R_burstDelay.x) / 2, R_burstDelay.y);
                fireRate /= 2 * (burst / 2);
                fireRateChange /= 4;
                break;
            case (2):
                burst = 0;
                burstDelay = 0;
                break;
            case (3):
                burst = 0;
                burstDelay = 0;
                break;
            case (4):
                burst = 0;
                burstDelay = 0;
                break;
            default:
                break;
        }

        bulletBonus = Random.Range(0, 3);
        switch (bulletBonus)
        {
            case (0):
                damageChange = Random.Range(R_damageChange.x, R_damageChange.y);
                speedChange = 0;
                bulletSizeChange = 0;
                break;
            case (1):
                bulletSizeChange = Random.Range(R_bulletSizeChange.x, R_bulletSizeChange.y);
                speedChange = 0;
                damageChange = 0;
                break;
            case (2):
                speedChange = Random.Range(R_speedChange.x, R_speedChange.y);
                bulletSizeChange = 0;
                damageChange = 0;
                break;
            case 3:
                speedChange = 0;
                bulletSizeChange = 0;
                damageChange = 0;
                break;
            case 4:
                speedChange = 0;
                bulletSizeChange = 0;
                damageChange = 0;
                break;
            default:
                break;
        }

        bulletSize = Random.Range(R_bulletSize.x, R_bulletSize.y);

        switch (CurveWeightedRandom(R_Amount))
        {
            case < .33f:
                amount = 1;
                break;
            case < .66f:
                amount = 2;
                break;
            case > .66f:
                amount = 3;
                break;
            default:
                break;
        }
        spread = Random.Range(R_spread.x, R_spread.y);

        speed = Random.Range(R_speed.x, R_speed.y);
        longevity = Random.Range(R_longevity.x, R_longevity.y);

        special = Random.Range(0, 5);
        switch (special)
        {
            case (0):
                homing = true;
                homingStrength = Random.Range(R_homingStrength.x, R_homingStrength.y);
                spread += homingStrength * 2;
                pierce = 0;
                bounce = 0;
                chargeUp = 0;
                break;
            case (1):
                pierce = Mathf.FloorToInt(Random.Range(R_pierce.x, R_pierce.y));
                homing = false;
                homingStrength = 0;
                bounce = 0;
                chargeUp = 0;
                break;
            case (2):
                bounce = Mathf.FloorToInt(Random.Range(R_bounce.x, R_bounce.y));
                homing = false;
                homingStrength = 0;
                pierce = 0;
                chargeUp = 0;
                break;
            case (3):
                chargeUp = Random.Range(R_chargeUp.x, R_chargeUp.y);
                if (burst == 0)
                {
                    switch (Random.Range(0, 2))
                    {
                        case (0):
                            burst = Mathf.FloorToInt(Random.Range(R_burst.x, R_burst.x + (R_burst.y - R_burst.x) / 2));
                            burstDelay = Random.Range(R_burstDelay.x, R_burstDelay.x + (R_burstDelay.y - R_burstDelay.x) / 2);
                            fireRate /= 2;
                            break;
                        case (1):
                            burst = Mathf.FloorToInt(Random.Range(R_burst.x + (R_burst.y - R_burst.x) / 2, R_burst.y));
                            burstDelay = Random.Range(R_burstDelay.x + (R_burstDelay.y - R_burstDelay.x) / 2, R_burstDelay.y);
                            fireRate /= 4;
                            break;
                        default:
                            break;
                    }
                }
                burst += 1;
                burst *= Mathf.CeilToInt(chargeUp + 1);
                burstDelay /= 2;
                homing = false;
                homingStrength = 0;
                bounce = 0;
                pierce = 0;
                break;
            case (4):
                break;
            default:
                break;
        }
        punch = Random.Range(R_punch.x, R_punch.y);
        SetValues();
    }
    float CurveWeightedRandom(AnimationCurve curve)
    {
        return curve.Evaluate(Random.value);
    }

    void SetValues()
    {
        StringBuilder _vString = new();
        for (int value = 0; value < textHolder.childCount; value++)
        {
            TMP_Text _v = textHolder.GetChild(value).GetComponent<TMP_Text>();
            switch (value)
            {
                case 1:
                    if (input.text == string.Empty)
                        _vString.Append("Seed: " + currentSeed);
                    else
                        _vString.Append("Seed: " + input.text);
                    break;
                case 2:
                    _vString.Append("Damage: " + damage.ToString("F2"));
                    break;
                case 3:
                    _vString.Append("Firerate: " + fireRate.ToString("F2"));
                    break;
                case 4:
                    _vString.Append("Amount: " + amount);
                    break;
                case 5:
                    _vString.Append("Spread: " + spread.ToString("F2"));
                    break;
                case 6:
                    _vString.Append("Speed: " + speed.ToString("F2"));
                    break;
                case 7:
                    _vString.Append("Longevity: " + longevity.ToString("F2") + " seconds");
                    break;
                case 8:
                    _vString.Append("Punch: " + punch.ToString("F2"));
                    break;
                case 9:
                    if (burst != 0)
                        _vString.Append("Burst: " + burst + ", Burst delay: " + burstDelay.ToString("F3"));
                    else
                        _vString.Append("Burst: No burst");
                    break;
                case 10:
                    _vString.Append("Bullet size: " + bulletSize.ToString("F2"));
                    break;
                case 11:
                    switch (bulletBonus)
                    {
                        case 0:
                            _vString.Append("Bullet bonus: Damage change: " + damageChange.ToString("F2"));
                            break;
                        case 1:
                            _vString.Append("Bullet bonus: Size change: " + bulletSizeChange.ToString("F2"));
                            break;
                        case 2:
                            _vString.Append("Bullet bonus: Speed change: " + speedChange.ToString("F2"));
                            break;
                        default:
                            _vString.Append("Bullet bonus: None");
                            break;
                    }
                    break;
                case 12:
                    switch (special)
                    {
                        case 0:
                            _vString.Append("Special: Homing: " + homingStrength.ToString("F2"));
                            break;
                        case 1:
                            _vString.Append("Special: Pierce: " + pierce);
                            break;
                        case 2:
                            _vString.Append("Special: Bounce: " + bounce);
                            break;
                        case 3:
                            _vString.Append("Special: Charge up: " + chargeUp.ToString("F2") + " seconds");
                            break;
                        default:
                            _vString.Append("Special: None");
                            break;
                    }
                    break;
                case 13:
                    switch (gunBonus)
                    {
                        case 0:
                            _vString.Append("Gun bonus: Fire rate change: " + fireRateChange.ToString("F2") + " over " + fireRateChangeTimer.ToString("F2") + " seconds");
                            break;
                        case 1:
                            _vString.Append("Gun bonus: Spread change: " + spreadChange.ToString("F2") + " over " + spreadChangeTimer.ToString("F2") + " seconds");
                            break;
                        default:
                            _vString.Append("Gun bonus: None");
                            break;
                    }
                    break;
                default:
                    break;
            }
            _v.text = new string(_vString.ToString());
            _vString.Clear();
        }
    }
}
