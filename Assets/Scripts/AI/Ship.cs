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
    public EnrageBehaviour enrageBehaviour;
    public float enrageStrength;
    public SpecialAttack enrageSpecialAttack;
    public ContactBehaviour contactBehaviour;

    [Header("Health/Shield")]
    public float maxHealth;
    public float maxShield;
    public float mass = 1;

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
    public bool isWeakened;
    public bool isDisarmed;

    [Header("SpecialAttack")]
    public SpecialAttack specialAttack;
    public bool startSpecialImmediately;
    public float specialAttackCD;
    public float specialAttackDamage;
    public float specialAttackSpeed;
    public float specialAttackSpread;
    public int specialAmount;
    public float damageTickTime;
    public bool specialIsHoming;
    public float specialHomingStrength;
    public bool aimTheSpecial;
    public bool stopCoreWhenSpecial;
    public bool stopCoreWhenSpecialLong;
    public float stopCoreForSeconds;
    public bool predictWhenAiming;
    public LayerMask specialHitMask;

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
    Stealth,
    AttackImmediately
}

public enum CombatBehaviour
{
    None,
    StandGroundAndAttack,
    RotateTowardsAndAttack,
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

public enum EnrageBehaviour
{
    None,
    DoubleAttackSpeed,
    DoubleDamage,
    EnableSpecialAttack,
    SpawnAllies,
    UnlockLockedGuns
}

public enum ContactBehaviour
{
    None,
    Nothing,
    DependOnRelation,
    Attack,
    Jump
}



public enum SpecialAttack
{
    None,
    ElectricBall,
    HomingMissiles,
    VoidNova,
    SelfDestruct,
    DestroyerBeam
}

public enum AvailableAttacks
{
    None,
    Normal,
    Special,
    NormalAndSpecial
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