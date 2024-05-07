using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class RandomizeEquipment : MonoBehaviour
{
    public Color[] typeColor;
    public ParticleSystem.MinMaxGradient[] gradients;
    public Equipment[] relics;
    public Color keyColor;
    public Sprite keySprite;
    public string keyText;
    public string keyName;
    [Space]

    public Transform textBoxParent;

    #region WeaponRandomizing
    [Header("Weapon")]
    [SerializeField] Sprite[] gunSprites;
    [SerializeField] Sprite[] gameSpaceGunSprites;
    [SerializeField] List<string> bulletNameFirst = new();
    [SerializeField] List<string> bulletNameLast = new();
    [SerializeField] List<string> beamNameFirst = new();
    [SerializeField] List<string> beamNameLast = new();
    [SerializeField] List<string> railgunNameFirst = new();
    [SerializeField] List<string> railgunNameLast = new();
    [SerializeField] List<string> laserNameFirst = new();
    [SerializeField] List<string> laserNameLast = new();
    [SerializeField] List<string> waveNameFirst = new();
    [SerializeField] List<string> waveNameLast = new();
    [SerializeField] List<string> rocketNameFirst = new();
    [SerializeField] List<string> rocketNameLast = new();
    [SerializeField] List<string> needleNameFirst = new();
    [SerializeField] List<string> needleNameLast = new();
    [SerializeField] List<string> mineNameFirst = new();
    [SerializeField] List<string> mineNameLast = new();
    [SerializeField] List<string> hammerNameFirst = new();
    [SerializeField] List<string> hammerNameLast = new();
    [SerializeField] List<string> clusterNameFirst = new();
    [SerializeField] List<string> clusterNameLast = new();
    [SerializeField] List<string> arrowNameFirst = new();
    [SerializeField] List<string> arrowNameLast = new();
    [SerializeField] List<string> grandNameFirst = new();
    [SerializeField] List<string> grandNameLast = new();
    [Header("WeaponStats")]
    [SerializeField] Vector3 damage;              //On bullet
    //[SerializeField] AnimationCurve damageChance;
    [SerializeField] Vector2 bulletSize;          //On gun
    [SerializeField] Vector3 fireRate;
    //[SerializeField] AnimationCurve fireRateChance;        //On gun
    [SerializeField] Vector2 fireRateChange;   //On gun
    [SerializeField] Vector2 fireRateChangeTimer; //On gun
    [SerializeField] Vector3 amount;                //On gun
    [SerializeField] AnimationCurve A_amount;
    [SerializeField] Vector3 spread;              //On gun
    [SerializeField] Vector2 spreadChange;        //On gun
    [SerializeField] Vector2 spreadChangeTimer;
    [SerializeField] Vector3 speed;               //On gun
    [SerializeField] Vector2 longevity;           //On bullet
    [SerializeField] Vector3 homingStrength;      //On bullet
    [SerializeField] Vector3 pierce;                //On bullet
    [SerializeField] Vector3 bounce;                //On bullet
    //[SerializeField] bool bounceToTarget;
    [SerializeField] Vector3 chargeUp;            //On gun
    [SerializeField] Vector3 burst;                 //On gun
    [SerializeField] Vector3 burstDelay;          //On gun
    [SerializeField] Vector3 punch;               //On bullet
    [SerializeField] Vector3 rotationSpeed;

    [SerializeField] Vector2 explosionDamageMultiplier;
    [SerializeField] Vector3 explosiveRange;

    [SerializeField] Vector2 cluster;
    [SerializeField] Vector2 clusterAmount;
    #endregion

    [Header("Thruster")]
    [SerializeField] Sprite[] thrusterSprites;
    [SerializeField] string[] thrusterNameFirst;
    [SerializeField] string[] thrusterNameLast;

    [SerializeField] Vector3 moveSpeed;
    [SerializeField] Vector3 maxSpeed;
    [SerializeField] Vector3 turnSpeed;
    [SerializeField] Vector3 maxTurnSpeed;
    [SerializeField] Vector3 turnBrakingSpeed;
    [SerializeField] Vector3 brakingSpeed;

    [SerializeField] Vector3 ftlAcc;
    [SerializeField] Vector3 ftlMaxSpeed;
    [SerializeField] Vector3 ftlRot;
    [SerializeField] Vector3 chargeTime;
    [SerializeField] Vector2 fuelCurrent;
    [SerializeField] Vector3 fuelMax;
    [SerializeField] Vector3 fuelDrain;
    [SerializeField] Vector3 maxDuration;

    //[Header("FTL Engine")]
    //[SerializeField] Sprite[] ftlSprites;
    //[SerializeField] string[] ftlNameFirst;
    //[SerializeField] string[] ftlNameLast;

    //[SerializeField] Vector2 acceleration;
    //[SerializeField] Vector2 ftlMaxSpeed;
    //[SerializeField] Vector2 rotSpeed;
    //[SerializeField] Vector2 chargeTime;
    //[SerializeField] Vector2 fuelCurrent;
    //[SerializeField] Vector2 fuelMax;
    //[SerializeField] Vector2 fuelDrain;
    //[SerializeField] Vector2 maxDuration;

    [Header("Collector")]
    [SerializeField] Sprite[] colSprites;
    [SerializeField] string[] colNameFirst;
    [SerializeField] string[] colNameLast;

    [SerializeField] Vector3 speedTo;
    [SerializeField] Vector3 speedFrom;
    [SerializeField] Vector3 colAmount;
    [SerializeField] Vector3 range;

    [Header("Shield")]
    [SerializeField] Sprite[] shieldSprites;
    [SerializeField] string[] shieldNameFirst;
    [SerializeField] string[] shieldNameLast;

    [SerializeField] Vector3 shieldHealth;
    [SerializeField] Vector3 shieldRechargeSpeed;
    [SerializeField] Vector3 shieldRechargeDelay;
    [SerializeField] Vector3 breakAnim;
    [SerializeField] Vector3 restoreAnim;

    [Header("Hull")]
    [SerializeField] Sprite[] hullSprites;
    [SerializeField] string[] hullNameFirst;
    [SerializeField] string[] hullNameLast;

    [SerializeField] Vector2 hullNodes;
    [SerializeField] Vector3 hullMax;
    [SerializeField] Vector3 hullDamageNeg;
    [SerializeField] Vector3 hullWeight;

    [Header("Scanner")]
    [SerializeField] Sprite[] scannerSprites;
    [SerializeField] string[] scannerNameFirst;
    [SerializeField] string[] scannerNameLast;

    [SerializeField] Vector4 zoom;
    [SerializeField] Vector3Int mapUpdateAmount;
    [SerializeField] Vector3 mapUpdateSpeed;

    #region Singleton
    public static RandomizeEquipment Instance;

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

    public Equipment RandomizeRandom(int level)
    {
        bool accepted = false;
        EquipmentTypes type;
        Equipment newEquipment = null;
        while (!accepted)
        {
            
            type = (EquipmentTypes)Random.Range(2, 11);
            
            switch (type)
            {
                case EquipmentTypes.Weapon:
                    newEquipment = RandomizeGun(level) as Weapon;
                    accepted = true;
                    break;
                case EquipmentTypes.Shield:
                    newEquipment = RandomizeShield(level) as Shield;
                    accepted = true;
                    break;
                case EquipmentTypes.Thruster:
                    newEquipment = RandomizeThruster(level) as Thrusters;
                    accepted = true;
                    break;
                //case EquipmentTypes.FTL:
                //    newEquipment = RandomizeEquipment.Instance.RandomizeFTLEngine() as FTLEngine;
                //    accepted = true;
                //    break;
                case EquipmentTypes.Hull:
                    newEquipment = RandomizeHull(level) as Hull;
                    accepted = true;
                    break;
                case EquipmentTypes.Scanner:
                    newEquipment = RandomizeScanner(level) as Scanner;
                    accepted = true;
                    break;
                case EquipmentTypes.Cargo:
                    break;
                case EquipmentTypes.Collector:
                    newEquipment = RandomizeCollector(level) as Collector;
                    accepted = true;
                    break;
                case EquipmentTypes.Relic:
                    newEquipment = RandomizeRelic();
                    accepted = true;
                    break;
                case EquipmentTypes.Key:
                    Equipment copyKey = ScriptableObject.CreateInstance<Equipment>();
                    copyKey.equipType = EquipmentTypes.Key;
                    copyKey.statsText = keyText;
                    copyKey.statLength = 1;
                    copyKey.color = keyColor;
                    copyKey.icon = keySprite;
                    copyKey.itemName = keyName;
                    newEquipment = copyKey;
                    accepted = true;
                    break;
                case EquipmentTypes.Default:
                    break;
                default:
                    break;
            }
        }
        return newEquipment;
    }
    public Equipment RandomizeRelic()
    {
        Debug.Log(relics.Length);
        Equipment copyRelic = relics[iR(0, relics.Length - 1)];
        Equipment relic = null;
        switch (copyRelic.GetType().FullName)
        {
            case "QuantumTargeting":
                relic = ScriptableObject.CreateInstance<QuantumTargeting>();
                break;
            case "FriendModule":
                relic = ScriptableObject.CreateInstance<FriendModule>();
                break;
            case "FissionBarrel":
                relic = ScriptableObject.CreateInstance<FissionBarrel>();
                break;
            default:
                Debug.Log("Something went wrong with the relics");
                break;
        }
        relic.gradient = gradients[(int)EquipmentTypes.Relic];

        relic.itemName = copyRelic.itemName;
        relic.description = copyRelic.description;
        relic.icon = copyRelic.icon;
        relic.value = copyRelic.value;
        relic.color = copyRelic.color;
        relic.equipType = copyRelic.equipType;
        relic.relic = copyRelic.relic;
        relic.statsText = copyRelic.statsText;
        relic.statLength = copyRelic.statLength;
        relic.id = Inventory.Instance.id;
        return relic;
    }
     
    #region RandomizeGun
    public Weapon RandomizeGun(int level)
    {
        bool fireRateBonus = false;
        bool accuracyBonus = false;
        bool isHoming = false;
        bool isBounce = false;
        bool isPierce = false;
        bool isBurst = false;
        bool isCharge = false;

        Weapon weapon = ScriptableObject.CreateInstance<Weapon>();
        weapon.name = weapon.itemName;
        weapon.id = Inventory.Instance.id;
        weapon.color = typeColor[(int)EquipmentTypes.Weapon];
        weapon.equipType = EquipmentTypes.Weapon;
        weapon.effectColor = Random.ColorHSV(0f, 1f, 0.8f, 1f, 0.6f, 1);
        weapon.level = level;
        weapon.weaponType = (WeaponType)iR(0, 11);
        if (weapon.weaponType == WeaponType.Arrow) { weapon.weaponType = (WeaponType)iR(0, 9); }

        switch (weapon.weaponType)
        {
            case WeaponType.Bullet:
                weapon.itemName = new string
                (bulletNameFirst[iR(0, bulletNameFirst.Count - 1)]
                + " "
                + bulletNameLast[iR(0, bulletNameLast.Count - 1)]);
                break;
            case WeaponType.Beam:
                weapon.itemName = new string
                (beamNameFirst[iR(0, beamNameFirst.Count - 1)]
                + " "
                + beamNameLast[iR(0, beamNameLast.Count - 1)]);
                break;
            case WeaponType.Railgun:
                weapon.itemName = new string
                (railgunNameFirst[iR(0, railgunNameFirst.Count - 1)]
                + " "
                + railgunNameLast[iR(0, railgunNameLast.Count - 1)]);
                break;
            case WeaponType.Laser:
                weapon.itemName = new string
                (laserNameFirst[iR(0, laserNameFirst.Count - 1)]
                + " "
                + laserNameLast[iR(0, laserNameLast.Count - 1)]);
                break;
            case WeaponType.Wave:
                weapon.itemName = new string
                (waveNameFirst[iR(0, waveNameFirst.Count - 1)]
                + " "
                + waveNameLast[iR(0, waveNameLast.Count - 1)]);
                break;
            case WeaponType.Rocket:
                weapon.itemName = new string
                (rocketNameFirst[iR(0, rocketNameFirst.Count - 1)]
                + " "
                + rocketNameLast[iR(0, rocketNameLast.Count - 1)]);
                break;
            case WeaponType.Needle:
                weapon.itemName = new string
                (needleNameFirst[iR(0, needleNameFirst.Count - 1)]
                + " "
                + needleNameLast[iR(0, needleNameLast.Count - 1)]);
                break;
            case WeaponType.Mine:
                weapon.itemName = new string
                (mineNameFirst[iR(0, mineNameFirst.Count - 1)]
                + " "
                + mineNameLast[iR(0, mineNameLast.Count - 1)]);
                break;
            case WeaponType.Hammer:
                weapon.itemName = new string
                (hammerNameFirst[iR(0, hammerNameFirst.Count - 1)]
                + " "
                + hammerNameLast[iR(0, hammerNameLast.Count - 1)]);
                break;
            case WeaponType.Cluster:
                weapon.itemName = new string
                (clusterNameFirst[iR(0, clusterNameFirst.Count - 1)]
                + " "
                + clusterNameLast[iR(0, clusterNameLast.Count - 1)]);
                break;
            case WeaponType.Arrow:
                break;
            case WeaponType.Grand:
                weapon.itemName = new string
                (grandNameFirst[iR(0, grandNameFirst.Count - 1)]
                + " "
                + grandNameLast[iR(0, grandNameLast.Count - 1)]);
                break;
            case WeaponType.Mirage:
                break;
            case WeaponType.Void:
                break;
            case WeaponType.Blade:
                break;
            default:
                break;
        }
        weapon.icon = gunSprites[(int)weapon.weaponType];
        weapon.gameSpaceSprite = gameSpaceGunSprites[(int)weapon.weaponType];

        weapon.gradient = gradients[(int)EquipmentTypes.Weapon];

        weapon.bulletSize = fR(bulletSize);

        weapon.amount = (int)Mathf.Lerp(amount.x, amount.y, CurveWeightedRandom(A_amount));

        weapon.longevity = fR(longevity);

        weapon.punch = 0;

        weapon.rotationSpeed = fR(rotationSpeed);


        int style = SetStyle(weapon);

        switch (weapon.weaponType)
        {
            case WeaponType.Bullet:
                switch (AddChargeOrBurst(weapon, style))
                {
                    case 0:
                        break;
                    case 1:
                        isCharge = true;
                        break;
                    case 2:
                        isBurst = true;
                        break;
                    default:
                        break;
                }
                
                switch (AddBonusFireROrAcc(weapon))
                {
                    case 0:
                        break;
                    case 1:
                        fireRateBonus = true;
                        break;
                    case 2:
                        accuracyBonus = true;
                        break;
                    default:
                        break;
                }

                switch (AddBounceOrPierce(weapon))
                {
                    case 0:
                        break;
                    case 1:
                        isBounce = true;
                        break;
                    case 2:
                        isPierce = true;
                        break;
                    default:
                        break;
                }
                isHoming = AddHoming(weapon);
                weapon.punch = fR(punch) / 2;
                break;
            case WeaponType.Beam:

                switch (AddBounceOrPierce(weapon))
                {
                    case 0:
                        break;
                    case 1:
                        isBounce = true;
                        break;
                    case 2:
                        isPierce = true;
                        break;
                    default:
                        break;
                }
                weapon.amount = 1;
                isBounce = false;
                isBurst = false;
                isCharge = false;
                isHoming = false;
                break;
            case WeaponType.Railgun:
                switch (AddChargeOrBurst(weapon, style))
                {
                    case 0:
                        break;
                    case 1:
                        isCharge = true;
                        break;
                    case 2:
                        isBurst = true;
                        break;
                    default:
                        break;
                }

                switch (AddBonusFireROrAcc(weapon))
                {
                    case 0:
                        break;
                    case 1:
                        fireRateBonus = true;
                        break;
                    case 2:
                        accuracyBonus = true;
                        break;
                    default:
                        break;
                }

                switch (AddBounceOrPierce(weapon))
                {
                    case 0:
                        break;
                    case 1:
                        isBounce = true;
                        break;
                    case 2:
                        isPierce = true;
                        break;
                    default:
                        break;
                }
                isHoming = false;
                isBounce = false;
                break;
            case WeaponType.Laser:
                switch (AddChargeOrBurst(weapon, style))
                {
                    case 0:
                        break;
                    case 1:
                        isCharge = true;
                        break;
                    case 2:
                        isBurst = true;
                        break;
                    default:
                        break;
                }

                switch (AddBonusFireROrAcc(weapon))
                {
                    case 0:
                        break;
                    case 1:
                        fireRateBonus = true;
                        break;
                    case 2:
                        accuracyBonus = true;
                        break;
                    default:
                        break;
                }

                switch (AddBounceOrPierce(weapon))
                {
                    case 0:
                        break;
                    case 1:
                        isBounce = true;
                        break;
                    case 2:
                        isPierce = true;
                        break;
                    default:
                        break;
                }
                isHoming = AddHoming(weapon);

                weapon.speed *= 2;
                weapon.fireRate *= 2;
                weapon.damage /= 2;

                break;
            case WeaponType.Wave:
                switch (AddChargeOrBurst(weapon, style))
                {
                    case 0:
                        break;
                    case 1:
                        isCharge = true;
                        break;
                    case 2:
                        isBurst = true;
                        break;
                    default:
                        break;
                }

                switch (AddBonusFireROrAcc(weapon))
                {
                    case 0:
                        break;
                    case 1:
                        fireRateBonus = true;
                        break;
                    case 2:
                        accuracyBonus = true;
                        break;
                    default:
                        break;
                }

                switch (AddBounceOrPierce(weapon))
                {
                    case 0:
                        break;
                    case 1:
                        isBounce = true;
                        break;
                    case 2:
                        isPierce = true;
                        break;
                    default:
                        break;
                }
                weapon.speed /= 4;
                weapon.pierce += 2;
                isPierce = true;
                weapon.fireRate /= 2;
                break;
            case WeaponType.Rocket:
                switch (AddChargeOrBurst(weapon, style))
                {
                    case 0:
                        break;
                    case 1:
                        isCharge = true;
                        break;
                    case 2:
                        isBurst = true;
                        break;
                    default:
                        break;
                }

                switch (AddBonusFireROrAcc(weapon))
                {
                    case 0:
                        break;
                    case 1:
                        fireRateBonus = true;
                        break;
                    case 2:
                        accuracyBonus = true;
                        break;
                    default:
                        break;
                }

                switch (AddBounceOrPierce(weapon))
                {
                    case 0:
                        break;
                    case 1:
                        isBounce = true;
                        break;
                    case 2:
                        isPierce = true;
                        break;
                    default:
                        break;
                }
                isHoming = AddHoming(weapon);

                SetExplosiveness(weapon);
                weapon.fireRate /= 2;

                break;
            case WeaponType.Needle:
                switch (AddBonusFireROrAcc(weapon))
                {
                    case 0:
                        break;
                    case 1:
                        fireRateBonus = true;
                        break;
                    case 2:
                        accuracyBonus = true;
                        break;
                    default:
                        break;
                }

                switch (AddBounceOrPierce(weapon))
                {
                    case 0:
                        break;
                    case 1:
                        isBounce = true;
                        break;
                    case 2:
                        isPierce = true;
                        break;
                    default:
                        break;
                }
                isHoming = true;
                weapon.homing = true;
                weapon.homingStrength = fR(homingStrength) * 3 * 10;
                weapon.amount += 2; 
                weapon.damage /= 4;
                weapon.fireRate *= 2;
                weapon.speed = 3;
                weapon.punch = 0;
                break;
            case WeaponType.Mine:

                switch (AddBonusFireROrAcc(weapon))
                {
                    case 0:
                        break;
                    case 1:
                        fireRateBonus = true;
                        break;
                    case 2:
                        accuracyBonus = true;
                        break;
                    default:
                        break;
                }
                isHoming = AddHoming(weapon);
                SetExplosiveness(weapon);
                weapon.explosiveMultiplier *= 2;
                weapon.splashDamage *= 2;
                weapon.splashRange *= 2;
                weapon.fireRate /= 10;
                weapon.speed /= 3;
                weapon.longevity *= 10;
                break;
            case WeaponType.Hammer:
                switch (AddChargeOrBurst(weapon, style))
                {
                    case 0:
                        break;
                    case 1:
                        isCharge = true;
                        break;
                    case 2:
                        isBurst = true;
                        break;
                    default:
                        break;
                }

                switch (AddBonusFireROrAcc(weapon))
                {
                    case 0:
                        break;
                    case 1:
                        fireRateBonus = true;
                        break;
                    case 2:
                        accuracyBonus = true;
                        break;
                    default:
                        break;
                }

                switch (AddBounceOrPierce(weapon))
                {
                    case 0:
                        break;
                    case 1:
                        isBounce = true;
                        break;
                    case 2:
                        isPierce = true;
                        break;
                    default:
                        break;
                }
                weapon.punch = fR(punch);
                weapon.amount += 10;
                weapon.damage *= 0.8f;
                weapon.spread *= 2;
                weapon.fireRate /= 10;
                break;
            case WeaponType.Cluster:
                switch (AddChargeOrBurst(weapon, style))
                {
                    case 0:
                        break;
                    case 1:
                        isCharge = true;
                        break;
                    case 2:
                        isBurst = true;
                        break;
                    default:
                        break;
                }

                switch (AddBonusFireROrAcc(weapon))
                {
                    case 0:
                        break;
                    case 1:
                        fireRateBonus = true;
                        break;
                    case 2:
                        accuracyBonus = true;
                        break;
                    default:
                        break;
                }

                switch (AddBounceOrPierce(weapon))
                {
                    case 0:
                        break;
                    case 1:
                        isBounce = true;
                        break;
                    case 2:
                        isPierce = true;
                        break;
                    default:
                        break;
                }
                isHoming = AddHoming(weapon);
                weapon.damage *= 2;
                weapon.spread /= 2;
                weapon.fireRate /= 4;
                weapon.cluster = iR(cluster);
                weapon.clusterAmount = iR(clusterAmount);
                weapon.clusterSpeed = weapon.speed / 2;
                break;
            case WeaponType.Arrow:
                switch (AddBonusFireROrAcc(weapon))
                {
                    case 0:
                        break;
                    case 1:
                        fireRateBonus = true;
                        break;
                    case 2:
                        accuracyBonus = true;
                        break;
                    default:
                        break;
                }

                switch (AddBounceOrPierce(weapon))
                {
                    case 0:
                        break;
                    case 1:
                        isBounce = true;
                        break;
                    case 2:
                        isPierce = true;
                        break;
                    default:
                        break;
                }
                weapon.punch = fR(punch);
                isHoming = AddHoming(weapon);
                weapon.damage *= 4;
                weapon.spread /= 2;
                weapon.fireRate /= 10;
                break;
            case WeaponType.Mirage:
                break;
            case WeaponType.Grand:
                switch (AddChargeOrBurst(weapon, style))
                {
                    case 0:
                        break;
                    case 1:
                        isCharge = true;
                        break;
                    case 2:
                        isBurst = true;
                        break;
                    default:
                        break;
                }

                switch (AddBonusFireROrAcc(weapon))
                {
                    case 0:
                        break;
                    case 1:
                        fireRateBonus = true;
                        break;
                    case 2:
                        accuracyBonus = true;
                        break;
                    default:
                        break;
                }

                switch (AddBounceOrPierce(weapon))
                {
                    case 0:
                        break;
                    case 1:
                        isBounce = true;
                        break;
                    case 2:
                        isPierce = true;
                        break;
                    default:
                        break;
                }
                weapon.punch = fR(punch);
                weapon.damage *= 10f;
                weapon.spread /= 2;
                weapon.fireRate /= 10;
                weapon.speed *= 2;
                break;
            case WeaponType.Void:
                break;
            case WeaponType.Blade:
                break;
            default:
                break;
        }

        if (weapon.isExplosive) { weapon.bounce = 0; isBounce = false; }

        for (int i = 0; i < level; i++)
        {
            weapon.damage *= 1 + (damage.z);
            weapon.fireRate *= 1 + (fireRate.z);
            weapon.amount *= 1 + Mathf.RoundToInt(amount.z);
            weapon.spread *= 1 + (spread.z);
            weapon.speed *= 1 + (speed.z);
            weapon.homingStrength *= 1 + (homingStrength.z);
            weapon.pierce *= 1 + Mathf.RoundToInt(pierce.z);
            weapon.bounce *= 1 + Mathf.RoundToInt(bounce.z);
            weapon.chargeUp *= 1 + (chargeUp.z);
            weapon.burst *= 1 + Mathf.RoundToInt(burst.z);
            weapon.punch *= 1 + (punch.z);
            weapon.rotationSpeed *= 1 + (rotationSpeed.z);
            weapon.splashRange *= 1 + (explosiveRange.z);
            weapon.splashDamage *= 1 + damage.z;
        }

        if (weapon.weaponType == WeaponType.Beam || weapon.weaponType == WeaponType.Railgun) { weapon.speed /= 2f; }

        #region Weapon type stats text
        string statsNames = "Something is wong";
        string statsValues = "WTF";
        //bool showFireRate = true;
        bool anyBonus = isHoming || isPierce || isBounce || isBurst || isCharge || fireRateBonus || accuracyBonus;
        bool anyBonusBeamSpecific = isPierce;
        bool anyBonusRailgunSpecific = isBurst || isCharge || isPierce;

        float dps = (weapon.damage + weapon.splashDamage) * weapon.amount * weapon.fireRate;
        float dpsBurst = ((weapon.damage + weapon.splashDamage) * weapon.amount * weapon.burst * (weapon.burst * weapon.burstDelay)) * weapon.fireRate;
        float dpsCharge = ((weapon.damage + weapon.splashDamage) * weapon.amount * weapon.burst * (weapon.burst * weapon.burstDelay)) / weapon.chargeUp;
        float range = weapon.speed * weapon.longevity;
        float accuracy = weapon.spread;


        switch (weapon.weaponType)
        {
            case WeaponType.Bullet:
                statsNames = "";
                statsNames += "DPS:\n"; weapon.statLength++;
                statsNames += "Accuracy:\n"; weapon.statLength++;
                statsNames += "Range:\n"; weapon.statLength++;
                statsNames += "Modifiers:\n";
                statsValues = "";
                if (isCharge) statsValues += dpsCharge.ToString("F2") + "\n";
                else if (isBurst) statsValues += dpsBurst.ToString("F2") + "\n";
                else statsValues += dps.ToString("F2") + "\n";
                statsValues += accuracy.ToString("F2") + "\n";
                statsValues += range.ToString("F2") + "\n";
                if (isHoming) { statsValues += "Homing: " + weapon.homingStrength.ToString("F2") + "\n"; weapon.statLength++; }
                if (isPierce) { statsValues += "Pierce: " + weapon.pierce + "\n"; weapon.statLength++; }
                if (isBounce) { statsValues += "Bounce: " + weapon.bounce + "\n"; weapon.statLength++; }
                if (isBurst) { statsValues += "Burst: " + weapon.burst + "\n"; weapon.statLength++; }
                if (isCharge) { statsValues += "Charge: " + weapon.chargeUp.ToString("F2") + "\n"; weapon.statLength++; }
                if (fireRateBonus) { statsValues += "Cascading: " + weapon.fireRateChange.ToString("F1") + "\n"; weapon.statLength++; }
                if (accuracyBonus) { statsValues += "Narrowing: " + weapon.spreadChange.ToString("F1") + "\n"; weapon.statLength++; }
                if (!anyBonus) { statsValues += "None"; weapon.statLength++; }
                break;
            //if (isHoming) { statsNames += "Homing:\n" + "Strength:\n"; weapon.statLength += 2; }
            //if (isPierce) { statsNames += "Piercing:\n"; weapon.statLength++; }
            //if (isBounce) { statsNames += "Bounces:\n"; weapon.statLength++; }
            //if (isBurst) { statsNames += "Burst:\n" + "Burst delay:\n"; weapon.statLength += 2; }
            //if (isCharge) { statsNames += "Charge up:\n" + "Burst:\n" + "Burst delay:\n"; weapon.statLength += 3; showFireRate = false; }
            //if (anyBonus) { statsNames += "\n"; weapon.statLength++; }


            //statsNames += "Bullet size:\n"; weapon.statLength++;
            //statsNames += "Projectiles:\n"; weapon.statLength++;

            //statsNames += "Projectile life:\n"; weapon.statLength++;
            //statsNames += "Punch:\n"; weapon.statLength++;





            //if (isHoming) statsValues += weapon.homing + "\n" + weapon.homingStrength.ToString("F2") + "\n";
            //if (isPierce) statsValues += weapon.pierce + "\n";
            //if (isBounce) statsValues += weapon.bounce + "\n";
            //if (isBurst) statsValues += weapon.burst + "\n" + weapon.burstDelay.ToString("F3") + "\n";
            //if (isCharge) statsValues += weapon.chargeUp.ToString("F2") + "\n" + weapon.burst + "\n" + weapon.burstDelay.ToString("F3") + "\n";
            //if (anyBonus) statsValues += "\n";
            //if (weapon.amount > 1) statsValues += weapon.damage.ToString("F2") + " x " + weapon.amount + "\n";
            //else statsValues += weapon.damage.ToString("F2") + "\n";

            //if (isCharge) { }
            //else if (fireRateBonus) { statsValues += weapon.fireRate.ToString("F2") + " -> " + (weapon.fireRate + weapon.fireRateChange).ToString("F2") + "\n"; }
            //else statsValues += weapon.fireRate.ToString("F2") + "\n";


            //statsValues += weapon.bulletSize.ToString("F2") + "\n";
            //statsValues += weapon.amount + "\n";

            //if (accuracyBonus) { statsValues += weapon.spread.ToString("F2") + " -> " + Mathf.Clamp(weapon.spread + weapon.spreadChange, 0, 100).ToString("F2") + "\n"; }
            //else { statsValues += weapon.spread.ToString("F2") + "\n"; }

            //statsValues += weapon.longevity.ToString("F2") + "\n";
            //statsValues += weapon.punch.ToString("F2") + "\n";
            //statsValues += weapon.rotationSpeed.ToString("F2") + "\n";


            case WeaponType.Beam:
                statsNames = "";
                statsNames += "DPS:\n"; weapon.statLength++;
                statsNames += "Accuracy:\n"; weapon.statLength++;
                statsNames += "Range:\n"; weapon.statLength++;
                statsNames += "Modifiers:\n";
                statsValues = "";
                if (isCharge) statsValues += dpsCharge.ToString("F2") + "\n";
                else if (isBurst) statsValues += dpsBurst.ToString("F2") + "\n";
                else statsValues += dps.ToString("F2") + "\n";
                statsValues += accuracy.ToString("F2") + "\n";
                statsValues += range.ToString("F2") + "\n";
                if (isHoming) { statsValues += "Homing: " + weapon.homingStrength.ToString("F0") + "\n"; weapon.statLength++; }
                if (isPierce) { statsValues += "Pierce: " + weapon.pierce + "\n"; weapon.statLength++; }
                if (isBounce) { statsValues += "Bounce: " + weapon.bounce + "\n"; weapon.statLength++; }
                if (isBurst) { statsValues += "Burst: " + weapon.burst + "\n"; weapon.statLength++; }
                if (isCharge) { statsValues += "Charge: " + weapon.chargeUp.ToString("F2") + "\n"; weapon.statLength++; }
                if (fireRateBonus) { statsValues += "Cascading: " + weapon.fireRateChange.ToString("F1") + "\n"; weapon.statLength++; }
                if (accuracyBonus) { statsValues += "Narrowing: " + weapon.spreadChange.ToString("F1") + "\n"; weapon.statLength++; }
                statsValues += "Hitscan"; weapon.statLength++;
                //statsNames = "";
                //if (isPierce) { statsNames += "Piercing:\n"; weapon.statLength++; }
                //if (anyBonusBeamSpecific) { statsNames += "\n"; weapon.statLength++; }
                //statsNames += "DPS:\n"; weapon.statLength++;
                ////if (isPierce) statsNames += "\n" + "Piercing:\n";
                //statsNames += "Range:\n"; weapon.statLength++;
                //statsNames += "Rotation speed:\n"; weapon.statLength++;

                //statsValues = "";
                //if (isPierce) statsValues += weapon.pierce + "\n";
                //if (anyBonusBeamSpecific) statsValues += "\n";
                //if (fireRateBonus) { statsValues += ((weapon.damage + weapon.fireRate)).ToString("F2") + 
                //        " -> "
                //        + (weapon.damage * (weapon.fireRate + weapon.fireRateChange)).ToString("F2") + "\n"; }
                //else { statsValues += (weapon.damage * weapon.fireRate).ToString("F2") + "\n"; }
                ////if (isPierce) statsValues += "\n" + weapon.pierce + "\n";
                //statsValues += (weapon.speed * weapon.longevity).ToString("F2") + "\n";
                //statsValues += weapon.rotationSpeed.ToString("F2") + "\n";
                break;
            case WeaponType.Railgun:
                statsNames = "";
                statsNames += "DPS:\n"; weapon.statLength++;
                statsNames += "Accuracy:\n"; weapon.statLength++;
                statsNames += "Range:\n"; weapon.statLength++;
                statsNames += "Modifiers:\n";
                statsValues = "";
                if (isCharge) statsValues += dpsCharge.ToString("F2") + "\n";
                else if (isBurst) statsValues += dpsBurst.ToString("F2") + "\n";
                else statsValues += dps.ToString("F2") + "\n";
                statsValues += accuracy.ToString("F2") + "\n";
                statsValues += range.ToString("F2") + "\n";
                if (isHoming) { statsValues += "Homing: " + weapon.homingStrength.ToString("F0") + "\n"; weapon.statLength++; }
                if (isPierce) { statsValues += "Pierce: " + weapon.pierce + "\n"; weapon.statLength++; }
                if (isBounce) { statsValues += "Bounce: " + weapon.bounce + "\n"; weapon.statLength++; }
                if (isBurst) { statsValues += "Burst: " + weapon.burst + "\n"; weapon.statLength++; }
                if (isCharge) { statsValues += "Charge: " + weapon.chargeUp.ToString("F2") + "\n"; weapon.statLength++; }
                if (fireRateBonus) { statsValues += "Cascading: " + weapon.fireRateChange.ToString("F1") + "\n"; weapon.statLength++; }
                if (accuracyBonus) { statsValues += "Narrowing: " + weapon.spreadChange.ToString("F1") + "\n"; weapon.statLength++; }
                statsValues += "Hitscan"; weapon.statLength++;
                //statsNames = "";
                //if (isPierce) { statsNames += "Piercing:\n"; weapon.statLength++; }
                //if (isBurst) { statsNames += "Burst:\n" + "Burst delay:\n"; weapon.statLength += 2; }
                //if (isCharge) { statsNames += "Charge up:\n" + "Burst:\n" + "Burst delay:\n"; weapon.statLength += 3; showFireRate = false; }
                //if (anyBonusRailgunSpecific) { statsNames += "\n"; weapon.statLength++; }
                //statsNames += "Damage:\n"; weapon.statLength++;
                //if (isCharge) { }
                //else { statsNames += "Fire rate:\n"; weapon.statLength++; }
                ////if (isPierce) statsNames += "\n" + "Piercing:\n";
                ////statsNames += "Projectiles:\n"; weapon.statLength++;
                //statsNames += "Spread:\n"; weapon.statLength++;
                //statsNames += "Range:\n"; weapon.statLength++;
                ////statsNames += "Punch:\n";
                //statsNames += "Rotation speed:\n"; weapon.statLength++;

                //statsValues = "";
                //if (isPierce) statsValues += weapon.pierce + "\n";
                //if (isBurst) statsValues += weapon.burst + "\n" + weapon.burstDelay.ToString("F3") + "\n";

                //if (isCharge) statsValues += weapon.chargeUp.ToString("F2") + "\n" + weapon.burst + "\n" + weapon.burstDelay.ToString("F3") + "\n";
                //if (anyBonusRailgunSpecific) statsValues += "\n";
                //if (weapon.amount > 1) statsValues += weapon.damage.ToString("F2") + " x " + weapon.amount.ToString("F0") + "\n";
                //else statsValues += weapon.damage.ToString("F2") + "\n";
                //if (isCharge) { }
                //else if (fireRateBonus) { statsValues += weapon.fireRate.ToString("F2") + " -> " + Mathf.Clamp(weapon.fireRate + weapon.fireRateChange, 0, 100).ToString("F2") + "\n"; }
                //else statsValues += weapon.fireRate.ToString("F2") + "\n";
                ////if (isPierce) statsValues += "\n" + weapon.pierce + "\n";
                ////statsValues += weapon.amount + "\n";
                //if (accuracyBonus) { statsValues += weapon.spread.ToString("F2") + " -> " + (weapon.spread + weapon.spreadChange).ToString("F2") + "\n"; }
                //else { statsValues += weapon.spread.ToString("F2") + "\n"; }
                //statsValues += (weapon.speed * weapon.longevity).ToString("F2") + "\n";
                ////statsValues += weapon.punch + "\n";
                //statsValues += weapon.rotationSpeed.ToString("F2") + "\n";
                break;
            case WeaponType.Laser:
                statsNames = "";
                statsNames += "DPS:\n"; weapon.statLength++;
                statsNames += "Accuracy:\n"; weapon.statLength++;
                statsNames += "Range:\n"; weapon.statLength++;
                statsNames += "Modifiers:\n";
                statsValues = "";
                if (isCharge) statsValues += dpsCharge.ToString("F2") + "\n";
                else if (isBurst) statsValues += dpsBurst.ToString("F2") + "\n";
                else statsValues += dps.ToString("F2") + "\n";
                statsValues += accuracy.ToString("F2") + "\n";
                statsValues += range.ToString("F2") + "\n";
                if (isHoming) { statsValues += "Homing: " + weapon.homingStrength.ToString("F0") + "\n"; weapon.statLength++; }
                if (isPierce) { statsValues += "Pierce: " + weapon.pierce + "\n"; weapon.statLength++; }
                if (isBounce) { statsValues += "Bounce: " + weapon.bounce + "\n"; weapon.statLength++; }
                if (isBurst) { statsValues += "Burst: " + weapon.burst + "\n"; weapon.statLength++; }
                if (isCharge) { statsValues += "Charge: " + weapon.chargeUp.ToString("F2") + "\n"; weapon.statLength++; }
                if (fireRateBonus) { statsValues += "Cascading: " + weapon.fireRateChange.ToString("F1") + "\n"; weapon.statLength++; }
                if (accuracyBonus) { statsValues += "Narrowing: " + weapon.spreadChange.ToString("F1") + "\n"; weapon.statLength++; }
                if (!anyBonus) { statsValues += "None"; weapon.statLength++; }
                //statsNames = "";
                //if (isHoming) { statsNames += "Homing:\n" + "Strength:\n"; weapon.statLength += 2; }
                //if (isPierce) { statsNames += "Piercing:\n"; weapon.statLength++; }
                //if (isBounce) { statsNames += "Bounces:\n"; weapon.statLength++; }
                //if (isBurst) { statsNames += "Burst:\n" + "Burst delay:\n"; weapon.statLength += 2; }
                //if (isCharge) { statsNames += "Charge up:\n" + "Burst:\n" + "Burst delay:\n"; weapon.statLength += 3; showFireRate = false; }
                //if (anyBonus) { statsNames += "\n"; weapon.statLength++; }

                //statsNames += "Damage:\n"; weapon.statLength++;
                //if (showFireRate) { statsNames += "Fire rate:\n"; weapon.statLength++; }
                ////statsNames += "Bullet size:\n"; weapon.statLength++;
                ////statsNames += "Projectiles:\n"; weapon.statLength++;
                //statsNames += "Spread:\n"; weapon.statLength++;
                //statsNames += "Speed:\n"; weapon.statLength++;
                ////statsNames += "Projectile life:\n"; weapon.statLength++;
                ////statsNames += "Punch:\n"; weapon.statLength++;
                //statsNames += "Rotation speed:\n"; weapon.statLength++;

                //statsValues = "";
                //if (isHoming) statsValues += weapon.homing + "\n" + weapon.homingStrength.ToString("F2") + "\n";
                //if (isPierce) statsValues += weapon.pierce + "\n";
                //if (isBounce) statsValues += weapon.bounce + "\n";
                //if (isBurst) statsValues += weapon.burst + "\n" + weapon.burstDelay.ToString("F3") + "\n";
                //if (isCharge) statsValues += weapon.chargeUp.ToString("F2") + "\n" + weapon.burst + "\n" + weapon.burstDelay.ToString("F3") + "\n";
                //if (anyBonus) statsValues += "\n";
                //if (weapon.amount > 1) statsValues += weapon.damage.ToString("F2") + " x " + weapon.amount + "\n";
                //else statsValues += weapon.damage.ToString("F2") + "\n";
                //if (isCharge) { }
                //else if (fireRateBonus) { statsValues += weapon.fireRate.ToString("F2") + " -> " + (weapon.fireRate + weapon.fireRateChange).ToString("F2") + "\n"; }
                //else statsValues += weapon.fireRate.ToString("F2") + "\n";


                ////statsValues += weapon.bulletSize.ToString("F2") + "\n";
                ////statsValues += weapon.amount + "\n";

                //if (accuracyBonus) { statsValues += weapon.spread.ToString("F2") + " -> " + Mathf.Clamp(weapon.spread + weapon.spreadChange, 0, 100).ToString("F2") + "\n"; }
                //else { statsValues += weapon.spread.ToString("F2") + "\n"; }
                //statsValues += weapon.speed.ToString("F2") + "\n";
                ////statsValues += weapon.longevity.ToString("F2") + "\n";
                ////statsValues += weapon.punch.ToString("F2") + "\n";
                //statsValues += weapon.rotationSpeed.ToString("F2") + "\n";
                break;
            case WeaponType.Wave:
                statsNames = "";
                statsNames += "DPS:\n"; weapon.statLength++;
                statsNames += "Accuracy:\n"; weapon.statLength++;
                statsNames +=   "Range:\n"; weapon.statLength++;
                statsNames += "Modifiers:\n";
                statsValues = "";
                if (isCharge) statsValues += dpsCharge.ToString("F2") + "\n";
                else if (isBurst) statsValues += dpsBurst.ToString("F2") + "\n";
                else statsValues += dps.ToString("F2") + "\n";
                statsValues += accuracy.ToString("F2") + "\n";
                statsValues += range.ToString("F2") + "\n";
                if (isHoming) { statsValues += "Homing: " + weapon.homingStrength.ToString("F0") + "\n"; weapon.statLength++; }
                if (isPierce) { statsValues += "Pierce: " + weapon.pierce + "\n"; weapon.statLength++; }
                if (isBounce) { statsValues += "Bounce: " + weapon.bounce + "\n"; weapon.statLength++; }
                if (isBurst) { statsValues += "Burst: " + weapon.burst + "\n"; weapon.statLength++; }
                if (isCharge) { statsValues += "Charge: " + weapon.chargeUp.ToString("F2") + "\n"; weapon.statLength++; }
                if (fireRateBonus) { statsValues += "Cascading: " + weapon.fireRateChange.ToString("F1") + "\n"; weapon.statLength++; }
                if (accuracyBonus) { statsValues += "Narrowing: " + weapon.spreadChange.ToString("F1") + "\n"; weapon.statLength++; }
                statsValues += "Expanding"; weapon.statLength++;
                //statsNames = "";
                //if (isPierce) { statsNames += "Piercing:\n"; weapon.statLength++; }
                //if (isBurst) { statsNames += "Burst:\n" + "Burst delay:\n"; weapon.statLength += 2; }
                //if (isCharge) { statsNames += "Charge up:\n" + "Burst:\n" + "Burst delay:\n"; weapon.statLength += 3; showFireRate = false; }
                //if (anyBonus) { statsNames += "\n"; weapon.statLength++; }

                //statsNames += "Damage:\n"; weapon.statLength++;
                //if (showFireRate) { statsNames += "Fire rate:\n"; weapon.statLength++; }
                ////statsNames += "Bullet size:\n"; weapon.statLength++;
                ////statsNames += "Projectiles:\n"; weapon.statLength++;
                //statsNames += "Spread:\n"; weapon.statLength++;
                //statsNames += "Speed:\n"; weapon.statLength++;
                ////statsNames += "Projectile life:\n"; weapon.statLength++;
                ////statsNames += "Punch:\n"; weapon.statLength++;
                //statsNames += "Rotation speed:\n"; weapon.statLength++;

                //statsValues = "";
                //if (isPierce) statsValues += weapon.pierce + "\n";
                //if (isBurst) statsValues += weapon.burst + "\n" + weapon.burstDelay.ToString("F3") + "\n";
                //if (isCharge) statsValues += weapon.chargeUp.ToString("F2") + "\n" + weapon.burst + "\n" + weapon.burstDelay.ToString("F3") + "\n";
                //if (anyBonus) statsValues += "\n";
                //if (weapon.amount > 1) statsValues += weapon.damage.ToString("F2") + " x " + weapon.amount + "\n";
                //else statsValues += weapon.damage.ToString("F2") + "\n";
                //if (isCharge) { }
                //else if (fireRateBonus) { statsValues += weapon.fireRate.ToString("F2") + " -> " + (weapon.fireRate + weapon.fireRateChange).ToString("F2") + "\n"; }
                //else statsValues += weapon.fireRate.ToString("F2") + "\n";


                ////statsValues += weapon.bulletSize.ToString("F2") + "\n";
                ////statsValues += weapon.amount + "\n";

                //if (accuracyBonus) { statsValues += weapon.spread.ToString("F2") + " -> " + Mathf.Clamp(weapon.spread + weapon.spreadChange, 0, 100).ToString("F2") + "\n"; }
                //else { statsValues += weapon.spread.ToString("F2") + "\n"; }
                //statsValues += weapon.speed.ToString("F2") + "\n";
                ////statsValues += weapon.longevity.ToString("F2") + "\n";
                ////statsValues += weapon.punch.ToString("F2") + "\n";
                //statsValues += weapon.rotationSpeed.ToString("F2") + "\n";
                break;
            case WeaponType.Rocket:
                statsNames = "";
                statsNames += "DPS:\n"; weapon.statLength++;
                statsNames += "Accuracy:\n"; weapon.statLength++;
                statsNames += "Range:\n"; weapon.statLength++;
                statsNames += "Modifiers:\n";
                statsValues = "";
                if (isCharge) statsValues += dpsCharge.ToString("F2") + "\n";
                else if (isBurst) statsValues += dpsBurst.ToString("F2") + "\n";
                else statsValues += dps.ToString("F2") + "\n";
                statsValues += accuracy.ToString("F2") + "\n";
                statsValues += range.ToString("F2") + "\n";
                if (isHoming) { statsValues += "Homing: " + weapon.homingStrength.ToString("F0") + "\n"; weapon.statLength++; }
                if (isPierce) { statsValues += "Pierce: " + weapon.pierce + "\n"; weapon.statLength++; }
                if (isBounce) { statsValues += "Bounce: " + weapon.bounce + "\n"; weapon.statLength++; }
                if (isBurst) { statsValues += "Burst: " + weapon.burst + "\n"; weapon.statLength++; }
                if (isCharge) { statsValues += "Charge: " + weapon.chargeUp.ToString("F2") + "\n"; weapon.statLength++; }
                if (fireRateBonus) { statsValues += "Cascading: " + weapon.fireRateChange.ToString("F1") + "\n"; weapon.statLength++; }
                if (accuracyBonus) { statsValues += "Narrowing: " + weapon.spreadChange.ToString("F1") + "\n"; weapon.statLength++; }
                if (!anyBonus) { statsValues += "None"; weapon.statLength++; }
                //statsNames = "";
                //if (isHoming) { statsNames += "Homing:\n" + "Strength:\n"; weapon.statLength += 2; }
                //if (isPierce) { statsNames += "Piercing:\n"; weapon.statLength++; }
                //if (isBurst) { statsNames += "Burst:\n" + "Burst delay:\n"; weapon.statLength += 2; }
                //if (isCharge) { statsNames += "Charge up:\n" + "Burst:\n" + "Burst delay:\n"; weapon.statLength += 3; showFireRate = false; }
                //if (anyBonus) { statsNames += "\n"; weapon.statLength++; }

                //statsNames += "Damage:\n"; weapon.statLength++;
                //statsNames += "Splash damage:\n"; weapon.statLength++;
                //statsNames += "Splash range:\n"; weapon.statLength++;
                //if (showFireRate) { statsNames += "Fire rate:\n"; weapon.statLength++; }
                ////statsNames += "Bullet size:\n"; weapon.statLength++;
                ////statsNames += "Projectiles:\n"; weapon.statLength++;
                //statsNames += "Spread:\n"; weapon.statLength++;
                //statsNames += "Speed:\n"; weapon.statLength++;
                ////statsNames += "Projectile life:\n"; weapon.statLength++;
                ////statsNames += "Punch:\n"; weapon.statLength++;
                //statsNames += "Rotation speed:\n"; weapon.statLength++;

                //statsValues = "";
                //if (isHoming) statsValues += weapon.homing + "\n" + weapon.homingStrength.ToString("F2") + "\n";
                //if (isPierce) statsValues += weapon.pierce + "\n";
                //if (isBurst) statsValues += weapon.burst + "\n" + weapon.burstDelay.ToString("F3") + "\n";
                //if (isCharge) statsValues += weapon.chargeUp.ToString("F2") + "\n" + weapon.burst + "\n" + weapon.burstDelay.ToString("F3") + "\n";
                //if (anyBonus) statsValues += "\n";
                //if (weapon.amount > 1) statsValues += weapon.damage.ToString("F2") + " x " + weapon.amount + "\n";
                //else statsValues += weapon.damage.ToString("F2") + "\n";
                //statsValues += weapon.splashDamage.ToString("F1") + "\n";
                //statsValues += weapon.splashRange.ToString("F2") + "\n";
                //if (isCharge) { }
                //else if (fireRateBonus) { statsValues += weapon.fireRate.ToString("F2") + " -> " + (weapon.fireRate + weapon.fireRateChange).ToString("F2") + "\n"; }
                //else statsValues += weapon.fireRate.ToString("F2") + "\n";


                ////statsValues += weapon.bulletSize.ToString("F2") + "\n";
                ////statsValues += weapon.amount + "\n";

                //if (accuracyBonus) { statsValues += weapon.spread.ToString("F2") + " -> " + Mathf.Clamp(weapon.spread + weapon.spreadChange, 0, 100).ToString("F2") + "\n"; }
                //else { statsValues += weapon.spread.ToString("F2") + "\n"; }
                //statsValues += weapon.speed.ToString("F2") + "\n";
                ////statsValues += weapon.longevity.ToString("F2") + "\n";
                ////statsValues += weapon.punch.ToString("F2") + "\n";
                //statsValues += weapon.rotationSpeed.ToString("F2") + "\n";
                break;
            case WeaponType.Needle:
                statsNames = "";
                statsNames += "DPS:\n"; weapon.statLength++;
                statsNames += "Accuracy:\n"; weapon.statLength++;
                statsNames += "Range:\n"; weapon.statLength++;
                statsNames += "Modifiers:\n";
                statsValues = "";
                if (isCharge) statsValues += dpsCharge.ToString("F2") + "\n";
                else if (isBurst) statsValues += dpsBurst.ToString("F2") + "\n";
                else statsValues += dps.ToString("F2") + "\n";
                statsValues += accuracy.ToString("F2") + "\n";
                statsValues += range.ToString("F2") + "\n";
                if (isHoming) { statsValues += "Homing: " + weapon.homingStrength.ToString("F0") + "\n"; weapon.statLength++; }
                if (isPierce) { statsValues += "Pierce: " + weapon.pierce + "\n"; weapon.statLength++; }
                if (isBounce) { statsValues += "Bounce: " + weapon.bounce + "\n"; weapon.statLength++; }
                if (isBurst) { statsValues += "Burst: " + weapon.burst + "\n"; weapon.statLength++; }
                if (isCharge) { statsValues += "Charge: " + weapon.chargeUp.ToString("F2") + "\n"; weapon.statLength++; }
                if (fireRateBonus) { statsValues += "Cascading: " + weapon.fireRateChange.ToString("F1") + "\n"; weapon.statLength++; }
                if (accuracyBonus) { statsValues += "Narrowing: " + weapon.spreadChange.ToString("F1") + "\n"; weapon.statLength++; }
                if (!anyBonus) { statsValues += "None"; weapon.statLength++; }
                //statsNames = "";
                //if (isHoming) { statsNames += "Homing:\n" + "Strength:\n"; weapon.statLength += 2; }
                //if (isPierce) { statsNames += "Piercing:\n"; weapon.statLength++; }
                //if (isBounce) { statsNames += "Bounces:\n"; weapon.statLength++; }
                //if (isBurst) { statsNames += "Burst:\n" + "Burst delay:\n"; weapon.statLength += 2; }
                //if (isCharge) { statsNames += "Charge up:\n" + "Burst:\n" + "Burst delay:\n"; weapon.statLength += 3; showFireRate = false; }
                //if (anyBonus) { statsNames += "\n"; weapon.statLength++; }

                //statsNames += "Damage:\n"; weapon.statLength++;
                //if (showFireRate) { statsNames += "Fire rate:\n"; weapon.statLength++; }
                ////statsNames += "Bullet size:\n"; weapon.statLength++;
                ////statsNames += "Projectiles:\n"; weapon.statLength++;
                //statsNames += "Spread:\n"; weapon.statLength++;
                //statsNames += "Speed:\n"; weapon.statLength++;
                ////statsNames += "Projectile life:\n"; weapon.statLength++;
                ////statsNames += "Punch:\n"; weapon.statLength++;
                //statsNames += "Rotation speed:\n"; weapon.statLength++;

                //statsValues = "";
                //if (isHoming) statsValues += weapon.homing + "\n" + weapon.homingStrength.ToString("F2") + "\n";
                //if (isPierce) statsValues += weapon.pierce + "\n";
                //if (isBounce) statsValues += weapon.bounce + "\n";
                //if (isBurst) statsValues += weapon.burst + "\n" + weapon.burstDelay.ToString("F3") + "\n";
                //if (isCharge) statsValues += weapon.chargeUp.ToString("F2") + "\n" + weapon.burst + "\n" + weapon.burstDelay.ToString("F3") + "\n";
                //if (anyBonus) statsValues += "\n";
                //if (weapon.amount > 1) statsValues += weapon.damage.ToString("F2") + " x " + weapon.amount + "\n";
                //else statsValues += weapon.damage.ToString("F2") + "\n";
                //if (isCharge) { }
                //else if (fireRateBonus) { statsValues += weapon.fireRate.ToString("F2") + " -> " + (weapon.fireRate + weapon.fireRateChange).ToString("F2") + "\n"; }
                //else statsValues += weapon.fireRate.ToString("F2") + "\n";


                ////statsValues += weapon.bulletSize.ToString("F2") + "\n";
                ////statsValues += weapon.amount + "\n";

                //if (accuracyBonus) { statsValues += weapon.spread.ToString("F2") + " -> " + Mathf.Clamp(weapon.spread + weapon.spreadChange, 0, 100).ToString("F2") + "\n"; }
                //else { statsValues += weapon.spread.ToString("F2") + "\n"; }
                //statsValues += weapon.speed.ToString("F2") + "\n";
                ////statsValues += weapon.longevity.ToString("F2") + "\n";
                ////statsValues += weapon.punch.ToString("F2") + "\n";
                //statsValues += weapon.rotationSpeed.ToString("F2") + "\n";
                break;
            case WeaponType.Mine:
                statsNames = "";
                statsNames += "DPS:\n"; weapon.statLength++;
                statsNames += "Accuracy:\n"; weapon.statLength++;
                statsNames += "Range:\n"; weapon.statLength++;
                statsNames += "Modifiers:\n";
                statsValues = "";
                if (isCharge) statsValues += dpsCharge.ToString("F2") + "\n";
                else if (isBurst) statsValues += dpsBurst.ToString("F2") + "\n";
                else statsValues += dps.ToString("F2") + "\n";
                statsValues += accuracy.ToString("F2") + "\n";
                statsValues += range.ToString("F2") + "\n";
                if (isHoming) { statsValues += "Homing: " + weapon.homingStrength.ToString("F0") + "\n"; weapon.statLength++; }
                if (isPierce) { statsValues += "Pierce: " + weapon.pierce + "\n"; weapon.statLength++; }
                if (isBounce) { statsValues += "Bounce: " + weapon.bounce + "\n"; weapon.statLength++; }
                if (isBurst) { statsValues += "Burst: " + weapon.burst + "\n"; weapon.statLength++; }
                if (isCharge) { statsValues += "Charge: " + weapon.chargeUp.ToString("F2") + "\n"; weapon.statLength++; }
                if (fireRateBonus) { statsValues += "Cascading: " + weapon.fireRateChange.ToString("F1") + "\n"; weapon.statLength++; }
                if (accuracyBonus) { statsValues += "Narrowing: " + weapon.spreadChange.ToString("F1") + "\n"; weapon.statLength++; }
                if (!anyBonus) { statsValues += "None"; weapon.statLength++; }
                //statsNames = "";
                //if (isHoming) { statsNames += "Homing:\n" + "Strength:\n"; weapon.statLength += 2; }
                //if (anyBonus) { statsNames += "\n"; weapon.statLength++; }

                //statsNames += "Damage:\n"; weapon.statLength++;
                //statsNames += "Splash damage:\n"; weapon.statLength++;
                //statsNames += "Splash range:\n"; weapon.statLength++;
                //if (showFireRate) { statsNames += "Fire rate:\n"; weapon.statLength++; }
                ////statsNames += "Bullet size:\n"; weapon.statLength++;
                ////statsNames += "Projectiles:\n"; weapon.statLength++;
                //statsNames += "Spread:\n"; weapon.statLength++;
                //statsNames += "Speed:\n"; weapon.statLength++;
                ////statsNames += "Projectile life:\n"; weapon.statLength++;
                ////statsNames += "Punch:\n"; weapon.statLength++;
                //statsNames += "Rotation speed:\n"; weapon.statLength++;

                //statsValues = "";
                //if (isHoming) statsValues += weapon.homing + "\n" + weapon.homingStrength.ToString("F2") + "\n";
                //if (anyBonus) statsValues += "\n";
                //if (weapon.amount > 1) statsValues += weapon.damage.ToString("F2") + " x " + weapon.amount + "\n";
                //else statsValues += weapon.damage.ToString("F2") + "\n";
                //statsValues += weapon.splashDamage.ToString("F1") + "\n";
                //statsValues += weapon.splashRange.ToString("F2") + "\n";
                //if (isCharge) { }
                //else if (fireRateBonus) { statsValues += weapon.fireRate.ToString("F2") + " -> " + (weapon.fireRate + weapon.fireRateChange).ToString("F2") + "\n"; }
                //else statsValues += weapon.fireRate.ToString("F2") + "\n";


                ////statsValues += weapon.bulletSize.ToString("F2") + "\n";
                ////statsValues += weapon.amount + "\n";

                //if (accuracyBonus) { statsValues += weapon.spread.ToString("F2") + " -> " + Mathf.Clamp(weapon.spread + weapon.spreadChange, 0, 100).ToString("F2") + "\n"; }
                //else { statsValues += weapon.spread.ToString("F2") + "\n"; }
                //statsValues += weapon.speed.ToString("F2") + "\n";
                ////statsValues += weapon.longevity.ToString("F2") + "\n";
                ////statsValues += weapon.punch.ToString("F2") + "\n";
                //statsValues += weapon.rotationSpeed.ToString("F2") + "\n";
                break;
            case WeaponType.Hammer:
                statsNames = "";
                statsNames += "DPS:\n"; weapon.statLength++;
                statsNames += "Accuracy:\n"; weapon.statLength++;
                statsNames += "Range:\n"; weapon.statLength++;
                statsNames += "Modifiers:\n";
                statsValues = "";
                if (isCharge) statsValues += dpsCharge.ToString("F2") + "\n";
                else if (isBurst) statsValues += dpsBurst.ToString("F2") + "\n";
                else statsValues += dps.ToString("F2") + "\n";
                statsValues += accuracy.ToString("F2") + "\n";
                statsValues += range.ToString("F2") + "\n";
                if (isHoming) { statsValues += "Homing: " + weapon.homingStrength.ToString("F0") + "\n"; weapon.statLength++; }
                if (isPierce) { statsValues += "Pierce: " + weapon.pierce + "\n"; weapon.statLength++; }
                if (isBounce) { statsValues += "Bounce: " + weapon.bounce + "\n"; weapon.statLength++; }
                if (isBurst) { statsValues += "Burst: " + weapon.burst + "\n"; weapon.statLength++; }
                if (isCharge) { statsValues += "Charge: " + weapon.chargeUp.ToString("F2") + "\n"; weapon.statLength++; }
                if (fireRateBonus) { statsValues += "Cascading: " + weapon.fireRateChange.ToString("F1") + "\n"; weapon.statLength++; }
                if (accuracyBonus) { statsValues += "Narrowing: " + weapon.spreadChange.ToString("F1") + "\n"; weapon.statLength++; }
                if (!anyBonus) { statsValues += "None"; weapon.statLength++; }
                //statsNames = "";
                //if (isPierce) { statsNames += "Piercing:\n"; weapon.statLength++; }
                //if (isBounce) { statsNames += "Bounces:\n"; weapon.statLength++; }
                //if (isBurst) { statsNames += "Burst:\n" + "Burst delay:\n"; weapon.statLength += 2; }
                //if (isCharge) { statsNames += "Charge up:\n" + "Burst:\n" + "Burst delay:\n"; weapon.statLength += 3; showFireRate = false; }
                //if (anyBonus) { statsNames += "\n"; weapon.statLength++; }

                //statsNames += "Damage:\n"; weapon.statLength++;
                //if (showFireRate) { statsNames += "Fire rate:\n"; weapon.statLength++; }
                ////statsNames += "Bullet size:\n"; weapon.statLength++;
                ////statsNames += "Projectiles:\n"; weapon.statLength++;
                //statsNames += "Spread:\n"; weapon.statLength++;
                //statsNames += "Speed:\n"; weapon.statLength++;
                ////statsNames += "Projectile life:\n"; weapon.statLength++;
                ////statsNames += "Punch:\n"; weapon.statLength++;
                //statsNames += "Rotation speed:\n"; weapon.statLength++;

                //statsValues = "";
                //if (isPierce) statsValues += weapon.pierce + "\n";
                //if (isBounce) statsValues += weapon.bounce + "\n";
                //if (isBurst) statsValues += weapon.burst + "\n" + weapon.burstDelay.ToString("F3") + "\n";
                //if (isCharge) statsValues += weapon.chargeUp.ToString("F2") + "\n" + weapon.burst + "\n" + weapon.burstDelay.ToString("F3") + "\n";
                //if (anyBonus) statsValues += "\n";
                //if (weapon.amount > 1) statsValues += weapon.damage.ToString("F2") + " x " + weapon.amount + "\n";
                //else statsValues += weapon.damage.ToString("F2") + "\n";
                //if (isCharge) { }
                //else if (fireRateBonus) { statsValues += weapon.fireRate.ToString("F2") + " -> " + (weapon.fireRate + weapon.fireRateChange).ToString("F2") + "\n"; }
                //else statsValues += weapon.fireRate.ToString("F2") + "\n";


                ////statsValues += weapon.bulletSize.ToString("F2") + "\n";
                ////statsValues += weapon.amount + "\n";

                //if (accuracyBonus) { statsValues += weapon.spread.ToString("F2") + " -> " + Mathf.Clamp(weapon.spread + weapon.spreadChange, 0, 100).ToString("F2") + "\n"; }
                //else { statsValues += weapon.spread.ToString("F2") + "\n"; }
                //statsValues += weapon.speed.ToString("F2") + "\n";
                ////statsValues += weapon.longevity.ToString("F2") + "\n";
                ////statsValues += weapon.punch.ToString("F2") + "\n";
                //statsValues += weapon.rotationSpeed.ToString("F2") + "\n";
                break;
            case WeaponType.Cluster:
                statsNames = "";
                statsNames += "DPS:\n"; weapon.statLength++;
                statsNames += "Accuracy:\n"; weapon.statLength++;
                statsNames += "Range:\n"; weapon.statLength++;
                statsNames += "Modifiers:\n";
                statsValues = "";
                if (isCharge) statsValues += dpsCharge.ToString("F2") + "\n";
                else if (isBurst) statsValues += dpsBurst.ToString("F2") + "\n";
                else statsValues += dps.ToString("F2") + "\n";
                statsValues += accuracy.ToString("F2") + "\n";
                statsValues += range.ToString("F2") + "\n";
                if (isHoming) { statsValues += "Homing: " + weapon.homingStrength.ToString("F0") + "\n"; weapon.statLength++; }
                if (isPierce) { statsValues += "Pierce: " + weapon.pierce + "\n"; weapon.statLength++; }
                if (isBounce) { statsValues += "Bounce: " + weapon.bounce + "\n"; weapon.statLength++; }
                if (isBurst) { statsValues += "Burst: " + weapon.burst + "\n"; weapon.statLength++; }
                if (isCharge) { statsValues += "Charge: " + weapon.chargeUp.ToString("F2") + "\n"; weapon.statLength++; }
                if (fireRateBonus) { statsValues += "Cascading: " + weapon.fireRateChange.ToString("F1") + "\n"; weapon.statLength++; }
                if (accuracyBonus) { statsValues += "Narrowing: " + weapon.spreadChange.ToString("F1") + "\n"; weapon.statLength++; }
                statsValues += "Splitting"; weapon.statLength++;
                //statsNames = "";
                //if (isHoming) { statsNames += "Homing:\n" + "Strength:\n"; weapon.statLength += 2; }
                //if (isPierce) { statsNames += "Piercing:\n"; weapon.statLength++; }
                //if (isBounce) { statsNames += "Bounces:\n"; weapon.statLength++; }
                //if (isBurst) { statsNames += "Burst:\n" + "Burst delay:\n"; weapon.statLength += 2; }
                //if (isCharge) { statsNames += "Charge up:\n" + "Burst:\n" + "Burst delay:\n"; weapon.statLength += 3; showFireRate = false; }
                //if (anyBonus) { statsNames += "\n"; weapon.statLength++; }

                //statsNames += "Damage:\n"; weapon.statLength++;
                //if (showFireRate) { statsNames += "Fire rate:\n"; weapon.statLength++; }
                ////statsNames += "Bullet size:\n"; weapon.statLength++;
                ////statsNames += "Projectiles:\n"; weapon.statLength++;
                //statsNames += "Spread:\n"; weapon.statLength++;
                //statsNames += "Speed:\n"; weapon.statLength++;
                ////statsNames += "Projectile life:\n"; weapon.statLength++;
                ////statsNames += "Punch:\n"; weapon.statLength++;
                //statsNames += "Rotation speed:\n"; weapon.statLength++;

                //statsValues = "";
                //if (isHoming) statsValues += weapon.homing + "\n" + weapon.homingStrength.ToString("F2") + "\n";
                //if (isPierce) statsValues += weapon.pierce + "\n";
                //if (isBounce) statsValues += weapon.bounce + "\n";
                //if (isBurst) statsValues += weapon.burst + "\n" + weapon.burstDelay.ToString("F3") + "\n";
                //if (isCharge) statsValues += weapon.chargeUp.ToString("F2") + "\n" + weapon.burst + "\n" + weapon.burstDelay.ToString("F3") + "\n";
                //if (anyBonus) statsValues += "\n";
                //if (weapon.amount > 1) statsValues += weapon.damage.ToString("F2") + " x " + weapon.amount + "\n";
                //else statsValues += weapon.damage.ToString("F2") + "\n";
                //if (isCharge) { }
                //else if (fireRateBonus) { statsValues += weapon.fireRate.ToString("F2") + " -> " + (weapon.fireRate + weapon.fireRateChange).ToString("F2") + "\n"; }
                //else statsValues += weapon.fireRate.ToString("F2") + "\n";


                ////statsValues += weapon.bulletSize.ToString("F2") + "\n";
                ////statsValues += weapon.amount + "\n";

                //if (accuracyBonus) { statsValues += weapon.spread.ToString("F2") + " -> " + Mathf.Clamp(weapon.spread + weapon.spreadChange, 0, 100).ToString("F2") + "\n"; }
                //else { statsValues += weapon.spread.ToString("F2") + "\n"; }
                //statsValues += weapon.speed.ToString("F2") + "\n";
                ////statsValues += weapon.longevity.ToString("F2") + "\n";
                ////statsValues += weapon.punch.ToString("F2") + "\n";
                //statsValues += weapon.rotationSpeed.ToString("F2") + "\n";
                break;
            //case WeaponType.Arrow:
            //    statsNames = "";
            //    if (isHoming) { statsNames += "Homing:\n" + "Strength:\n"; weapon.statLength += 2; }
            //    if (isPierce) { statsNames += "Piercing:\n"; weapon.statLength++; }
            //    if (isBounce) { statsNames += "Bounces:\n"; weapon.statLength++; }
            //    if (isBurst) { statsNames += "Burst:\n" + "Burst delay:\n"; weapon.statLength += 2; }
            //    if (isCharge) { statsNames += "Charge up:\n" + "Burst:\n" + "Burst delay:\n"; weapon.statLength += 3; showFireRate = false; }
            //    if (anyBonus) { statsNames += "\n"; weapon.statLength++; }

            //    statsNames += "Damage:\n"; weapon.statLength++;
            //    if (showFireRate) { statsNames += "Fire rate:\n"; weapon.statLength++; }
            //    //statsNames += "Bullet size:\n"; weapon.statLength++;
            //    //statsNames += "Projectiles:\n"; weapon.statLength++;
            //    statsNames += "Spread:\n"; weapon.statLength++;
            //    statsNames += "Speed:\n"; weapon.statLength++;
            //    //statsNames += "Projectile life:\n"; weapon.statLength++;
            //    //statsNames += "Punch:\n"; weapon.statLength++;
            //    statsNames += "Rotation speed:\n"; weapon.statLength++;

            //    statsValues = "";
            //    if (isHoming) statsValues += weapon.homing + "\n" + weapon.homingStrength.ToString("F2") + "\n";
            //    if (isPierce) statsValues += weapon.pierce + "\n";
            //    if (isBounce) statsValues += weapon.bounce + "\n";
            //    if (isBurst) statsValues += weapon.burst + "\n" + weapon.burstDelay.ToString("F3") + "\n";
            //    if (isCharge) statsValues += weapon.chargeUp.ToString("F2") + "\n" + weapon.burst + "\n" + weapon.burstDelay.ToString("F3") + "\n";
            //    if (anyBonus) statsValues += "\n";
            //    if (weapon.amount > 1) statsValues += weapon.damage.ToString("F2") + " x " + weapon.amount + "\n";
            //    else statsValues += weapon.damage.ToString("F2") + "\n";
            //    if (isCharge) { }
            //    else if (fireRateBonus) { statsValues += weapon.fireRate.ToString("F2") + " -> " + (weapon.fireRate + weapon.fireRateChange).ToString("F2") + "\n"; }
            //    else statsValues += weapon.fireRate.ToString("F2") + "\n";


            //    //statsValues += weapon.bulletSize.ToString("F2") + "\n";
            //    //statsValues += weapon.amount + "\n";

            //    if (accuracyBonus) { statsValues += weapon.spread.ToString("F2") + " -> " + Mathf.Clamp(weapon.spread + weapon.spreadChange, 0, 100).ToString("F2") + "\n"; }
            //    else { statsValues += weapon.spread.ToString("F2") + "\n"; }
            //    statsValues += weapon.speed.ToString("F2") + "\n";
            //    //statsValues += weapon.longevity.ToString("F2") + "\n";
            //    //statsValues += weapon.punch.ToString("F2") + "\n";
            //    statsValues += weapon.rotationSpeed.ToString("F2") + "\n";
            //    break;
            case WeaponType.Mirage:
                break;
            case WeaponType.Grand:
                statsNames = "";
                statsNames += "DPS:\n"; weapon.statLength++;
                statsNames += "Accuracy:\n"; weapon.statLength++;
                statsNames += "Range:\n"; weapon.statLength++;
                statsNames += "Modifiers:\n";
                statsValues = "";
                if (isCharge) statsValues += dpsCharge.ToString("F2") + "\n";
                else if (isBurst) statsValues += dpsBurst.ToString("F2") + "\n";
                else statsValues += dps.ToString("F2") + "\n";
                statsValues += accuracy.ToString("F2") + "\n";
                statsValues += range.ToString("F2") + "\n";
                if (isHoming) { statsValues += "Homing: " + weapon.homingStrength.ToString("F0") + "\n"; weapon.statLength++; }
                if (isPierce) { statsValues += "Pierce: " + weapon.pierce + "\n"; weapon.statLength++; }
                if (isBounce) { statsValues += "Bounce: " + weapon.bounce + "\n"; weapon.statLength++; }
                if (isBurst) { statsValues += "Burst: " + weapon.burst + "\n"; weapon.statLength++; }
                if (isCharge) { statsValues += "Charge: " + weapon.chargeUp.ToString("F2") + "\n"; weapon.statLength++; }
                if (fireRateBonus) { statsValues += "Cascading: " + weapon.fireRateChange.ToString("F1") + "\n"; weapon.statLength++; }
                if (accuracyBonus) { statsValues += "Narrowing: " + weapon.spreadChange.ToString("F1") + "\n"; weapon.statLength++; }
                if (!anyBonus) { statsValues += "None"; weapon.statLength++; }
                //statsNames = "";
                //if (isHoming) { statsNames += "Homing:\n" + "Strength:\n"; weapon.statLength += 2; }
                //if (isPierce) { statsNames += "Piercing:\n"; weapon.statLength++; }
                //if (isBounce) { statsNames += "Bounces:\n"; weapon.statLength++; }
                //if (isBurst) { statsNames += "Burst:\n" + "Burst delay:\n"; weapon.statLength += 2; }
                //if (isCharge) { statsNames += "Charge up:\n" + "Burst:\n" + "Burst delay:\n"; weapon.statLength += 3; showFireRate = false; }
                //if (anyBonus) { statsNames += "\n"; weapon.statLength++; }

                //statsNames += "Damage:\n"; weapon.statLength++;
                //if (showFireRate) { statsNames += "Fire rate:\n"; weapon.statLength++; }
                ////statsNames += "Bullet size:\n"; weapon.statLength++;
                ////statsNames += "Projectiles:\n"; weapon.statLength++;
                //statsNames += "Spread:\n"; weapon.statLength++;
                //statsNames += "Speed:\n"; weapon.statLength++;
                ////statsNames += "Projectile life:\n"; weapon.statLength++;
                ////statsNames += "Punch:\n"; weapon.statLength++;
                //statsNames += "Rotation speed:\n"; weapon.statLength++;

                //statsValues = "";
                //if (isHoming) statsValues += weapon.homing + "\n" + weapon.homingStrength.ToString("F2") + "\n";
                //if (isPierce) statsValues += weapon.pierce + "\n";
                //if (isBounce) statsValues += weapon.bounce + "\n";
                //if (isBurst) statsValues += weapon.burst + "\n" + weapon.burstDelay.ToString("F3") + "\n";
                //if (isCharge) statsValues += weapon.chargeUp.ToString("F2") + "\n" + weapon.burst + "\n" + weapon.burstDelay.ToString("F3") + "\n";
                //if (anyBonus) statsValues += "\n";
                //if (weapon.amount > 1) statsValues += weapon.damage.ToString("F2") + " x " + weapon.amount + "\n";
                //else statsValues += weapon.damage.ToString("F2") + "\n";
                //if (isCharge) { }
                //else if (fireRateBonus) { statsValues += weapon.fireRate.ToString("F2") + " -> " + (weapon.fireRate + weapon.fireRateChange).ToString("F2") + "\n"; }
                //else statsValues += weapon.fireRate.ToString("F2") + "\n";


                ////statsValues += weapon.bulletSize.ToString("F2") + "\n";
                ////statsValues += weapon.amount + "\n";

                //if (accuracyBonus) { statsValues += weapon.spread.ToString("F2") + " -> " + Mathf.Clamp(weapon.spread + weapon.spreadChange, 0, 100).ToString("F2") + "\n"; }
                //else { statsValues += weapon.spread.ToString("F2") + "\n"; }
                //statsValues += weapon.speed.ToString("F2") + "\n";
                ////statsValues += weapon.longevity.ToString("F2") + "\n";
                ////statsValues += weapon.punch.ToString("F2") + "\n";
                //statsValues += weapon.rotationSpeed.ToString("F2") + "\n";
                break;
            case WeaponType.Void:
                break;
            case WeaponType.Blade:
                break;
            default:
                break;
        }
        #endregion

        weapon.statsText = statsNames;
        Debug.Log(weapon.statsText);
        weapon.statsValues = statsValues;
        weapon.savedHoming = weapon.homing;
        weapon.savedIsExplosive = weapon.isExplosive;
        return weapon;
    }

    int AddChargeOrBurst(Weapon weapon, int style)
    {
        int chargeOrBurst = Random.Range(0, 6);
        switch (chargeOrBurst)
        {
            case 0:
                weapon.chargeUp = fR(chargeUp);
                weapon.burst = iR(burst) * 2;
                weapon.burstDelay = fR(burstDelay);
                //weapon.amount *= 2;
                weapon.fireRate = 60;
                if (style == 0) // is low damage and high attack
                {
                    weapon.burst *= 2;
                }
                return 1;
            case 1:
                weapon.burst = iR(burst);
                weapon.burstDelay = fR(burstDelay);
                weapon.fireRate *= 0.6f;
                return 2;
            default:
                return 0;
        }
    }
    int AddBounceOrPierce(Weapon weapon)
    {
        int bounceOrPierce = Random.Range(0, 4);
        switch (bounceOrPierce)
        {
            case 0:
                weapon.bounce = iR(bounce);
                return 1;
            case 1:
                weapon.pierce = iR(pierce);
                return 2;
            default:
                return 0;
        }
    }

    bool AddHoming(Weapon weapon)
    {
        if (Random.value <= 0.2f)
        {
            weapon.homing = true;
            weapon.homingStrength = fR(homingStrength) * 10f;
            return true;
        }
        else
        {
            weapon.homing = false;
            weapon.homingStrength = 0;
            return false;
        }
    }

    int AddBonusFireROrAcc(Weapon weapon)
    {
        int bonus = Random.Range(0, 4);
        switch (bonus)
        {
            case 0: // bonus fire rate
                weapon.fireRateChange = fR(fireRateChange);
                weapon.fireRateChangeTimer = fR(fireRateChangeTimer);
                if (weapon.fireRateChange > 0) { } else { weapon.damage *= 1.25f; };
                return 1;
            case 1: // bonus accuracy
                weapon.spreadChange = fR(spreadChange);
                weapon.spreadChangeTimer = fR(spreadChangeTimer);
                if (weapon.spreadChange < 0f) { weapon.spread *= 0.8f; } else { weapon.spread *= 1.25f; }
                return 2;
            default: // nothing
                return 0;
        }
    }

    void SetExplosiveness(Weapon weapon)
    {
        weapon.isExplosive = true;
        weapon.splashRange = fR(explosiveRange);
        weapon.explosiveMultiplier = fR(explosionDamageMultiplier);
        weapon.splashDamage = weapon.damage;
        weapon.damage /= weapon.explosiveMultiplier;
    }
    int SetStyle(Weapon weapon)
    {
        int styleInt = Random.Range(0, 5);
        //0 = low dmg, high rof 
        //1 = high dmg, low rof 
        //2 = both medium 
        //3 = high speed, slightly lower both
        //4 = low speed, slightly higher both
        switch (styleInt)
        {
            case 0:
                weapon.damage = fR(damage) * 0.5f;
                weapon.fireRate = fR(fireRate) * 2;
                weapon.speed = fR(speed);
                weapon.spread = fR(spread) * 2;
                break;
            case 1:
                weapon.damage = fR(damage) * 2;
                weapon.fireRate = fR(fireRate) * 0.5f;
                weapon.speed = fR(speed);
                weapon.spread = fR(spread) * 0.5f;
                break;
            case 2:
                weapon.damage = fR(damage);
                weapon.fireRate = fR(fireRate);
                weapon.speed = fR(speed);
                weapon.spread = fR(spread);
                break;
            case 3:
                weapon.damage = fR(damage) * 0.75f;
                weapon.fireRate = fR(fireRate) * 0.75f;
                weapon.speed = fR(speed) * 2;
                weapon.spread = fR(spread) * 0.9f;
                break;
            case 4:
                weapon.damage = fR(damage) * 1.5f;
                weapon.fireRate = fR(fireRate) * 1.5f;
                weapon.speed = fR(speed) * 0.5f;
                weapon.spread = fR(spread) * 1.1f;
                break;
            default:
                Debug.Log("Out of bounds");
                break;
        }
        return styleInt;
    }
    #endregion

    #region RandomizeThruster
    public Thrusters RandomizeThruster(int level)
    {
        Thrusters thruster = ScriptableObject.CreateInstance<Thrusters>();
        thruster.itemName = new string
            (thrusterNameFirst[iR(0, thrusterNameFirst.Length - 1)]
            + " "
            + thrusterNameLast[iR(0, thrusterNameLast.Length - 1)]);
        thruster.name = thruster.itemName;
        thruster.id = Inventory.Instance.id;
        thruster.color = typeColor[(int)EquipmentTypes.Thruster];
        thruster.equipType = EquipmentTypes.Thruster;
        thruster.level = level;
        thruster.gradient = gradients[(int)EquipmentTypes.Thruster];

        thruster.stlType = (STLTypes)iR(0, 5);
        thruster.icon = thrusterSprites[(int)thruster.stlType];

        thruster.speed = fR(moveSpeed);
        thruster.maxSpeed = fR(maxSpeed);
        thruster.turnSpeed = fR(turnSpeed);
        thruster.maxTurnSpeed = fR(maxTurnSpeed);
        thruster.turnBrakingSpeed = fR(turnBrakingSpeed);
        thruster.brakingSpeed = fR(brakingSpeed);

        switch (thruster.stlType)
        {
            case STLTypes.Drive: //Generalized
                
                break;
            case STLTypes.Agile: //Slower but better rotation stats
                thruster.speed *= 0.7f;
                thruster.maxSpeed *= 0.7f;
                thruster.turnSpeed *= 3f;
                thruster.maxTurnSpeed *= 2;
                thruster.turnBrakingSpeed *= 2;
                thruster.brakingSpeed *= 2;
                break;
            case STLTypes.Pulse: //Faster but worse rotation stats
                thruster.speed *= 2f;
                thruster.maxSpeed *= 2f;
                thruster.turnSpeed *= 0.5f;
                thruster.maxTurnSpeed *= 0.5f;
                thruster.turnBrakingSpeed *= 1;
                thruster.brakingSpeed *= 2;
                break;
            case STLTypes.Boost: //High speed, and rotation, but little control
                thruster.speed *= 3f;
                thruster.maxSpeed *= 3f;
                thruster.turnSpeed *= 0.8f;
                thruster.maxTurnSpeed *= 0.8f;
                thruster.turnBrakingSpeed *= 0.7f;
                thruster.brakingSpeed *= 0.6f;
                break;
            case STLTypes.Sail: //Very little drag
                thruster.speed *= 1f;
                thruster.maxSpeed *= 10;
                thruster.turnSpeed *= 0.8f;
                thruster.maxTurnSpeed *= 0.8f;
                thruster.turnBrakingSpeed *= 0.1f;
                thruster.brakingSpeed *= 0.1f;
                break;
            case STLTypes.Crawler: //Slow but very controllable rotation
                thruster.speed *= 1f;
                thruster.maxSpeed *= 0.5f;
                thruster.turnSpeed *= 3f;
                thruster.maxTurnSpeed *= 3f;
                thruster.turnBrakingSpeed *= 5f;
                thruster.brakingSpeed *= 3f;
                break;
            case STLTypes.Default:
                break;
            default:
                break;
        }
        thruster.speed *= 1 + (thruster.brakingSpeed / 10);
        thruster.turnSpeed *= 1 + (thruster.turnBrakingSpeed / 10);

        //thruster.ftlType = (FTLTypes)iR(0, 3);
        //thruster.ftlAcc = fR(ftlAcc);
        //thruster.ftlMaxSpeed = fR(ftlMaxSpeed);
        //thruster.ftlRotSpeed = fR(ftlRot);
        //thruster.chargeTime = fR(chargeTime);
        //thruster.fuelMax = fR(fuelMax);
        //thruster.fuelCurrent = thruster.fuelMax - (thruster.fuelMax * (fR(fuelCurrent) / 100));
        //thruster.fuelDrain = fR(fuelDrain);
        //thruster.maxDuration = fR(maxDuration);


        //switch (thruster.ftlType)
        //{
        //    case FTLTypes.Ready: //Short charge time, shorter duration, more fuel drain
        //        thruster.chargeTime *= 0.2f;
        //        thruster.fuelDrain *= 3f;
        //        thruster.maxDuration *= 0.5f;
        //        break;
        //    case FTLTypes.Burst: //High acceleration, higher max speed
        //        thruster.ftlAcc *= 2f;
        //        thruster.ftlMaxSpeed *= 1.5f;
        //        thruster.chargeTime *= 0.8f;
        //        thruster.fuelDrain *= 2f;
        //        thruster.maxDuration *= 0.3f;
        //        break;
        //    case FTLTypes.Flight: //Long charge time, high max speed and acceleration, lower fuel drain
        //        thruster.ftlAcc *= 1.5f;
        //        thruster.ftlMaxSpeed *= 1.5f;
        //        thruster.ftlRotSpeed *= 0;
        //        thruster.chargeTime *= 2f;
        //        thruster.fuelDrain *= 0.5f;
        //        thruster.maxDuration *= 3f;
        //        break;
        //    case FTLTypes.Scout: //Medium all round, but high rot speed and duration
        //        thruster.ftlAcc *= 0.5f;
        //        thruster.ftlMaxSpeed *= 0.8f;
        //        thruster.ftlRotSpeed *= 5f;
        //        thruster.chargeTime *= 0.75f;
        //        thruster.fuelDrain *= 1.5f;
        //        break;
        //    case FTLTypes.Crash:
        //        break;
        //    case FTLTypes.Default:
        //        break;
        //    default:
        //        break;
        //}
        for (int i = 0; i < level; i++)
        {
            thruster.speed *= 1 + (moveSpeed.z);
            thruster.maxSpeed *= 1 + (maxSpeed.z);
            thruster.turnSpeed *= 1 + (turnSpeed.z);
            thruster.maxTurnSpeed *= 1 + (maxTurnSpeed.z);
            thruster.turnBrakingSpeed *= 1 + (turnBrakingSpeed.z);
            thruster.brakingSpeed *= 1 + (brakingSpeed.z);
            //thruster.ftlAcc *= 1 + (ftlAcc.z);
            //thruster.ftlMaxSpeed *= 1 + (ftlMaxSpeed.z);
            //thruster.ftlRotSpeed *= 1 + (ftlRot.z);
            //thruster.chargeTime *= 1 + (chargeTime.z);
            //thruster.fuelMax *= 1 + (fuelMax.z);
            //thruster.fuelDrain *= 1 + (fuelDrain.z);
            //thruster.maxDuration *= 1 + (maxDuration.z);
        }
        //ddd
        string statsNames = "Something is wong";
        string statsValues = "WTF";
        statsNames = "";
        statsNames += "Mobility:\n"; thruster.statLength++;
        //statsNames += "Max speed: \n";
        statsNames += "Agility: \n"; thruster.statLength++;
        //statsNames += "Max turn speed: \n";
        statsNames += "Control: \n"; thruster.statLength++;
        //statsNames += "Angular control: \n"; thruster.statLength++;

        //statsNames += "Acceleration:\n"; thruster.statLength++;
        //statsNames += "Rotation speed: \n"; thruster.statLength++;
        //statsNames += "Charge time: \n";thruster.statLength++;
        //statsNames += "Fuel: \n"; thruster.statLength++;
        ////statsNames += "Max fuel: \n"; thruster.statLength++;
        //statsNames += "Fuel drain: \n"; thruster.statLength++;
        //statsNames += "Duration: \n"; thruster.statLength++;


        thruster.statsText = statsNames;

        statsValues = "";

        statsValues +=
            thruster.speed.ToString("F0")
            //+ " / " + (thruster.maxSpeed).ToString("F0")
            + "\n";
        //+ " FTL: "
        //+ (thruster.ftlAcc).ToString("F0") 
        //+ " / " 
        //+ (thruster.ftlMaxSpeed).ToString("F0")
        //+ "\n";
        //statsValues += thruster.maxSpeed.ToString("F0") + " / " + (thruster.maxSpeed * 10 * 0.5f).ToString("F0") + "\n";
        statsValues +=
            (thruster.turnSpeed * 10).ToString("F0")
            //+ " / "
            //+ (thruster.maxTurnSpeed * 10).ToString("F0")
            + "\n";
        //+ " FTL: "
        //+ (thruster.ftlRotSpeed * 10).ToString("F0")
        //+ " / "
        //+ (thruster.maxTurnSpeed).ToString("F0")
        //+ "\n";
        //statsValues += (thruster.maxTurnSpeed * 10).ToString("F0") + "\n";
        float control = thruster.brakingSpeed * thruster.turnBrakingSpeed;
        statsValues += control.ToString("F") + "\n";
        //statsValues += thruster.turnBrakingSpeed.ToString("F0") + "\n";

        //statsValues += thruster.acceleration.ToString("F0") + "\n";
        //statsValues += thruster.maxSpeed.ToString("F0") + "\n";
        //statsValues += thruster.rotSpeed.ToString("F2") + "\n";
        //statsValues += thruster.chargeTime.ToString("F2") + "\n";
        //statsValues += 
        //    thruster.fuelCurrent.ToString("F0") 
        //    + " / "
        //    + thruster.fuelMax.ToString("F0")
        //    + "\n";
        ////statsValues += thruster.fuelMax.ToString("F0") + "\n";
        //statsValues += thruster.fuelDrain.ToString("F2") + "\n";
        //statsValues += thruster.maxDuration.ToString("F0") + "\n";

        thruster.statsValues = statsValues;

        thruster.statsText = statsNames;

        return thruster;
    }
    #endregion

    //#region RandomizeFTLEngine
    //public FTLEngine RandomizeFTLEngine()
    //{
    //    FTLEngine ftl = ScriptableObject.CreateInstance<FTLEngine>();
    //    ftl.itemName = new string
    //        (ftlNameFirst[iR(0, ftlNameFirst.Length - 1)]
    //        + " "
    //        + ftlNameLast[iR(0, ftlNameLast.Length - 1)]);
    //    ftl.name = ftl.itemName;
    //    ftl.id = Inventory.Instance.id;
    //    ftl.icon = ftlSprites[iR(0, ftlSprites.Length - 1)];
    //    ftl.color = typeColor[(int)EquipmentTypes.Thruster];
    //    ftl.equipType = EquipmentTypes.Thruster;

    //    ftl.ftlType = (FTLTypes)iR(0, 3);

    //    switch (ftl.ftlType)
    //    {
    //        case FTLTypes.Ready: //Short charge time, shorter duration, more fuel drain
    //            ftl.acceleration = fR(acceleration);
    //            ftl.maxSpeed = fR(ftlMaxSpeed);
    //            ftl.rotSpeed = fR(rotSpeed);
    //            ftl.chargeTime = fR(chargeTime) * 0.2f;
    //            ftl.fuelMax = fR(fuelMax);
    //            ftl.fuelCurrent = ftl.fuelMax - (ftl.fuelMax * (fR(fuelCurrent) / 100));
    //            ftl.fuelDrain = fR(fuelDrain) * 3f;
    //            ftl.maxDuration = fR(maxDuration) * 0.5f;
    //            break;
    //        case FTLTypes.Burst: //High acceleration, higher max speed
    //            ftl.acceleration = fR(acceleration) * 3;
    //            ftl.maxSpeed = fR(ftlMaxSpeed) * 3;
    //            ftl.rotSpeed = fR(rotSpeed);
    //            ftl.chargeTime = fR(chargeTime) * 0.8f;
    //            ftl.fuelMax = fR(fuelMax);
    //            ftl.fuelCurrent = ftl.fuelMax - (ftl.fuelMax * (fR(fuelCurrent) / 100));
    //            ftl.fuelDrain = fR(fuelDrain) * 2;
    //            ftl.maxDuration = fR(maxDuration) * 0.3f;
    //            break;
    //        case FTLTypes.Flight: //Long charge time, high max speed and acceleration, lower fuel drain
    //            ftl.acceleration = fR(acceleration);
    //            ftl.maxSpeed = fR(ftlMaxSpeed);
    //            ftl.rotSpeed = fR(rotSpeed) * 0;
    //            ftl.chargeTime = fR(chargeTime) * 2;
    //            ftl.fuelMax = fR(fuelMax) * 2;
    //            ftl.fuelCurrent = ftl.fuelMax - (ftl.fuelMax * (fR(fuelCurrent) / 100));
    //            ftl.fuelDrain = fR(fuelDrain) * 0.5f;
    //            ftl.maxDuration = fR(maxDuration) * 3;
    //            break;
    //        case FTLTypes.Scout: //Medium all round, but high rot speed and duration
    //            ftl.acceleration = fR(acceleration);
    //            ftl.maxSpeed = fR(ftlMaxSpeed);
    //            ftl.rotSpeed = fR(rotSpeed) * 10;
    //            ftl.chargeTime = fR(chargeTime) * 0.75f;
    //            ftl.fuelMax = fR(fuelMax);
    //            ftl.fuelCurrent = ftl.fuelMax - (ftl.fuelMax * (fR(fuelCurrent) / 100));
    //            ftl.fuelDrain = fR(fuelDrain) * 1.5f;
    //            ftl.maxDuration = fR(maxDuration);
    //            break;
    //        case FTLTypes.Crash:
    //            break;
    //        case FTLTypes.Default:
    //            break;
    //        default:
    //            break;
    //    }

    //    string statsNames = "Something is wong";
    //    string statsValues = "WTF";
    //    statsNames = "";
    //    statsNames += "Acceleration:\n"; ftl.statLength++;
    //    statsNames += "Max speed: \n"; ftl.statLength++;
    //    statsNames += "Rotation speed: \n"; ftl.statLength++;
    //    statsNames += "Charge time: \n"; ftl.statLength++;
    //    statsNames += "Current fuel: \n"; ftl.statLength++;
    //    statsNames += "Max fuel: \n"; ftl.statLength++;
    //    statsNames += "Fuel drain: \n"; ftl.statLength++;
    //    statsNames += "Max duration: \n"; ftl.statLength++;

    //    ftl.statsText = statsNames;

    //    statsValues = "";
    //    statsValues += ftl.acceleration.ToString("F0") + "\n";
    //    statsValues += ftl.maxSpeed.ToString("F0") + "\n";
    //    statsValues += ftl.rotSpeed.ToString("F2") + "\n";
    //    statsValues += ftl.chargeTime.ToString("F2") + "\n";
    //    statsValues += ftl.fuelCurrent.ToString("F0") + "\n";
    //    statsValues += ftl.fuelMax.ToString("F0") + "\n";
    //    statsValues += ftl.fuelDrain.ToString("F2") + "\n";
    //    statsValues += ftl.maxDuration.ToString("F0") + "\n";

    //    ftl.statsValues = statsValues;

    //    return ftl;
    //}
    //#endregion //Not used

    #region RandomizeCollector
    public Collector RandomizeCollector(int level)
    {
        Collector col = ScriptableObject.CreateInstance<Collector>();
        col.itemName = new string(
             colNameFirst[iR(0, colNameFirst.Length - 1)]
            + " "
            + colNameLast[iR(0, colNameLast.Length - 1)]);
        col.name = col.itemName;
        col.id = Inventory.Instance.id;
        col.color = typeColor[(int)EquipmentTypes.Collector];
        col.equipType = EquipmentTypes.Collector;
        col.level = level;
        col.gradient = gradients[(int)EquipmentTypes.Collector];

        col.collectorType = (CollectorTypes)iR(0, 1);
        col.icon = colSprites[(int)col.collectorType];

        switch (col.collectorType)
        {
            case CollectorTypes.Grabber:
                col.collectorSpeedTo = fR(speedTo);
                col.collectorSpeedFrom = fR(speedFrom);
                col.amount = iR(colAmount) * 2;
                col.range = fR(range) * 1.5f;
                break;
            case CollectorTypes.Harpoon:
                col.collectorSpeedTo = Mathf.Clamp(fR(speedTo) / 10, 0.1f, 100);
                col.collectorSpeedFrom = fR(speedFrom) * 3;
                col.amount = iR(colAmount);
                col.range = fR(range) * 0.8f;
                break;
            case CollectorTypes.Tractor:
                break;
            case CollectorTypes.Drone:
                break;
            case CollectorTypes.Default:
                break;
            default:
                break;
        }
        for (int i = 0; i < level; i++)
        {
            col.amount *= 1 + Mathf.RoundToInt(colAmount.z);
            col.collectorSpeedTo *= 1 + (speedTo.z);
            col.collectorSpeedFrom *= 1 + (speedFrom.z);
            col.range *= 1 + (range.z);
        }

        string statsNames = "Something is wong";
        string statsValues = "WTF";
        statsNames = "";
        statsNames += "Speed:\n"; col.statLength++;
        //statsNames += "Speed from: \n"; col.statLength++;
        statsNames += "Amount: \n"; col.statLength++;
        statsNames += "Range: \n"; col.statLength++;

        col.statsText = statsNames;

        statsValues = "";
        float speed = col.collectorSpeedFrom + col.collectorSpeedTo;
        statsValues += speed.ToString("F2") + "\n";
        //statsValues += col.collectorSpeedFrom.ToString("F2") + "\n";
        statsValues += col.amount.ToString("F0") + "\n";
        statsValues += col.range.ToString("F1") + "\n";

        col.statsValues = statsValues;

        return col;
    }
    #endregion

    #region RandomizeShield
    public Shield RandomizeShield(int level)
    {
        Shield shield = ScriptableObject.CreateInstance<Shield>();
        shield.itemName = new string(
             shieldNameFirst[iR(0, shieldNameFirst.Length - 1)]
            + " "
            + shieldNameLast[iR(0, shieldNameLast.Length - 1)]);
        shield.name = shield.itemName;
        shield.id = Inventory.Instance.id;
        shield.color = typeColor[(int)EquipmentTypes.Shield];
        shield.equipType = EquipmentTypes.Shield;
        shield.level = level;
        shield.gradient = gradients[(int)EquipmentTypes.Shield];

        shield.shieldType = (ShieldType)iR(0, 2);
        shield.icon = shieldSprites[(int)shield.shieldType];

        switch (shield.shieldType)
        {
            case ShieldType.Hardlight:
                shield.shieldHealth = fR(shieldHealth) * 2;
                shield.shieldRechargeSpeed = fR(shieldRechargeSpeed);
                shield.shieldRechargeDelay = fR(shieldRechargeDelay) * 2;
                shield.shieldBreakAnimTime = fR(breakAnim);
                shield.shieldRestoreAnimTime = fR(restoreAnim);
                shield.shieldColor = Random.ColorHSV(0f, 1f, 0.8f, 1f, 0.6f, 1);
                shield.shieldColor.a = 1f;
                shield.breakColor = shield.shieldColor * 2f;
                break;
            case ShieldType.Energy:
                shield.shieldHealth = fR(shieldHealth);
                shield.shieldRechargeSpeed = fR(shieldRechargeSpeed) * 1.25f;
                shield.shieldRechargeDelay = fR(shieldRechargeDelay);
                shield.shieldBreakAnimTime = fR(breakAnim);
                shield.shieldRestoreAnimTime = fR(restoreAnim);
                shield.shieldColor = Random.ColorHSV(0f, 1f, 0.8f, 1f, 0.6f, 1);
                shield.shieldColor.a = 1f;
                shield.breakColor = shield.shieldColor * 2f;
                break;
            case ShieldType.Phaze:
                shield.shieldHealth = fR(shieldHealth) * 0.5f;
                shield.shieldRechargeSpeed = fR(shieldRechargeSpeed) * 2;
                shield.shieldRechargeDelay = fR(shieldRechargeDelay)* 0.5f;
                shield.shieldBreakAnimTime = fR(breakAnim);
                shield.shieldRestoreAnimTime = fR(restoreAnim);
                shield.shieldColor = Random.ColorHSV(0f, 1f, 0.8f, 1f, 0.6f, 1);
                shield.shieldColor.a = 1f;
                shield.breakColor = shield.shieldColor * 2f;
                break;
            case ShieldType.Obliterator:
                break;
            case ShieldType.Portal:
                break;
            case ShieldType.Forcefield:
                break;
            case ShieldType.Mirror:
                break;
            case ShieldType.Default:
                break;
            default:
                break;
        }
        Debug.Log(shield.shieldColor);

        float difficulty = Difficulty.dif.difficulty / Difficulty.dif.difSetting;
        for (int i = 0; i < level; i++)
        {
            shield.shieldHealth *= 1 + (shieldHealth.z);
            shield.shieldRechargeDelay *= 1 + (shieldRechargeDelay.z);
            shield.shieldRechargeSpeed *= 1 + (shieldRechargeSpeed.z);
        }
        string statsNames = "Something is wong";
        string statsValues = "WTF";
        statsNames = "";
        statsNames += "Max shield:\n"; shield.statLength++;
        statsNames += "Recharge rate: \n"; shield.statLength++;
        statsNames += "Recharge delay: \n"; shield.statLength++;

        shield.statsText = statsNames;

        statsValues = "";
        statsValues += shield.shieldHealth.ToString("F2") + "\n";
        statsValues += shield.shieldRechargeSpeed.ToString("F2") + "\n";
        statsValues += shield.shieldRechargeDelay.ToString("F1") + "\n";

        shield.statsValues = statsValues;

        return shield;
    }
    #endregion

    #region RandomizeHull
    public Hull RandomizeHull(int level)
    {
        Hull hull = ScriptableObject.CreateInstance<Hull>();
        hull.itemName = new string(
             hullNameFirst[iR(0, hullNameFirst.Length - 1)]
            + " "
            + hullNameLast[iR(0, hullNameLast.Length - 1)]);
        hull.name = hull.itemName;
        hull.id = Inventory.Instance.id;
        hull.color = typeColor[(int)EquipmentTypes.Hull];
        hull.equipType = EquipmentTypes.Hull;
        hull.level = level;
        hull.gradient = gradients[(int)EquipmentTypes.Hull];

        hull.hullType = (HullTypes)iR(0, 2);
        hull.icon = hullSprites[(int)hull.hullType];

        switch (hull.hullType)
        {
            case HullTypes.Default:
                hull.hullNodesMax = iR(hullNodes);
                hull.hullHealth = fR(hullMax);
                hull.hullDamageNegation = fR(hullDamageNeg);
                hull.hullWeight = fR(hullWeight);
                break;
            case HullTypes.HeavyClass:
                hull.hullNodesMax = iR(hullNodes) * 2;
                hull.hullHealth = fR(hullMax);
                hull.hullDamageNegation = fR(hullDamageNeg) * 2;
                hull.hullWeight = fR(hullWeight) * 1.5f;
                break;
            case HullTypes.LightClass:
                hull.hullNodesMax = Mathf.RoundToInt((float)iR(hullNodes) * 0.5f);
                hull.hullHealth = fR(hullMax);
                hull.hullDamageNegation = fR(hullDamageNeg) * 0.5f;
                hull.hullWeight = fR(hullWeight) * 0.8f;
                break;
            case HullTypes.NebularProtection:
                break;
            case HullTypes.NucularProtection:
                break;
            case HullTypes.IONICProtection:
                break;
            case HullTypes.Stealth:
                break;
            case HullTypes.Barrier:
                break;
            case HullTypes.ImpactInduction:
                break;
            case HullTypes.Reactive:
                break;
            case HullTypes.Sleek:
                break;
            default:
                break;
        }
        float difficulty = Difficulty.dif.difficulty / Difficulty.dif.difSetting;

        for (int i = 0; i < level; i++)
        {
            hull.hullHealth *= 1 + (hullMax.z);
            hull.hullDamageNegation *= 1 + (hullDamageNeg.z);
            //hull.hullSpecialEffectChance = 1 + (hull.level * .z);
        }
        hull.hullNodesCurrent = hull.hullNodesMax;

        string statsNames = "Something is wong";
        string statsValues = "WTF";
        statsNames = "";
        statsNames += "Health nodes:\n"; hull.statLength++;
        statsNames += "Max health: \n"; hull.statLength++;
        statsNames += "Damage negation: \n"; hull.statLength++;
        statsNames += "Weight: \n"; hull.statLength++;

        hull.statsText = statsNames;

        statsValues = "";
        statsValues += hull.hullNodesMax.ToString("F0") + "\n";
        statsValues += hull.hullHealth.ToString("F1") + "\n";
        statsValues += (hull.hullDamageNegation).ToString("F0") + "%\n";
        statsValues += (hull.hullWeight * 100).ToString("F0") + "\n";

        hull.statsValues = statsValues;

        return hull;
    }
    #endregion

    #region RandomizeScanner
    public Scanner RandomizeScanner(int level)
    {
        Scanner scanner = ScriptableObject.CreateInstance<Scanner>();
        scanner.itemName = new string(
             scannerNameFirst[iR(0, scannerNameFirst.Length - 1)]
            + " "
            + scannerNameLast[iR(0, scannerNameLast.Length - 1)]);
        scanner.name = scanner.itemName;
        scanner.id = Inventory.Instance.id;
        scanner.color = typeColor[(int)EquipmentTypes.Scanner];
        scanner.equipType = EquipmentTypes.Scanner;
        scanner.level = level;
        scanner.gradient = gradients[(int)EquipmentTypes.Scanner];

        scanner.type = (ScannerTypes)iR(0, 2);
        scanner.icon = scannerSprites[(int)scanner.type];

        switch (scanner.type)
        {
            case ScannerTypes.Custom:
                scanner.zoom.x = Mathf.Clamp((int)(iR((int)zoom.x, (int)zoom.y) * 1f), 1, 10); 
                scanner.zoom.y = Mathf.Clamp((int)(iR((int)zoom.z, (int)zoom.w) * 1.5f), 5, 10);
                scanner.mapUpdateAmount = iR(mapUpdateAmount);
                scanner.mapUpdateSpeed = fR(mapUpdateSpeed) * 0.5f;
                scanner.frequency = (Frequencies)iR(0, 1);
                break;
            case ScannerTypes.Lookout:
                scanner.zoom.x = Mathf.Clamp((int)(iR((int)zoom.x, (int)zoom.y)), 1, 10);
                scanner.zoom.y = Mathf.Clamp((int)(iR((int)zoom.z, (int)zoom.w) * 2f), 5, 10);
                scanner.mapUpdateAmount = (int)(iR(mapUpdateAmount) * 0.5f);
                scanner.mapUpdateSpeed = fR(mapUpdateSpeed) * 2f;
                scanner.frequency = (Frequencies)iR(1, 2);
                break;
            case ScannerTypes.Radio:
                scanner.zoom.x = Mathf.Clamp((int)(iR((int)zoom.x, (int)zoom.y) * 1f), 1, 10);
                scanner.zoom.y = Mathf.Clamp((int)(iR((int)zoom.z, (int)zoom.w) * 0.8f), 5, 10);
                scanner.mapUpdateAmount = (int)(iR(mapUpdateAmount) * 2);
                scanner.mapUpdateSpeed = fR(mapUpdateSpeed) * 1f;
                scanner.frequency = (Frequencies)iR(0, 4);
                break;
            case ScannerTypes.Gamma:
                scanner.zoom.x = Mathf.Clamp((int)(iR((int)zoom.x, (int)zoom.y) * 1f), 1, 10);
                scanner.zoom.y = Mathf.Clamp((int)(iR((int)zoom.z, (int)zoom.w) * 0.7f), 5, 10);
                scanner.mapUpdateAmount = (int)(iR(mapUpdateAmount) * 2f);
                scanner.mapUpdateSpeed = fR(mapUpdateSpeed) * 1f;
                scanner.frequency = (Frequencies)iR(3, 4);
                break;
            case ScannerTypes.Default:
                break;
            default:
                break;
        }
        if (scanner.zoom.x > scanner.zoom.y)
        {
            if (Random.value < 0.5f) { scanner.zoom.x = scanner.zoom.y; }
            else { scanner.zoom.y = scanner.zoom.x; }
        }
        //if (scanner.mapUpdateAmount <= 0) scanner.mapUpdateAmount = 4;

        for (int i = 0; i < level; i++)
        {
            scanner.mapUpdateSpeed *= 1 + (mapUpdateSpeed.z);
            scanner.mapUpdateAmount += mapUpdateAmount.z;
        }

        string statsNames = "Something is wong";
        string statsValues = "WTF";
        statsNames = "";
        statsNames += "Maps:\n"; scanner.statLength++;
        statsNames += "Zoom: \n"; scanner.statLength++;
        statsNames += "Map speed: \n"; scanner.statLength++;
        //statsNames += "Update delay: \n"; scanner.statLength++;

        scanner.statsText = statsNames;

        statsValues = "";
        statsValues += scanner.frequency.ToString() + "\n";
        statsValues += scanner.zoom.x.ToString("F0") + " - " + scanner.zoom.y.ToString("F0") + "\n";
        //statsValues += (scanner.mapUpdateAmount).ToString("F0") + "\n";
        float mapSpeed = scanner.mapUpdateSpeed * scanner.mapUpdateAmount;
        statsValues += mapSpeed.ToString("F1") + "\n";

        scanner.statsValues = statsValues;

        return scanner;
    }
#endregion

    #region Methods
    public float fR(float min, float max)
    {
        return Random.Range(min, max);
    }
    public float fR(Vector2 minMax)
    {
        return Random.Range(minMax.x, minMax.y);
    }
    public int iR(int min, int max)
    {
        return Random.Range(min, max + 1);
    }
    public int iR(Vector2 minMax)
    {
        return (int)Random.Range(minMax.x, minMax.y);
    }
    public int iR(Vector3 minMax)
    {
        return (int)Random.Range(minMax.x, minMax.y);
    }
    float CurveWeightedRandom(AnimationCurve curve)
    {
        return curve.Evaluate(Random.value);
    }
    #endregion
}
