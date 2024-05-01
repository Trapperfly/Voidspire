using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Player movement")]
    [field: SerializeField] public EventReference mainThruster {  get; private set; }
    [field: SerializeField] public EventReference sideThruster { get; private set; }
    [field: SerializeField] public EventReference rotateThruster { get; private set; }
    [field: SerializeField] public EventReference backThruster { get; private set; }
    [field: SerializeField] public EventReference ftlAmbient { get; private set; }
    [field: SerializeField] public EventReference ftlCharge { get; private set; }
    [field: SerializeField] public EventReference ftlExit { get; private set; }

    [field: Header("Gun")]
    [field: SerializeField] public EventReference gunLaser { get; private set; }
    [field: SerializeField] public EventReference gunLight { get; private set; }
    [field: SerializeField] public EventReference gunHeavy { get; private set; }
    [field: SerializeField] public EventReference beam { get; private set; }
    [field: SerializeField] public EventReference railgun { get; private set; }
    [field: SerializeField] public EventReference weaponChargeUp { get; private set; }

    [field: Header("Targeting")]
    [field: SerializeField] public EventReference targetNothing { get; private set; }
    [field: SerializeField] public EventReference targetEnemy { get; private set; }
    [field: SerializeField] public EventReference targetCancel { get; private set; }

    [field: Header("Com")]
    [field: SerializeField] public EventReference comEventStart { get; private set; }
    
    [field: Header("Enemy")]
    [field: SerializeField] public EventReference enemyActions { get; private set; }
    [field: SerializeField] public EventReference enemyChargeFTL { get; private set; }
    [field: SerializeField] public EventReference enemyEnterFTL { get; private set; }
    [field: SerializeField] public EventReference enemyFireProjectile { get; private set; }
    [field: SerializeField] public EventReference enemyFireLightProjectile { get; private set; }
    [field: SerializeField] public EventReference enemyFireMediumProjectile { get; private set; }
    [field: SerializeField] public EventReference enemyFireHeavyProjectile { get; private set; }
    [field: SerializeField] public EventReference enemyFireBigProjectile { get; private set; }
    [field: SerializeField] public EventReference enemyFireMissile { get; private set; }
    [field: SerializeField] public EventReference electricBallFire { get; private set; }
    [field: SerializeField] public EventReference electricBallAmbient { get; private set; }

    [field: Header("Hit")]
    [field: SerializeField] public EventReference playerHit { get; private set; }
    [field: SerializeField] public EventReference shieldHit { get; private set; }
    [field: SerializeField] public EventReference enemyHit { get; private set; }
    [field: SerializeField] public EventReference explosion { get; private set; }

    [field: Header("Shield")]
    [field: SerializeField] public EventReference shieldDown { get; private set; }
    [field: SerializeField] public EventReference shieldUp { get; private set; }


    [field: Header("UI")]
    [field: SerializeField] public EventReference hover { get; private set; }
    [field: SerializeField] public EventReference click { get; private set; }
    [field: SerializeField] public EventReference openInventory { get; private set; }
    [field: SerializeField] public EventReference equipItem { get; private set; }
    [field: SerializeField] public EventReference swapItem { get; private set; }

    [field: Header("Music/Ambient")]
    [field: SerializeField] public EventReference mainMenuAmbient { get; private set; }
    [field: SerializeField] public EventReference spaceAmbient { get; private set; }

    [field: SerializeField] public EventReference music { get; private set; }


    public static FMODEvents Instance { get; private set; }
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
        DontDestroyOnLoad(gameObject);
    }


}
