using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class GunRandomizer : MonoBehaviour
{
    //HEAVILY NEEDS A REWORK WHEN WEAPONS ARE DROPPED
    //Based on the testing phase of weapon implementation
    [SerializeField] GunMaster gm;
    [SerializeField] Transform bcMaster;
    [SerializeField] TMP_InputField input;
    [SerializeField] Transform textHolder;
    public int currentSeed = 0;
    string currentRandomSeed;
    const string characters = "abcdefghijklmnopqrstuvwxyz0123456789";
    [SerializeField] int minSeedChars;
    [SerializeField] int maxSeedChars;

    int special = 100;
    int gunBonus = 100;
    int bulletBonus = 100;
    public static Transform[] BulletTransforms;

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
    private void Start()
    {
        foreach (var stat in gm.gunArray)
        {
            StartCoroutine(SetBulletValues(stat));
            SetValues(stat);
        }
    }

    public void SetBulletValuesFunc()
    {
        foreach (var stat in gm.gunArray)
        {
            StartCoroutine(SetBulletValues(stat));
        }
    }

    public IEnumerator SetBulletValues(GunStats stat)
    {
        BulletController bc = stat.gameObject.GetComponent<GunController>().bc;
        bc.damage = stat.damage;
        bc.damageChange = stat.damageChange;
        bc.sizeChange = stat.bulletSizeChange;
        bc.speed = stat.speed;
        bc.speedChange = stat.speedChange;
        bc.bulletLongevity = stat.longevity;
        bc.pierce = stat.pierce;
        bc.bounce = stat.bounce;
        //bulletSC._bounceToTarget = bounceToTarget;
        bc.homing = stat.homing;
        bc.homingStrength = stat.homingStrength * 5 * stat.speed;
        bc.punch = stat.punch;
        bc.weightScalar = stat.weightScalar;
        yield return null;
    }

    float CurveWeightedRandom(AnimationCurve curve)
    {
        return curve.Evaluate(Random.value);
    }

    public void RandomizeGun()
    {
        foreach (GunStats stat in gm.gunArray)
        {
            Randomize(stat);
        }
    }
    void Randomize(GunStats stat)
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
            currentRandomSeed = seedBuilder.ToString();
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
            stat.damage = Mathf.Lerp(R_damage.x, R_damage.y, CurveWeightedRandom(R_damageChance) * 1.5f);
            stat.fireRate = Mathf.Lerp(R_fireRate.x, R_fireRate.y, CurveWeightedRandom(R_fireRateChance) * 0.75f);
        }
        else
        {
            stat.damage = Mathf.Lerp(R_damage.x, R_damage.y, CurveWeightedRandom(R_damageChance) * 0.75f);
            stat.fireRate = Mathf.Lerp(R_fireRate.x, R_fireRate.y, CurveWeightedRandom(R_fireRateChance) * 1.5f);
        }

        gunBonus = Random.Range(0, 4);
        switch (gunBonus)
        {
            case (0):
                stat.fireRateChange = Random.Range(R_fireRateChange.x, R_fireRateChange.y);
                stat.fireRateChangeTimer = Random.Range(R_fireRateChangeTimer.x, R_fireRateChangeTimer.y);
                stat.spreadChange = 0;
                stat.spreadChangeTimer = 0;
                break;
            case (1):
                switch (Random.Range(0, 2))
                {
                    case 0:
                        stat.spreadChange = Random.Range(R_spreadChange.y / 2, R_spreadChange.y);
                        break;
                    case 1:
                        stat.spreadChange = Random.Range(R_spreadChange.x, R_spreadChange.x / 2);
                        break;
                    default:
                        break;
                }
                stat.spreadChangeTimer = Random.Range(R_spreadChangeTimer.x, R_spreadChangeTimer.y);
                stat.fireRateChange = 0;
                stat.fireRateChangeTimer = 0;
                break;
            case (2):
                stat.fireRateChange = 0;
                stat.fireRateChangeTimer = 0;
                stat.spreadChange = 0;
                stat.spreadChangeTimer = 0;
                break;
            case (3):
                stat.fireRateChange = 0;
                stat.fireRateChangeTimer = 0;
                stat.spreadChange = 0;
                stat.spreadChangeTimer = 0;
                break;
            default:
                break;
        }

        switch (Random.Range(0, 5))
        {
            case (0):
                stat.burst = Mathf.FloorToInt(Random.Range(R_burst.x, R_burst.x + (R_burst.y - R_burst.x) / 2));
                stat.burstDelay = Random.Range(R_burstDelay.x, R_burstDelay.x + (R_burstDelay.y - R_burstDelay.x) / 2);
                stat.fireRate /= 2 * (stat.burst / 2);
                stat.fireRateChange /= 2;
                break;
            case (1):
                stat.burst = Mathf.FloorToInt(Random.Range(R_burst.x + (R_burst.y - R_burst.x) / 2, R_burst.y));
                stat.burstDelay = Random.Range(R_burstDelay.x + (R_burstDelay.y - R_burstDelay.x) / 2, R_burstDelay.y);
                stat.fireRate /= 2 * (stat.burst / 2);
                stat.fireRateChange /= 4;
                break;
            case (2):
                stat.burst = 0;
                stat.burstDelay = 0;
                break;
            case (3):
                stat.burst = 0;
                stat.burstDelay = 0;
                break;
            case (4):
                stat.burst = 0;
                stat.burstDelay = 0;
                break;
            default:
                break;
        }

        bulletBonus = Random.Range(0, 3);
        switch (bulletBonus)
        {
            case (0):
                stat.damageChange = Random.Range(R_damageChange.x, R_damageChange.y);
                stat.speedChange = 0;
                stat.bulletSizeChange = 0;
                break;
            case (1):
                stat.bulletSizeChange = Random.Range(R_bulletSizeChange.x, R_bulletSizeChange.y);
                stat.speedChange = 0;
                stat.damageChange = 0;
                break;
            case (2):
                stat.speedChange = Random.Range(R_speedChange.x, R_speedChange.y);
                stat.bulletSizeChange = 0;
                stat.damageChange = 0;
                break;
            case 3:
                stat.speedChange = 0;
                stat.bulletSizeChange = 0;
                stat.damageChange = 0;
                break;
            case 4:
                stat.speedChange = 0;
                stat.bulletSizeChange = 0;
                stat.damageChange = 0;
                break;
            default:
                break;
        }

        stat.bulletSize = Random.Range(R_bulletSize.x, R_bulletSize.y);

        switch (CurveWeightedRandom(R_Amount))
        {
            case < .33f:
                stat.amount = 1;
                break;
            case < .66f:
                stat.amount = 2;
                break;
            case > .66f:
                stat.amount = 3;
                break;
            default:
                break;
        }
        stat.spread = Random.Range(R_spread.x, R_spread.y);

        stat.speed = Random.Range(R_speed.x, R_speed.y);
        stat.longevity = Random.Range(R_longevity.x, R_longevity.y);

        special = Random.Range(0, 5);
        switch (special)
        {
            case (0):
                stat.homing = true;
                stat.homingStrength = Random.Range(R_homingStrength.x, R_homingStrength.y);
                stat.spread += stat.homingStrength * 2;
                stat.pierce = 0;
                stat.bounce = 0;
                stat.chargeUp = 0;
                break;
            case (1):
                stat.pierce = Mathf.FloorToInt(Random.Range(R_pierce.x, R_pierce.y));
                stat.homing = false;
                stat.homingStrength = 0;
                stat.bounce = 0;
                stat.chargeUp = 0;
                break;
            case (2):
                stat.bounce = Mathf.FloorToInt(Random.Range(R_bounce.x, R_bounce.y));
                stat.homing = false;
                stat.homingStrength = 0;
                stat.pierce = 0;
                stat.chargeUp = 0;
                break;
            case (3):
                stat.chargeUp = Random.Range(R_chargeUp.x, R_chargeUp.y);
                if (stat.burst == 0)
                {
                    switch (Random.Range(0, 2))
                    {
                        case (0):
                            stat.burst = Mathf.FloorToInt(Random.Range(R_burst.x, R_burst.x + (R_burst.y - R_burst.x) / 2));
                            stat.burstDelay = Random.Range(R_burstDelay.x, R_burstDelay.x + (R_burstDelay.y - R_burstDelay.x) / 2);
                            stat.fireRate /= 2;
                            break;
                        case (1):
                            stat.burst = Mathf.FloorToInt(Random.Range(R_burst.x + (R_burst.y - R_burst.x) / 2, R_burst.y));
                            stat.burstDelay = Random.Range(R_burstDelay.x + (R_burstDelay.y - R_burstDelay.x) / 2, R_burstDelay.y);
                            stat.fireRate /= 4;
                            break;
                        default:
                            break;
                    }
                }
                stat.burst += 1;
                stat.burst *= Mathf.CeilToInt(stat.chargeUp + 1);
                stat.burstDelay /= 2;
                stat.homing = false;
                stat.homingStrength = 0;
                stat.bounce = 0;
                stat.pierce = 0;
                break;
            case (4):
                stat.bounce = 0; 
                stat.homing = false;
                stat.homingStrength = 0;
                stat.pierce = 0;
                stat.chargeUp = 0;
                break;
            default:
                break;
        }
        stat.punch = Random.Range(R_punch.x, R_punch.y);
        SetValues(stat);
        StartCoroutine(SetBulletValues(stat));
    }

    void SetValues(GunStats stat)
    {
        StringBuilder _vString = new();
        for (int value = 0; value < textHolder.childCount; value++)
        {
            TMP_Text _v = textHolder.GetChild(value).GetComponent<TMP_Text>();
            switch (value)
            {
                case 1:
                    if (input.text == string.Empty)
                        _vString.Append("Seed: " + currentRandomSeed);
                    else
                        _vString.Append("Seed: " + input.text);
                    break;
                case 2:
                    _vString.Append("Damage: " + stat.damage.ToString("F2"));
                    break;
                case 3:
                    _vString.Append("Firerate: " + stat.fireRate.ToString("F2"));
                    break;
                case 4:
                    _vString.Append("Amount: " + stat.amount);
                    break;
                case 5:
                    _vString.Append("Spread: " + stat.spread.ToString("F2"));
                    break;
                case 6:
                    _vString.Append("Speed: " + stat.speed.ToString("F2"));
                    break;
                case 7:
                    _vString.Append("Longevity: " + stat.longevity.ToString("F2") + " seconds");
                    break;
                case 8:
                    _vString.Append("Punch: " + stat.punch.ToString("F2"));
                    break;
                case 9:
                    if (stat.burst != 0)
                        _vString.Append("Burst: " + stat.burst + ", Burst delay: " + stat.burstDelay.ToString("F3"));
                    else
                        _vString.Append("Burst: No burst");
                    break;
                case 10:
                    _vString.Append("Bullet size: " + stat.bulletSize.ToString("F2"));
                    break;
                case 11:
                    switch (bulletBonus)
                    {
                        default:
                            _vString.Append("Bullet bonus is deprecated, to be removed");
                            break;
                    }
                    break;
                case 12:
                    switch (special)
                    {
                        case 0:
                            _vString.Append("Special: Homing: " + stat.homingStrength.ToString("F2"));
                            break;
                        case 1:
                            _vString.Append("Special: Pierce: " + stat.pierce);
                            break;
                        case 2:
                            _vString.Append("Special: Bounce: " + stat.bounce);
                            break;
                        case 3:
                            _vString.Append("Special: Charge up: " + stat.chargeUp.ToString("F2") + " seconds");
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
                            _vString.Append("Gun bonus: Fire rate change: " + stat.fireRateChange.ToString("F2") + " over " + stat.fireRateChangeTimer.ToString("F2") + " seconds");
                            break;
                        case 1:
                            _vString.Append("Gun bonus: Spread change: " + stat.spreadChange.ToString("F2") + " over " + stat.spreadChangeTimer.ToString("F2") + " seconds");
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
