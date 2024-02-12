using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New ship", menuName = "AI/Ship")]
public class Ship : AI
{
    [Header("Base")]
    public GameObject shipPrefab;

    [Header("Behaviour")]
    public Behaviour behaviour;
    public CombatBehaviour combatBehaviour;
    public LowHealthBehaviour lowHealthBehaviour;
    public float lowHealthPercent;
    public ContactBehaviour contactBehaviour;

    [Header("Movement")]
    public bool isDisabled;
    public float speed;
    public float rotSpeed;
    public float strafeSpeed;
    [Space]
    public int jumpTime;
    public float jumpSpeed;

    [Header("Movement modifiers")]
    public float exploreMod;
    public float fleeMod;
    public float combatMod;
    public float repairMod;

    public float combatDrag;
    public float combatAngularDrag;

    [Header("Repairing")]
    public bool canRepair;
    public float repairSpeed;

    [Header("Range")]
    public float viewRange;
    public float reactRange;
    public float attackRange;

    [Header("Targeting")]
    public Vector2 newMoveTargetTime;
    public float targetRadius;
    public float changeTargetChance;

    [Header("Weapon stats")]
    public AvailableAttacks availableAttacks;
    public NormalAttack normalAttack;
    public EffectFromAttack effect;
    public float effectChance;
    public bool isWeakened;
    public bool isDisarmed;
    public float damage;
    public float shotSpeed;
    public float fireRate;
    public float spread;
    public float gunRotSpeed;

    [Header("SpecialAttack")]
    public SpecialAttack specialAttack;
    public float specialAttackCD;
    public float specialAttackDamage;
    public float specialAttackSpeed;
    public float damageTickTime;
    public bool stopCoreWhenSpecial;

    [Header("Rewards")]
    public Reward reward;
    public float value;
}

public enum Behaviour
{
    None,
    NotDefined,
    TargetResources,
    StalkTarget,
    JumpOnSight,
    RepairTarget,
    Stealth
}

public enum CombatBehaviour
{
    None,
    ChaseAndAttack,
    RunAndAttack,
    SelfDestruct,
    Crash,
    JumpImmediately
}

public enum LowHealthBehaviour
{
    None,
    Flee,
    Jump,
    DropLootAndJump,
    Surrender,
    SurrenderButBargain,
    SurrenderButTrick,
    Enrage,
    SelfDestruct
}

public enum ContactBehaviour
{
    None,
    Nothing,
    DependOnRelation,
    Attack,
    Jump
}

public enum NormalAttack
{
    None,
    Bullet,
    VoidSphere,
    FireMissile,
    ElectricRailgun
}

public enum SpecialAttack
{
    None,
    ElectricBall,
    HomingMissiles,
    VoidNova,
    SelfDestruct
}

public enum AvailableAttacks
{
    None,
    Normal,
    Special,
    NormalAndSpecial
}

public enum EffectFromAttack
{
    None,
    Burn,
    Void,
    Shock
}
public enum Reward
{
    None,
    Normal,
    JustScrap,
    JustNormalLoot,
    DropsGunsAndNormalLoot,
    ChanceForGunsAndNormalLoot,
    SpecialReward,
    EventTargetWithReward,
    EventTargetWithNormalLoot,
    EventTargetWithJustScrap,
    BossLoot,
    MiniBossLoot,
    Nothing
}