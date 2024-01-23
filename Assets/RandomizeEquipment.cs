using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RandomizeEquipment : MonoBehaviour
{
    bool fireRateBonus;
    bool accuracyBonus;
    bool isHoming;
    bool isBounce;
    bool isPierce;
    bool isBurst;
    bool isCharge;

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
    //[SerializeField] AnimationCurve A_amount;
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
        return (int)Random.Range((float)min, (float)max);
    }

    public int iR(Vector2 minMax)
    {
        return (int)Random.Range((float)minMax.x, minMax.y);
    }
    #region RandomizeGun
    public Weapon RandomizeGun()
    {
        Weapon weapon = ScriptableObject.CreateInstance<Weapon>();
        weapon.itemName = new string
            (nameFirst[iR(0, nameFirst.Length)]
            + " "
            + nameMid[iR(0, nameMid.Length)] 
            + " " 
            + nameLast[iR(0, nameLast.Length)]);
        weapon.name = weapon.itemName;
        weapon.id = Inventory.Instance.id;
        weapon.icon = gunSprites[iR(0, gunSprites.Length)];
        weapon.color = typeColor[(int)EquipmentTypes.Weapon];
        weapon.equipType = EquipmentTypes.Weapon;

        weapon.weaponType = (WeaponType)iR(0,3);
        weapon.damage = fR(damage);
        weapon.bulletSize = fR(bulletSize);
        weapon.fireRate = fR(fireRate);
        weapon.fireRateChange = fR(fireRateChange);
        weapon.fireRateChangeTimer = fR(fireRateChangeTimer);
        weapon.amount = iR(amount);
        weapon.spread = fR(spread);
        weapon.spreadChange = fR(spreadChange);
        weapon.spreadChangeTimer = fR(spreadChangeTimer);
        weapon.speed = fR(speed);
        weapon.longevity = fR(longevity);
        weapon.homing = false;
        weapon.homingStrength = fR(homingStrength);
        weapon.pierce = iR(pierce);
        weapon.bounce = iR(bounce);
        weapon.chargeUp = fR(chargeUp);
        weapon.burst = iR(burst);
        weapon.burstDelay = fR(burstDelay);
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
                if (isBurst) { statsNames += "\n" + "Burst:\n" + "Burst delay:\n"; weapon.statLength++; }
                if (isCharge) { statsNames += "\n" + "Charge up:\n"; weapon.statLength++; }
                statsNames += "Fire rate:\n"; weapon.statLength++;
                if (isPierce) { statsNames += "\n" + "Piercing:\n"; weapon.statLength++; }
                if (isBounce) { statsNames += "\n" + "Bounces:\n"; weapon.statLength++; }
                statsNames += "Bullet size:\n"; weapon.statLength++;
                statsNames += "Projectiles:\n"; weapon.statLength++;
                if (isHoming) { statsNames += "\n" + "Homing:\n" + "Strength:\n"; weapon.statLength++; }
                statsNames += "Spread:\n"; weapon.statLength++;
                statsNames += "Speed:\n"; weapon.statLength++;
                statsNames += "Projectile life:\n"; weapon.statLength++;
                statsNames += "Punch:\n"; weapon.statLength++;
                statsNames += "Rotation speed:\n"; weapon.statLength++;

                statsValues = "";
                statsValues += weapon.damage.ToString("F2") + "\n";
                if (isBurst) statsValues += "\n" + weapon.burst + "\n" + weapon.burstDelay.ToString("F3") + "\n";
                if (isCharge) statsValues += "\n" + weapon.chargeUp.ToString("F2") + "\n";
                if (fireRateBonus) { statsValues += weapon.fireRate.ToString("F2") + " -> " + (weapon.fireRate + weapon.fireRateChange).ToString("F2") + "\n"; }
                else { statsValues += weapon.fireRate.ToString("F2") + "\n"; }
                if (isPierce) statsValues += "\n" + weapon.pierce + "\n";
                if (isBounce) statsValues += "\n" + weapon.bounce + "\n";
                statsValues += weapon.bulletSize.ToString("F2") + "\n";
                statsValues += weapon.amount + "\n";
                if (isHoming) statsValues += "\n" + weapon.homing + "\n" + weapon.homingStrength.ToString("F2") + "\n";
                if (accuracyBonus) { statsValues += weapon.spread.ToString("F2") + " -> " + (weapon.spread + weapon.spreadChange).ToString("F2") + "\n"; }
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
                if (fireRateBonus) { statsValues += ((weapon.damage + weapon.fireRate) * weapon.amount).ToString("F2") + 
                        " -> "
                        + (weapon.damage * (weapon.fireRate + weapon.fireRateChange) * weapon.amount).ToString("F2") + "\n"; }
                else { statsValues += (weapon.damage * weapon.fireRate * weapon.amount).ToString("F2") + "\n"; }
                //if (isPierce) statsValues += "\n" + weapon.pierce + "\n";
                statsValues += (weapon.speed * weapon.longevity).ToString("F2") + "\n";
                statsValues += weapon.rotationSpeed.ToString("F2") + "\n";
                break;
            case WeaponType.Railgun:
                statsNames = "";
                statsNames += "Damage:\n"; weapon.statLength++;
                if (isBurst) { statsNames += "\n" + "Burst:\n" + "Burst delay:\n"; weapon.statLength++; }
                if (isCharge) { statsNames += "\n" + "Charge up:\n"; weapon.statLength++; }
                statsNames += "Fire rate:\n"; weapon.statLength++;
                //if (isPierce) statsNames += "\n" + "Piercing:\n";
                statsNames += "Projectiles:\n"; weapon.statLength++;
                statsNames += "Spread:\n"; weapon.statLength++;
                statsNames += "Range:\n"; weapon.statLength++;
                //statsNames += "Punch:\n";
                statsNames += "Rotation speed:\n"; weapon.statLength++;

                statsValues = "";
                statsValues += weapon.damage.ToString("F2") + "\n";
                if (isBurst) statsValues += "\n" + weapon.burst + "\n" + weapon.burstDelay.ToString("F3") + "\n";
                if (isCharge) statsValues += "\n" + weapon.chargeUp.ToString("F2") + "\n";
                if (fireRateBonus) { statsValues += weapon.fireRate.ToString("F2") + " -> " + (weapon.fireRate + weapon.fireRateChange).ToString("F2") + "\n"; }
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

    float CurveWeightedRandom(AnimationCurve curve)
    {
        return curve.Evaluate(Random.value);
    }
}
