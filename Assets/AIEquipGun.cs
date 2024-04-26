using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New AI gun", menuName = "AI/AI gun")]
public class AIEquipGun : ScriptableObject
{
    public int id;

    public AIAttack attack;
    public EffectFromAttack effect;
    public float effectChance;

    public float damage;
    public int amount;
    public float shotSpeed;
    public float fireRate;
    public float spread;
    public float gunRotSpeed;
    public bool homing;
    public float homingStrength;
}
public enum AIAttack
{
    None,
    Bullet,
    VoidSphere,
    FireMissile,
    ElectricRailgun,
    Flak,
    RapidFire,
    Cannon,
    SpawnEnemy,
    LayMine,
}
public enum EffectFromAttack
{
    None,
    Burn,
    Void,
    Shock
}
