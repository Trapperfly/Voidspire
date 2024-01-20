using System.Collections;
using System.Collections.Generic;
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

        return weapon;
    }

    float CurveWeightedRandom(AnimationCurve curve)
    {
        return curve.Evaluate(Random.value);
    }
}
