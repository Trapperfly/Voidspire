using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RandomizeEquipment : MonoBehaviour
{
    [Header("Arrays")]
    [SerializeField] Sprite[] hullSprites;
    [SerializeField] Sprite[] shieldSprites;
    [SerializeField] Sprite[] cargoSprites;
    [SerializeField] Sprite[] ftlSprites;
    [SerializeField] Sprite[] stlSprites;

    public Color[] typeColor;
    [Space]

    public Transform textBoxParent;

    #region WeaponRandomizing
    [SerializeField] Sprite[] gunSprites;
    [SerializeField] string[] nameFirst;
    [SerializeField] string[] nameMid;
    [SerializeField] string[] nameLast;
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
            (nameFirst[iR(0, nameFirst.Length - 1)]
            + " "
            + nameMid[iR(0, nameMid.Length - 1)] 
            + " " 
            + nameLast[iR(0, nameLast.Length - 1)]);
        weapon.name = weapon.itemName;
        weapon.id = Inventory.Instance.id;
        weapon.icon = gunSprites[iR(0, gunSprites.Length - 1)];
        weapon.color = typeColor[(int)EquipmentTypes.Weapon];
        weapon.equipType = EquipmentTypes.Weapon;

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

        #region Weapon type stats text
        string statsNames = "Something is wong";
        string statsValues = "WTF";
        switch (weapon.weaponType)
        {
            case WeaponType.Bullet:
                statsNames = "";
                statsNames += "Damage:\n"; weapon.statLength++;
                if (isBurst) { statsNames += "Burst:\n" + "Burst delay:\n"; weapon.statLength++; weapon.statLength++; }
                if (isCharge) { statsNames += "Charge up:\n" + "Burst:\n" + "Burst delay:\n"; weapon.statLength++; weapon.statLength++; weapon.statLength++; }
                else statsNames += "Fire rate:\n"; weapon.statLength++;
                if (isPierce) { statsNames += "Piercing:\n"; weapon.statLength++; }
                if (isBounce) { statsNames += "Bounces:\n"; weapon.statLength++; }
                statsNames += "Bullet size:\n"; weapon.statLength++;
                statsNames += "Projectiles:\n"; weapon.statLength++;
                if (isHoming) { statsNames += "Homing:\n" + "Strength:\n"; weapon.statLength++; weapon.statLength++; }
                statsNames += "Spread:\n"; weapon.statLength++;
                statsNames += "Speed:\n"; weapon.statLength++;
                statsNames += "Projectile life:\n"; weapon.statLength++;
                statsNames += "Punch:\n"; weapon.statLength++;
                statsNames += "Rotation speed:\n"; weapon.statLength++;

                statsValues = "";
                statsValues += weapon.damage.ToString("F2") + "\n";
                if (isBurst) statsValues += weapon.burst + "\n" + weapon.burstDelay.ToString("F3") + "\n";
                if (isCharge) statsValues += weapon.chargeUp.ToString("F2") + "\n" + weapon.burst + "\n" + weapon.burstDelay.ToString("F3") + "\n";
                else if (fireRateBonus) { statsValues += weapon.fireRate.ToString("F2") + " -> " + (weapon.fireRate + weapon.fireRateChange).ToString("F2") + "\n"; }
                else { statsValues += weapon.fireRate.ToString("F2") + "\n"; }
                if (isPierce) statsValues += weapon.pierce + "\n";
                if (isBounce) statsValues += weapon.bounce + "\n";
                statsValues += weapon.bulletSize.ToString("F2") + "\n";
                statsValues += weapon.amount + "\n";
                if (isHoming) statsValues += weapon.homing + "\n" + weapon.homingStrength.ToString("F2") + "\n";
                if (accuracyBonus) { statsValues += weapon.spread.ToString("F2") + " -> " + Mathf.Clamp(weapon.spread + weapon.spreadChange, 0, 100).ToString("F2") + "\n"; }
                else { statsValues += weapon.spread.ToString("F2") + "\n"; }
                statsValues += weapon.speed.ToString("F2") + "\n";
                statsValues += weapon.longevity.ToString("F2") + "\n";
                statsValues += weapon.punch.ToString("F2") + "\n";
                statsValues += weapon.rotationSpeed.ToString("F2") + "\n";
                break;
            case WeaponType.Beam:
                statsNames = "";
                statsNames += "DPS:\n"; weapon.statLength++;
                //if (isPierce) statsNames += "\n" + "Piercing:\n";
                statsNames += "Range:\n"; weapon.statLength++;
                statsNames += "Rotation speed:\n"; weapon.statLength++;

                statsValues = "";
                if (fireRateBonus) { statsValues += ((weapon.damage + weapon.fireRate)).ToString("F2") + 
                        " -> "
                        + (weapon.damage * (weapon.fireRate + weapon.fireRateChange)).ToString("F2") + "\n"; }
                else { statsValues += (weapon.damage * weapon.fireRate * weapon.amount).ToString("F2") + "\n"; }
                //if (isPierce) statsValues += "\n" + weapon.pierce + "\n";
                statsValues += (weapon.speed * weapon.longevity).ToString("F2") + "\n";
                statsValues += weapon.rotationSpeed.ToString("F2") + "\n";
                break;
            case WeaponType.Railgun:
                statsNames = "";
                statsNames += "Damage:\n"; weapon.statLength++;
                if (isBurst) { statsNames += "Burst:\n" + "Burst delay:\n"; weapon.statLength++; weapon.statLength++; }
                if (isCharge) { statsNames += "Charge up:\n" + "Burst:\n" + "Burst delay:\n"; weapon.statLength++; weapon.statLength++; weapon.statLength++; }
                else statsNames += "Fire rate:\n"; weapon.statLength++;
                //if (isPierce) statsNames += "\n" + "Piercing:\n";
                statsNames += "Projectiles:\n"; weapon.statLength++;
                statsNames += "Spread:\n"; weapon.statLength++;
                statsNames += "Range:\n"; weapon.statLength++;
                //statsNames += "Punch:\n";
                statsNames += "Rotation speed:\n"; weapon.statLength++;

                statsValues = "";
                statsValues += weapon.damage.ToString("F2") + "\n";
                if (isBurst) statsValues += weapon.burst + "\n" + weapon.burstDelay.ToString("F3") + "\n";
                if (isCharge) statsValues += weapon.chargeUp.ToString("F2") + "\n" + weapon.burst + "\n" + weapon.burstDelay.ToString("F3") + "\n";
                else if (fireRateBonus) { statsValues += weapon.fireRate.ToString("F2") + " -> " + (weapon.fireRate + weapon.fireRateChange).ToString("F2") + "\n"; }
                else { statsValues += weapon.fireRate.ToString("F2") + "\n"; }
                //if (isPierce) statsValues += "\n" + weapon.pierce + "\n";
                statsValues += weapon.amount + "\n";
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
