using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New boss event", menuName = "Events/BossEvent")]
public class BossEvent : Event
{
    public GameObject boss;
    public GameObject[] ads;
    public GameObject[] spawnEnemies;

    public PlayerIsTooFarAwayAction playerIsTooFarAway;

    public float range;
    public float timer;

    public BossEventVictoryCondition winCondition;

    public EventReward reward;
}

public enum PlayerIsTooFarAwayAction
{
    None,
    Despawn,
    LimitPlayerMovement,
    JumpToPlayer
}

public enum BossEventVictoryCondition
{
    None,
    KillEverything,
    KillBossOnly,
    Escape
}

public enum EventReward
{
    None,
    SectorPortalSpawn,
    Loot,
    SpecialLoot
}
