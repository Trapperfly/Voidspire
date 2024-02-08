using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RandomizeEquipment : MonoBehaviour
{
    public Color[] typeColor;
    [Space]

    public Transform textBoxParent;

    #region WeaponRandomizing
    [Header("Weapon")]
    [SerializeField] Sprite[] gunSprites;
    [SerializeField] string[] wNameFirst;
    [SerializeField] string[] wNameMid;
    [SerializeField] string[] wNameLast;
    [Header("WeaponStats")]
    [SerializeField] Vector2 damage;              //On bullet
    //[SerializeField] AnimationCurve damageChance;
    [SerializeField] Vector2 bulletSize;          //On gun
    [SerializeField] Vector2 fireRate;
    //[SerializeField] AnimationCurve fireRateChance;        //On gun
    [SerializeField] Vector2 fireRateChange;   //On gun
    [SerializeField] Vector2 fireRateChangeTimer; //On gun
    [SerializeField] Vector2 amount;                //On gun
    [SerializeField] AnimationCurve A_amount;
    [SerializeField] Vector2 spread;              //On gun
    [SerializeField] Vector2 spreadChange;        //On gun
    [SerializeField] Vector2 spreadChangeTimer;
    [SerializeField] Vector2 speed;               //On gun
    [SerializeField] Vector2 longevity;           //On bullet
    [SerializeField] Vector2 homingStrength;      //On bullet
    [SerializeField] Vector2 pierce;                //On bullet
    [SerializeField] Vector2 bounce;                //On bullet
    //[SerializeField] bool bounceToTarget;
    [SerializeField] Vector2 chargeUp;            //On gun
    [SerializeField] Vector2 burst;                 //On gun
    [SerializeField] Vector2 burstDelay;          //On gun
    [SerializeField] Vector2 punch;               //On bullet
    [SerializeField] Vector2 rotationSpeed;
    #endregion

    [Header("STL Engine")]
    [SerializeField] Sprite[] stlSprites;
    [SerializeField] string[] stlNameFirst;
    [SerializeField] string[] stlNameLast;

    [SerializeField] Vector2 moveSpeed;
    [SerializeField] Vector2 maxSpeed;
    [SerializeField] Vector2 turnSpeed;
    [SerializeField] Vector2 maxTurnSpeed;
    [SerializeField] Vector2 turnBrakingSpeed;
    [SerializeField] Vector2 brakingSpeed;

    [Header("FTL Engine")]
    [SerializeField] Sprite[] ftlSprites;
    [SerializeField] string[] ftlNameFirst;
    [SerializeField] string[] ftlNameLast;

    [SerializeField] Vector2 acceleration;
    [SerializeField] Vector2 ftlMaxSpeed;
    [SerializeField] Vector2 rotSpeed;
    [SerializeField] Vector2 chargeTime;
    [SerializeField] Vector2 fuelCurrent;
    [SerializeField] Vector2 fuelMax;
    [SerializeField] Vector2 fuelDrain;
    [SerializeField] Vector2 maxDuration;

    [Header("Collector")]
    [SerializeField] Sprite[] colSprites;
    [SerializeField] string[] colNameFirst;
    [SerializeField] string[] colNameLast;

    [SerializeField] Vector2 speedTo;
    [SerializeField] Vector2 speedFrom;
    [SerializeField] Vector2 colAmount;
    [SerializeField] Vector2 range;

    [Header("Shield")]
    [SerializeField] Sprite[] shieldSprites;
    [SerializeField] string[] shieldNameFirst;
    [SerializeField] string[] shieldNameLast;

    [SerializeField] Vector2 shieldHealth;
    [SerializeField] Vector2 shieldRechargeSpeed;
    [SerializeField] Vector2 shieldRechargeDelay;
    [SerializeField] Vector2 breakAnim;
    [SerializeField] Vector2 restoreAnim;

    [Header("Hull")]
    [SerializeField] Sprite[] hullSprites;
    [SerializeField] string[] hullNameFirst;
    [SerializeField] string[] hullNameLast;

    [SerializeField] Vector2 hullCurrent;
    [SerializeField] Vector2 hullMax;
    [SerializeField] Vector2 hullDamageNeg;
    [SerializeField] Vector2 hullWeight;

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


    #region RandomizeGun
    public Weapon RandomizeGun()
    {
        bool fireRateBonus = false;
        bool accuracyBonus = false;
        bool isHoming = false;
        bool isBounce = false;
        bool isPierce = false;
        bool isBurst = false;
        bool isCharge = false;

        Weapon weapon = ScriptableObject.CreateInstance<Weapon>();
        weapon.itemName = new string
            (wNameFirst[iR(0, wNameFirst.Length - 1)]
            + " "
            + wNameMid[iR(0, wNameMid.Length - 1)] 
            + " " 
            + wNameLast[iR(0, wNameLast.Length - 1)]);
        weapon.name = weapon.itemName;
        weapon.id = Inventory.Instance.id;
        weapon.icon = gunSprites[iR(0, gunSprites.Length - 1)];
        weapon.color = typeColor[(int)EquipmentTypes.Weapon];
        weapon.equipType = EquipmentTypes.Weapon;
        weapon.effectColor = Random.ColorHSV(0f, 1f, 0.8f, 1f, 0.6f, 1);

        weapon.weaponType = (WeaponType)iR(0, 2);
        int style = Random.Range(0,5);
        //0 = low dmg, high rof 
        //1 = high dmg, low rof 
        //2 = both medium 
        //3 = high speed, slightly lower both
        //4 = low speed, slightly higher both
        switch (style)
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

        int bonus = Random.Range(0, 4);
        switch (bonus)
        {
            case 0: // bonus fire rate
                weapon.fireRateChange = fR(fireRateChange);
                weapon.fireRateChangeTimer = fR(fireRateChangeTimer);
                if (weapon.fireRateChange > 0) { } else { weapon.damage *= 1.25f; };
                fireRateBonus = true;
                break;
            case 1: // bonus accuracy
                weapon.spreadChange = fR(spreadChange);
                weapon.spreadChangeTimer = fR(spreadChangeTimer);
                if (weapon.spreadChange < 0f) { weapon.spread *= 0.8f; } else { weapon.spread *= 1.25f; }
                accuracyBonus = true;
                break;
            default: // nothing
                break;
        }
        weapon.bulletSize = fR(bulletSize);

        weapon.amount = (int)Mathf.Lerp(amount.x, amount.y, CurveWeightedRandom(A_amount));

        weapon.longevity = fR(longevity);

        if (Random.value <= 0.2f)
        {
            weapon.homing = true;
            weapon.homingStrength = fR(homingStrength) * 10f;
            isHoming = true;
        }
         else
        {
            weapon.homing = false;
            weapon.homingStrength = 0;
        }

        int bounceOrPierce = Random.Range(0,4);
        switch (bounceOrPierce)
        {
            case 0:
                weapon.bounce = iR(bounce);
                isBounce = true;
                break;
            case 1:
                weapon.pierce = iR(pierce);
                isPierce = true;
                break;
            default:
                break;
        }

        if (weapon.weaponType == WeaponType.Beam) { }
        else
        {
            int chargeOrBurst = Random.Range(0, 6);
            switch (chargeOrBurst)
            {
                case 0:
                    weapon.chargeUp = fR(chargeUp);
                    weapon.burst = iR(burst) * 2;
                    weapon.burstDelay = fR(burstDelay);
                    weapon.amount *= 2;
                    if (style == 0) // is low damage and high attack
                    {
                        weapon.burst *= 2;
                    }
                    isCharge = true;
                    break;
                case 1:
                    weapon.burst = iR(burst);
                    weapon.burstDelay = fR(burstDelay);
                    weapon.fireRate *= 0.6f;
                    isBurst = true;
                    break;
                default:
                    break;
            }
        }
        

        weapon.punch = fR(punch);

        weapon.rotationSpeed = fR(rotationSpeed);

        float difficulty = Difficulty.dif.difficulty / 2;

        weapon.damage *= difficulty;
        weapon.fireRate *= difficulty;
        weapon.speed *= 1 + (0.1f * difficulty);
        weapon.homingStrength *= difficulty;
        if (weapon.pierce > 0) { weapon.pierce *= 1 + (int)difficulty; if (weapon.pierce == 0) { weapon.pierce = 1; } }
        if (weapon.bounce > 0) { weapon.bounce *= 1 + (int)difficulty; if (weapon.bounce == 0) { weapon.bounce = 1; } }
        weapon.chargeUp /= difficulty;
        if (weapon.burst > 0) { weapon.burst *= 1 + (int)difficulty; if (weapon.burst == 0) { weapon.burst = 1; } }
        weapon.burstDelay /= difficulty;
        weapon.punch *= difficulty;
        weapon.rotationSpeed *= difficulty;

        if (weapon.weaponType == WeaponType.Beam || weapon.weaponType == WeaponType.Railgun) { weapon.speed /= 2f; }

        #region Weapon type stats text
        string statsNames = "Something is wong";
        string statsValues = "WTF";
        bool showFireRate = true;
        bool anyBonus = isHoming || isPierce || isBounce || isBurst || isCharge;
        bool anyBonusBeamSpecific = isPierce;
        bool anyBonusRailgunSpecific = isBurst || isCharge || isPierce;
        switch (weapon.weaponType)
        {
            case WeaponType.Bullet:
                statsNames = "";
                if (isHoming) { statsNames += "Homing:\n" + "Strength:\n"; weapon.statLength += 2; }
                if (isPierce) { statsNames += "Piercing:\n"; weapon.statLength++; }
                if (isBounce) { statsNames += "Bounces:\n"; weapon.statLength++; }
                if (isBurst) { statsNames += "Burst:\n" + "Burst delay:\n"; weapon.statLength += 2; }
                if (isCharge) { statsNames += "Charge up:\n" + "Burst:\n" + "Burst delay:\n"; weapon.statLength += 3; showFireRate = false; }
                if (anyBonus) { statsNames += "\n"; weapon.statLength++; }

                statsNames += "Damage:\n"; weapon.statLength++;
                if (showFireRate) { statsNames += "Fire rate:\n"; weapon.statLength++; }
                //statsNames += "Bullet size:\n"; weapon.statLength++;
                //statsNames += "Projectiles:\n"; weapon.statLength++;
                statsNames += "Spread:\n"; weapon.statLength++;
                statsNames += "Speed:\n"; weapon.statLength++;
                //statsNames += "Projectile life:\n"; weapon.statLength++;
                //statsNames += "Punch:\n"; weapon.statLength++;
                statsNames += "Rotation speed:\n"; weapon.statLength++;

                statsValues = "";
                if (isHoming) statsValues += weapon.homing + "\n" + weapon.homingStrength.ToString("F2") + "\n";
                if (isPierce) statsValues += weapon.pierce + "\n";
                if (isBounce) statsValues += weapon.bounce + "\n";
                if (isBurst) statsValues += weapon.burst + "\n" + weapon.burstDelay.ToString("F3") + "\n";
                if (isCharge) statsValues += weapon.chargeUp.ToString("F2") + "\n" + weapon.burst + "\n" + weapon.burstDelay.ToString("F3") + "\n";
                if (anyBonus) statsValues += "\n";
                if (weapon.amount > 1) statsValues += weapon.damage.ToString("F2") + " x " + weapon.amount + "\n";
                else statsValues += weapon.damage.ToString("F2") + "\n";
                if (isCharge) { }
                else if (fireRateBonus) { statsValues += weapon.fireRate.ToString("F2") + " -> " + (weapon.fireRate + weapon.fireRateChange).ToString("F2") + "\n"; }
                else statsValues += weapon.fireRate.ToString("F2") + "\n";


                //statsValues += weapon.bulletSize.ToString("F2") + "\n";
                //statsValues += weapon.amount + "\n";

                if (accuracyBonus) { statsValues += weapon.spread.ToString("F2") + " -> " + Mathf.Clamp(weapon.spread + weapon.spreadChange, 0, 100).ToString("F2") + "\n"; }
                else { statsValues += weapon.spread.ToString("F2") + "\n"; }
                statsValues += weapon.speed.ToString("F2") + "\n";
                //statsValues += weapon.longevity.ToString("F2") + "\n";
                //statsValues += weapon.punch.ToString("F2") + "\n";
                statsValues += weapon.rotationSpeed.ToString("F2") + "\n";
                break;
            case WeaponType.Beam:
                statsNames = "";
                if (isPierce) { statsNames += "Piercing:\n"; weapon.statLength++; }
                if (anyBonusBeamSpecific) { statsNames += "\n"; weapon.statLength++; }
                statsNames += "DPS:\n"; weapon.statLength++;
                //if (isPierce) statsNames += "\n" + "Piercing:\n";
                statsNames += "Range:\n"; weapon.statLength++;
                statsNames += "Rotation speed:\n"; weapon.statLength++;

                statsValues = "";
                if (isPierce) statsValues += weapon.pierce + "\n";
                if (anyBonusBeamSpecific) statsValues += "\n";
                if (fireRateBonus) { statsValues += ((weapon.damage + weapon.fireRate)).ToString("F2") + 
                        " -> "
                        + (weapon.damage * (weapon.fireRate + weapon.fireRateChange)).ToString("F2") + "\n"; }
                else { statsValues += (weapon.damage * weapon.fireRate).ToString("F2") + "\n"; }
                //if (isPierce) statsValues += "\n" + weapon.pierce + "\n";
                statsValues += (weapon.speed * weapon.longevity).ToString("F2") + "\n";
                statsValues += weapon.rotationSpeed.ToString("F2") + "\n";
                break;
            case WeaponType.Railgun:
                statsNames = "";
                if (isPierce) { statsNames += "Piercing:\n"; weapon.statLength++; }
                if (isBurst) { statsNames += "Burst:\n" + "Burst delay:\n"; weapon.statLength += 2; }
                if (isCharge) { statsNames += "Charge up:\n" + "Burst:\n" + "Burst delay:\n"; weapon.statLength += 3; showFireRate = false; }
                if (anyBonusRailgunSpecific) { statsNames += "\n"; weapon.statLength++; }
                statsNames += "Damage:\n"; weapon.statLength++;
                if (isCharge) { }
                else { statsNames += "Fire rate:\n"; weapon.statLength++; }
                //if (isPierce) statsNames += "\n" + "Piercing:\n";
                //statsNames += "Projectiles:\n"; weapon.statLength++;
                statsNames += "Spread:\n"; weapon.statLength++;
                statsNames += "Range:\n"; weapon.statLength++;
                //statsNames += "Punch:\n";
                statsNames += "Rotation speed:\n"; weapon.statLength++;

                statsValues = "";
                if (isPierce) statsValues += weapon.pierce + "\n";
                if (isBurst) statsValues += weapon.burst + "\n" + weapon.burstDelay.ToString("F3") + "\n";

                if (isCharge) statsValues += weapon.chargeUp.ToString("F2") + "\n" + weapon.burst + "\n" + weapon.burstDelay.ToString("F3") + "\n";
                if (anyBonusRailgunSpecific) statsValues += "\n";
                if (weapon.amount > 1) statsValues += weapon.damage.ToString("F2") + " x " + weapon.amount.ToString("F0") + "\n";
                else statsValues += weapon.damage.ToString("F2") + "\n";
                if (isCharge) { }
                else if (fireRateBonus) { statsValues += weapon.fireRate.ToString("F2") + " -> " + Mathf.Clamp(weapon.fireRate + weapon.fireRateChange, 0, 100).ToString("F2") + "\n"; }
                else statsValues += weapon.fireRate.ToString("F2") + "\n";
                //if (isPierce) statsValues += "\n" + weapon.pierce + "\n";
                //statsValues += weapon.amount + "\n";
                if (accuracyBonus) { statsValues += weapon.spread.ToString("F2") + " -> " + (weapon.spread + weapon.spreadChange).ToString("F2") + "\n"; }
                else { statsValues += weapon.spread.ToString("F2") + "\n"; }
                statsValues += (weapon.speed * weapon.longevity).ToString("F2") + "\n";
                //statsValues += weapon.punch + "\n";
                statsValues += weapon.rotationSpeed.ToString("F2") + "\n";
                break;
            case WeaponType.Laser:
                break;
            case WeaponType.Wave:
                break;
            case WeaponType.Rocket:
                break;
            case WeaponType.Needle:
                break;
            case WeaponType.Mine:
                break;
            case WeaponType.Hammer:
                break;
            case WeaponType.Cluster:
                break;
            case WeaponType.Arrow:
                break;
            case WeaponType.Mirage:
                break;
            case WeaponType.Grand:
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

        return weapon;
    }
    #endregion

    #region RandomizeSTLEngine
    public STLEngine RandomizeSTLEngine()
    {
        STLEngine stl = ScriptableObject.CreateInstance<STLEngine>();
        stl.itemName = new string
            (stlNameFirst[iR(0, stlNameFirst.Length - 1)]
            + " "
            + stlNameLast[iR(0, stlNameLast.Length - 1)]);
        stl.name = stl.itemName;
        stl.id = Inventory.Instance.id;
        stl.icon = stlSprites[iR(0, stlSprites.Length - 1)];
        stl.color = typeColor[(int)EquipmentTypes.STL];
        stl.equipType = EquipmentTypes.STL;

        stl.stlType = (STLTypes)iR(0, 1);

        stl.speed = fR(moveSpeed);
        stl.maxSpeed = fR(maxSpeed);
        stl.turnSpeed = fR(turnSpeed);
        stl.maxTurnSpeed = fR(maxTurnSpeed);
        stl.turnBrakingSpeed = fR(turnBrakingSpeed);
        stl.brakingSpeed = fR(brakingSpeed);

        string statsNames = "Something is wong";
        string statsValues = "WTF";
        statsNames = "";
        statsNames += "Speed:\n"; stl.statLength++;
        statsNames += "Max speed: \n"; stl.statLength++;
        statsNames += "Brake: \n"; stl.statLength++;
        statsNames += "Turn speed: \n"; stl.statLength++;
        statsNames += "Max turn speed: \n"; stl.statLength++;
        statsNames += "Rotation brake: \n"; stl.statLength++;
        
        stl.statsText = statsNames;

        statsValues = "";
        statsValues += stl.speed.ToString("F2") + "\n";
        statsValues += stl.maxSpeed.ToString("F2") + "\n";
        statsValues += stl.brakingSpeed.ToString("F2") + "\n";
        statsValues += stl.turnSpeed.ToString("F2") + "\n";
        statsValues += stl.maxTurnSpeed.ToString("F2") + "\n";
        statsValues += stl.turnBrakingSpeed.ToString("F2") + "\n";

        stl.statsValues = statsValues;

        return stl;
    }
    #endregion

    #region RandomizeFTLEngine
    public FTLEngine RandomizeFTLEngine()
    {
        FTLEngine ftl = ScriptableObject.CreateInstance<FTLEngine>();
        ftl.itemName = new string
            (ftlNameFirst[iR(0, ftlNameFirst.Length - 1)]
            + " "
            + ftlNameLast[iR(0, ftlNameLast.Length - 1)]);
        ftl.name = ftl.itemName;
        ftl.id = Inventory.Instance.id;
        ftl.icon = ftlSprites[iR(0, ftlSprites.Length - 1)];
        ftl.color = typeColor[(int)EquipmentTypes.FTL];
        ftl.equipType = EquipmentTypes.FTL;

        ftl.ftlType = (FTLTypes)iR(0, 3);

        switch (ftl.ftlType)
        {
            case FTLTypes.Ready: //Short charge time, shorter duration, more fuel drain
                ftl.acceleration = fR(acceleration);
                ftl.maxSpeed = fR(ftlMaxSpeed);
                ftl.rotSpeed = fR(rotSpeed);
                ftl.chargeTime = fR(chargeTime) * 0.5f;
                ftl.fuelMax = fR(fuelMax);
                ftl.fuelCurrent = ftl.fuelMax - (ftl.fuelMax * (fR(fuelCurrent) / 100));
                ftl.fuelDrain = fR(fuelDrain) * 1.5f;
                ftl.maxDuration = fR(maxDuration) * 0.5f;
                break;
            case FTLTypes.Burst: //High acceleration, higher max speed
                ftl.acceleration = fR(acceleration) * 3;
                ftl.maxSpeed = fR(ftlMaxSpeed) * 3;
                ftl.rotSpeed = fR(rotSpeed);
                ftl.chargeTime = fR(chargeTime) * 0.8f;
                ftl.fuelMax = fR(fuelMax);
                ftl.fuelCurrent = ftl.fuelMax - (ftl.fuelMax * (fR(fuelCurrent) / 100));
                ftl.fuelDrain = fR(fuelDrain) * 2;
                ftl.maxDuration = fR(maxDuration) * 0.3f;
                break;
            case FTLTypes.Flight: //Long charge time, high max speed and acceleration, lower fuel drain
                ftl.acceleration = fR(acceleration);
                ftl.maxSpeed = fR(ftlMaxSpeed);
                ftl.rotSpeed = fR(rotSpeed) * 0;
                ftl.chargeTime = fR(chargeTime) * 2;
                ftl.fuelMax = fR(fuelMax) * 2;
                ftl.fuelCurrent = ftl.fuelMax - (ftl.fuelMax * (fR(fuelCurrent) / 100));
                ftl.fuelDrain = fR(fuelDrain) * 0.5f;
                ftl.maxDuration = fR(maxDuration) * 3;
                break;
            case FTLTypes.Scout: //Medium all round, but high rot speed and duration
                ftl.acceleration = fR(acceleration);
                ftl.maxSpeed = fR(ftlMaxSpeed);
                ftl.rotSpeed = fR(rotSpeed) * 10;
                ftl.chargeTime = fR(chargeTime) * 0.75f;
                ftl.fuelMax = fR(fuelMax);
                ftl.fuelCurrent = ftl.fuelMax - (ftl.fuelMax * (fR(fuelCurrent) / 100));
                ftl.fuelDrain = fR(fuelDrain) * 1.5f;
                ftl.maxDuration = fR(maxDuration);
                break;
            case FTLTypes.Crash:
                break;
            case FTLTypes.Default:
                break;
            default:
                break;
        }

        string statsNames = "Something is wong";
        string statsValues = "WTF";
        statsNames = "";
        statsNames += "Acceleration:\n"; ftl.statLength++;
        statsNames += "Max speed: \n"; ftl.statLength++;
        statsNames += "Rotation speed: \n"; ftl.statLength++;
        statsNames += "Charge time: \n"; ftl.statLength++;
        statsNames += "Current fuel: \n"; ftl.statLength++;
        statsNames += "Max fuel: \n"; ftl.statLength++;
        statsNames += "Fuel drain: \n"; ftl.statLength++;
        statsNames += "Max duration: \n"; ftl.statLength++;

        ftl.statsText = statsNames;

        statsValues = "";
        statsValues += ftl.acceleration.ToString("F0") + "\n";
        statsValues += ftl.maxSpeed.ToString("F0") + "\n";
        statsValues += ftl.rotSpeed.ToString("F2") + "\n";
        statsValues += ftl.chargeTime.ToString("F2") + "\n";
        statsValues += ftl.fuelCurrent.ToString("F0") + "\n";
        statsValues += ftl.fuelMax.ToString("F0") + "\n";
        statsValues += ftl.fuelDrain.ToString("F2") + "\n";
        statsValues += ftl.maxDuration.ToString("F0") + "\n";

        ftl.statsValues = statsValues;

        return ftl;
    }
    #endregion

    #region RandomizeCollector
    public Collector RandomizeCollector()
    {
        Collector col = ScriptableObject.CreateInstance<Collector>();
        col.itemName = new string(
             colNameFirst[iR(0, colNameFirst.Length - 1)]
            + " "
            + colNameLast[iR(0, colNameLast.Length - 1)]);
        col.name = col.itemName;
        col.id = Inventory.Instance.id;
        col.icon = colSprites[iR(0, colSprites.Length - 1)];
        col.color = typeColor[(int)EquipmentTypes.Collector];
        col.equipType = EquipmentTypes.Collector;

        col.collectorType = (CollectorTypes)iR(0, 1);

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
        

        string statsNames = "Something is wong";
        string statsValues = "WTF";
        statsNames = "";
        statsNames += "Speed to:\n"; col.statLength++;
        statsNames += "Speed from: \n"; col.statLength++;
        statsNames += "Amount: \n"; col.statLength++;
        statsNames += "Range: \n"; col.statLength++;

        col.statsText = statsNames;

        statsValues = "";
        statsValues += col.collectorSpeedTo.ToString("F2") + "\n";
        statsValues += col.collectorSpeedFrom.ToString("F2") + "\n";
        statsValues += col.amount.ToString("F0") + "\n";
        statsValues += col.range.ToString("F2") + "\n";

        col.statsValues = statsValues;

        return col;
    }
    #endregion

    #region RandomizeShield
    public Shield RandomizeShield()
    {
        Shield shield = ScriptableObject.CreateInstance<Shield>();
        shield.itemName = new string(
             shieldNameFirst[iR(0, shieldNameFirst.Length - 1)]
            + " "
            + shieldNameLast[iR(0, shieldNameLast.Length - 1)]);
        shield.name = shield.itemName;
        shield.id = Inventory.Instance.id;
        shield.icon = shieldSprites[iR(0, shieldSprites.Length - 1)];
        shield.color = typeColor[(int)EquipmentTypes.Shield];
        shield.equipType = EquipmentTypes.Shield;

        shield.shieldType = (ShieldType)iR(0, 2);

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
    public Hull RandomizeHull()
    {
        Hull hull = ScriptableObject.CreateInstance<Hull>();
        hull.itemName = new string(
             hullNameFirst[iR(0, hullNameFirst.Length - 1)]
            + " "
            + hullNameLast[iR(0, hullNameLast.Length - 1)]);
        hull.name = hull.itemName;
        hull.id = Inventory.Instance.id;
        hull.icon = hullSprites[iR(0, hullSprites.Length - 1)];
        hull.color = typeColor[(int)EquipmentTypes.Hull];
        hull.equipType = EquipmentTypes.Hull;

        hull.hullType = (HullTypes)iR(0, 2);

        switch (hull.hullType)
        {
            case HullTypes.Default:
                hull.hullHealth = fR(hullMax);
                hull.hullCurrentHealth = hull.hullHealth - (hull.hullHealth * (fR(hullCurrent) / 100));
                hull.hullDamageNegation = fR(hullDamageNeg);
                hull.hullWeight = fR(hullWeight);
                break;
            case HullTypes.HeavyClass:
                hull.hullHealth = fR(hullMax) * 2;
                hull.hullCurrentHealth = hull.hullHealth - (hull.hullHealth * (fR(hullCurrent) / 100));
                hull.hullDamageNegation = fR(hullDamageNeg) * 2;
                hull.hullWeight = fR(hullWeight) * 1.5f;
                break;
            case HullTypes.LightClass:
                hull.hullHealth = fR(hullMax) * 0.5f;
                hull.hullCurrentHealth = hull.hullHealth - (hull.hullHealth * (fR(hullCurrent) / 100));
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


        string statsNames = "Something is wong";
        string statsValues = "WTF";
        statsNames = "";
        statsNames += "Current health:\n"; hull.statLength++;
        statsNames += "Max health: \n"; hull.statLength++;
        statsNames += "Damage negation: \n"; hull.statLength++;
        statsNames += "Weight: \n"; hull.statLength++;

        hull.statsText = statsNames;

        statsValues = "";
        statsValues += hull.hullCurrentHealth.ToString("F1") + "\n";
        statsValues += hull.hullHealth.ToString("F1") + "\n";
        statsValues += (hull.hullDamageNegation).ToString("F0") + "%\n";
        statsValues += (hull.hullWeight * 100).ToString("F0") + "\n";

        hull.statsValues = statsValues;

        return hull;
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
    float CurveWeightedRandom(AnimationCurve curve)
    {
        return curve.Evaluate(Random.value);
    }
    #endregion
}
